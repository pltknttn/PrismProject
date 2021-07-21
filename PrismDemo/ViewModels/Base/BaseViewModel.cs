using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PrismDemo.ViewModels.Base
{
    public class BaseViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        private Window _activeWindow = Application.Current.Windows.OfType<Window>().First();
        public Window CurrentWindow => _activeWindow;

        private string _title;
        public string Title
        {
            get => _title;
            set { _title = value; }
        }
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
            var view = navigationContext.NavigationService.Region.GetView(navigationContext.Uri.OriginalString);
        }
    }
}
