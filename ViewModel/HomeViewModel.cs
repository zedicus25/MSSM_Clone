using GalaSoft.MvvmLight.Command;
using System;

namespace MSSM_Clone.ViewModel
{
    public class HomeViewModel : BaseViewModel
    {
        public static event Action StartAddingData;
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

        public HomeViewModel()
        {
        }
    }
}
