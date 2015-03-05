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
using Microsoft.Practices.ServiceLocation;
using POS.ViewModels.ItemSale.ViewModel;

namespace POS.ViewModels.PosMainView.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class PosViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public PosViewModelLocator()
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

            SimpleIoc.Default.Register<PosMainViewVm>();
            SimpleIoc.Default.Register<ItemSaleVm>();
            //SimpleIoc.Default.Register<AddBookVm>();
        
        }

        public PosMainViewVm PosMainView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PosMainViewVm>();
            }
        }

        public ItemSaleVm InStorePurchaseView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ItemSaleVm>();
            }
        }

        //public AddItemVm AddItemView
        //{
        //    get
        //    {
        //        return ServiceLocator.Current.GetInstance<AddItemVm>();
        //    }
        //}
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}