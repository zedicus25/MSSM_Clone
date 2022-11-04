using GalaSoft.MvvmLight.Command;
using MSSM_Clone.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSM_Clone.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public string ServerName
        {
            get => _serverName;
            set { _serverName = value; }
        }

        public string DatabaseName
        {
            get => _databaseName;
            set { _databaseName = value; }
        }


        private string _serverName;
        private string _databaseName;

        private RelayCommand _loginCommand;

        public RelayCommand LoginCommand
        {
            get
            {
                return _loginCommand ?? (_loginCommand = new RelayCommand(() =>
                {
                    if(_serverName != String.Empty & _databaseName != String.Empty)
                    {
                        MainViewModel.GetInstance().CreateSqlController(_serverName, _databaseName);
                    }
                }));
            }
        }

        private void ConnectionResult(bool res)
        {
            if (res)
                MainViewModel.GetInstance().ChangeViewModel(new HomeViewModel());
            else
                MainViewModel.GetInstance().SendMessageToView("Cannot connect to server or database");
        }

        public LoginViewModel()
        {
            SqlServerController.ConnectionResult += ConnectionResult;
        }
    }
}
