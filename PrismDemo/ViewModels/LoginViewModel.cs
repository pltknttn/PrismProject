using Prism.Commands;
using Prism.Regions;
using PrismDemo.ViewModels.Base;
using PrismDemo.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PrismDemo.ViewModels
{
    public class LoginViewModel : BaseViewModel
    { 
        public bool IsAllowLoging => !string.IsNullOrWhiteSpace(Key) && !string.IsNullOrWhiteSpace(ArmName);

        private string _key;
        public string Key { get => _key; set { SetProperty(ref _key, value); RaisePropertyChanged(nameof(IsAllowLoging)); } }

        private string _armName;
        public string ArmName { get => _armName; set { SetProperty(ref _armName, value); RaisePropertyChanged(nameof(IsAllowLoging)); } }
        
        public ICommand LoginCommand { get; set; }

        IRegionManager _regionManager;

        public LoginViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            LoginCommand = new DelegateCommand( OnLogin, CanOnLogin).ObservesProperty(() => IsAllowLoging); 
        }
            
        private void OnLogin()
        {
            _regionManager.RequestNavigate(Region.Main, nameof(MainView));
        }

        private bool CanOnLogin()
        {
            return IsAllowLoging;
        }
    }
}
