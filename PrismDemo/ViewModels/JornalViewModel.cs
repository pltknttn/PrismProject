using Prism.Regions;
using PrismDemo.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrismDemo.ViewModels
{
    public class JornalViewModel : BaseViewModel
    {
        public JornalViewModel(IRegionManager regionManager) : base(regionManager)
        {
            Title = "Журнал";
        }
    }
}
