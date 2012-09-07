using System;

namespace IMAGO.Framework.MVVM.Interfaces
{
    public interface IIMAGOViewModelBase
    {
        void SubscribeIsBusyAction(Action<Action> actionToRun);
    }
}