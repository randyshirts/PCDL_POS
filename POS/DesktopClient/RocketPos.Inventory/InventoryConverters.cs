using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using Inventory.Controller.Elements;
using Inventory.Controller.Elements.ItemElements;
using RocketPos.Common.Helpers;
using RocketPos.Inventory.Resources;

namespace Inventory
{

        public class SelectedBookItemConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return value;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is BookItem)
                        return value;
                return null;
            }
        }

        
        

        public class SelectedGameItemConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return value;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is GameItem)
                    return value;
                return null;
            }
        }

        public class SelectedOtherItemConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return value;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is OtherItem)
                    return value;
                return null;
            }
        }

        public class SelectedTeachingAideItemConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return value;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is TeachingAideItem)
                    return value;
                return null;
            }
        }

        public class SelectedVideoItemConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return value;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is VideoItem)
                    return value;
                return null;
            }
        }

        public class SelectedConsignorInfoConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return value;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is ConsignorInfo)
                    return value;
                return null;
            }
        }

        public class SelectedMemberInfoConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return value;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is MemberInfo)
                    return value;
                return null;
            }
        }

        public class SelectedConsignorItemConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return value;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is ConsignorItem)
                    return value;
                return null;
            }
        }

}
