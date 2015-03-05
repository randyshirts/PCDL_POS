using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Reflection;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;

namespace RocketPos.Common.Helpers
{

    /// This class provides us with an object to fill a ComboBox with
    /// that can be bound to string fields in the binding object.
    public class ComboBoxListValues
    {
        //Property that combobox will bind to in xaml
        public string ValueString { get; set; }
        //List of values
        public List<ComboBoxListValues> ComboValues = new List<ComboBoxListValues>();

        //Initializes combobox with list of strings
        public void InitializeComboBox(List<string> initializer)
        {
            ComboValues.Clear();

            foreach (var listIdx in initializer)
            {
                ComboValues.Add(new ComboBoxListValues { ValueString = listIdx });
            }
        }
    }

    public class KeyUpWithArgsBehavior : Behavior<FrameworkElement>
    {
        public ICommand KeyUpCommand
        {
            get { return (ICommand)GetValue(KeyUpCommandProperty); }
            set { SetValue(KeyUpCommandProperty, value); }
        }

        public static readonly DependencyProperty KeyUpCommandProperty =
            DependencyProperty.Register("KeyUpCommand", typeof(ICommand), typeof(KeyUpWithArgsBehavior), new UIPropertyMetadata(null));


        protected override void OnAttached()
        {
            AssociatedObject.KeyUp += AssociatedObjectKeyUp;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.KeyUp -= AssociatedObjectKeyUp;
            base.OnDetaching();
        }

        private void AssociatedObjectKeyUp(object sender, KeyEventArgs e)
        {
            if (KeyUpCommand != null)
            {
                KeyUpCommand.Execute(e.Key);
            }
        }
    }

    /// <summary>
    /// Behavior that will connect an UI event to a viewmodel Command,
    /// allowing the event arguments to be passed as the CommandParameter.
    /// </summary>
    public class EventToCommandBehavior : Behavior<FrameworkElement>
    {
        private Delegate _handler;
        private EventInfo _oldEvent;

        // Event
        public string Event { get { return (string)GetValue(EventProperty); } set { SetValue(EventProperty, value); } }
        public static readonly DependencyProperty EventProperty = DependencyProperty.Register("Event", typeof(string), typeof(EventToCommandBehavior), new PropertyMetadata(null, OnEventChanged));

        // Command
        public ICommand Command { get { return (ICommand)GetValue(CommandProperty); } set { SetValue(CommandProperty, value); } }
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(EventToCommandBehavior), new PropertyMetadata(null));

        // PassArguments (default: false)
        public bool PassArguments { get { return (bool)GetValue(PassArgumentsProperty); } set { SetValue(PassArgumentsProperty, value); } }
        public static readonly DependencyProperty PassArgumentsProperty = DependencyProperty.Register("PassArguments", typeof(bool), typeof(EventToCommandBehavior), new PropertyMetadata(false));


        private static void OnEventChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var beh = (EventToCommandBehavior)d;

            if (beh.AssociatedObject != null) // is not yet attached at initial load
                beh.AttachHandler((string)e.NewValue);
        }

        protected override void OnAttached()
        {
            AttachHandler(Event); // initial set
        }

        /// <summary>
        /// Attaches the handler to the event
        /// </summary>
        private void AttachHandler(string eventName)
        {
            // detach old event
            if (_oldEvent != null)
                _oldEvent.RemoveEventHandler(AssociatedObject, _handler);

            // attach new event
            if (!string.IsNullOrEmpty(eventName))
            {
                EventInfo ei = AssociatedObject.GetType().GetEvent(eventName);
                if (ei != null)
                {
                    MethodInfo mi = GetType().GetMethod("ExecuteCommand", BindingFlags.Instance | BindingFlags.NonPublic);
                    _handler = Delegate.CreateDelegate(ei.EventHandlerType, this, mi);
                    ei.AddEventHandler(AssociatedObject, _handler);
                    _oldEvent = ei; // store to detach in case the Event property changes
                }
                else
                    throw new ArgumentException(string.Format("The event '{0}' was not found on type '{1}'", eventName, this.AssociatedObject.GetType().Name));
            }
        }

        /// <summary>
        /// Executes the Command
        /// </summary>
        private void ExecuteCommand(object sender, EventArgs e)
        {
            object parameter = PassArguments ? e : null;
            if (Command != null)
            {
                if (Command.CanExecute(parameter))
                    Command.Execute(parameter);
            }
        }
    }

    public static class FocusExtension
    {
        public static bool GetIsFocused(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsFocusedProperty);
        }


        public static void SetIsFocused(DependencyObject obj, bool value)
        {
            obj.SetValue(IsFocusedProperty, value);
        }


        public static readonly DependencyProperty IsFocusedProperty =
            DependencyProperty.RegisterAttached(
             "IsFocused", typeof(bool), typeof(FocusExtension),
             new UIPropertyMetadata(false, OnIsFocusedPropertyChanged));


        private static void OnIsFocusedPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var uie = (UIElement)d;
            if ((bool)e.NewValue)
            {
                uie.Focus(); // Don't care about false values.
                if (uie.GetType() == typeof (TextBox))
                {
                    var tb = uie as TextBox;
                    tb.SelectAll();
                }
            }
        }
    }

    public static class WindowService 
    {
        public static void ShowWindow(object viewModel)
        {
            var win = new Window {Content = viewModel};
            win.Show();
        }

        public static void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
    }

    public static class WpfHelpers
    {
        public static string GetTextFromCellEditingEventArgs(DataGridCellEditEndingEventArgs e)
        {
            if (e.EditingElement.GetType() == typeof(TextBox))
            {
                //Get the TextBox that was edited.
                var element = e.EditingElement as TextBox;
                return element != null ? element.Text : null;
            }
            if (e.EditingElement.GetType() == typeof(ComboBox))
            {
                //Get the TextBox that was edited.
                var element = e.EditingElement as ComboBox;
                return element != null ? element.Text : null;
            }
            if (e.EditingElement.GetType() == typeof(CheckBox))
            {
                //Get the TextBox that was edited.
                var element = e.EditingElement as CheckBox; 
                return element != null ? element.IsChecked.ToString() : null;
            }
            if (e.EditingElement.GetType() == typeof(ContentPresenter))
            {
                //Get the DatePicker that was edited.
                var element = e.EditingElement as ContentPresenter;
                
                //Quit if EditingElement is null
                if (element == null) return null;

                var editingTemplate = element.ContentTemplate;
                var column = e.Column.Header.ToString();

                switch (column)
                {
                    case "DateAdded":
                    {
                        var dp = editingTemplate.FindName("DateAddedPicker", element)
                            as DatePicker;
                        return dp != null ? dp.Text : null;
                    }
                    case "Date Listed":
                    {
                        var dp = editingTemplate.FindName("DateListedPicker", element)
                            as DatePicker;
                        return dp != null ? dp.Text : null;
                    }
                    case "Date Sold":
                    {
                        var dp = editingTemplate.FindName("DateSoldPicker", element)
                            as DatePicker;
                        return dp != null ? dp.Text : null;
                    }
                    case "DateListed":
                    {
                        var dp = editingTemplate.FindName("DateListedPicker", element)
                            as DatePicker;
                        return dp != null ? dp.Text : null;
                    }
                    case "Cell Phone":
                    {
                        var tb = editingTemplate.FindName("CellPhoneNumberTextBox", element)
                            as TextBox;
                        return tb != null ? tb.Text : null;
                    }
                    case "Home Phone":
                    {
                        var tb = editingTemplate.FindName("HomePhoneNumberTextBox", element)
                            as TextBox;
                        return tb != null ? tb.Text : null;
                    }
                    case "Alt Phone":
                    {
                        var tb = editingTemplate.FindName("AltPhoneNumberTextBox", element)
                            as TextBox;
                        return tb != null ? tb.Text : null;
                    }
                    default:
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        public static bool UpdateStatus(string value, string status)
        {
            switch (value)
            {
                case "Sold":
                    {
                        switch (status)
                        {
                            case "Sold":
                                {
                                    return false;
                                }
                            case "Paid":
                                {
                                    MessageBox.Show("Cannot change - delete a Consignor Payment instead");
                                    return false;
                                }
                            case "Pending":
                                {                                   
                                    return true;
                                }
                            case "Not yet arrived in store":
                            case "Arrived but not shelved":
                            case "Shelved":
                            case "Lost":
                                {
                                    MessageBox.Show("Cannot change - add a transaction instead");
                                    return false;
                                }
                        }
                        return false;
                    }
                case "Paid":
                    {
                        MessageBox.Show("Cannot change - submit a Consignor Payment instead");
                        return false;
                    }
                case "Pending":
                    {
                        switch (status)
                        {
                            case "Sold":
                                {
                                    return true;
                                }
                            case "Paid":
                                {
                                    MessageBox.Show("Cannot change - delete a Consignor Payment instead");
                                    return false;
                                }
                            case "Pending":
                                {
                                    return false;
                                }
                            case "Not yet arrived in store":
                            case "Arrived but not shelved":
                            case "Shelved":
                            case "Lost":
                                {
                                    MessageBox.Show("Cannot change - add a transaction instead");
                                    return false;
                                }
                        }
                        return false;
                    }
                case "Not yet arrived in store":
                case "Arrived but not shelved":
                case "Shelved":
                case "Lost":
                    {
                        switch (status)
                        {
                            case "Sold":
                                {
                                    MessageBox.Show("Cannot change - delete a Transaction instead");
                                    return false;
                                }
                            case "Paid":
                                {
                                    MessageBox.Show("Cannot change - delete a Consignor Payment instead");
                                    return false;
                                }
                            case "Pending":
                                {
                                    return false;
                                }
                            case "Not yet arrived in store":
                            case "Arrived but not shelved":
                            case "Shelved":
                            case "Lost":
                                {
                                    return true;
                                }
                        }
                        return false;
                    }

            }
            return false;
        }

        public static MemoryStream FlowDocumentToXps(FlowDocument flowDocument)
        {
            var stream = new MemoryStream();
            using (var package = Package.Open(stream, FileMode.Create, FileAccess.ReadWrite))
            {
                using (var xpsDoc = new XpsDocument(package, CompressionOption.Maximum))
                {
                    var rsm = new XpsSerializationManager(new XpsPackagingPolicy(xpsDoc), false);
                    var paginator = ((IDocumentPaginatorSource)flowDocument).DocumentPaginator;
                    //paginator.PageSize = new Size(width, height);
                    rsm.SaveAsXaml(paginator);
                    rsm.Commit();
                }
            }
            stream.Position = 0;
            Console.WriteLine(stream.Length);
            Console.WriteLine(stream.Position);
            return stream;
        }

        public static MemoryStream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }

}
