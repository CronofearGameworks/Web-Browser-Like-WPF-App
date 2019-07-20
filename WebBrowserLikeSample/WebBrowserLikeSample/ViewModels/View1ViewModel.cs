using Prism;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBrowserLikeSample.Interfaces;

namespace WebBrowserLikeSample.ViewModels
{
    public class View1ViewModel : BaseViewModel, IActiveTabAware
    {
        public View1ViewModel(IEventAggregator eventAggregator)
        {
            Title = "View 1";
        }

        private bool isModalOpen;

        public bool IsModalOpen
        {
            get { return isModalOpen; }
            set
            {
                SetProperty(ref isModalOpen, value);
            }
        }


        public void Activated()
        {
            IsModalOpen = true;
        }

        public void Deactivated()
        {
            IsModalOpen = false;
        }
    }
}
