using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using PrismDemo.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrismDemo
{
    public class MainModule : IModule
    {
        private IRegionManager _regionManager;

        public MainModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion(Region.Main, typeof(LoginView));
            _regionManager.RegisterViewWithRegion(Region.Main, typeof(MainView));
            
            _regionManager.RegisterViewWithRegion(Region.Navigate, typeof(JornalView)); 
            _regionManager.RegisterViewWithRegion(Region.Navigate, typeof(SettingsView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        { 
        }
    }
}
