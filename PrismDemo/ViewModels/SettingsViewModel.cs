using Prism.Regions;
using PrismDemo.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrismDemo.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel(IRegionManager regionManager) : base(regionManager)
        {
            Title = "Настройки";
        }
    }
}
