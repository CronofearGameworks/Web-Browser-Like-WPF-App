using CHRobinson.Enterprise.Shell.Regions.Behaviors;
using DryIoc;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WebBrowserLikeSample.ViewModels;
using WebBrowserLikeSample.Views;

namespace WebBrowserLikeSample
{
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.Register<HomeView>();
            containerRegistry.RegisterForNavigation<View1>();
            containerRegistry.RegisterForNavigation<View2>();
            containerRegistry.RegisterForNavigation<View3>();

        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            Application.Current.ShutdownMode = System.Windows.ShutdownMode.OnLastWindowClose;
            // create region
            var region = new SingleActiveRegion() { Name = "MainRegion" };

            region.Behaviors.Add(DragablzWindowBehavior.BehaviorKey, new DragablzWindowBehavior());

            var regionManager = Container.Resolve<IRegionManager>();
            regionManager.Regions.Add("MainRegion", region);

            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
        }

    }
}
