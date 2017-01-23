using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using ComJanWpf.Models;
using AForge.Video.DirectShow;
using System.Collections;

namespace ComJanWpf.ViewModels
{
    public class DeviceFilters : ViewModel
    {
        public void Initialize()
        {
        }

        public string Name { set; get; }
        public string MonikerString { set; get; }

        public IEnumerable Get()
        {
            return from FilterInfo info in new FilterInfoCollection(FilterCategory.VideoInputDevice)
                   select new DeviceFilters { Name = info.Name, MonikerString = info.MonikerString };
        }
    }
}
