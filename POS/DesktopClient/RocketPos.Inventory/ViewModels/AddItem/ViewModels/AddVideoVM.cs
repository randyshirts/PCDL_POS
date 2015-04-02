using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using Castle.Core.Internal;
using DataModel.Data.DataLayer.Entities;
using GalaSoft.MvvmLight.Messaging;
using Inventory.Controller.CustomClasses;
using Inventory.Controller.Elements.ItemElements;
using RocketPos.Common.Foundation;
using RocketPos.Common.Helpers;

namespace Inventory.ViewModels.AddItem.ViewModels
{
    public class AddVideoVm : ViewModel
    {

        public static readonly Guid Token = Guid.NewGuid();         //So others know messages came from this instance
       
        //List for sources of ComboBoxes
        private readonly ComboBoxListValues _formatComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _ratingComboValues = new ComboBoxListValues();


        public AddVideoVm()
        {
            Videos = new ObservableCollection<Video>();

            _videoItem = new VideoItem();

            IsAmazonEnabled = true;

            _formatComboValues.InitializeComboBox(EnumsAndLists.VideoFormats);
             _ratingComboValues.InitializeComboBox(EnumsAndLists.Ratings);

             //Register for messages
             Messenger.Default.Register<PropertySetterString>(this, AddItemVm.Token, msg => SetItemProperty(msg.PropertyName, msg.PropertyValue));
        }

        /// <summary>
        /// Sets a property from AddItemVm.
        /// </summary>
        private void SetItemProperty(string propertyName, string propertyValue)
        {
            //Find the property
            if (propertyName == "VideoLowestNewPrice")
            {
                LowestNewPrice = Double.Parse(propertyValue);
            }
            if (propertyName == "VideoLowestUsedPrice")
            {
                LowestUsedPrice = Double.Parse(propertyValue);
            }
            if (propertyName == "VideoImage")
            {
                VideoImage = propertyValue;
            }
        }

        /// <summary>
        /// Gets the collection of Video entities.
        /// </summary>
        public ICollection<Video> Videos { get; private set; }

        /// <summary>
        /// Gets the formatComboValues list.
        /// </summary>
        public List<ComboBoxListValues> FormatComboValues
        {
            get { return _formatComboValues.ComboValues; }
        }

        /// <summary>
        /// Gets the ratingComboValues list.
        /// </summary>
        public List<ComboBoxListValues> RatingComboValues
        {
            get { return _ratingComboValues.ComboValues; }
        }

        /// <summary>
        /// Gets or sets the Title value.
        /// </summary>
        private string _title;
        [Required]
        public string Title 
        { 
            get
            {
                return _title;    
            } 
            set
            { 
                _title = value;
                OnPropertyChanged();
                Messenger.Default.Send(new PropertySetter("Title", Title), Token);
            }
        }
        /// <summary>
        /// Gets or sets the Publisher value.
        /// </summary>
        private string _publisher;
        public string Publisher 
        { 
            get
            {
                return _publisher;
            }
            set
            {
                _publisher = value;
                OnPropertyChanged();
                Messenger.Default.Send(new PropertySetter("Publisher", Publisher), Token);
            }
        }

        private string _videoImage;
        public string VideoImage
        {
            get
            { 
                return _videoImage;
            }
            set
            {
                _videoImage = value;
                OnPropertyChanged();
            }
        }

        private string _videoFormat;
        [Required]
        public string VideoFormat
        {
            get
            {
                return _videoFormat;
            }
            set
            {
                _videoFormat = value;
                OnPropertyChanged();
                Messenger.Default.Send(new PropertySetter("VideoFormat", VideoFormat), Token);
            }
        }

        private string _rating;
        public string Rating
        {
            get
            {
                return _rating;
            }
            set
            {
                _rating = value;
                OnPropertyChanged();
                Messenger.Default.Send(new PropertySetter("Rating", Rating), Token);
            }
        }

        private string _ean;
        public string Ean
        {
            get
            {
                return _ean;
            }
            set
            {
                _ean = value;
                OnPropertyChanged();
                Messenger.Default.Send(new PropertySetter("EAN", Ean), Token);
            }
        }

        /// <summary>
        /// Gets or sets the LowestNewPrice value.
        /// </summary>
        private double _lowestNewPrice;
        public double LowestNewPrice
        {
            get
            {
                return _lowestNewPrice;
            }
            set
            {
                _lowestNewPrice = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the LowestUsedPrice value.
        /// </summary>
        private double _lowestUsedPrice;
        public double LowestUsedPrice
        {
            get
            {
                return _lowestUsedPrice;
            }
            set
            {
                _lowestUsedPrice = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the IsAmazonEnabled checkbox value.
        /// </summary>
        private bool _isAmazonEnabled;
        public bool IsAmazonEnabled
        {
            get
            {
                return _isAmazonEnabled;
            }
            set
            {
                _isAmazonEnabled = value;
                OnPropertyChanged();
            }
        }

        #region AutoComplete TextBox
        private readonly List<string> _waitMessage = new List<string>() { "Please Wait..." };
        public IEnumerable WaitMessage { get { return _waitMessage; } }

        private string _queryText;
        public string QueryText
        {
            get { return _queryText; }
            set
            {
                if (_queryText == value) return;
                _queryText = value;
                OnPropertyChanged();
                QueryCollection = null;
            }
        }

        private string _titleQueryText;
        public string TitleQueryText
        {
            get { return _titleQueryText; }
            set
            {
                if (_titleQueryText == value) return;
                _titleQueryText = value;
                OnPropertyChanged();
                Title = value;

                //If TeachingAide is at least 2 characters search amazon 
                //  and change title and author fields if found
                if (!string.IsNullOrEmpty(_titleQueryText) && IsAmazonEnabled)
                    QueryCollection = QueryAmazonByName(_titleQueryText);
            }
        }

        private IEnumerable _queryCollection;
        public IEnumerable QueryCollection
        {
            get
            {
                return _queryCollection ?? (_queryCollection = new List<string>());
            }
            set
            {

                _queryCollection = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the VideoItem value.
        /// </summary>
        private VideoItem _videoItem;
        public VideoItem VideoItem
        {
            get
            {
                return _videoItem;
            }
            set
            {
                _videoItem = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable QueryAmazonByName(string name)
        {
            try
            {
                var search = SearchAmazon.SearchAmazonByTitle(name, "Other");

                if (search != null)
                {
                    var selectedIndex = search.Title.FindIndex(g => g == Title);
                    if (selectedIndex >= 0)
                    {
                        if(!search.Publisher.IsNullOrEmpty()) Publisher = search.Publisher[selectedIndex];
                        if (!search.Image.IsNullOrEmpty()) VideoImage = search.Image[selectedIndex].URL;
                        if (!search.Ean.IsNullOrEmpty()) Ean = search.Ean[selectedIndex];
                        if (!search.Format.IsNullOrEmpty()) VideoFormat = search.Format[selectedIndex];
                        if (!search.Rating.IsNullOrEmpty()) Rating = search.Rating[selectedIndex];
                            
                        //Get the lowest prices from Amazon
                        VideoItem = (VideoItem)SearchAmazon.QueryAmazonByEan(VideoItem, Ean, "Video");
                        if (VideoItem != null)
                        {
                            LowestNewPrice = VideoItem.LowestNewPrice;
                            LowestUsedPrice = VideoItem.LowestUsedPrice;
                        }
                        //Send message to change property so that addItem works correctly
                        Messenger.Default.Send(new PropertySetter("Publisher", Publisher), Token);
                        Messenger.Default.Send(new PropertySetter("Title", Title), Token);
                        Messenger.Default.Send(new PropertySetter("VideoImage", VideoImage), Token);
                        Messenger.Default.Send(new PropertySetter("VideoFormat", VideoFormat), Token);
                        Messenger.Default.Send(new PropertySetter("EAN", Ean), Token);
                        Messenger.Default.Send(new PropertySetter("Rating", Rating), Token);
                    }
                    //Else no title was found in search so empty fields
                    else
                    {
                        Publisher = null;
                        VideoImage = null;
                        VideoFormat = null;
                        Ean = null;
                        Rating = null;
                            
                        //Send message to change property so that addItem works correctly
                        Messenger.Default.Send(new PropertySetter("Publisher", null), Token);
                        Messenger.Default.Send(new PropertySetter("Title", null), Token);
                        Messenger.Default.Send(new PropertySetter("VideoImage", null), Token);
                        Messenger.Default.Send(new PropertySetter("VideoFormat", null), Token);
                        Messenger.Default.Send(new PropertySetter("EAN", null), Token);
                        Messenger.Default.Send(new PropertySetter("Rating", null), Token);
                    }
                    return search.Title;
                }
                Publisher = null;
                VideoImage = null;
                VideoFormat = null;
                Ean = null;
                Rating = null;
                //Send message to change property so that addItem works correctly
                Messenger.Default.Send(new PropertySetter("Publisher", null), Token);
                Messenger.Default.Send(new PropertySetter("Title", null), Token);
                Messenger.Default.Send(new PropertySetter("VideoImage", null), Token);
                Messenger.Default.Send(new PropertySetter("VideoFormat", null), Token);
                Messenger.Default.Send(new PropertySetter("EAN", Ean), Token);
                Messenger.Default.Send(new PropertySetter("Rating", null), Token);
                return null;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "VideoQueryAmazonByNameException");
                return null;
            }
        }

       
        #endregion
    }
}
