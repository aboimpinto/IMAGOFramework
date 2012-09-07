using System.ComponentModel.Composition;
using GalaSoft.MvvmLight;

namespace IMAGO.Framework.ViewModels
{
    [Export("ViewModel")]
    [ExportMetadata("Name", "MainWindowViewModel")]
    public class MainWindowViewModel : ViewModelBase
    {
        #region Private Fields 
        private string _firstName;
        #endregion

        #region Public Properties 

        public string FirstName
        {
            get { return _firstName;  }
            set
            {
                _firstName = value;
                RaisePropertyChanged("FirstName");
            }
        }
        #endregion

        #region Constructor 
        public MainWindowViewModel()
        {
            this.FirstName = "Paulo";
        }
        #endregion
    }
}