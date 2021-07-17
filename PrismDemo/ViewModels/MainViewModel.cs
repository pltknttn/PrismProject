using Prism.Regions;
using PrismDemo.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrismDemo.ViewModels
{
    public class MainViewModel : BaseViewModel
    { 
        public MainViewModel(IRegionManager regionManager) : base(regionManager)
        {
            Title = "Клиент";
             
        }
    }
}
