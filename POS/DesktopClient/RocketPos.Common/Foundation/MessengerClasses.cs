using GalaSoft.MvvmLight.Messaging;


namespace RocketPos.Common.Foundation
{
	/*
	 * If you're using the MVVM Messenger, this classes can be helpful in order to send 
	 * specific types of messages, with specific results, like changing the status
	 * bar in your Main window from any part of the system, or switching views as response
	 * to clicking a button.
	 * 
	 * For example, using MVVM data service DI:
	 * Messenger.Default.Send(new SwitchView(new SomeViewModel(_dataService)));
	 *
	 * Or without using MVVM data service DI:
	 * Messenger.Default.Send(new SwitchView(new SomeOtherViewModel()));
	 */

	#region Set Status

	/// <summary>
	/// Global class used to set the status message. Whatever you set to catch 
	/// this message will be the one changing the status label/textbox.
	/// 
	/// This is static, so it can be called from any point in the program. It 
	/// sends a "StatusMessage" class, which is just a wrapper fro a string,
	/// but you can obviously extend this if needed.
	/// </summary>
	public static class StatusSetter
	{
		public static void SetStatus(string s)
		{
			Messenger.Default.Send(new StatusMessage(s));
		}
	}

	/// <summary>
	/// A class that holds a status message. Used in StatusSeter
	/// </summary>
	public class StatusMessage
	{
		public StatusMessage(string status)
		{
			NewStatus = status;
		}

		public string NewStatus { get; set; }
	}
	
	#endregion

    /// <summary>
    /// A class that holds a Property Name and Value. 
    /// </summary>
    public class PropertySetter
    {
        public PropertySetter(string propertyName, object propertyValue)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        public string PropertyName { get; set; }
        public object PropertyValue { get; set; }
    }

    public class PropertySetterString
    {
        public PropertySetterString(string propertyName, string propertyValue)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
    }

	#region Switch view

	/// <summary>
	/// Used as message, to switch the view to a different one. Whatever you set 
	/// to catches this message will be the one changing the views. 
	/// </summary>
	/// <remarks>
	/// NOT USED IN THIS TEMPLATE. This is just the prototype to 
	/// show you how you can easily do it.
	/// </remarks>
	public class SwitchView
	{
		public SwitchView(ViewModel viewmodel)
		{
			ViewModel = viewmodel;
		}

		public ViewModel ViewModel { get; set; }
	}

	/// <summary>
	/// Global class used to refresh the people in the datagrid.
	/// Used so you can call this from the menu, or another view model
	/// for example.
	/// </summary>
	public class RefreshPeople
	{
		public RefreshPeople(int anAmount)
		{
			PeopleToFetch = anAmount;
		}

		public int PeopleToFetch { get; set; }
	}

	#endregion
	
}
