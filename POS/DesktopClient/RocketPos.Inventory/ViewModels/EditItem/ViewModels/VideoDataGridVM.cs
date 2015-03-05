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
    public class VideoDataGridVm : ViewModel
    {

        public static readonly Guid Token = Guid.NewGuid();         //So others know messages came from this instance

        //private EnumsAndLists myEnumsLists = new EnumsAndLists();   //Various lists and enums used by the application

        //List for sources of ComboBoxes
        private readonly ComboBoxListValues _bindingComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _statusComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _conditionComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _subjectComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _consignorNameComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _ratingComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _formatComboValues = new ComboBoxListValues();

        public VideoDataGridVm()
        {
            //Initialize collections
            DataGridVideos = new TrulyObservableCollection<VideoItem>();
            _bindingComboValues.InitializeComboBox(EnumsAndLists.Bindings);
            _statusComboValues.InitializeComboBox(EnumsAndLists.Statuses);
            _conditionComboValues.InitializeComboBox(EnumsAndLists.Conditions);
            _subjectComboValues.InitializeComboBox(EnumsAndLists.Subjects);
            _ratingComboValues.InitializeComboBox(EnumsAndLists.Ratings);
            _formatComboValues.InitializeComboBox(EnumsAndLists.VideoFormats);

            var controller = new ConsignorController();
            _consignorNameComboValues.InitializeComboBox(controller.GetConsignorNames());
            

            //Register for messages to listen to
            Messenger.Default.Register<PropertySetter>(this, EditItemVm.Token, msg => SetDataGridVideos(msg.PropertyName, msg.PropertyValue));

            //Fill grid with all videos on record
            InitializeDataGrid();

        }

        /// <summary>
        /// Gets the collection of VideoItem entities.
        /// </summary>
        private TrulyObservableCollection<VideoItem> _dataGridVideos;
        public TrulyObservableCollection<VideoItem> DataGridVideos
        {
            get { return _dataGridVideos; }
            set
            {
                _dataGridVideos = value;
                OnPropertyChanged("DataGridVideos");
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
        /// Gets the RatingComboValues list.
        /// </summary>
        public List<ComboBoxListValues> RatingComboValues
        {
            get { return _ratingComboValues.ComboValues; }
        }

        /// <summary>
        /// Gets the FormatComboValues list.
        /// </summary>
        public List<ComboBoxListValues> FormatComboValues
        {
            get { return _formatComboValues.ComboValues; }
        }


        /// <summary>
        /// Gets the selectedItem row from the datagrid.
        /// </summary>
        private VideoItem _selectedVideoItem;
        public VideoItem SelectedVideoItem
        {
            get { return _selectedVideoItem; }
            set
            {
                _selectedVideoItem = value;
                OnSelected();
            }
        }

        private void OnSelected()
        {
            if (SelectedVideoItem != null)
                Messenger.Default.Send(new PropertySetter("VideoImage", SelectedVideoItem.VideoImage), Token);
        }

        public void SetDataGridVideos(string name, object videos)
        {
            if (name == "DataGridVideos")
                DataGridVideos = (TrulyObservableCollection<VideoItem>)(videos);
        }


        /// <summary>
        /// Gets the command that allows a cell to be updated.
        /// </summary>
        public RelayCommand<DataGridCellEditEndingEventArgs> CellEditEndingCommand
        {
            get
            {
                return new RelayCommand<DataGridCellEditEndingEventArgs>(UpdateVideoItem);
            }
        }

        /// <summary>
        /// Gets the command that allows a row to be deleted.
        /// </summary>
        public RelayCommand DeleteSelectedCommand
        {
            get
            {
                return new RelayCommand(DeleteVideoItem);
            }
        }

        private void DeleteVideoItem()
        {

            //Warn user
            if (MessageBox.Show("Deletions are permanent. Are you sure you want to delete this item? ", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            //Quit if Selected is null
            if (null == SelectedVideoItem) return;

            //Find in grid
            var videoItem = DataGridVideos.FirstOrDefault(bi => bi.ItemId == SelectedVideoItem.ItemId);

            //Quit if not found in grid
            if (videoItem == null) return;

            //Copy grid to temp
            var tempGames = DataGridVideos;

            //Get the index and adjust if it is the last item on the grid
            var gridIndex = tempGames.IndexOf(videoItem);
            if ((gridIndex + 1) == tempGames.Count)
                gridIndex--;

            //Remove from grid
            tempGames.Remove(videoItem);
            DataGridVideos = tempGames;

            //Select new item on grid
            SelectedVideoItem = DataGridVideos.ElementAtOrDefault(gridIndex);

            //Delete from db 
            var itemDeleteVisitor = new ItemDeleteVisitor();
            videoItem.Accept(itemDeleteVisitor);


        }


        private void UpdateVideoItem(DataGridCellEditEndingEventArgs e)
        {

            var text = WpfHelpers.GetTextFromCellEditingEventArgs(e);

            //Get the Column header
            var column = e.Column.Header.ToString();

            //Add info to new VideoItem
            var videoItem = e.Row.Item as VideoItem;
            var success = (videoItem != null) && videoItem.SetProperty(column, text);

            if (IsValid(videoItem) && success)
            {

                switch (column)
                {
                    case "Title":
                    case "Publisher":
                    case "Format":
                    case "Rating":
                    case "EAN":
                        {
                            var itemUpdateVisitor = new ItemUpdateVisitor();
                            videoItem.Accept(itemUpdateVisitor);
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
                            videoItem.Accept(itemTypeUpdateVisitor);
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
            var items = new ObservableCollection<Item>(icontroller.QueryItemsThatAreVideos());

            var vcontroller = new VideoController();
            var videos = new ObservableCollection<Video>(vcontroller.GetAllItems());
            DataGridVideos = MergeVideoItems(videos, items);
            
        }

        //Make a collection of VideoItems with info from both the Item and Video Tables
        private TrulyObservableCollection<VideoItem> MergeVideoItems(ObservableCollection<Video> videos, ObservableCollection<Item> items)
        {
            //Temp Collection Definition
            TrulyObservableCollection<VideoItem> tempVideoItems = new TrulyObservableCollection<VideoItem>();

            //Clear DataGridVideos
            DataGridVideos.Clear();

            //Make the collection
            IEnumerator<Item> i = items.GetEnumerator();
            while (i.MoveNext())
            {
                //Get the video that corresponds to the current item
                Video currentVideo = videos.FirstOrDefault(b => b.Id == i.Current.VideoId);
                //Create an instance of a VideoItem with the current video and item info
                var currentVideoItem = new VideoItem(currentVideo, i.Current);
                //Add the new instance to the collection
                tempVideoItems.Add(currentVideoItem);
            }

            return tempVideoItems;
        }

        /// <summary>
        /// Gets a value indicating whether the view model is valid.
        /// </summary>
        public bool IsValid(VideoItem bi)
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