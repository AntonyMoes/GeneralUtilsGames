using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneralUtils {
    public class UpdatedValue<T> : IUpdatedValue<T> {
        private readonly List<PredicatePair> _waiters = new List<PredicatePair>();
        private readonly List<Action<T>> _subscribers = new List<Action<T>>();

        private readonly Func<T, T> _setter;
        private readonly bool _setIfNotChanged;

        private T _value;

        public T Value {
            get => _value;
            set {
                var valueToSet = _setter == null ? value : _setter(value);
                if (!_setIfNotChanged && (valueToSet?.Equals(_value) ?? _value == null)) {
                    return;
                }

                _value = valueToSet;

                var activated = new List<PredicatePair>();
                foreach (var pair in _waiters) {
                    if (pair.Predicate(Value)) {
                        activated.Add(pair);
                    }
                }

                foreach (var pair in activated) {
                    _waiters.Remove(pair);
                }

                foreach (var pair in activated) {
                    pair.Callback?.Invoke();
                }

                foreach (var subscriber in _subscribers.ToArray()) {
                    subscriber?.Invoke(_value);
                }
            }
        }

        public UpdatedValue(T initialValue = default, Func<T, T> setter = null, bool setIfNotChanged = false) {
            _value = initialValue;
            _setter = setter;
            _setIfNotChanged = setIfNotChanged;
        }

        public IDisposable WaitForChange(Action onDone) {
            // HACK
            var counter = 0;

            bool SecondTimePredicate(T value) {
                return counter++ >= 1;
            }

            return WaitFor(SecondTimePredicate, onDone);
        }

        public IDisposable WaitFor(T concreteValue, Action onDone) {
            return WaitFor(value => value.Equals(concreteValue), onDone);
        }

        public IDisposable WaitFor(Func<T, bool> predicate, Action onDone) {
            if (predicate(Value)) {
                onDone?.Invoke();
                return WaitToken.Empty;
            }

            var predicatePair = new PredicatePair(predicate, onDone);
            _waiters.Add(predicatePair);
            return new WaitToken(() => _waiters.Remove(predicatePair));
        }

        public IDisposable Subscribe(Action<T> onChange, bool triggerInitialUpdate = false) {
            _subscribers.Add(onChange);

            if (triggerInitialUpdate) {
                onChange?.Invoke(Value);
            }

            return new DisposeCallback(() => Unsubscribe(onChange));
        }

        public void Unsubscribe(Action<T> onChange) {
            _subscribers.Remove(onChange);
        }

        public void Clear() {
            _waiters.Clear();
            _subscribers.Clear();
        }

        private class PredicatePair {
            public readonly Func<T, bool> Predicate;
            public readonly Action Callback;

            public PredicatePair(Func<T, bool> predicate, Action callback) {
                Predicate = predicate;
                Callback = callback;
            }
        }

        private class WaitToken : IDisposable {
            public static readonly WaitToken Empty = new WaitToken();

            private Action _removeWaiter;

            public WaitToken(Action removeWaiter = null) {
                _removeWaiter = removeWaiter;
            }

            public void Dispose() {
                var remove = _removeWaiter;
                _removeWaiter = null;
                remove?.Invoke();
            }
        }
    }

    public interface IUpdatedValue<T> {
        public T Value { get; }

        public IDisposable WaitForChange(Action onDone);
        public IDisposable WaitFor(T concreteValue, Action onDone);
        public IDisposable WaitFor(Func<T, bool> predicate, Action onDone);

        public IDisposable Subscribe(Action<T> onChange, bool triggerInitialUpdate = false);
        public void Unsubscribe(Action<T> onChange);

        public void Clear();
    }

    public static class UpdatedValueWaiter {
        public static IDisposable WaitForAll<T>(IEnumerable<IUpdatedValue<T>> values, T concreteValue, Action onDone) =>
            WaitForAll(values, value => value.Equals(concreteValue), onDone);

        public static IDisposable WaitForAll<T>(IEnumerable<IUpdatedValue<T>> values, Func<T, bool> predicate,
            Action onDone) {
            var cancelled = false;
            var valueArray = values.ToArray();
            var countLeft = valueArray.Length;

            foreach (var value in valueArray) {
                value.WaitFor(predicate, OnPredicate);
            }

            void OnPredicate() {
                if (!cancelled) {
                    if (--countLeft == 0) {
                        onDone?.Invoke();
                    }
                }
            }

            return new DisposeCallback(() => cancelled = true);
        }
    }
}