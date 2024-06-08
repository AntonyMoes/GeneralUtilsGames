using System;
using JetBrains.Annotations;

namespace GeneralUtils.States {
    public abstract class SubStateMachineWithInfo<TBaseStateEnum, TStateEnum, TStateInfo> : AbstractSubStateMachine<TBaseStateEnum, TStateEnum>
        where TBaseStateEnum : struct, Enum
        where TStateEnum : struct, Enum
        where TStateInfo : class, IStateInfo {
        protected sealed override IStateInfo OnStateEnterLogic(IStateInfo stateInfo = null) {
            return PerformStateEnter(stateInfo as TStateInfo);
        }

        [CanBeNull]
        protected virtual IStateInfo PerformStateEnter(TStateInfo stateInfo = null) {
            return null;
        }
    }
}
