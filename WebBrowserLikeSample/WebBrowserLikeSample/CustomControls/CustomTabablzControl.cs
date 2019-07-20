using Dragablz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WebBrowserLikeSample.CustomControls
{
    public class CustomTabablzControl : TabablzControl
    {


        public ICommand testAddItem
        {
            get { return (ICommand)GetValue(testAddItemProperty); }
            set { SetValue(testAddItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for testAddItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty testAddItemProperty =
            DependencyProperty.Register("testAddItem", typeof(ICommand), typeof(CustomTabablzControl));
    }
}
