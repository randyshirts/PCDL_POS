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
    public class TeachingAideDataGridVm : ViewModel
    {

        public static readonly Guid Token = Guid.NewGuid();         //So others know messages came from this instance

        //private EnumsAndLists myEnumsLists = new EnumsAndLists();   //Various lists and enums used by the application

        //List for sources of ComboBoxes
        private readonly ComboBoxListValues _bindingComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _statusComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _conditionComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _subjectComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _consignorNameComboValues = new ComboBoxListValues();

        public TeachingAideDataGridVm()
        {
            //Initialize collections
            DataGridTeachingAides = new TrulyObservableCollection<TeachingAideItem>();
            _bindingComboValues.InitializeComboBox(EnumsAndLists.Bindings);
            _statusComboValues.InitializeComboBox(EnumsAndLists.Statuses);
            _conditionComboValues.InitializeComboBox(EnumsAndLists.Conditions);
            _subjectComboValues.InitializeComboBox(EnumsAndLists.Subjects);
            
            var controller = new ConsignorController();
            _consignorNameComboValues.InitializeComboBox(controller.GetConsignorNames());
            

            //Register for messages to listen to
            Messenger.Default.Register<PropertySetter>(this, EditItemVm.Token, msg => SetDataGridTeachingAides(msg.PropertyName, msg.PropertyValue));

            //Fill grid with all teachingAides on record
            InitializeDataGrid();

        }

        /// <summary>
        /// Gets the collection of TeachingAideItem entities.
        /// </summary>
        private TrulyObservableCollection<TeachingAideItem> _dataGridTeachingAides;
        public TrulyObservableCollection<TeachingAideItem> DataGridTeachingAides
        {
            get { return _dataGridTeachingAides; }
            set
            {
                _dataGridTeachingAides = value;
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
        private TeachingAideItem _selectedTeachingAideItem;
        public TeachingAideItem SelectedTeachingAideItem
        {
            get { return _selectedTeachingAideItem; }
            set
            {
                _selectedTeachingAideItem = value;
                OnSelected();
            }
        }

        private void OnSelected()
        {
            if (SelectedTeachingAideItem != null)
                Messenger.Default.Send(new PropertySetter("TeachingAideImage", SelectedTeachingAideItem.TeachingAideImage), Token);
        }

        public void SetDataGridTeachingAides(string name, object teachingAides)
        {
            if (name == "DataGridTeachingAides")
                DataGridTeachingAides = (TrulyObservableCollection<TeachingAideItem>)(teachingAides);
        }


        /// <summary>
        /// Gets the command that allows a cell to be updated.
        /// </summary>
        public RelayCommand<DataGridCellEditEndingEventArgs> CellEditEndingCommand
        {
            get
            {
                return new RelayCommand<DataGridCellEditEndingEventArgs>(UpdateTeachingAideItem);
            }
        }

        /// <summary>
        /// Gets the command that allows a row to be deleted.
        /// </summary>
        public RelayCommand DeleteSelectedCommand
        {
            get
            {
                return new RelayCommand(DeleteTeachingAideItem);
            }
        }

        private void DeleteTeachingAideItem()
        {

            //Warn user
            if (MessageBox.Show("Deletions are permanent. Are you sure you want to delete this item? ", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            //Quit if Selected is null
            if (null == SelectedTeachingAideItem) return;

            //Find in grid
            var teachingAideItem = DataGridTeachingAides.FirstOrDefault(bi => bi.ItemId == SelectedTeachingAideItem.ItemId);

            //Quit if not found in grid
            if (teachingAideItem == null) return;

            //Copy grid to temp
            var tempGames = DataGridTeachingAides;

            //Get the index and adjust if it is the last item on the grid
            var gridIndex = tempGames.IndexOf(teachingAideItem);
            if ((gridIndex + 1) == tempGames.Count)
                gridIndex--;

            //Remove from grid
            tempGames.Remove(teachingAideItem);
            DataGridTeachingAides = tempGames;

            //Select new item on grid
            SelectedTeachingAideItem = DataGridTeachingAides.ElementAtOrDefault(gridIndex);

            //Delete from db 
            var itemDeleteVisitor = new ItemDeleteVisitor();
            teachingAideItem.Accept(itemDeleteVisitor);


        }


        private void UpdateTeachingAideItem(DataGridCellEditEndingEventArgs e)
        {

            var text = WpfHelpers.GetTextFromCellEditingEventArgs(e);

            //Get the Column header
            var column = e.Column.Header.ToString();

            //Add info to new TeachingAide
            var teachingAideItem = e.Row.Item as TeachingAideItem;
            var success = (teachingAideItem != null) && teachingAideItem.SetProperty(column, text);

            if (IsValid(teachingAideItem) && success)
            {

                switch (column)
                {
                    case "Title":
                    case "Manufacturer":
                    case "EAN":
                        {
                            var itemUpdateVisitor = new ItemUpdateVisitor();
                            teachingAideItem.Accept(itemUpdateVisitor);
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
                            teachingAideItem.Accept(itemTypeUpdateVisitor);
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
            items = new ObservableCollection<Item>(icontroller.QueryItemsThatAreTeachingAides());

            var tcontroller = new TeachingAideController();
            ObservableCollection<TeachingAide> teachingAides = new ObservableCollection<TeachingAide>(tcontroller.GetAllItems());
            DataGridTeachingAides = MergeTeachingAideItems(teachingAides, items);
        }

        //Make a collection of TeachingAideItems with info from both the Item and TeachingAide Tables
        private TrulyObservableCollection<TeachingAideItem> MergeTeachingAideItems(ObservableCollection<TeachingAide> teachingAides, ObservableCollection<Item> items)
        {
            //Temp Collection Definition
            TrulyObservableCollection<TeachingAideItem> tempTeachingAideItems = new TrulyObservableCollection<TeachingAideItem>();

            //Clear DataGridTeachingAides
            DataGridTeachingAides.Clear();

            //Make the collection
            IEnumerator<Item> i = items.GetEnumerator();
            while (i.MoveNext())
            {
                //Get the teachingAide that corresponds to the current item
                TeachingAide currentTeachingAide = teachingAides.FirstOrDefault(b => b.Id == i.Current.TeachingAideId);
                //Create an instance of a TeachingAideItem with the current teachingAide and item info
                var currentTeachingAideItem = new TeachingAideItem(currentTeachingAide, i.Current);
                //Add the new instance to the collection
                tempTeachingAideItems.Add(currentTeachingAideItem);
            }

            return tempTeachingAideItems;
        }

        /// <summary>
        /// Gets a value indicating whether the view model is valid.
        /// </summary>
        public bool IsValid(TeachingAideItem bi)
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