using Prism.Regions;
using PrismDemo.Images;
using PrismDemo.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace PrismDemo.ViewModels
{
    public class MailViewModel : BaseViewModel
    {
        public static string ImagePath = "PrismDemo;component/Images/microsoft_outlook_32.png";

        public MailViewModel(IRegionManager regionManager) : base(regionManager)
        {
            Title = "Почта";             
            Image = SampleImage.CreateImage(ImagePath);
        }
    }
}
