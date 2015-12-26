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
using Inventory.ViewModels.AddEditConsignor.ViewModels;
using Inventory.ViewModels.AddItem.ViewModels;
using Inventory.ViewModels.AddMember.ViewModels;
using Inventory.ViewModels.ConsignorItems.ViewModels;
using Inventory.ViewModels.EditItem.ViewModels;
using Inventory.ViewModels.PrintBarcodesView.ViewModels;
using Inventory.ViewModels.RenewMember.ViewModels;
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
            SimpleIoc.Default.Register<AddGameVm>();
            SimpleIoc.Default.Register<AddOtherVm>();
            SimpleIoc.Default.Register<AddTeachingAideVm>();
            SimpleIoc.Default.Register<AddVideoVm>();
            SimpleIoc.Default.Register<EditItemVm>();
            SimpleIoc.Default.Register<BookDataGridVm>();
            SimpleIoc.Default.Register<GameDataGridVm>();
            SimpleIoc.Default.Register<OtherDataGridVm>();
            SimpleIoc.Default.Register<TeachingAideDataGridVm>();
            SimpleIoc.Default.Register<VideoDataGridVm>();
            SimpleIoc.Default.Register<AddConsignorVm>();
            SimpleIoc.Default.Register<AddMemberVm>();
            SimpleIoc.Default.Register<ConsignorItemsVm>();
            SimpleIoc.Default.Register<RenewMemberVm>();
            SimpleIoc.Default.Register<PrintBarcodesVm>();
            SimpleIoc.Default.Register<AddItemBarcodesVm>();
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

        public AddConsignorVm AddConsignorView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddConsignorVm>();
            }
        }

        public AddMemberVm AddMemberView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddMemberVm>();
            }
        }

        public ConsignorItemsVm ConsignorItemsView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ConsignorItemsVm>();
            }
        }

        public EditItemVm EditItemView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EditItemVm>();
            }
        }

        public PrintBarcodesVm PrintBarcodesView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PrintBarcodesVm>();
            }
        }

        public RenewMemberVm RenewMemberView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RenewMemberVm>();
            }
        }

        public AddGameVm AddGameView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddGameVm>();
            }
        }

        public AddOtherVm AddOtherView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddOtherVm>();
            }
        }

        public AddTeachingAideVm AddTeachingAideView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddTeachingAideVm>();
            }
        }

        public AddVideoVm AddVideoView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddVideoVm>();
            }
        }

        public AddItemBarcodesVm AddItemBarcodesView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddItemBarcodesVm>();
            }
        }

        public BookDataGridVm BookDataGridView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<BookDataGridVm>();
            }
        }

        public GameDataGridVm GameDataGridView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<GameDataGridVm>();
            }
        }

        public OtherDataGridVm OtherDataGridView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<OtherDataGridVm>();
            }
        }

        public TeachingAideDataGridVm TeachingAideDataGridView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TeachingAideDataGridVm>();
            }
        }

        public VideoDataGridVm VideoDataGridView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<VideoDataGridVm>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}