using System;
using JetBrains.Annotations;

namespace GeneralUtils.States {
    public abstract class SubStateMachine<TBaseStateEnum, TStateEnum> : AbstractSubStateMachine<TBaseStateEnum, TStateEnum>
        where TBaseStateEnum : struct, Enum
        where TStateEnum : struct, Enum {

        protected sealed override IStateInfo OnStateEnterLogic(IStateInfo stateInfo = null) {
            return PerformStateEnter();
        }

        [CanBeNull]
        protected virtual IStateInfo PerformStateEnter() {
            return null;
        }
    }
}
