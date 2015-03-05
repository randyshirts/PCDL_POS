using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;

namespace RocketPos.Common.Helpers
{



    public class NullImageConverter : IValueConverter
    {

        public class SelectedBarcodeItemConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return value;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is BarcodeItem)
                    return value;
                return null;
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ?? DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class IntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strVal = value != null ? value.ToString() : null;

            if (string.IsNullOrEmpty(strVal))
                return null;

            return int.Parse(strVal);
        }



        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string temp = null;
            if (value != null)
                temp = value.ToString();
            return String.IsNullOrEmpty(temp) ? null : temp;
        }
    }

    public class MoneyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strVal = value != null ? value.ToString() : null;

            return string.IsNullOrEmpty(strVal) ? null : double.Parse(strVal).ToString("C2");
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string temp = null;
            if (value != null)
                temp = value.ToString();
            return String.IsNullOrEmpty(temp) ? null : temp;
        }
    }

    public class PercentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strVal = value != null ? value.ToString() : null;

            return string.IsNullOrEmpty(strVal) ? null : double.Parse(strVal).ToString("P0");
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string temp = null;
            if (value != null)
                temp = value.ToString();
            return String.IsNullOrEmpty(temp) ? null : temp;
        }
    }


    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strVal = value != null ? value.ToString() : null;

            if (strVal == DateTime.MinValue.ToString())
                return null;

            return string.IsNullOrEmpty(strVal) ? null : DateTime.Parse(strVal).ToString("d");
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string temp = null;
            if (value != null)
                temp = value.ToString();
            return String.IsNullOrEmpty(temp) ? null : temp;
        }
    }

    public class PhoneConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var strVal = value.ToString();
            switch (strVal.Length)
            {
                case 7:
                    return Regex.Replace(strVal, @"(\d{3})(\d{4})", "$1-$2");
                case 10:
                    return Regex.Replace(strVal, @"(\d{3})(\d{3})(\d{4})", "($1) $2-$3");
                case 11:
                    return Regex.Replace(strVal, @"(\d{1})(\d{3})(\d{3})(\d{4})", "$1-$2-$3-$4");
                default:
                    return strVal;
            }
        }



        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var temp = value.ToString();
            switch (temp.Length)
            {
                case 7:
                    return Regex.Replace(temp, @"(\d{3})(\d{4})", "$1-$2");
                case 10:
                    return Regex.Replace(temp, @"(\d{3})(\d{3})(\d{4})", "($1) $2-$3");
                case 11:
                    return Regex.Replace(temp, @"(\d{1})(\d{3})(\d{3})(\d{4})", "$1-$2-$3-$4");
                default:
                    return temp;
            }
        }

        

    }

    public class SelectedBarcodeItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BarcodeItem)
                return value;
            return null;
        }
    }
}
