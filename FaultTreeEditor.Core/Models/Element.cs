using FaultTreeEditor.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace FaultTreeEditor.Core.Models
{
    public abstract class Element : Observable
    {
        public ObservableCollection<Element> Parents { get; set; } = new ObservableCollection<Element>();
        public ObservableCollection<Element> Children { get; set; } = new ObservableCollection<Element>();
        private string title;
        public string Title
        {
            get { return title; }
            set { Set(ref title, value); }
        }
        public abstract string ImageSource
        {
            get;
        }
        private double x = 150;
        public double X
        {
            get { return x; }
            set { Set(ref x, value); }
        }
        private double y = 150;
        public double Y
        {
            get { return y; }
            set { Set(ref y, value); }
        }
        public string getChildrenString()
        {
            return string.Join(" ", Children.Select(c => c.Title));
        }
        public abstract string ToGalileo();
        public abstract string DisplayTitle
        {
            get;
        }
    }
}
