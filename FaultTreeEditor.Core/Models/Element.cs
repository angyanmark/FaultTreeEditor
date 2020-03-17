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
        private ObservableCollection<Element> parents = new ObservableCollection<Element>();
        public ObservableCollection<Element> Parents
        {
            get { return parents; }
            set { Set(ref parents, value); }
        }
        private ObservableCollection<Element> children = new ObservableCollection<Element>();
        public ObservableCollection<Element> Children
        {
            get { return children; }
            set { Set(ref children, value); }
        }
        protected string imageSource;
        public string ImageSource
        {
            get
            {
                return "/Assets/Images/Elements/" + ThemeColor + "/" + Source;
            }
            set
            {
                { Set(ref imageSource, value); }
            }
        }
        public abstract string Source
        {
            get;
        }
        public static string ThemeColor
        {
            get;
            set;
        }
        private string title;
        public string Title
        {
            get { return title; }
            set { Set(ref title, value); }
        }
        private double x;
        public double X
        {
            get { return x; }
            set { Set(ref x, value); }
        }
        private double y;
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
        public abstract string Info
        {
            get;
        }
        public abstract string GalileoFormat
        {
            get;
        }
    }
}
