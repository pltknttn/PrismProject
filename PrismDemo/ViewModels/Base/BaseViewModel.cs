using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PrismDemo.ViewModels.Base
{
    public class BaseViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        public string Title { get; set; }
        public BitmapImage Image { get; set; }

        public bool KeepAlive { get; set; } = true;

        IRegionManager _regionManager;
        public IRegionManager RegionManager => _regionManager;

        public BaseViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

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
