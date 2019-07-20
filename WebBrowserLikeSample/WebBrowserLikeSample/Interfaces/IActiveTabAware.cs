using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBrowserLikeSample.Interfaces
{
    /// <summary>
    /// Activated is triggered when the tab is activated (selected, focused)
    /// </summary>
    public interface IActiveTabAware
    {
        void Activated();
        void Deactivated();
    }
}
