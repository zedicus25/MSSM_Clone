using MSSM_Clone.Controllers;

namespace MSSM_Clone.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public static MainViewModel Instance { get; private set; }

		public SqlServerController ServerContoller => _sqlServerController;

		

		public string Message
		{
			get => _message;
			set
			{ 
				_message = value;
				OnPropertyChanged("Message");
			}
		}


		public BaseViewModel SelectedViewModel
		{
			get => _selectedViewModel;
			set 
			{ 
				_selectedViewModel = value;
				OnPropertyChanged("SelectedViewModel");
			}
		}

        private BaseViewModel _selectedViewModel;
        private SqlServerController _sqlServerController;
        private string _message;

        public MainViewModel()
		{
			if (Instance == null)
				Instance = this;
            SqlServerController.SendMessage += SendMessageToView;
            SelectedViewModel = new LoginViewModel();
		}



		public void CreateSqlController(string server, string databaseName)
		{
            _sqlServerController = new SqlServerController(server, databaseName);
        }

		public void SendMessageToView(string message) => Message = message;
					
		public void ChangeViewModel(BaseViewModel viewModel)
		{
			if (viewModel == null)
				return;

			SelectedViewModel = viewModel;
		} 
    }
}
