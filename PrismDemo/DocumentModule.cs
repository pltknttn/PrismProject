using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using PrismDemo.ViewModels;
using PrismDemo.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrismDemo
{
    public class DocumentModule : IModule
    {
        private IRegionManager _regionManager;

        public DocumentModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager ??= containerProvider.Resolve<IRegionManager>();
                         
            if (!_regionManager.Regions.ContainsRegionWithName(Region.Document))
            {
                var region = new Prism.Regions.Region { Name = Region.Document };
                _regionManager.Regions.Add(region);
            }

            _regionManager.RegisterViewWithRegion(Region.Document, typeof(JornalView)); 
            _regionManager.RegisterViewWithRegion(Region.Document, typeof(SettingsView));
            _regionManager.RegisterViewWithRegion(Region.Document, typeof(MailView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.RegisterSingleton<ShellViewModel>();
            //containerRegistry.RegisterSingleton<MainViewModel>();
            //containerRegistry.RegisterSingleton<LoginViewModel>();

            //containerRegistry.Register<JornalViewModel>();
            //containerRegistry.Register<SettingsViewModel>();
            //containerRegistry.Register<MailViewModel>(); 
        }
    }
}
