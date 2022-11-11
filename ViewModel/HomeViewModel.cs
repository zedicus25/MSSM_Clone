using GalaSoft.MvvmLight.Command;
using System;

namespace MSSM_Clone.ViewModel
{
    public class HomeViewModel : BaseViewModel
    {
        public static event Action StartAddingData;
        public static event Action StartUpdatingData;
        public static event Action StartDeletingData;
        public RelayCommand AddData
        {
            get
            {
                return _addData ?? (_addData = new RelayCommand(() =>
                {
                    StartAddingData?.Invoke();
                }));
            }
        }
        private RelayCommand _addData;
        public RelayCommand UpdateData
        {
            get
            {
                return _updateData ?? (_updateData = new RelayCommand(() =>
                {
                    StartUpdatingData?.Invoke();
                }));
            }
        }
        private RelayCommand _updateData;

        public RelayCommand DeleteData
        {
            get
            {
                return _deleteData ?? (_deleteData = new RelayCommand(() =>
                {
                    StartDeletingData?.Invoke();
                }));
            }
        }
        private RelayCommand _deleteData;

        public HomeViewModel()
        {
        }
    }
}
