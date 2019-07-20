using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebBrowserLikeSample.Regions;

namespace WebBrowserLikeSample.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IRegion mainRegion;

        public MainWindow(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            InitializeComponent();
            this.eventAggregator = eventAggregator;
            eventAggregator.GetEvent<DragablzWindowEvent>().Publish(new DragablzWindowEventArgs() { TabControl = TabablzControl, Type = DragablzWindowEventType.Opened });

            mainRegion = regionManager.Regions["MainRegion"];

            AddItemAction();
            AddItemAction();
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            eventAggregator.GetEvent<DragablzWindowEvent>().Publish(new DragablzWindowEventArgs() { TabControl = TabablzControl, Type = DragablzWindowEventType.Closed });
        }

        private void MainWindow_OnActivated(object sender, EventArgs e)
        {
            eventAggregator.GetEvent<DragablzWindowEvent>().Publish(new DragablzWindowEventArgs() { TabControl = TabablzControl, Type = DragablzWindowEventType.Activated });
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void AddItemAction()
        {
            var view = new HomeView();
            //False means that this tab will be positioned on the last column of the tab manager
            eventAggregator.GetEvent<OpenTabEvent>().Publish(false);
            //Always add a new instance of HomeView to the region when pressing the + button
            mainRegion.Add(view);
            mainRegion.Activate(view);
        }

        private void DefaultAddButton_Click(object sender, RoutedEventArgs e)
        {
            AddItemAction();
        }

        private void MetroWindow_KeyDown(object sender, KeyEventArgs e)
        {
            ///Control + Key Events
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                switch (e.Key)
                {
                    case Key.T: AddItemAction();
                        break;
                }
            }
        }
    }
}
