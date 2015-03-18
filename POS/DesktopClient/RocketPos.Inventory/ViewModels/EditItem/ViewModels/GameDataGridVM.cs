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

namespace Inventory.ViewModels.EditItem.ViewModels
{
    /// <summary>
    /// A View-Model that represents a customer and its state information.
    /// </summary>
    public class GameDataGridVm : ViewModel
    {

        public static readonly Guid Token = Guid.NewGuid();         //So others know messages came from this instance

        //private EnumsAndLists myEnumsLists = new EnumsAndLists();   //Various lists and enums used by the application

        //List for sources of ComboBoxes
        private readonly ComboBoxListValues _bindingComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _statusComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _conditionComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _subjectComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _consignorNameComboValues = new ComboBoxListValues();

        public GameDataGridVm()
        {
            //Initialize collections
            DataGridGames = new TrulyObservableCollection<GameItem>();
            _bindingComboValues.InitializeComboBox(EnumsAndLists.Bindings);
            _statusComboValues.InitializeComboBox(EnumsAndLists.Statuses);
            _conditionComboValues.InitializeComboBox(EnumsAndLists.Conditions);
            _subjectComboValues.InitializeComboBox(EnumsAndLists.Subjects);
            
            var controller = new ConsignorController();
            _consignorNameComboValues.InitializeComboBox(controller.GetConsignorNames());
            

            //Register for messages to listen to
            Messenger.Default.Register<PropertySetter>(this, EditItemVm.Token, msg => SetDataGridGames(msg.PropertyName, msg.PropertyValue));

            //Fill grid with all games on record
            InitializeDataGrid();

        }

        /// <summary>
        /// Gets the collection of GameItem entities.
        /// </summary>
        private TrulyObservableCollection<GameItem> _dataGridGames;
        public TrulyObservableCollection<GameItem> DataGridGames
        {
            get { return _dataGridGames; }
            set
            {
                _dataGridGames = value;
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
        private GameItem _selectedGameItem;
        public GameItem SelectedGameItem
        {
            get { return _selectedGameItem; }
            set
            {
                _selectedGameItem = value;
                OnSelected();
            }
        }

        private void OnSelected()
        {
            if (SelectedGameItem != null)
                Messenger.Default.Send(new PropertySetter("GameImage", SelectedGameItem.GameImage), Token);
        }

        public void SetDataGridGames(string name, object games)
        {
            if (name == "DataGridGames")
                DataGridGames = (TrulyObservableCollection<GameItem>)(games);
        }


        /// <summary>
        /// Gets the command that allows a cell to be updated.
        /// </summary>
        public RelayCommand<DataGridCellEditEndingEventArgs> CellEditEndingCommand
        {
            get
            {
                return new RelayCommand<DataGridCellEditEndingEventArgs>(UpdateGameItem);
            }
        }

        /// <summary>
        /// Gets the command that allows a row to be deleted.
        /// </summary>
        public RelayCommand DeleteSelectedCommand
        {
            get
            {
                return new RelayCommand(DeleteGameItem);
            }
        }

        private void DeleteGameItem()
        {
            //Warn user
            if (MessageBox.Show("Deletions are permanent. Are you sure you want to delete this item? ", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            //Quit if Selected is null
            if (null == SelectedGameItem) return;

            //Find in grid
            var gameItem = DataGridGames.FirstOrDefault(bi => bi.ItemId == SelectedGameItem.ItemId);

            //Quit if not found in grid
            if (gameItem == null) return;

            //Copy grid to temp
            var tempGames = DataGridGames;

            //Get the index and adjust if it is the last item on the grid
            var gridIndex = tempGames.IndexOf(gameItem);
            if ((gridIndex + 1) == tempGames.Count)
                gridIndex--;

            //Remove from grid
            tempGames.Remove(gameItem);
            DataGridGames = tempGames;

            //Select new item on grid
            SelectedGameItem = DataGridGames.ElementAtOrDefault(gridIndex);

            //Delete from db 
            var itemDeleteVisitor = new ItemDeleteVisitor();
            gameItem.Accept(itemDeleteVisitor);
        }


        private void UpdateGameItem(DataGridCellEditEndingEventArgs e)
        {


            var text = WpfHelpers.GetTextFromCellEditingEventArgs(e);

            //Get the Column header
            var column = e.Column.Header.ToString();

            //Add info to new GameItem
            var gameItem = e.Row.Item as GameItem;
            var success = (gameItem != null) && gameItem.SetProperty(column, text);

            if (IsValid(gameItem) && success)
            {

                switch (column)
                {
                    case "Title":
                    case "Manufacturer":
                    case "EAN":
                        {
                            var itemUpdateVisitor = new ItemUpdateVisitor();
                            gameItem.Accept(itemUpdateVisitor);
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
                            gameItem.Accept(itemTypeUpdateVisitor);
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
            var icontroller = new ItemController();
            var items = new ObservableCollection<Item>(icontroller.QueryItemsThatAreGames());

            var gcontroller = new GameController();
            var games = new ObservableCollection<Game>(gcontroller.GetAllItems());
            DataGridGames = MergeGameItems(games, items);
            
        }

        //Make a collection of GameItems with info from both the Item and Game Tables
        private TrulyObservableCollection<GameItem> MergeGameItems(ObservableCollection<Game> games, IEnumerable<Item> items)
        {
            //Temp Collection Definition
            var tempGameItems = new TrulyObservableCollection<GameItem>();

            //Clear DataGridGames
            DataGridGames.Clear();

            //Make the collection
            var i = items.GetEnumerator();
            while (i.MoveNext())
            {
                //Get the game that corresponds to the current item
                var currentGame = games.FirstOrDefault(b => b.Id == i.Current.GameId);
                //Create an instance of a GameItem with the current game and item info
                var currentGameItem = new GameItem(currentGame, i.Current);
                //Add the new instance to the collection
                tempGameItems.Add(currentGameItem);
            }

            return tempGameItems;
        }

        /// <summary>
        /// Gets a value indicating whether the view model is valid.
        /// </summary>
        public bool IsValid(GameItem gi)
        {
            if (gi == null) return false;

            return !String.IsNullOrWhiteSpace(gi.Title) &&
                    !String.IsNullOrWhiteSpace(gi.ItemStatus) &&
                    !Double.IsNaN(gi.ListedPrice) &&
                    !(gi.ListedPrice < ConfigSettings.MIN_LISTED_PRICE) &&
                    !String.IsNullOrWhiteSpace(gi.Condition) &&
                    !String.IsNullOrWhiteSpace(gi.ConsignorName);


        }
    }
}