using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrismDemo.ViewModels.Base
{
    public class BaseViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        public bool KeepAlive { get; set; } = true;

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            string id = (string)navigationContext.Parameters["ID"];
            //return _currentCustomer.Id.Equals(id);
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
             
        }
    }
}
