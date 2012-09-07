using System.ComponentModel.Composition;
using IMAGO.Framework.MVVM;
using System.Windows.Input;

namespace IMAGO.Framework.ViewModels
{
    [Export("ViewModel")]
    [ExportMetadata("Name", "MainWindowViewModel")]
    public class MainWindowViewModel : IMAGOViewModelBase
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

        public ICommand OkCommand { get; private set; }
        #endregion

        #region Constructor 
        public MainWindowViewModel()
        {
            this.FirstName = "Paulo";

            
        }
        #endregion
    }
}