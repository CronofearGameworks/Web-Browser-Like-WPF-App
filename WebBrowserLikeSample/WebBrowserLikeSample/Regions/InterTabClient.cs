using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommonServiceLocator;
using Dragablz;
using WebBrowserLikeSample.Views;

namespace WebBrowserLikeSample.Regions
{
    public class InterTabClient : IInterTabClient
    {
        public INewTabHost<Window> GetNewHost(IInterTabClient interTabClient, object partition, TabablzControl source)
        {
            //When tearing a windows, create a new main window
            var view = ServiceLocator.Current.GetInstance<MainWindow>();

            return new NewTabHost<Window>(view, view.TabablzControl);
        }

        public TabEmptiedResponse TabEmptiedHandler(TabablzControl tabControl, Window window)
        {
            //When closing the last tab, close the root window
            return TabEmptiedResponse.CloseWindowOrLayoutBranch;
        }
    }
}
