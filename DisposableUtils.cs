using System;

namespace GeneralUtils {
    public class DisposeCallback : IDisposable {
        private Action _callback;

        public DisposeCallback(Action callback) {
            _callback = callback;
        }

        public void Dispose() {
            var callback = _callback;
            _callback = null;
            callback?.Invoke();
        }
    }
}