using System;
using System.Collections.Generic;
using System.Text;
using Prism.Regions;
using PrismDemo.ViewModels.Base;

namespace PrismDemo.ViewModels
{
    public class ShellViewModel: BaseViewModel
    { 
        public ShellViewModel(IRegionManager regionManager) : base(regionManager)
        {
            Title = "Пускач";
        }
    }
}
