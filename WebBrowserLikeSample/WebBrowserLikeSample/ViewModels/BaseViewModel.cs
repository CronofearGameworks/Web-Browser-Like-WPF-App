using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBrowserLikeSample.Interfaces;

namespace WebBrowserLikeSample.ViewModels
{
    public class BaseViewModel : BindableBase, ICommonData
    {
        private string title;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                SetProperty(ref title, value);
            }
        }
    }
}
