using FaultTreeEditor.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FaultTreeEditor.Core.Models
{
    public class FaultTree : Observable
    {
        private ObservableCollection<Element> elements = new ObservableCollection<Element>();
        public ObservableCollection<Element> Elements
        {
            get { return elements; }
            set { Set(ref elements, value); }
        }

        private ObservableCollection<Connection> connections = new ObservableCollection<Connection>();
        public ObservableCollection<Connection> Connections
        {
            get { return connections; }
            set { Set(ref connections, value); }
        }
    }
}
