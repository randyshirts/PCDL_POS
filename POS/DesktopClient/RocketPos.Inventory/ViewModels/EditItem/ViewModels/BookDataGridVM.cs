// -----------------------------------------------------------------------------
//  <copyright file="CustomerViewModel.cs" company="DCOM Engineering, LLC">
//      Copyright (c) DCOM Engineering, LLC.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DataModel.Data.ApplicationLayer.WpfControllers;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.TransactionalLayer.Repositories;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Inventory.Controller.Elements.ItemElements;
using Inventory.Controller.Visitors.ItemVisitors;
using RocketPos.Common.Foundation;
using RocketPos.Common.Helpers;
using RocketPos.Data.TransactionalLayer;

namespace Inventory.ViewModels.EditItem.ViewModels
{
    /// <summary>
    /// A View-Model that represents a customer and its state information.
    /// </summary>
    public class BookDataGridVm : ViewModel
    {

        public static readonly Guid Token = Guid.NewGuid();         //So others know messages came from this instance

        //private EnumsAndLists myEnumsLists = new EnumsAndLists();   //Various lists and enums used by the application

        //List for sources of ComboBoxes
        private readonly ComboBoxListValues _bindingComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _statusComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _conditionComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _subjectComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _consignorNameComboValues = new ComboBoxListValues();

        public BookDataGridVm()
        {
            //Initialize collections
            DataGridBooks = new TrulyObservableCollection<BookItem>();
            _bindingComboValues.InitializeComboBox(EnumsAndLists.Bindings);
            _statusComboValues.InitializeComboBox(EnumsAndLists.Statuses);
            _conditionComboValues.InitializeComboBox(EnumsAndLists.Conditions);
            _subjectComboValues.InitializeComboBox(EnumsAndLists.Subjects);
            
            var controller = new ConsignorController();
            _consignorNameComboValues.InitializeComboBox(controller.GetConsignorNames());
            

            //Register for messages to listen to
            Messenger.Default.Register<PropertySetter>(this, EditItemVm.Token, msg => SetDataGridBooks(msg.PropertyName, msg.PropertyValue));

            //Fill grid with all books on record
            InitializeDataGrid();

        }

        /// <summary>
        /// Gets the collection of BookItem entities.
        /// </summary>
        private TrulyObservableCollection<BookItem> _dataGridBooks;
        public TrulyObservableCollection<BookItem> DataGridBooks
        {
            get { return _dataGridBooks; }
            set
            {
                _dataGridBooks = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the bindingComboValues list.
        /// </summary>
        public List<ComboBoxListValues> BindingComboValues
        {
            get { return _bindingComboValues.ComboValues; }
        }

        /// <summary>
        /// Gets the statusComboValues list.
        /// </summary>
        public List<ComboBoxListValues> StatusComboValues
        {
            get { return _statusComboValues.ComboValues; }
        }

        /// <summary>
        /// Gets the conditionComboValues list.
        /// </summary>
        public List<ComboBoxListValues> ConditionComboValues
        {
            get { return _conditionComboValues.ComboValues; }
        }

        /// <summary>
        /// Gets the subjectComboValues list.
        /// </summary>
        public List<ComboBoxListValues> SubjectComboValues
        {
            get { return _subjectComboValues.ComboValues; }
        }

        /// <summary>
        /// Gets the ConsignorNameComboValues list.
        /// </summary>
        public List<ComboBoxListValues> ConsignorNameComboValues
        {
            get { return _consignorNameComboValues.ComboValues; }
        }

        /// <summary>
        /// Gets the selectedItem row from the datagrid.
        /// </summary>
        private BookItem _selectedBookItem;
        public BookItem SelectedBookItem
        {
            get { return _selectedBookItem; }
            set
            {
                _selectedBookItem = value;
                OnSelected();
            }
        }

        private void OnSelected()
        {
            if (SelectedBookItem != null)
                Messenger.Default.Send(new PropertySetter("BookImage", SelectedBookItem.BookImage), Token);
        }

        public void SetDataGridBooks(string name, object books)
        {
            if (name == "DataGridBooks")
                DataGridBooks = (TrulyObservableCollection<BookItem>)(books);
        }


        /// <summary>
        /// Gets the command that allows a cell to be updated.
        /// </summary>
        public RelayCommand<DataGridCellEditEndingEventArgs> CellEditEndingCommand
        {
            get
            {
                return new RelayCommand<DataGridCellEditEndingEventArgs>(UpdateBookItem);
            }
        }

        /// <summary>
        /// Gets the command that allows a row to be deleted.
        /// </summary>
        public RelayCommand DeleteSelectedCommand
        {
            get
            {
                return new RelayCommand(DeleteBookItem);
            }
        }

        private void DeleteBookItem()
        {
            //Warn user
            if (MessageBox.Show("Deletions are permanent. Are you sure you want to delete this item? ", "Confirm Delete",
                    MessageBoxButton.YesNo) == MessageBoxResult.No) return;

            if (null == SelectedBookItem) return;

            //Find record of selected bookItem in Item DB table
            var bookItem = DataGridBooks.FirstOrDefault(bi => bi.ItemId == SelectedBookItem.ItemId);

            //Quit if Id is not found in Items 
            if (bookItem == null) return;

            //Copy datagrid to temp
            var tempBooks = DataGridBooks;

            //Get the index of the current item
            var gridIndex = tempBooks.IndexOf(bookItem);

            //Adjust index if index is last index in grid
            if ((gridIndex + 1) == tempBooks.Count)
                gridIndex--;

            //Remove from temp
            tempBooks.Remove(bookItem);

            //Assign temp to datagrid
            DataGridBooks = tempBooks;

            //Assign new index
            SelectedBookItem = DataGridBooks.ElementAtOrDefault(gridIndex);

            //Delete from db table
            var itemDeleteVisitor = new ItemDeleteVisitor();
            bookItem.Accept(itemDeleteVisitor);

        }


        private void UpdateBookItem(DataGridCellEditEndingEventArgs e)
        {



            var text = WpfHelpers.GetTextFromCellEditingEventArgs(e);

            //Get the Column header as string
            var column = e.Column.Header.ToString();

            //Add info to new BookItem
            var bookItem = e.Row.Item as BookItem;
            var success = (bookItem != null) && (bookItem.SetProperty(column, text));

            if (IsValid(bookItem) && success)
            {

                switch (column)
                {
                    case "Title":
                    case "Author":
                    case "Binding":
                    case "# Of Pages":
                    case "PublicationDate":
                    case "TradeInValue":
                    case "ISBN":
                        {
                            var itemUpdateVisitor = new ItemUpdateVisitor();
                            bookItem.Accept(itemUpdateVisitor);
                            break;
                        }
                    case "Consignor Name":
                    case "Status":
                    case "Subject":
                    case "ListedDate":
                    case "ListedPrice":
                    case "Condition":
                    case "Discount":
                    case "Description":
                        {
                            var itemTypeUpdateVisitor = new ItemTypeUpdateVisitor();
                            bookItem.Accept(itemTypeUpdateVisitor);
                            break;
                        }
                }
            }
            else
            {
                e.Cancel = true;

                MessageBox.Show("Input Error - '" + text + "' - Try again", "Error");
            }

        }


        private void InitializeDataGrid()
        {
            ObservableCollection<Item> items;

            var icontroller = new ItemController();
            items = new ObservableCollection<Item>(icontroller.QueryItemsThatAreBooks());

            var bcontroller = new BookController();
            {
                ObservableCollection<Book> books = new ObservableCollection<Book>(bcontroller.GetAllItems());
                DataGridBooks = MergeBookItems(books, items);
            }
        }

        //Make a collection of BookItems with info from both the Item and Book Tables
        private TrulyObservableCollection<BookItem> MergeBookItems(ObservableCollection<Book> books, ObservableCollection<Item> items)
        {
            //Temp Collection Definition
            TrulyObservableCollection<BookItem> tempBookItems = new TrulyObservableCollection<BookItem>();

            //Clear DataGridBooks
            DataGridBooks.Clear();

            //Make the collection
            IEnumerator<Item> i = items.GetEnumerator();
            while (i.MoveNext())
            {
                //Get the book that corresponds to the current item
                Book currentBook = books.FirstOrDefault(b => b.Id == i.Current.Id);
                //Create an instance of a BookItem with the current book and item info
                var currentBookItem = new BookItem(currentBook, i.Current);
                //Add the new instance to the collection
                tempBookItems.Add(currentBookItem);
            }

            return tempBookItems;
        }

        /// <summary>
        /// Gets a value indicating whether the view model is valid.
        /// </summary>
        public bool IsValid(BookItem bi)
        {
            if (bi != null)
            {
                return !String.IsNullOrWhiteSpace(bi.Title) &&
                        !String.IsNullOrWhiteSpace(bi.ItemStatus) &&
                        !String.IsNullOrWhiteSpace(bi.Isbn) &&
                        !Double.IsNaN(bi.ListedPrice) &&
                        !(bi.ListedPrice < ConfigSettings.MIN_LISTED_PRICE) &&
                        !String.IsNullOrWhiteSpace(bi.Condition) &&
                        !String.IsNullOrEmpty(bi.ConsignorName);

            }
            else
                return false;
        }

    }
}