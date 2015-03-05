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
    public class OtherDataGridVm : ViewModel
    {

        public static readonly Guid Token = Guid.NewGuid();         //So others know messages came from this instance

        //private EnumsAndLists myEnumsLists = new EnumsAndLists();   //Various lists and enums used by the application

        //List for sources of ComboBoxes
        private readonly ComboBoxListValues _bindingComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _statusComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _conditionComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _subjectComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _consignorNameComboValues = new ComboBoxListValues();

        public OtherDataGridVm()
        {
            //Initialize collections
            DataGridOthers = new TrulyObservableCollection<OtherItem>();
            _bindingComboValues.InitializeComboBox(EnumsAndLists.Bindings);
            _statusComboValues.InitializeComboBox(EnumsAndLists.Statuses);
            _conditionComboValues.InitializeComboBox(EnumsAndLists.Conditions);
            _subjectComboValues.InitializeComboBox(EnumsAndLists.Subjects);
            
            var controller = new ConsignorController();
            _consignorNameComboValues.InitializeComboBox(controller.GetConsignorNames());
            

            //Register for messages to listen to
            Messenger.Default.Register<PropertySetter>(this, EditItemVm.Token, msg => SetDataGridOthers(msg.PropertyName, msg.PropertyValue));

            //Fill grid with all others on record
            InitializeDataGrid();

        }

        /// <summary>
        /// Gets the collection of OtherItem entities.
        /// </summary>
        private TrulyObservableCollection<OtherItem> _dataGridOthers;
        public TrulyObservableCollection<OtherItem> DataGridOthers
        {
            get { return _dataGridOthers; }
            set
            {
                _dataGridOthers = value;
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
        private OtherItem _selectedOtherItem;
        public OtherItem SelectedOtherItem
        {
            get { return _selectedOtherItem; }
            set
            {
                _selectedOtherItem = value;
                OnSelected();
            }
        }

        private void OnSelected()
        {
            if (SelectedOtherItem != null)
                Messenger.Default.Send(new PropertySetter("OtherImage", SelectedOtherItem.OtherImage), Token);
        }

        public void SetDataGridOthers(string name, object others)
        {
            if (name == "DataGridOthers")
                DataGridOthers = (TrulyObservableCollection<OtherItem>)(others);
        }


        /// <summary>
        /// Gets the command that allows a cell to be updated.
        /// </summary>
        public RelayCommand<DataGridCellEditEndingEventArgs> CellEditEndingCommand
        {
            get
            {
                return new RelayCommand<DataGridCellEditEndingEventArgs>(UpdateOtherItem);
            }
        }

        /// <summary>
        /// Gets the command that allows a row to be deleted.
        /// </summary>
        public RelayCommand DeleteSelectedCommand
        {
            get
            {
                return new RelayCommand(DeleteOtherItem);
            }
        }

        private void DeleteOtherItem()
        {

            //Warn user
            if (MessageBox.Show("Deletions are permanent. Are you sure you want to delete this item? ", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            //Quit if Selected is null
            if (null == SelectedOtherItem) return;

            //Find in grid
            var otherItem = DataGridOthers.FirstOrDefault(bi => bi.ItemId == SelectedOtherItem.ItemId);

            //Quit if not found in grid
            if (otherItem == null) return;

            //Copy grid to temp
            var tempGames = DataGridOthers;

            //Get the index and adjust if it is the last item on the grid
            var gridIndex = tempGames.IndexOf(otherItem);
            if ((gridIndex + 1) == tempGames.Count)
                gridIndex--;

            //Remove from grid
            tempGames.Remove(otherItem);
            DataGridOthers = tempGames;

            //Select new item on grid
            SelectedOtherItem = DataGridOthers.ElementAtOrDefault(gridIndex);

            //Delete from db 
            var itemDeleteVisitor = new ItemDeleteVisitor();
            otherItem.Accept(itemDeleteVisitor);


        }


        private void UpdateOtherItem(DataGridCellEditEndingEventArgs e)
        {

            var text = WpfHelpers.GetTextFromCellEditingEventArgs(e);

            //Get the Column header
            var column = e.Column.Header.ToString();

            //Add info to new OtherItem
            var otherItem = e.Row.Item as OtherItem;
            var success = (otherItem != null) && otherItem.SetProperty(column, text);

            if (IsValid(otherItem) && success)
            {

                switch (column)
                {
                    case "Title":
                    case "Manufacturer":
                    case "EAN":
                        {
                            var itemUpdateVisitor = new ItemUpdateVisitor();
                            otherItem.Accept(itemUpdateVisitor);
                            break;
                        }
                    case "Consignor Name":
                    case "Status":
                    case "Subject":
                    case "ListedDate":
                    case "ListedPrice":
                    case "Condition":
                    case "Description":
                        {
                            var itemTypeUpdateVisitor = new ItemTypeUpdateVisitor();
                            otherItem.Accept(itemTypeUpdateVisitor);
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
            {
                items = new ObservableCollection<Item>(icontroller.QueryItemsThatAreOthers());
            }
            var ocontroller = new OtherController();
            {
                ObservableCollection<Other> others = new ObservableCollection<Other>(ocontroller.GetAllItems());

                DataGridOthers = MergeOtherItems(others, items);
            }
        }

        //Make a collection of OtherItems with info from both the Item and Other Tables
        private TrulyObservableCollection<OtherItem> MergeOtherItems(ObservableCollection<Other> others, ObservableCollection<Item> items)
        {
            //Temp Collection Definition
            TrulyObservableCollection<OtherItem> tempOtherItems = new TrulyObservableCollection<OtherItem>();

            //Clear DataGridOthers
            DataGridOthers.Clear();

            //Make the collection
            IEnumerator<Item> i = items.GetEnumerator();
            while (i.MoveNext())
            {
                //Get the other that corresponds to the current item
                Other currentOther = others.FirstOrDefault(b => b.Id == i.Current.OtherId);
                //Create an instance of a OtherItem with the current other and item info
                var currentOtherItem = new OtherItem(currentOther, i.Current);
                //Add the new instance to the collection
                tempOtherItems.Add(currentOtherItem);
            }

            return tempOtherItems;
        }

        /// <summary>
        /// Gets a value indicating whether the view model is valid.
        /// </summary>
        public bool IsValid(OtherItem bi)
        {
            if (bi != null)
            {
                return !String.IsNullOrWhiteSpace(bi.Title) &&
                        !String.IsNullOrWhiteSpace(bi.ItemStatus) &&
                        !Double.IsNaN(bi.ListedPrice) &&
                        !(bi.ListedPrice < ConfigSettings.MIN_LISTED_PRICE) &&
                        !String.IsNullOrWhiteSpace(bi.Condition) &&
                        !String.IsNullOrEmpty(bi.ConsignorName);
            }
            return false;
        }
    }
}