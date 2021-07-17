using System;
using System.Collections.Generic;
using System.Text;
using PrismDemo.ViewModels.Base;

namespace PrismDemo.ViewModels
{
    public class ShellViewModel: BaseViewModel
    {
        public string Title { get; set; }

        public ShellViewModel()
        {
            Title = "Пускач";
        }
    }
}
