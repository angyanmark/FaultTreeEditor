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

        public string GetGalileoString()
        {
            string builder = "";
            foreach (var v in Elements)
            {
                builder += v.ToGalileo();
            }
            if (String.IsNullOrWhiteSpace(builder))
            {
                return "No output...";
            }
            else
            {
                return builder;
            }
        }

        public string ListConnections()
        {
            string builder = "";
            foreach (var v in Connections)
            {
                builder += $"{v.From.Title} -> {v.To.Title}\n";
            }
            if (String.IsNullOrWhiteSpace(builder))
            {
                return "No connections...";
            }
            else
            {
                return builder;
            }
        }

        public void RemoveConnections(Element element)
        {
            element.Parents.Clear();
            element.Children.Clear();

            foreach (var v in Elements)
            {
                v.Parents.Remove(element);
                v.Children.Remove(element);
            }

            var toRemove = new List<Connection>();
            foreach (var v in Connections)
            {
                if (v.From == element || v.To == element)
                {
                    toRemove.Add(v);
                }
            }
            foreach (var v in toRemove)
            {
                Connections.Remove(v);
            }
        }

        public void RemoveElement(Element element)
        {
            Elements.Remove(element);
            RemoveConnections(element);
        }
    }
}
