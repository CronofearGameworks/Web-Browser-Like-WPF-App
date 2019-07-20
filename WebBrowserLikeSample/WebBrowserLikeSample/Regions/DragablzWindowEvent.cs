using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dragablz;
using Prism.Events;

namespace WebBrowserLikeSample.Regions
{
    /// <summary>
    /// If true, the new tab will replace current active tab.
    /// If false, the new tab will be placed on the last position.
    /// </summary>
    /// 
    public class ToggleModalEvent : PubSubEvent<bool>
    {

    }
    public class OpenTabEvent : PubSubEvent<bool>
    {

    }

    public class DragablzWindowEvent : PubSubEvent<DragablzWindowEventArgs>
    {

    }

    public class DragablzWindowEventArgs
    {
        public DragablzWindowEventType Type { get; set; }
        public TabablzControl TabControl { get; set; }
    }

    public enum DragablzWindowEventType
    {
        Opened,
        Closed,
        Activated
    }
}
