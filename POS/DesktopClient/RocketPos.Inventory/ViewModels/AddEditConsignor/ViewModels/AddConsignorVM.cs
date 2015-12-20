// -----------------------------------------------------------------------------
//  <copyright file="CustomerViewModel.cs" company="DCOM Engineering, LLC">
//      Copyright (c) DCOM Engineering, LLC.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DataModel.Data.ApplicationLayer.WpfControllers;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.TransactionalLayer.Repositories;
using GalaSoft.MvvmLight.Command;
using RocketPos.Common.Foundation;
using RocketPos.Common.Helpers;
using RocketPos.Inventory.Resources;

namespace Inventory.ViewModels.AddEditConsignor.ViewModels
{
    //using Google.Apis.Books.v1;

    /// <summary>
    /// A View-Model that represents a Consignor and its state information.
    /// </summary>
    public class AddConsignorVm : ViewModel
    {
        public static readonly Guid Token = Guid.NewGuid();         //So others know messages came from this instance
        private readonly ComboBoxListValues _nameComboValues = new ComboBoxListValues();

        //private EnumsAndLists myEnumsLists = new EnumsAndLists();   //Various lists and enums used by the application

        //List for sources of ComboBoxes
        private ComboBoxListValues stateComboValues = new ComboBoxListValues();

        public AddConsignorVm()
        {
            Consignors = new ObservableCollection<Consignor>();
            DataGridConsignors = new ObservableCollection<ConsignorInfo>();

            //Initialize ComboBoxes
            stateComboValues.InitializeComboBox(EnumsAndLists.States);

            //var controller = new PersonController();
            //AllPersons = controller.GetAllPersons().ToList();
            //var names = StringHelpers.CombineNames(AllPersons);
            //var nameComboList = new List<string>(names);
            ////NameComboList.Insert(0, "All");
            //_nameComboValues.InitializeComboBox(nameComboList);

            //Register for messages
            //Messenger.Default.Register<PropertySetter>(this, AddGameVM.Token, msg => SetGameProperty(msg.PropertyName, (string)msg.PropertyValue));         

            //Fill grid with all consignors on record
            InitializeDataGrid();
        }


        public ICommand WindowLoaded
        {
            get
            {
                return new ActionCommand(p => LoadWindow());
            }
        }

        private void LoadWindow()
        {
            //Load ExistingNames
            var controller = new PersonController();
            AllPersons = controller.GetAllPersons().ToList();
            var names = StringHelpers.CombineNames(AllPersons);
            var nameComboList = new List<string>(names);
            //NameComboList.Insert(0, "All");
            _nameComboValues.InitializeComboBox(nameComboList);

        }


        /// <summary>
        /// Sets a property of myAddGame view model.
        /// </summary>
        //private void SetGameProperty(string propertyName, string propertyValue)
        //{
        //    //Find the property
        //    if(propertyName == "Title")
        //    {
        //        GameTitle = propertyValue;
        //    }
        //    if(propertyName == "Manufacturer")
        //    {
        //        GameManufacturer = propertyValue;
        //    }
        //    if (propertyName == "GameImage")
        //    {
        //        GameImage = propertyValue;
        //    }
        //    if (propertyName == "EAN")
        //    {
        //        GameEAN = propertyValue;
        //    }
        //}

        //private string GameTitle { get; set; }
        //private string GameManufacturer { get; set; }
        //private string GameImage { get; set; }
        //private string GameEAN { get; set; }

        /// <summary>
        /// Gets the collection of Consignor entities.
        /// </summary>
        public ICollection<Consignor> Consignors { get; private set; }

        #region AddConsignor Stuff
        /// <summary>
        /// Gets the stateComboValues list.
        /// </summary>
        public List<ComboBoxListValues> StateComboValues
        {
            get { return stateComboValues.ComboValues; }
            private set { stateComboValues.ComboValues = value; }
        }

        /// <summary>
        /// Gets the NameComboValues list.
        /// </summary>
        public List<ComboBoxListValues> NameComboValues
        {
            get { return _nameComboValues.ComboValues; }
        }

        /// <summary>
        /// Gets or sets the existing person name.
        /// </summary>
        private string _existingName;
        public string ExistingName
        {
            get { return _existingName; }
            set
            {
                _existingName = value;

                if (_existingName != null)
                {
                    SelectedPerson =
                        AllPersons.Find(
                            p =>
                                p.FirstName == _existingName.Split().ElementAt(0) &&
                                (p.LastName == _existingName.Split().ElementAt(1)));
                    FirstName = SelectedPerson.FirstName;
                    LastName = SelectedPerson.LastName;
                    EmailAddress = SelectedPerson.EmailAddresses.FirstOrDefault().EmailAddress;
                    CellPhoneNumber = SelectedPerson.PhoneNumbers.CellPhoneNumber;
                    HomePhoneNumber = SelectedPerson.PhoneNumbers.HomePhoneNumber;
                    AltPhoneNumber = SelectedPerson.PhoneNumbers.AltPhoneNumber;

                    if (SelectedPerson.MailingAddresses.Any())
                    {
                        MailingAddress1 = SelectedPerson.MailingAddresses.FirstOrDefault().MailingAddress1;
                        MailingAddress2 = SelectedPerson.MailingAddresses.FirstOrDefault().MailingAddress2;
                        City = SelectedPerson.MailingAddresses.FirstOrDefault().City;
                        State = SelectedPerson.MailingAddresses.FirstOrDefault().State;
                        ZipCode = SelectedPerson.MailingAddresses.FirstOrDefault().ZipCode;
                    }
                    else
                    {
                        MailingAddress1 = null;
                        MailingAddress2 = null;
                        City = null;
                        State = null;
                        ZipCode = null;
                    }

                    OnPropertyChanged("FirstName");
                    OnPropertyChanged("LastName");
                    OnPropertyChanged("EmailAddress");
                    OnPropertyChanged("CellPhoneNumber");
                    OnPropertyChanged("HomePhoneNumber");
                    OnPropertyChanged("AltPhoneNumber");
                    OnPropertyChanged("MailingAddress1");
                    OnPropertyChanged("MailingAddress2");
                    OnPropertyChanged("City");
                    OnPropertyChanged("State");
                    OnPropertyChanged("ZipCode");

                }
                else
                {
                    SelectedPerson = null;
                }

                OnPropertyChanged();
            }
        }

        public List<Person> AllPersons { get; set; }
        private Person SelectedPerson { get; set; }

        /// <summary>
        /// Gets or sets the date the consignor was added.
        /// </summary>
        [Required]
        public string DateAdded { get; set; }

        private string m_firstName;
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        [Required]
        [StringLength(20)]
        public string FirstName
        {
            get { return m_firstName; }
            set
            {
                m_firstName = StringHelpers.SanitizeName(value);
            }
        }

        private string m_lastName;
        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        [Required]
        [StringLength(20)]
        public string LastName
        {
            get { return m_lastName; }
            set
            {
                m_lastName = StringHelpers.SanitizeName(value);
            }
        }

        /// <summary>
        /// Gets or sets the EmailAddress value.
        /// </summary>
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the CellPhoneNumber value.
        /// </summary>
        public string CellPhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the HomePhoneNumber value.
        /// </summary>
        public string HomePhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the AltPhoneNumber value.
        /// </summary>
        public string AltPhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the MailingAddress1 value.
        /// </summary>
        [Required]
        public string MailingAddress1 { get; set; }

        /// <summary>
        /// Gets or sets the MailingAddress2 value.
        /// </summary>
        [StringLength(40)]
        public string MailingAddress2 { get; set; }

        /// <summary>
        /// Gets or sets the City value.
        /// </summary>
        [Required]
        [StringLength(30)]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the State value.
        /// </summary>
        [Required]
        [StringLength(2, MinimumLength = 2)]
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the ZipCode value.
        /// </summary>
        [Required]
        public string ZipCode { get; set; }


        /// <summary>
        /// Gets a value indicating whether the view model is valid.
        /// </summary>
        public bool IsValid
        {
            get
            {
                EmailAddressAttribute email = new EmailAddressAttribute();

                if (!String.IsNullOrWhiteSpace(FirstName))
                {
                    if (FirstName.Length > 20)
                        return false;
                }
                else
                    return false;

                if (!String.IsNullOrWhiteSpace(LastName))
                {
                    if (LastName.Length >= 20)
                        return false;
                }
                else
                    return false;

                if (!String.IsNullOrWhiteSpace(EmailAddress))
                {
                    if (!email.IsValid(EmailAddress) || (EmailAddress.Length > 40))
                        return false;
                }
                else
                    return false;

                if (!String.IsNullOrWhiteSpace(CellPhoneNumber) || (!String.IsNullOrWhiteSpace(HomePhoneNumber)))
                {
                    if (!Validation_Helper.ParsePhoneNumberLogic(CellPhoneNumber))
                        return false;
                    if (!Validation_Helper.ParsePhoneNumberLogic(HomePhoneNumber))
                        return false;
                    if (!Validation_Helper.ParsePhoneNumberLogic(AltPhoneNumber))
                        return false;
                }
                else
                    return false;

                if (!String.IsNullOrWhiteSpace(MailingAddress1))
                {
                    if (MailingAddress1.Length > 40)
                        return false;
                }
                else
                    return false;

                if (!String.IsNullOrWhiteSpace(MailingAddress2))
                {
                    if (MailingAddress2.Length > 40)
                        return false;
                }

                if (!String.IsNullOrWhiteSpace(City))
                {
                    if (City.Length > 30)
                        return false;
                }
                else
                    return false;

                if (!String.IsNullOrWhiteSpace(State))
                {
                    if (State.Length != 2)
                        return false;
                }
                else
                    return false;


                if (!String.IsNullOrWhiteSpace(ZipCode))
                {
                    if (!Validation_Helper.ParseZipCodeLogic(ZipCode))
                        return false;
                }
                else
                    return false;

                return true;

            }
        }

        /// <summary>
        /// Gets the command that allows a customer to be added.
        /// </summary>
        public ActionCommand AddConsignorCommand
        {
            get
            {
                return new ActionCommand(p => AddConsignor(FirstName, LastName, EmailAddress, CellPhoneNumber,
                                                HomePhoneNumber, AltPhoneNumber, MailingAddress1, MailingAddress2,
                                                City, State, ZipCode),
                                         p => IsValid);
            }
        }

        protected override string OnValidate(string propertyName)
        {

            if (propertyName == "FirstName")
            {
                if (FirstName == null)
                    return "First Name is Required";
                else if (FirstName.Length > 20)
                    return "Name must be 20 characters or less in length";
            }

            if (propertyName == "LastName")
            {
                if (LastName == null)
                    return "Last Name is Required";
                else if (LastName.Length > 20)
                    return "Name must be 20 characters or less in length";

                if (LastName.Contains(" ")) { LastName.Replace(" ", String.Empty); }
            }

            if (propertyName == "EmailAddress")
            {
                EmailAddressAttribute email = new EmailAddressAttribute();
                if (EmailAddress == null)
                    return "Email Address is Required";
                else if (!email.IsValid(EmailAddress))
                    return "Email Address must include '@' and '.' (e.g. 'johndoe@gmail.com')";
                else if (EmailAddress.Length > 40)
                    return "Email Address must be 40 characters or less in length.";

            }

            if (propertyName == "CellPhoneNumber")
            {
                if ((CellPhoneNumber == null) && (HomePhoneNumber == null))
                    return "Cell or Home Phone number is required";
                else if (!Validation_Helper.ParsePhoneNumberLogic(CellPhoneNumber))
                    return (("Cell phone must be 10 characters in length ") +
                                ("and must contain only numbers (e.g. '2565229999')"));
            }

            if (propertyName == "HomePhoneNumber")
            {
                if ((CellPhoneNumber == null) && (HomePhoneNumber == null))
                    return "Cell or Home Phone number is required";
                else if (!Validation_Helper.ParsePhoneNumberLogic(HomePhoneNumber))
                    return (("Home phone must be 10 characters in length ") +
                                ("and must contain only numbers (e.g. '2565229999')"));
            }

            if (propertyName == "AltPhoneNumber")
            {
                if (!Validation_Helper.ParsePhoneNumberLogic(AltPhoneNumber))
                    return (("Alt phone must be 10 characters in length") +
                                    ("and must contain only numbers (e.g. '2565229999')"));
            }

            if (propertyName == "MailingAddress1")
            {
                if ((MailingAddress1 == null) || (MailingAddress1.Length > 40))
                    return "Mailing Address is Required and must be 40 characters or less in length";
            }

            if (propertyName == "MailingAddress2")
            {
                if (MailingAddress2 != null)
                    if ((MailingAddress2.Length > 40))
                        return "Mailing Address must be 40 characters or less in length";
            }

            if (propertyName == "City")
            {
                if (City == null)
                    return "City is Required";
                else if (City.Length > 30)
                    return "City must be 30 characters or less in length";

            }

            if (propertyName == "State")
            {
                if (State == null)
                    return "State is Required";
                else if (State.Length != 2)
                    return "State must be 2 characters in length (e.g. 'AL')";
            }

            if (propertyName == "ZipCode")
            {
                if (ZipCode == null)
                    return "ZipCode is Required";
                else if (!Validation_Helper.ParseZipCodeLogic(ZipCode))
                    return "ZipCode must be 5 digits in length and contain only numbers(e.g. '35824')";
            }

            return base.OnValidate(propertyName);
        }

        private void AddConsignor(string firstName, string lastName, string emailAddress, string cellPhoneNumber,
                                  string homePhoneNumber, string altPhoneNumber, string mailingAddress1,
                                  string mailingAddress2, string city, string state, string zipCode)
        {


            DateTime dateAdded;
            if (!(DateTime.TryParse(DateAdded, out dateAdded)))
                dateAdded = DateTime.Now;

            Consignor consignor = new Consignor
            {
                DateAdded = dateAdded,

                Consignor_Person = new Person
                {
                    FirstName = firstName,
                    LastName = lastName,

                    EmailAddresses = new Collection<Email>
                        {
                            new Email
                            {
                                EmailAddress = emailAddress
                            }
                        },

                    MailingAddresses = new Collection<MailingAddress>
                        {
                            new MailingAddress
                            {
                                MailingAddress1 = mailingAddress1,
                                MailingAddress2 = mailingAddress2,
                                City = city,
                                State = state,
                                ZipCode = zipCode
                            }
                        },

                    PhoneNumbers = new PhoneNumber
                    {
                        CellPhoneNumber = cellPhoneNumber,
                        HomePhoneNumber = homePhoneNumber,
                        AltPhoneNumber = altPhoneNumber
                    }
                }
            };

            try
            {
                var controller = new ConsignorController();
                var id = controller.AddNewConsignor(consignor);
                var pc = new PersonController();
                var person = pc.GetPerson(id);
                ConsignorInfo consignorInfo = new ConsignorInfo(consignor, person);
                DataGridConsignors.Add(consignorInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("AddConsignorException", ex.Message);
            }


        }

        #endregion

        #region DataGrid Stuff
        /// <summary>
        /// Gets the collection of ConsignorInfo entities.
        /// </summary>
        public ObservableCollection<ConsignorInfo> DataGridConsignors { get; set; }

        /// <summary>
        /// Gets the selectedItem row from the datagrid.
        /// </summary>
        private ConsignorInfo selectedConsignor;
        public ConsignorInfo SelectedConsignor
        {
            get { return selectedConsignor; }
            set
            {
                selectedConsignor = value;
                OnSelected();
            }
        }

        private void OnSelected()
        {
            //if (SelectedConsignor != null)
            //    Messenger.Default.Send(new PropertySetter("GameImage", SelectedGameItem.GameImage), Token);
        }

        /// <summary>
        /// Gets the command that allows a cell to be updated.
        /// </summary>
        public RelayCommand<DataGridCellEditEndingEventArgs> CellEditEndingCommand
        {
            get
            {
                return new RelayCommand<DataGridCellEditEndingEventArgs>(UpdateConsignorInfo);
            }
        }

        /// <summary>
        /// Gets the command that allows a row to be deleted.
        /// </summary>
        public RelayCommand DeleteSelectedCommand
        {
            get
            {
                return new RelayCommand(DeleteConsignorInfo);
            }
        }

        private void DeleteConsignorInfo()
        {
            if (MessageBox.Show("Deletions are permanent. Are you sure you want to delete this item? ", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;
            if (null != SelectedConsignor)
            {
                var consignorInfo = DataGridConsignors.FirstOrDefault(bi => bi.Id == SelectedConsignor.Id);
                if (consignorInfo != null)
                {
                    var tempConsignors = DataGridConsignors;
                    var gridIndex = tempConsignors.IndexOf(consignorInfo);
                    if ((gridIndex + 1) == tempConsignors.Count)
                        gridIndex--;
                    tempConsignors.Remove(consignorInfo);
                    DataGridConsignors = tempConsignors;
                    SelectedConsignor = DataGridConsignors.ElementAtOrDefault(gridIndex);

                    var controller = new ConsignorController();
                    controller.DeleteConsignerById(consignorInfo.Id);
                    
                }
            }
        }

        private void UpdateConsignorInfo(DataGridCellEditEndingEventArgs e)
        {
            var text = WpfHelpers.GetTextFromCellEditingEventArgs(e);

            //Get the Column header
            var column = e.Column.Header.ToString();

            //Add info to new ConsignorInfo
            var consignorInfo = e.Row.Item as ConsignorInfo;
            var success = consignorInfo.SetProperty(column, text);

            if (ConsignorInfoIsValid(consignorInfo) && success)
            {
                switch (column)
                {
                    case "DateAdded":
                        {
                            var consignor = consignorInfo.ConvertConsignorInfoToConsignor((ConsignorInfo)(e.Row.Item));
                            var controller = new ConsignorController();
                            controller.UpdateConsignor(consignor);
                            break;
                        }
                    case "First Name":
                    case "Last Name":
                    case "Email":
                    case "Mailing Address 1":
                    case "Mailing Address 2":
                    case "Cell Phone":
                    case "Home Phone":
                    case "Alt Phone":
                    case "City":
                    case "State":
                    case "ZipCode":
                        {

                            var person = consignorInfo.ConvertConsignorInfoToPerson((ConsignorInfo)(e.Row.Item));
                            person.FirstName = StringHelpers.SanitizeName(person.FirstName);
                            person.LastName = StringHelpers.SanitizeName(person.LastName);
                            var controller = new PersonController();
                            controller.UpdatePerson(person);
                            break;
                        }
                }
            }
            else
            {
                e.Cancel = true;
                consignorInfo.ReportError(column, text);
            }

        }

        private bool ConsignorInfoIsValid(ConsignorInfo consignorInfo)
        {
            EmailAddressAttribute email = new EmailAddressAttribute();//consignorInfo.EmailAddress

            if (String.IsNullOrEmpty(consignorInfo.MailingAddress2))
            {
                return !String.IsNullOrWhiteSpace(consignorInfo.FirstName) &&
                        !String.IsNullOrWhiteSpace(consignorInfo.LastName) &&
                        (consignorInfo.FirstName.Length <= 20) &&
                        (consignorInfo.LastName.Length <= 20) &&
                        email.IsValid(consignorInfo.EmailAddress) &&
                        (consignorInfo.EmailAddress.Length <= 40) &&
                        (!String.IsNullOrWhiteSpace(consignorInfo.CellPhoneNumber) || (!String.IsNullOrWhiteSpace(consignorInfo.HomePhoneNumber))) &&
                        Validation_Helper.ParsePhoneNumberLogic(consignorInfo.CellPhoneNumber) &&
                        Validation_Helper.ParsePhoneNumberLogic(consignorInfo.HomePhoneNumber) &&
                        Validation_Helper.ParsePhoneNumberLogic(consignorInfo.AltPhoneNumber) &&
                        !String.IsNullOrWhiteSpace(consignorInfo.MailingAddress1) &&
                        (consignorInfo.MailingAddress1.Length <= 40) &&
                        !String.IsNullOrWhiteSpace(consignorInfo.City) &&
                        (consignorInfo.City.Length <= 30) &&
                        !String.IsNullOrWhiteSpace(consignorInfo.State) &&
                        (consignorInfo.State.Length == 2) &&
                        !String.IsNullOrWhiteSpace(consignorInfo.ZipCode) &&
                        Validation_Helper.ParseZipCodeLogic(consignorInfo.ZipCode);
            }
            else
            {
                return !String.IsNullOrWhiteSpace(consignorInfo.FirstName) &&
                        !String.IsNullOrWhiteSpace(consignorInfo.LastName) &&
                        (consignorInfo.FirstName.Length <= 20) &&
                        (consignorInfo.LastName.Length <= 20) &&
                        email.IsValid(consignorInfo.EmailAddress) &&
                        (consignorInfo.EmailAddress.Length <= 40) &&
                        (!String.IsNullOrWhiteSpace(consignorInfo.CellPhoneNumber) || (!String.IsNullOrWhiteSpace(consignorInfo.HomePhoneNumber))) &&
                        Validation_Helper.ParsePhoneNumberLogic(consignorInfo.CellPhoneNumber) &&
                        Validation_Helper.ParsePhoneNumberLogic(consignorInfo.HomePhoneNumber) &&
                        Validation_Helper.ParsePhoneNumberLogic(consignorInfo.AltPhoneNumber) &&
                        !String.IsNullOrWhiteSpace(consignorInfo.MailingAddress1) &&
                        (consignorInfo.MailingAddress1.Length <= 40) &&
                        (consignorInfo.MailingAddress2.Length <= 40) &&
                        !String.IsNullOrWhiteSpace(consignorInfo.City) &&
                        (consignorInfo.City.Length <= 30) &&
                        !String.IsNullOrWhiteSpace(consignorInfo.State) &&
                        (consignorInfo.State.Length == 2) &&
                        !String.IsNullOrWhiteSpace(consignorInfo.ZipCode) &&
                        Validation_Helper.ParseZipCodeLogic(consignorInfo.ZipCode);
            }

        }

        private void InitializeDataGrid()
        {
            var pcontroller = new PersonController();
            ObservableCollection<Person> persons = new ObservableCollection<Person>(pcontroller.QueryPersonsThatAreConsigners());
            
            var ccontroller = new ConsignorController();
            ObservableCollection<Consignor> consignors = new ObservableCollection<Consignor>(ccontroller.GetAllConsignors());

            DataGridConsignors = MergeConsignorInfo(consignors, persons);
            
        }

        //Make a collection of GameItems with info from both the Item and Game Tables
        private ObservableCollection<ConsignorInfo> MergeConsignorInfo(ObservableCollection<Consignor> consignors, ObservableCollection<Person> persons)
        {
            //Temp Collection Definition
            ObservableCollection<ConsignorInfo> tempConsignorInfos = new ObservableCollection<ConsignorInfo>();

            //Clear DataGridGames
            DataGridConsignors.Clear();

            //Make the collection
            IEnumerator<Person> p = persons.GetEnumerator();
            while (p.MoveNext())
            {
                //Get the consignor that corresponds to the current person
                Consignor currentConsignor = consignors.FirstOrDefault(c => c.Consignor_Person.Id == p.Current.Id);
                //Create an instance of a ConsignorInfo with the current consignor and person info
                var currentConsignorInfo = new ConsignorInfo(currentConsignor, p.Current);
                //Add the new instance to the collection
                tempConsignorInfos.Add(currentConsignorInfo);
            }

            return tempConsignorInfos;
        }

        #endregion
    }
}