using FaultTreeEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace FaultTreeEditor.Converters
{
    public class ElementTypeToTitleStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            ElementType elementType = (ElementType)value;

            string title = "";

            switch (elementType)
            {
                case ElementType.TopLevelEvent:
                    title = "Top level event";
                    break;
                case ElementType.Event:
                    title = "Event";
                    break;
                case ElementType.BasicEvent:
                    title = "Basic event";
                    break;
                case ElementType.AndGate:
                    title = "AND gate";
                    break;
                case ElementType.OrGate:
                    title = "OR gate";
                    break;
                case ElementType.VoteGate:
                    title = "Vote gate";
                    break;
                default:
                    break;
            }

            return title;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
