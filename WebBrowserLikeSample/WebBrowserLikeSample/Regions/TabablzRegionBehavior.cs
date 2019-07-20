using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using CommonServiceLocator;
using Dragablz;
using Prism.Events;
using Prism.Regions;
using WebBrowserLikeSample.Interfaces;
using WebBrowserLikeSample.Regions;
using WebBrowserLikeSample.ViewModels;

namespace CHRobinson.Enterprise.Shell.Regions.Behaviors
{
    public class DragablzWindowBehavior : RegionBehavior
    {
        public const string BehaviorKey = "DragablzWindowBehavior";

        private TabablzControl activeWindow;
        private readonly ObservableCollection<TabablzControl> windows;
        private readonly Dictionary<object, TabClientProxy> itemToTabClientMapping;

        public DragablzWindowBehavior()
        {
            this.windows = new ObservableCollection<TabablzControl>();
            this.itemToTabClientMapping = new Dictionary<object, TabClientProxy>();
        }

        protected override void OnAttach()
        {
            this.Region.Views.CollectionChanged += Views_CollectionChanged;
            this.Region.ActiveViews.CollectionChanged += ActiveViews_CollectionChanged;

            var eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<DragablzWindowEvent>().Subscribe(OnDragablzWindowEvent);
            eventAggregator.GetEvent<OpenTabEvent>().Subscribe(OnOpenTabEvent);

        }

        private bool replaceCurrentTab;

        private void OnOpenTabEvent(bool replaceCurrentTab)
        {
            this.replaceCurrentTab = replaceCurrentTab;
        }

        public void OnDragablzWindowEvent(DragablzWindowEventArgs args)
        {
            switch (args.Type)
            {
                case DragablzWindowEventType.Opened:
                    OnWindowOpened(args.TabControl);
                    break;

                case DragablzWindowEventType.Closed:
                    OnWindowClosed(args.TabControl);
                    break;

                case DragablzWindowEventType.Activated:
                    OnWindowActivated(args.TabControl);
                    break;
            }
        }

        private void OnWindowActivated(TabablzControl tabControl)
        {
            if (this.activeWindow != tabControl)
            {
                SetActiveView(tabControl);
            }
        }

        private void OnWindowClosed(TabablzControl tabControl)
        {
            ClearRelatedTabs(tabControl);
            this.windows.Remove(tabControl);
            tabControl.SelectionChanged -= TabControl_SelectionChanged;

            if (this.activeWindow == tabControl)
            {
                this.activeWindow = this.windows.FirstOrDefault();
            }
        }

        private void ClearRelatedTabs(TabablzControl tabControl)
        {
            var items = tabControl.Items.OfType<TabClientProxy>().ToList();

            items.ForEach(item =>
            {
                try
                {
                    this.Region.Remove(item.Content);
                }
                catch (ArgumentException) { }
            });
        }

        private void OnWindowOpened(TabablzControl tabControl)
        {
            this.activeWindow = tabControl;
            this.windows.Add(tabControl);
            tabControl.ClosingItemCallback = ClosingItemCallback;
            tabControl.SelectionChanged += TabControl_SelectionChanged;
        }

        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var item = e.AddedItems[0] as TabClientProxy;
                if (item != null)
                {
                    var viewToNavigate = item.Content;
                    var viewModelToNavigate = item.CommonData as IActiveTabAware;
                    var currentActiveView = this.Region.ActiveViews.LastOrDefault() as FrameworkElement;
                    var currentActiveViewModel = currentActiveView.DataContext as IActiveTabAware;

                    if (this.Region.Views.Contains(viewToNavigate))
                    {
                        currentActiveViewModel?.Deactivated();
                        viewModelToNavigate?.Activated();
                        this.Region.Activate(viewToNavigate);
                    }
                }
            }
        }


        private void ClosingItemCallback(ItemActionCallbackArgs<TabablzControl> args)
        {
            // remove from region
            this.Region.Remove(((TabClientProxy)args.DragablzItem.DataContext).Content);
        }

        private void ActiveViews_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    ActivateView(e.NewItems[0]);
                    break;
            }
        }

        private void Views_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var currentActiveView = this.Region.ActiveViews.LastOrDefault() as FrameworkElement;
                    var currentActiveViewModel = currentActiveView?.DataContext as IActiveTabAware;
                    currentActiveViewModel?.Deactivated();
                    AddView(e.NewItems[0]);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    RemoveView(e.OldItems[0]);
                    break;
            }
        }

        private void ActivateView(object view)
        {
            var proxy = GetProxy(view);
            var window = GetWindow(view);

            Debug.WriteLine($"Activating {proxy.CommonData.Title}");

            if (window.SelectedItem != proxy || window != this.activeWindow)
            {
                window.SelectedItem = proxy;
                window.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private void SetActiveView(TabablzControl window)
        {
            if (this.activeWindow != window)
            {
                this.activeWindow = window;
                this.activeWindow.BringIntoView();
                this.activeWindow.Focus();

                var view = this.activeWindow.SelectedItem as TabClientProxy;
                if (view != null && this.Region.Views.Contains(view.Content))
                {
                    this.Region.Activate(view.Content);
                }
            }
        }

        private void RemoveView(object view)
        {
            var window = GetWindow(view);
            var proxy = GetProxy(view);
            window.Items.Remove(proxy);

            CleanUp(view);
        }

        private void CleanUp(object view)
        {
            this.itemToTabClientMapping.Remove(view);
        }

        private void AddView(object view)
        {
            if (replaceCurrentTab)
            {
                this.activeWindow.AddLocationHint = AddLocationHint.After;
                this.activeWindow.AddToSource(CreateProxy(view));
                //this.activeWindow.RemoveFromSource(CurrentTab); We're deleting from the Prism MainRegion instead
            }
            else
            {
                this.activeWindow.AddLocationHint = AddLocationHint.Last;
                this.activeWindow.AddToSource(CreateProxy(view));
            }
            //this.activeWindow.Items.Add(view);
        }

        private TabablzControl GetWindow(object view)
        {
            var proxy = GetProxy(view);

            foreach (var window in this.windows)
            {
                if (ContainsView(window, proxy))
                {
                    return window;
                }
            }

            return null;
        }

        private bool ContainsView(TabablzControl window, TabClientProxy proxy)
        {
            if (proxy == null || window == null) return false;

            return window.Items.OfType<TabClientProxy>().Any(tc => tc == proxy);
        }

        private TabClientProxy GetProxy(object view)
        {
            return this.itemToTabClientMapping[view];
        }

        private TabClientProxy CreateProxy(object view)
        {
            var proxy = new TabClientProxy
            {
                Content = view,
                CommonData = RegionUtility.GetInterfaceFromView<ICommonData>(view)
            };

            this.itemToTabClientMapping.Add(view, proxy);

            return proxy;
        }
    }
}
