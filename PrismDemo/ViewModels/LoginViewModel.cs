using Prism.Commands;
using Prism.Regions;
using PrismDemo.ViewModels.Base;
using PrismDemo.Views;
using PrismDemo.Common;
using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using System.Windows.Input;

namespace PrismDemo.ViewModels
{
    public class LoginViewModel : BaseViewModel
    { 
        public bool IsAllowLoging => !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password?.ToUnsecuredString());

        private string _login;
        public string Login { get => _login; set { SetProperty(ref _login, value); RaisePropertyChanged(nameof(IsAllowLoging)); } }

        private SecureString _password;
        public SecureString Password { get => _password; set { SetProperty(ref _password, value); RaisePropertyChanged(nameof(IsAllowLoging)); } }
        
        public ICommand LoginCommand { get; set; }
 
        public LoginViewModel(IRegionManager regionManager) : base(regionManager)
        { 
            LoginCommand = new DelegateCommand( OnLogin, CanOnLogin).ObservesProperty(() => IsAllowLoging);
            Title = "Авторизация";
        }
            
        private void OnLogin()
        {
            RegionManager.RequestNavigate(Regions.Main, nameof(MainView));
        }

        private bool CanOnLogin()
        {
            return IsAllowLoging;
        }
    }
}
