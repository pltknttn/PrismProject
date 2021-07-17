 using Prism.Regions;
using PrismDemo.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace PrismDemo.Common
{
    public class TabControlAdapter : RegionAdapterBase<TabControl>
    { 

        public TabControlAdapter (IRegionBehaviorFactory regionBehaviorFactory) : base(regionBehaviorFactory)
        { 
        }

        protected override void Adapt(IRegion region, TabControl regionTarget)
        {
            region.Views.CollectionChanged += (s, e) => { 
            
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (UserControl item in e.NewItems)
                    {
                        var controlType = item.GetType();
                        var title = (item.DataContext as BaseViewModel)?.Title ?? controlType.Name;

                        regionTarget.Items.Add(new TabItem { Header = title, Tag = controlType, Content = item });
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (UserControl item in e.OldItems)
                    {
                        var tabDeleted = regionTarget.Items.OfType<TabItem>().FirstOrDefault(f => f.Content == item);
                        if (tabDeleted == null) continue;

                        regionTarget.Items.Remove(tabDeleted);
                    }
                }            
            };
        }

        protected override void AttachBehaviors(IRegion region, TabControl regionTarget)
        {
            base.AttachBehaviors(region, regionTarget);
        }

        protected override void AttachDefaultBehaviors(IRegion region, TabControl regionTarget)
        {
            base.AttachDefaultBehaviors(region, regionTarget);
        }

        protected override IRegion CreateRegion()
        {
            return new SingleActiveRegion();
            //return _regionManager.Regions["Document"] ?? new AllActiveRegion();// new SingleActiveRegion();
        }
    }
}
