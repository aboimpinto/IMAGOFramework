using System;
using GalaSoft.MvvmLight;
using IMAGO.Framework.MVVM.Interfaces;

namespace IMAGO.Framework.MVVM 
{
    public abstract class IMAGOViewModelBase : ViewModelBase, IIMAGOViewModelBase
    {
        #region Private Fields 
        private bool _isBusy;

        private int _busyCount;
        private object oLock;
        #endregion

        #region Public Properties 
        public bool IsBusy 
        {
            get { return _isBusy; }
            set 
            {
                _isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }
        #endregion

        #region Constructor 
        public IMAGOViewModelBase()
        {
            _busyCount = 0;
        }
        #endregion

        #region IIMAGOViewModelBase Implementation 
        public void SubscribeIsBusyAction(Action<Action> actionToRun) 
        {
            InscressIsBusy();
            if (actionToRun != null) { actionToRun(() => { DecressIsBusy(); }); }
        }
        #endregion

        #region Private Methods 
        private void InscressIsBusy() 
        {
            lock(oLock)
            {
                _busyCount++;
                this.IsBusy = true;
            }
        }
        private void DecressIsBusy() 
        {
            lock(oLock)
            {
                _busyCount--;
                if (_busyCount <= 0)
                {
                    _busyCount = 0;
                    this.IsBusy = false;
                }
            }
        }
        #endregion
    }
}