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
    public class AddTeachingAideVm : ViewModel
    {

        public static readonly Guid Token = Guid.NewGuid();         //So others know messages came from this instance
        
        public AddTeachingAideVm()
        {
            TeachingAides = new ObservableCollection<TeachingAide>();

            IsAmazonEnabled = true;

            _teachingAideItem = new TeachingAideItem();

            //Register for messages
            Messenger.Default.Register<PropertySetterString>(this, AddItemVm.Token, msg => SetItemProperty(msg.PropertyName, msg.PropertyValue));
        }

        /// <summary>
        /// Sets a property from AddItemVm.
        /// </summary>
        private void SetItemProperty(string propertyName, string propertyValue)
        {
            //Find the property
            if (propertyName == "TeachingAideLowestNewPrice")
            {
                LowestNewPrice = Double.Parse(propertyValue);
            }
            if (propertyName == "TeachingAideLowestUsedPrice")
            {
                LowestUsedPrice = Double.Parse(propertyValue);
            }
            if (propertyName == "TeachingAideImage")
            {
                TeachingAideImage = propertyValue;
            }
        }

        /// <summary>
        /// Gets the collection of TeachingAide entities.
        /// </summary>
        public ICollection<TeachingAide> TeachingAides { get; private set; }


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

        private string _teachingAideImage;
        public string TeachingAideImage
        {
            get
            {
                return _teachingAideImage;
            }
            set
            {
                _teachingAideImage = value;
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
        /// Gets or sets the TeachingAideItem value.
        /// </summary>
        private TeachingAideItem _teachingAideItem;
        public TeachingAideItem TeachingAideItem
        {
            get
            {
                return _teachingAideItem;
            }
            set
            {
                _teachingAideItem = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable QueryAmazonByName(string name)
        {
            try
            {
                var search = SearchAmazon.SearchAmazonByTitle(name, "TeachingAide");

                if (search != null)
                {
                    var selectedIndex = search.Title.FindIndex(g => g == Title);
                    if (selectedIndex >= 0)
                    {
                        if (!search.Manufacturer.IsNullOrEmpty()) Manufacturer = search.Manufacturer[selectedIndex];
                        if (!search.Image.IsNullOrEmpty()) TeachingAideImage = search.Image[selectedIndex].URL;
                        if (!search.Ean.IsNullOrEmpty()) Ean = search.Ean[selectedIndex];

                        //Get the lowest prices from Amazon
                        TeachingAideItem = (TeachingAideItem)SearchAmazon.QueryAmazonByEan(TeachingAideItem, Ean, "TeachingAide");
                        if (TeachingAideItem != null)
                        {
                            LowestNewPrice = TeachingAideItem.LowestNewPrice;
                            LowestUsedPrice = TeachingAideItem.LowestUsedPrice;
                        }

                        //Send message to change property so that addItem works correctly
                        Messenger.Default.Send(new PropertySetter("Manufacturer", Manufacturer), Token);
                        Messenger.Default.Send(new PropertySetter("Title", Title), Token);
                        Messenger.Default.Send(new PropertySetter("TeachingAideImage", TeachingAideImage), Token);
                        Messenger.Default.Send(new PropertySetter("EAN", Ean), Token);
                    }
                    else
                    {
                        Manufacturer = null;
                        TeachingAideImage = null;
                        //Send message to change property so that addItem works correctly
                        Messenger.Default.Send(new PropertySetter("Manufacturer", null), Token);
                        Messenger.Default.Send(new PropertySetter("Title", null), Token);
                        Messenger.Default.Send(new PropertySetter("TeachingAideImage", null), Token);
                    }
                    return search.Title;

                }
                Manufacturer = null;
                TeachingAideImage = null;
                //Send message to change property so that addItem works correctly
                Messenger.Default.Send(new PropertySetter("Manufacturer", null), Token);
                Messenger.Default.Send(new PropertySetter("Title", null), Token);
                Messenger.Default.Send(new PropertySetter("TeachingAideImage", null), Token);
                return null;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "TeachingAideQueryAmazonByNameException");
                return null;
            }
        }
        
        //Example of how to query google
/*        private void QueryGoogle(string SearchTerm)
        {
            try
            {
                if (SearchTerm != null)
                {
                    string sanitized = HttpUtility.HtmlEncode(SearchTerm);
                    string url = @"http://google.com/complete/search?output=toolbar&q=" + sanitized;
                    WebRequest httpWebRequest = HttpWebRequest.Create(url);
                    var webResponse = httpWebRequest.GetResponse();

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(webResponse.GetResponseStream());
                    var result = xmlDoc.SelectNodes("//CompleteSuggestion");
                    _QueryCollection = result;
                }
            }
            catch (Exception ex)
            {

                return;
            }

        }
 */
        #endregion
    }
}
