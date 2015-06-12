/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Inventory"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight.Ioc;
using Inventory.ViewModels.AddItem.ViewModels;
using Microsoft.Practices.ServiceLocation;

//using Microsoft.Practices.ServiceLocation;

namespace Inventory.ViewModels.MainView.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //if (ViewModelBase.IsInDesignModeStatic)
            //{
            //    // Create design time view services and models
            //    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            //}
            //else
            //{
            //    // Create run time view services and models
            //    SimpleIoc.Default.Register<IDataService, DataService>();
            //}

            SimpleIoc.Default.Register<MainViewVm>();
            SimpleIoc.Default.Register<AddItemVm>();
            SimpleIoc.Default.Register<AddBookVm>();
        
        }

        public MainViewVm MainView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewVm>();
            }
        }

        public AddBookVm AddBookView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddBookVm>();
            }
        }

        public AddItemVm AddItemView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddItemVm>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}