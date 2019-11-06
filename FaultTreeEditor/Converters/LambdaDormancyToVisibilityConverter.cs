using FaultTreeEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace FaultTreeEditor.Converters
{
    public class LambdaDormancyToVisibilityConverter : IValueConverter
    {
        // Define the Convert method to change a DateTime object to 
        // a month string.
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var v = value as Element;
            if(v != null)
            {
                if (v.DisplayTitle == "Basic event")
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            return Visibility.Collapsed;
        }

        // ConvertBack is not implemented for a OneWay binding.
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
