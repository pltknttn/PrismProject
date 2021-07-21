using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Prism.Events;
using Prism.Regions;
using PrismDemo.Common;
using PrismDemo.ViewModels.Base;
using Unity;

namespace PrismDemo.ViewModels
{
    public class ShellViewModel: BaseViewModel
    {
        protected IUnityContainer Container { get; set; }
         
        public ShellViewModel(IUnityContainer container, IRegionManager regionManager, IRegionNavigationService regionNavigationService) : base(regionManager)
        {
            Title = "Пускач";
            Container = container;
            regionNavigationService.Navigated += RegionNavigationService_Navigated;
             
        }

        private void RegionNavigationService_Navigated(object sender, RegionNavigationEventArgs e)
        { 
        } 

        public async Task TestAsync()
        {
            await Worker.ExecuteProcess("notepad.exe");
        }
    }
}
