using GalaSoft.MvvmLight.Messaging;
using Inventory.ViewModels.AddEditConsignor.ViewModels;
using Inventory.ViewModels.AddItem.ViewModels;
using Inventory.ViewModels.AddMember.ViewModels;
using Inventory.ViewModels.ConsignorItems.ViewModels;
using Inventory.ViewModels.EditItem.ViewModels;
using Inventory.ViewModels.PrintBarcodesView.ViewModels;
using Inventory.ViewModels.RenewMember.ViewModels;
using RocketPos.Common.Foundation;

namespace Inventory.ViewModels.MainView.ViewModels
{
	public class MainViewVm : ViewModel 
	{
        public MainViewVm()
		{
            InitializeChildViews();

            //Register for messages
            Messenger.Default.Register<SwitchView>(this, AddItemVm.Token, msg => ViewSelector(msg.ViewModel));
            Messenger.Default.Register<SwitchView>(this, EditItemVm.Token, msg => ViewSelector(msg.ViewModel));
		}

	    private ViewModel m_addItemVm;
        private ViewModel m_editItemVm;
        private ViewModel m_ConsignorItemsVm;
        private ViewModel m_addConsignorVm;
        private ViewModel m_printBarcodesVm;
        private ViewModel m_renewMemberVm;
        private ViewModel m_addBookVm;
        private ViewModel m_addGameVm;
        private ViewModel m_addOtherVm;
        private ViewModel m_addTeachingAideVm;
        private ViewModel m_addVideoVm;
	    private ViewModel m_addItemBarcodesVm;


        private void ViewSelector(ViewModel newViewModel)
        {
            if ((newViewModel.GetType() == (typeof(AddItemVm))) ||
                (newViewModel.GetType() == (typeof(EditItemVm))) ||
                (newViewModel.GetType() == (typeof(AddConsignorVm))) ||
                (newViewModel.GetType() == (typeof(AddMemberVm))) ||
                (newViewModel.GetType() == (typeof(RenewMemberVm))) ||
                (newViewModel.GetType() == (typeof(ConsignorItemsVm))))
            {
                LoadAddNewMainView(newViewModel);
            }
            else if((newViewModel.GetType() == (typeof(BookDataGridVm))) ||
                    (newViewModel.GetType() == (typeof(GameDataGridVm))) ||
                    (newViewModel.GetType() == (typeof(OtherDataGridVm))) ||
                    (newViewModel.GetType() == (typeof(TeachingAideDataGridVm))) ||
                    (newViewModel.GetType() == (typeof(VideoDataGridVm))))
            {
                BottomLeftView = newViewModel;
                BottomLeftColSpan = "3";
                TopLeftColSpan = "2";
            }
            else if ((newViewModel.GetType() == (typeof(AddBookVm))) ||
               (newViewModel.GetType() == (typeof(AddGameVm))) ||
               (newViewModel.GetType() == (typeof(AddOtherVm))) ||
               (newViewModel.GetType() == (typeof(AddVideoVm))) ||
               (newViewModel.GetType() == (typeof(AddTeachingAideVm))))
            {
                TopRightView = newViewModel;  
            }
            else if(newViewModel.GetType() == (typeof(AddItemBarcodesVm)))
            {
                BottomLeftView = newViewModel;
                TopLeftColSpan = "1";
                BottomLeftColSpan = "1";
            }


        }

        private void LoadAddNewMainView(ViewModel newViewModel)
        {
            TopLeftView = newViewModel;
            BottomLeftView = null;
            TopRightView = null;

            if (newViewModel.GetType() == (typeof(AddItemVm)))
            {
                TopLeftColSpan = "1";
                TopLeftRowSpan = "1";
            }
            if (newViewModel.GetType() == (typeof(EditItemVm)))
            {
                TopLeftColSpan = "1";
                TopLeftRowSpan = "1";
            }

            if (newViewModel.GetType() == (typeof(AddConsignorVm)))
            {
                TopLeftColSpan = "2";
                TopLeftRowSpan = "2";
            }

            if (newViewModel.GetType() == (typeof(AddMemberVm)))
            {
                TopLeftColSpan = "2";
                TopLeftRowSpan = "2";
            }

            if (newViewModel.GetType() == (typeof(RenewMemberVm)))
            {
                TopLeftColSpan = "2";
                TopLeftRowSpan = "2";
            }

            if (newViewModel.GetType() == (typeof(ConsignorItemsVm)))
            {
                TopLeftColSpan = "2";
                TopLeftRowSpan = "2";
            }

            if (newViewModel.GetType() == (typeof(PrintBarcodesVm)))
            {
                TopLeftColSpan = "2";
                TopLeftRowSpan = "2";
            }
            

        }

        private void InitializeChildViews()
        {
            //Assign the bottom right-hand view
            TopLeftView = null;

            TopRightView = null;

            BottomLeftView = null;

            BottomLeftColSpan = "2";
            TopLeftColSpan = "1";
            TopLeftRowSpan = "1";
            TopRightRowSpan = "2";
        }

        private ViewModel _topLeftView;
        public ViewModel TopLeftView
        {
            get { return _topLeftView; }
            set
            {
                if (_topLeftView == value) return;
                _topLeftView = value;
                OnPropertyChanged("TopLeftView");
            }
        }

        private ViewModel _bottomLeftView;
        public ViewModel BottomLeftView
        {
            get { return _bottomLeftView; }
            set
            {
                if (_bottomLeftView == value) return;
                _bottomLeftView = value;
                OnPropertyChanged("BottomLeftView");
            }
        }

        private string _bottomLeftColSpan;
        public string BottomLeftColSpan
        {
            get { return _bottomLeftColSpan; }
            set
            {
                if (_bottomLeftColSpan == value) return;
                _bottomLeftColSpan = value;
                OnPropertyChanged("BottomLeftColSpan");
            }
        }

        private string _topLeftColSpan;
        public string TopLeftColSpan
        {
            get { return _topLeftColSpan; }
            set
            {
                if (_topLeftColSpan == value) return;
                _topLeftColSpan = value;
                OnPropertyChanged("TopLeftColSpan");
            }
        }

        private string _topRightRowSpan;
        public string TopRightRowSpan
        {
            get { return _topRightRowSpan; }
            set
            {
                if (_topRightRowSpan == value) return;
                _topRightRowSpan = value;
                OnPropertyChanged("TopRightRowSpan");
            }
        }

        private string _topLeftRowSpan;
        public string TopLeftRowSpan
        {
            get { return _topLeftRowSpan; }
            set
            {
                if (_topLeftRowSpan == value) return;
                _topLeftRowSpan = value;
                OnPropertyChanged("TopLeftRowSpan");
            }
        }

        private ViewModel _topRightView;
        public ViewModel TopRightView 
        {
            get { return _topRightView;}
            set
            {
                if (_topRightView == value) return;
                _topRightView = value;
                OnPropertyChanged("TopRightView");
            }
        }

        public ActionCommand AddItemButtonCommand
        {
            get
            {
                var myAddItemVm = new AddItemVm();
                return new ActionCommand(p => LoadAddNewMainView(myAddItemVm));
            }

        }

        public ActionCommand EditItemButtonCommand
        {
            get
            {
                var myEditItemVm = new EditItemVm();
                return new ActionCommand(p => LoadAddNewMainView(myEditItemVm));
            }

        }

        public ActionCommand AddConsignorButtonCommand
        {
            get
            {
                var myAddConsignorVm = new AddConsignorVm();
                return new ActionCommand(p => LoadAddNewMainView(myAddConsignorVm));
            }

        }

        public ActionCommand AddMemberButtonCommand
        {
            get
            {
                var myAddMemberVm = new AddMemberVm();
                return new ActionCommand(p => LoadAddNewMainView(myAddMemberVm));
            }

        }

        public ActionCommand RenewMemberButtonCommand
        {
            get
            {
                var myRenewMemberVm = new RenewMemberVm();
                return new ActionCommand(p => LoadAddNewMainView(myRenewMemberVm));
            }

        }

        public ActionCommand ConsignorItemsButtonCommand
        {
            get
            {
                var myConsignorItemsVm = new ConsignorItemsVm();
                return new ActionCommand(p => LoadAddNewMainView(myConsignorItemsVm));
            }

        }

        public ActionCommand PrintBarcodesButtonCommand
        {
            get
            {
                var myPrintBarcodesVm = new PrintBarcodesVm();
                return new ActionCommand(p => LoadAddNewMainView(myPrintBarcodesVm));
            }

        }

	}
}