using System;
using System.Linq;

namespace GeneralUtils {
    public class StateSwitcher<TEState> where TEState : Enum {
        private readonly UpdatedValue<TEState> _state;
        public IUpdatedValue<TEState> State => _state;

        public StateSwitcher(TEState initialState) {
            _state = new UpdatedValue<TEState>(initialState);
        }

        public void CheckAndSwitchState(TEState newState, params TEState[] expected) {
            CheckState(newState, expected);
            _state.Value = newState;
        }

        private void CheckState(TEState newState, params TEState[] expected) {
            if (!CorrectState(expected)) {
                throw new ApplicationException($"Expected {string.Join(" or ", expected)} state but got {State.Value}; target: {newState}");
            }
        }

        public void CheckState(params TEState[] expected) {
            if (!CorrectState(expected)) {
                throw new ApplicationException($"Expected {string.Join(" or ", expected)} state but got {State.Value}");
            }
        }

        private bool CorrectState(params TEState[] expected) {
            return expected.Length == 0 || expected.Contains(_state.Value);
        }
    }
}
