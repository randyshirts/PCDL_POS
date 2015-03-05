using POS.ViewModels.ItemSale.ViewModel;
using RocketPos.Common.Foundation;

namespace POS.ViewModels.PosMainView.ViewModel
{
    public class PosMainViewVm : RocketPos.Common.Foundation.ViewModel
    {
        public PosMainViewVm()
        {
            InitializeChildViews();

            //Register for messages
            //Messenger.Default.Register<SwitchView>(this, AddItemVm.Token, msg => ViewSelector(msg.ViewModel));
            //Messenger.Default.Register<SwitchView>(this, EditItemVm.Token, msg => ViewSelector(msg.ViewModel));
        }

        //private void ViewSelector(RocketPos.Common.Foundation.ViewModel newViewModel)
        //{
        //    if ((newViewModel.GetType() == (typeof(AddItemVm))) ||
        //        (newViewModel.GetType() == (typeof(EditItemVm))) ||
        //        (newViewModel.GetType() == (typeof(AddConsignorVm))) ||
        //        (newViewModel.GetType() == (typeof(ConsignorItemsVm))))
        //    {
        //        LoadAddNewMainView(newViewModel);
        //    }
        //    else if((newViewModel.GetType() == (typeof(BookDataGridVm))) ||
        //            (newViewModel.GetType() == (typeof(GameDataGridVm))) ||
        //            (newViewModel.GetType() == (typeof(OtherDataGridVm))) ||
        //            (newViewModel.GetType() == (typeof(TeachingAideDataGridVm))) ||
        //            (newViewModel.GetType() == (typeof(VideoDataGridVm))))
        //    {
        //        BottomLeftView = newViewModel;
        //        BottomLeftColSpan = "3";
        //        TopLeftColSpan = "2";
        //    }
        //    else if ((newViewModel.GetType() == (typeof(AddBookVm))) ||
        //       (newViewModel.GetType() == (typeof(AddGameVm))) ||
        //       (newViewModel.GetType() == (typeof(AddOtherVm))) ||
        //       (newViewModel.GetType() == (typeof(AddVideoVm))) ||
        //       (newViewModel.GetType() == (typeof(AddTeachingAideVm))))
        //    {
        //        TopRightView = newViewModel;  
        //    }
        //    else if(newViewModel.GetType() == (typeof(AddItemBarcodesVm)))
        //    {
        //        BottomLeftView = newViewModel;
        //        TopLeftColSpan = "1";
        //        BottomLeftColSpan = "1";
        //    }


        //}

        private void LoadAddNewMainView(RocketPos.Common.Foundation.ViewModel newViewModel)
        {
            TopLeftView = newViewModel;
            BottomLeftView = null;
            TopRightView = null;

            if (newViewModel.GetType() == (typeof (ItemSaleVm)))
            {
                TopLeftColSpan = "2";
                TopLeftRowSpan = "2";
            }
            //if (newViewModel.GetType() == (typeof(EditItemVm)))
            //{
            //    TopLeftColSpan = "1";
            //    TopLeftRowSpan = "1";
            //}

            //if (newViewModel.GetType() == (typeof(AddConsignorVm)))
            //{
            //    TopLeftColSpan = "2";
            //    TopLeftRowSpan = "2";
            //}

            //if (newViewModel.GetType() == (typeof(ConsignorItemsVm)))
            //{
            //    TopLeftColSpan = "2";
            //    TopLeftRowSpan = "2";
            //}

            //if (newViewModel.GetType() == (typeof(PrintBarcodesVm)))
            //{
            //    TopLeftColSpan = "2";
            //    TopLeftRowSpan = "2";
            //}


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

        private RocketPos.Common.Foundation.ViewModel _topLeftView;

        public RocketPos.Common.Foundation.ViewModel TopLeftView
        {
            get { return _topLeftView; }
            set
            {
                if (_topLeftView == value) return;
                _topLeftView = value;
                OnPropertyChanged();
            }
        }

        private RocketPos.Common.Foundation.ViewModel _bottomLeftView;

        public RocketPos.Common.Foundation.ViewModel BottomLeftView
        {
            get { return _bottomLeftView; }
            set
            {
                if (_bottomLeftView == value) return;
                _bottomLeftView = value;
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }

        private RocketPos.Common.Foundation.ViewModel _topRightView;

        public RocketPos.Common.Foundation.ViewModel TopRightView
        {
            get { return _topRightView; }
            set
            {
                if (_topRightView == value) return;
                _topRightView = value;
                OnPropertyChanged();
            }
        }

        public ActionCommand ItemSaleButtonCommand
        {
            get
            {
                var itemSaleVm = new ItemSaleVm();
                return new ActionCommand(p => LoadAddNewMainView(itemSaleVm));
            }

        }

        //public ActionCommand OnlineSaleButtonCommand
        //{
        //    get
        //    {
        //        var myEditItemVM = new EditItemVm();
        //        return new ActionCommand(p => this.LoadAddNewMainView(myEditItemVM));
        //    }

        //}

        //public ActionCommand ClassSaleButtonCommand
        //{
        //    get
        //    {
        //        var myAddConsignorVM = new AddConsignorVm();
        //        return new ActionCommand(p => this.LoadAddNewMainView(myAddConsignorVM));
        //    }

        //}

        //public ActionCommand RoomSaleButtonCommand
        //{
        //    get
        //    {
        //        var myConsignorItemsVM = new ConsignorItemsVm();
        //        return new ActionCommand(p => this.LoadAddNewMainView(myConsignorItemsVM));
        //    }

        //}

        //public ActionCommand ItemPmtButtonCommand
        //{
        //    get
        //    {
        //        var myPrintBarcodesVM = new PrintBarcodesVm();
        //        return new ActionCommand(p => this.LoadAddNewMainView(myPrintBarcodesVM));
        //    }

        //}

        //public ActionCommand InstructorPmtButtonCommand
        //{
        //    get
        //    {
        //        var myPrintBarcodesVM = new PrintBarcodesVm();
        //        return new ActionCommand(p => this.LoadAddNewMainView(myPrintBarcodesVM));
        //    }

        //}

        //public ActionCommand DepositPmtButtonCommand
        //{
        //    get
        //    {
        //        var myPrintBarcodesVM = new PrintBarcodesVm();
        //        return new ActionCommand(p => this.LoadAddNewMainView(myPrintBarcodesVM));
        //    }

        //}

        //public ActionCommand OtherPmtButtonCommand
        //{
        //    get
        //    {
        //        var myPrintBarcodesVM = new PrintBarcodesVm();
        //        return new ActionCommand(p => this.LoadAddNewMainView(myPrintBarcodesVM));
        //    }

        //}
    }
}