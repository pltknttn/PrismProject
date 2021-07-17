using Prism.Regions;
using PrismDemo.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrismDemo.ViewModels
{
    public class MailViewModel : BaseViewModel
    {
        public MailViewModel(IRegionManager regionManager) : base(regionManager)
        {
            Title = "Почта";
        }
    }
}
