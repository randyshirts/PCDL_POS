using System;
using System.Globalization;
using System.Windows.Data;
using POS.Controller.Elements;

namespace POS
{

        public class SelectedSaleItemConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return value;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is SaleItem)
                        return value;
                return null;
            }
        }

        

}
