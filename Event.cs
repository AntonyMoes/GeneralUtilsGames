using System;

namespace GeneralUtils {
    public class Event : IEvent {
        private Action _event;

        public void Subscribe(Action subscriber) {
            _event += subscriber;
        }

        public void Unsubscribe(Action subscriber) {
            _event -= subscriber;
        }

        public void ClearSubscribers() {
            _event = null;
        }

        public void Invoke() {
            _event?.Invoke();
        }
    }

    public interface IEvent {
        public void Subscribe(Action subscriber);
        public void Unsubscribe(Action subscriber);
        public void ClearSubscribers();
    }

    public class Event<T> : IEvent<T> {
        private Action<T> _event;

        public void Subscribe(Action<T> subscriber) {
            _event += subscriber;
        }

        public void Unsubscribe(Action<T> subscriber) {
            _event -= subscriber;
        }

        public void ClearSubscribers() {
            _event = null;
        }

        public void Invoke(T value) {
            _event?.Invoke(value);
        }
    }

    public interface IEvent<out T> {
        public void Subscribe(Action<T> subscriber);
        public void Unsubscribe(Action<T> subscriber);
        public void ClearSubscribers();
    }

    public class Event<T1, T2> : IEvent<T1, T2> {
        private Action<T1, T2> _event;

        public void Subscribe(Action<T1, T2> subscriber) {
            _event += subscriber;
        }

        public void Unsubscribe(Action<T1, T2> subscriber) {
            _event -= subscriber;
        }

        public void ClearSubscribers() {
            _event = null;
        }

        public void Invoke(T1 value1, T2 value2) {
            _event?.Invoke(value1, value2);
        }
    }

    public interface IEvent<out T1, out T2> {
        public void Subscribe(Action<T1, T2> subscriber);
        public void Unsubscribe(Action<T1, T2> subscriber);
        public void ClearSubscribers();
    }

    public class Event<T1, T2, T3> : IEvent<T1, T2, T3> {
        private Action<T1, T2, T3> _event;

        public void Subscribe(Action<T1, T2, T3> subscriber) {
            _event += subscriber;
        }

        public void Unsubscribe(Action<T1, T2, T3> subscriber) {
            _event -= subscriber;
        }

        public void ClearSubscribers() {
            _event = null;
        }

        public void Invoke(T1 value1, T2 value2, T3 value3) {
            _event?.Invoke(value1, value2, value3);
        }
    }

    public interface IEvent<out T1, out T2, out T3> {
        public void Subscribe(Action<T1, T2, T3> subscriber);
        public void Unsubscribe(Action<T1, T2, T3> subscriber);
        public void ClearSubscribers();
    }
}
