using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win8TestBedClient.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Private Fields 
        private string _firstName; 
        #endregion

        #region Public Properties 
        public string FirstName 
        {
            get { return _firstName; }
            set 
            {
                _firstName = value;
                RaisePropertyChanged(() => FirstName);
            }
        }
        #endregion

        #region Constructor 
        public MainPageViewModel()
        {
            this.FirstName = "Paulo";
        }
        #endregion
    }
}
