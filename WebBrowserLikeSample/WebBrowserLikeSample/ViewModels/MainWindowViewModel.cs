using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Dragablz;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using WebBrowserLikeSample.Regions;
using WebBrowserLikeSample.ViewModels;
using WebBrowserLikeSample.Views;

namespace WebBrowserLikeSample.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        private readonly IRegion mainRegion;


        public ICommand OpenView1 { get; }
        public ICommand OpenView2 { get; }
        public ICommand OpenView3 { get; }
        public ICommand AddItem { get; }
        public IInterTabClient InterTabClient { get; }

        public MainWindowViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
            OpenView1 = new DelegateCommand(OpenView1Action);
            OpenView2 = new DelegateCommand(OpenView2Action);
            OpenView3 = new DelegateCommand(OpenView3Action);
            //AddItem = new DelegateCommand(AddItemAction);

            InterTabClient = new InterTabClient();

            mainRegion = this.regionManager.Regions["MainRegion"];

            
            
        }

        //The same code from here was moved to the code behind from MainWindow
        //Since i didn't find a good way to add a HomeView to the mainRegion without adding a view dependency
        //And since calling AddItemAction() in the constructor throws an error (Prism probably has a way to manage the lifecycle of the viewmodel to fix this, but i didn't found it, yet)

        //private void AddItemAction()
        //{
        //    ///This code should probably go to the view, MainWindow code behind
        //    var view = new HomeView();
        //    //False means that this tab will be positioned on the last column of the tab manager
        //    eventAggregator.GetEvent<OpenTabEvent>().Publish(false);
        //    //Always add a new instance of HomeView
        //    mainRegion.Add(view);
        //    mainRegion.Activate(view);
        //}

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

            ///If the view to navigate doesn't exist yet, navigate and remove current view
            ///Also, the currentView should exist (there should be at least one opened tab)
            if (viewToNavigate == null && currentView != null)
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
    }
}