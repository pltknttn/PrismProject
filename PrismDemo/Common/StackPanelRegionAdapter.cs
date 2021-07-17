using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace PrismDemo.Common
{
    public class StackPanelRegionAdapter : RegionAdapterBase<StackPanel>
    {
        public StackPanelRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory) : base(regionBehaviorFactory)
        {
        }

        protected override void Adapt(IRegion region, StackPanel regionTarget)
        {
            region.Views.CollectionChanged += (se, ev) =>
            {
                if (ev.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (UIElement item in ev.NewItems)
                    {
                        regionTarget.Children.Add(item);
                    }
                }
            };
        }

        protected override IRegion CreateRegion() => new AllActiveRegion();
    } 
}
