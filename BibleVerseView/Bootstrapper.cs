using Prism.Modularity;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VviewUserControls;
using Microsoft.Practices.Unity;
using VviewUserControls.Abstraction;
using VviewUserControls.Concrete;

namespace BibleVerseView
{
    class Bootstrapper:UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            //return base.CreateShell();
            return new Shell();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();
            App.Current.MainWindow = (Window)this.Shell;
            App.Current.MainWindow.Show();
        }
        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
            ModuleCatalog moduleCatalog = (ModuleCatalog)ModuleCatalog;

            moduleCatalog.AddModule(typeof(VviewUserControls.VviewUcModule));
            //moduleCatalog.AddModule(typeof(VviewUserControls.PreviewPane));
            //moduleCatalog.AddModule(typeof(Dashboard_Module.DashboardModule));
            //moduleCatalog.AddModule(typeof(WpfApp1module2.WpfApp1module2));
        }
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            try
            {
                //Registring Modules in Container
                this.Container.RegisterType<Object, VviewUserControls.VviewUcModule>("VviewUc");
                //this.Container.RegisterType<Object, PreviewPane>("PreviewPane");
                //this.Container.RegisterType<Object, DashboardModule>("Dashboard");

                //Registring for Navigation
                //this.Container.RegisterTypeForNavigation<Previewpane>();
                //this.Container.RegisterTypeForNavigation<Searchpane>();

                //Dependency Injection
                //this.Container.RegisterType<IDataTableBuilder, DataTableBuilder>();
                //this.Container.RegisterType<IServiceRequest, ServiceRequest>();


                this.Container.RegisterType<IFetchFromXml, XmlHandler>();
                //this.Container.RegisterType<IFetchFromXml, MainSearchViewModel>();
            }
            catch (Exception ex)
            {
                ex.GetRootException();
            }
           

        }


    }
}
