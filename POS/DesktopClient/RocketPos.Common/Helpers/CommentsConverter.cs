using System;
using System.Windows.Data;
using System.Globalization;

namespace RocketPos.Common.Helpers
{
	class CommentsConverter : IValueConverter
	{
		/// <summary>
		/// An example for a converter that will return "Warning: Wild Unicorns found on premise." 
		/// if the word "unicorn" exists in the string.
		/// </summary>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var inputString = value as string;

			if (String.IsNullOrEmpty(inputString)) 
				return "The cake is a lie.";

			if (inputString.Contains("unicorn"))
				return "Warning: Wild Unicorns found on premise.";
			
			return "This following text is Unicorn free: [" + inputString + "]";
		}


		/// <summary>
		/// It it makes sense to convert back, IMPLEMENT the following method as well !!!!
		/// </summary>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

	}
}