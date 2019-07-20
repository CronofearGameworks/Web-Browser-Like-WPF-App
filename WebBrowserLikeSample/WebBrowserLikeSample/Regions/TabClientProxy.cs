using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBrowserLikeSample.Interfaces;

namespace WebBrowserLikeSample.Regions
{
    public class TabClientProxy
    {
        public ICommonData CommonData { get; set; }
        public object Content { get; set; }
    }
}
