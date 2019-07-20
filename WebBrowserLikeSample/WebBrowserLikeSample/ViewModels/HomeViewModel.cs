using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WebBrowserLikeSample.Regions;

namespace WebBrowserLikeSample.ViewModels
{
    class HomeViewModel : BaseViewModel, INavigationAware
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        private readonly IRegion mainRegion;

        public ICommand OpenView1 { get; }
        public ICommand OpenView2 { get; }
        public ICommand OpenView3 { get; }

        public HomeViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
            mainRegion = this.regionManager.Regions["MainRegion"];

            OpenView1 = new DelegateCommand(OpenView1Action);
            OpenView2 = new DelegateCommand(OpenView2Action);
            OpenView3 = new DelegateCommand(OpenView3Action);

            Title = "Home";

        }

        private void OpenView3Action()
        {
            CheckOpenTabAndNavigate("View3");
        }

        private void OpenView2Action()
        {
            CheckOpenTabAndNavigate("View2");

        }

        private void OpenView1Action()
        {
            CheckOpenTabAndNavigate("View1");
        }

        private void CheckOpenTabAndNavigate(string viewName)
        {
            var currentView = mainRegion.ActiveViews.LastOrDefault();
            var viewToNavigate = mainRegion.Views.FirstOrDefault(x => x.GetType().Name == viewName);
            ///True means that the viewToNavigate will replace the currentView. See Logic in TabablzRegionBehavior > AddView
            eventAggregator.GetEvent<OpenTabEvent>().Publish(true); 

            ///If the view to navigate doesn't exist, navigate and remove current view
            if (viewToNavigate == null)
            {
                mainRegion.RequestNavigate(viewName);
                mainRegion.Remove(currentView);
            }
            /// Else, only navigate
            else 
            {
                mainRegion.RequestNavigate(viewName);
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {          

        }
    }
}
