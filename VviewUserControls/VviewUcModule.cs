using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VviewUserControls.Views;

namespace VviewUserControls
{
  public  class VviewUcModule: IModule
    {
        IRegionManager _regionManager;
        IUnityContainer _container;

        public VviewUcModule(RegionManager regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
        }

        public void Initialize()
        {
            //_regionManager.RegisterViewWithRegion("SearchRegion", typeof(Searchpane));
            _regionManager.RegisterViewWithRegion("SearchRegion", typeof(MainSearch));
            //_regionManager.RegisterViewWithRegion("PreviewRegion", typeof(PrevSearch));

            //_container.RegisterTypeForNavigation<SearchPane>();

        }

    }
}
