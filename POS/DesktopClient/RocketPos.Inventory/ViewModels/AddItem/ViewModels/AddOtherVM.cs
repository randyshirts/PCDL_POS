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

namespace Inventory.ViewModels.AddItem.ViewModels
{
    public class AddOtherVm : ViewModel
    {

        public static readonly Guid Token = Guid.NewGuid();         //So others know messages came from this instance

        public AddOtherVm()
        {
            Others = new ObservableCollection<Other>();

            IsAmazonEnabled = true;

            _otherItem = new OtherItem();

            //Register for messages
            Messenger.Default.Register<PropertySetterString>(this, AddItemVm.Token, msg => SetItemProperty(msg.PropertyName, msg.PropertyValue));
        }

        /// <summary>
        /// Sets a property from AddItemVm.
        /// </summary>
        private void SetItemProperty(string propertyName, string propertyValue)
        {
            //Find the property
            if (propertyName == "OtherLowestNewPrice")
            {
                LowestNewPrice = Double.Parse(propertyValue);
            }
            if (propertyName == "OtherLowestUsedPrice")
            {
                LowestUsedPrice = Double.Parse(propertyValue);
            }
            if (propertyName == "OtherImage")
            {
                OtherImage = propertyValue;
            }
        }

        /// <summary>
        /// Gets the collection of Other entities.
        /// </summary>
        public ICollection<Other> Others { get; private set; }


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
        /// Gets or sets the Manufacturer value.
        /// </summary>
        private string _manufacturer;
        public string Manufacturer 
        { 
            get
            {
                return _manufacturer;
            }
            set
            {
                _manufacturer = value;
                OnPropertyChanged();
                Messenger.Default.Send(new PropertySetter("Manufacturer", Manufacturer), Token);
            }
        }

        private string _otherImage;
        public string OtherImage
        {
            get
            { 
                return _otherImage;
            }
            set
            {
                _otherImage = value;
                OnPropertyChanged();
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

                //If Other is at least 2 characters search amazon 
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
        /// Gets or sets the OtherItem value.
        /// </summary>
        private OtherItem _otherItem;
        public OtherItem OtherItem
        {
            get
            {
                return _otherItem;
            }
            set
            {
                _otherItem = value;
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
                        if (!search.Manufacturer.IsNullOrEmpty()) Manufacturer = search.Manufacturer[selectedIndex];
                        if (!search.Image.IsNullOrEmpty()) OtherImage = search.Image[selectedIndex].URL;
                        if (!search.Ean.IsNullOrEmpty()) Ean = search.Ean[selectedIndex];

                        //Get the lowest prices from Amazon
                        OtherItem = (OtherItem)SearchAmazon.QueryAmazonByEan(OtherItem, Ean, "Other");
                        if (OtherItem != null)
                        {
                            LowestNewPrice = OtherItem.LowestNewPrice;
                            LowestUsedPrice = OtherItem.LowestUsedPrice;
                        }

                        //Send message to change property so that addItem works correctly
                        Messenger.Default.Send(new PropertySetter("Manufacturer", Manufacturer), Token);
                        Messenger.Default.Send(new PropertySetter("Title", Title), Token);
                        Messenger.Default.Send(new PropertySetter("OtherImage", OtherImage), Token);
                        Messenger.Default.Send(new PropertySetter("EAN", Ean), Token);
                    }
                    else
                    {
                        Manufacturer = null;
                        OtherImage = null;
                        //Send message to change property so that addItem works correctly
                        Messenger.Default.Send(new PropertySetter("Manufacturer", null), Token);
                        Messenger.Default.Send(new PropertySetter("Title", null), Token);
                        Messenger.Default.Send(new PropertySetter("OtherImage", null), Token);
                    }
                    return search.Title;

                }
                Manufacturer = null;
                OtherImage = null;
                //Send message to change property so that addItem works correctly
                Messenger.Default.Send(new PropertySetter("Manufacturer", null), Token);
                Messenger.Default.Send(new PropertySetter("Title", null), Token);
                Messenger.Default.Send(new PropertySetter("OtherImage", null), Token);
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
