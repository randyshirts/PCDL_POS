using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Windows;
using DataModel.Data.ApplicationLayer.WpfControllers;
using DataModel.Data.DataLayer.Entities;
using RocketPos.Common.Foundation;
using RocketPos.Common.Helpers;
using RocketPos.Inventory.Resources;
using GalaSoft.MvvmLight.Command;
using System.Windows.Controls;
using System.Windows.Input;
using MemberInfo = RocketPos.Inventory.Resources.MemberInfo;

namespace Inventory.ViewModels.AddMember.ViewModels
{
    public class AddMemberVm : ViewModel
    {
        public static readonly Guid Token = Guid.NewGuid();         //So others know messages came from this instance

        //private EnumsAndLists myEnumsLists = new EnumsAndLists();   //Various lists and enums used by the application

        //List for sources of ComboBoxes
        private ComboBoxListValues stateComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _nameComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _memberTypeComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _memberTermComboValues = new ComboBoxListValues();

        public AddMemberVm()
        {
            Members = new ObservableCollection<Member>();
            DataGridMembers = new ObservableCollection<MemberInfo>();

            //Initialize ComboBoxes
            stateComboValues.InitializeComboBox(EnumsAndLists.States);
            _memberTypeComboValues.InitializeComboBox(EnumsAndLists.MemberTypes.Keys.ToList());
            _memberTermComboValues.InitializeComboBox(EnumsAndLists.MemberTerms);

            //var controller = new PersonController();
            //AllPersons = controller.GetAllPersons().ToList();
            //var names = StringHelpers.CombineNames(AllPersons);
            //var nameComboList = new List<string>(names);
            ////NameComboList.Insert(0, "All");
            //_nameComboValues.InitializeComboBox(nameComboList);

            //Register for messages
            //Messenger.Default.Register<PropertySetter>(this, AddGameVM.Token, msg => SetGameProperty(msg.PropertyName, (string)msg.PropertyValue));         

            //Fill grid with all members on record
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
        /// Gets the collection of Member entities.
        /// </summary>
        public ICollection<Member> Members { get; private set; }

        #region AddMember Stuff
        /// <summary>
        /// Gets the stateComboValues list.
        /// </summary>
        public List<ComboBoxListValues> MemberTypeComboValues
        {
            get { return _memberTypeComboValues.ComboValues; }
            private set { _memberTypeComboValues.ComboValues = value; }
        }

        /// <summary>
        /// Gets the stateComboValues list.
        /// </summary>
        public List<ComboBoxListValues> MemberTermComboValues
        {
            get { return _memberTermComboValues.ComboValues; }
            private set { _memberTermComboValues.ComboValues = value; }
        }

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

        public DateTime StartDate { get; set; }
        public DateTime RenewDate { get; set; }
        private int _memberTypeKey { get; set; }

        private string _memberType;
        [Required]
        public string MemberType
        {
            get { return _memberType; }
            set
            {
                _memberType = value;
                if(_memberType != null)
                    _memberTypeKey = EnumsAndLists.MemberTypes[_memberType];
            }
        }

        private string _memberTerm;
        [Required]
        public string MemberTerm
        {
            get { return _memberTerm; }
            set
            {
                _memberTerm = value;
                if (_memberTerm != null)
                {
                    if (_memberTerm.StartsWith("Full-Year"))
                    {
                        RenewDate = DateTime.Parse("7/1/" + (DateTime.Now.Year + 1));
                        StartDate = DateTime.Parse("7/1/" + DateTime.Now.Year);
                    }
                    else if (_memberTerm.StartsWith("Fall Semester") && (DateTime.Now.Month < 10))
                    {
                        RenewDate = DateTime.Parse("1/1/" + (DateTime.Now.Year + 1));
                        StartDate = DateTime.Parse("7/1/" + DateTime.Now.Year);
                    }
                    else if (_memberTerm.StartsWith("Fall Semester") && (DateTime.Now.Month >= 10))
                    {
                        RenewDate = DateTime.Parse("1/1/" + (DateTime.Now.Year + 2));
                        StartDate = DateTime.Parse("7/1/" + (DateTime.Now.Year + 1));
                    }
                    else if (_memberTerm.StartsWith("Spring Semester") && (DateTime.Now.Month >= 6))
                    {
                        RenewDate = DateTime.Parse("7/1/" + (DateTime.Now.Year + 1));
                        StartDate = DateTime.Parse("1/1/" + (DateTime.Now.Year + 1));
                    }
                    else if (_memberTerm.StartsWith("Spring Semester") && (DateTime.Now.Month < 6))
                    {
                        RenewDate = DateTime.Parse("7/1/" + DateTime.Now.Year);
                        StartDate = DateTime.Parse("1/1/" + DateTime.Now.Year);
                    }
                }
            }
        }
        public List<Person> AllPersons { get; set; }
        private Person SelectedPerson { get; set; }

        /// <summary>
        /// Gets or sets the date the member was added.
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
        public ActionCommand AddMemberCommand
        {
            get
            {
                return new ActionCommand(p => AddMember(FirstName, LastName, EmailAddress, CellPhoneNumber,
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

                if (FirstName.Contains(" ")) { FirstName.Replace(" ", String.Empty); }
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

        private void AddMember(string firstName, string lastName, string emailAddress, string cellPhoneNumber,
                                  string homePhoneNumber, string altPhoneNumber, string mailingAddress1,
                                  string mailingAddress2, string city, string state, string zipCode)
        {


            DateTime dateAdded;
            if (!(DateTime.TryParse(DateAdded, out dateAdded)))
                dateAdded = DateTime.Now;
           
            //int memberType = 1;

            Member member = new Member
            {
                DateAdded = dateAdded,
                RenewDate = RenewDate,
                StartDate = StartDate,
                MemberType = _memberTypeKey,

                Member_Person = new Person
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
                var controller = new MemberController();
                var id = controller.AddNewMember(member);
                var pc = new PersonController();
                var person = pc.GetPerson(id);
                MemberInfo memberInfo = new MemberInfo(member, person);
                DataGridMembers.Add(memberInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "AddMemberException");
            }


        }

        

        #endregion

        #region DataGrid Stuff
        /// <summary>
        /// Gets the collection of MemberInfo entities.
        /// </summary>
        public ObservableCollection<MemberInfo> DataGridMembers { get; set; }

        /// <summary>
        /// Gets the selectedItem row from the datagrid.
        /// </summary>
        private MemberInfo selectedMember;
        public MemberInfo SelectedMember
        {
            get { return selectedMember; }
            set
            {
                selectedMember = value;
                OnSelected();
            }
        }

        private void OnSelected()
        {
            //if (SelectedMember != null)
            //    Messenger.Default.Send(new PropertySetter("GameImage", SelectedGameItem.GameImage), Token);
        }

        /// <summary>
        /// Gets the command that allows a cell to be updated.
        /// </summary>
        public RelayCommand<DataGridCellEditEndingEventArgs> CellEditEndingCommand
        {
            get
            {
                return new RelayCommand<DataGridCellEditEndingEventArgs>(UpdateMemberInfo);
            }
        }

        /// <summary>
        /// Gets the command that allows a row to be deleted.
        /// </summary>
        public RelayCommand DeleteSelectedCommand
        {
            get
            {
                return new RelayCommand(DeleteMemberInfo);
            }
        }

        private void DeleteMemberInfo()
        {
            if (MessageBox.Show("Deletions are permanent. Are you sure you want to delete this item? ", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;
            if (null != SelectedMember)
            {
                var memberInfo = DataGridMembers.FirstOrDefault(bi => bi.Id == SelectedMember.Id);
                if (memberInfo != null)
                {
                    var tempMembers = DataGridMembers;
                    var gridIndex = tempMembers.IndexOf(memberInfo);
                    if ((gridIndex + 1) == tempMembers.Count)
                        gridIndex--;
                    tempMembers.Remove(memberInfo);
                    DataGridMembers = tempMembers;
                    SelectedMember = DataGridMembers.ElementAtOrDefault(gridIndex);

                    var controller = new MemberController();
                    controller.DeleteMemberById(memberInfo.Id);
                    
                }
            }
        }

        private void UpdateMemberInfo(DataGridCellEditEndingEventArgs e)
        {
            var text = WpfHelpers.GetTextFromCellEditingEventArgs(e);

            //Get the Column header
            var column = e.Column.Header.ToString();

            //Add info to new MemberInfo
            var memberInfo = e.Row.Item as MemberInfo;
            var success = memberInfo.SetProperty(column, text);

            if (MemberInfoIsValid(memberInfo) && success)
            {
                switch (column)
                {
                    case "DateAdded":
                        {
                            var member = memberInfo.ConvertMemberInfoToMember((MemberInfo)(e.Row.Item));
                            var controller = new MemberController();
                            controller.UpdateMember(member);
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
                            var person = memberInfo.ConvertMemberInfoToPerson((MemberInfo)(e.Row.Item));
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
                memberInfo.ReportError(column, text);
            }

        }

        private bool MemberInfoIsValid(MemberInfo memberInfo)
        {
            EmailAddressAttribute email = new EmailAddressAttribute();//memberInfo.EmailAddress

            if (String.IsNullOrEmpty(memberInfo.MailingAddress2))
            {
                return !String.IsNullOrWhiteSpace(memberInfo.FirstName) &&
                        !String.IsNullOrWhiteSpace(memberInfo.LastName) &&
                        (memberInfo.FirstName.Length <= 20) &&
                        (memberInfo.LastName.Length <= 20) &&
                        email.IsValid(memberInfo.EmailAddress) &&
                        (memberInfo.EmailAddress.Length <= 40) &&
                        (!String.IsNullOrWhiteSpace(memberInfo.CellPhoneNumber) || (!String.IsNullOrWhiteSpace(memberInfo.HomePhoneNumber))) &&
                        Validation_Helper.ParsePhoneNumberLogic(memberInfo.CellPhoneNumber) &&
                        Validation_Helper.ParsePhoneNumberLogic(memberInfo.HomePhoneNumber) &&
                        Validation_Helper.ParsePhoneNumberLogic(memberInfo.AltPhoneNumber) &&
                        !String.IsNullOrWhiteSpace(memberInfo.MailingAddress1) &&
                        (memberInfo.MailingAddress1.Length <= 40) &&
                        !String.IsNullOrWhiteSpace(memberInfo.City) &&
                        (memberInfo.City.Length <= 30) &&
                        !String.IsNullOrWhiteSpace(memberInfo.State) &&
                        (memberInfo.State.Length == 2) &&
                        !String.IsNullOrWhiteSpace(memberInfo.ZipCode) &&
                        Validation_Helper.ParseZipCodeLogic(memberInfo.ZipCode);
            }
            else
            {
                return !String.IsNullOrWhiteSpace(memberInfo.FirstName) &&
                        !String.IsNullOrWhiteSpace(memberInfo.LastName) &&
                        (memberInfo.FirstName.Length <= 20) &&
                        (memberInfo.LastName.Length <= 20) &&
                        email.IsValid(memberInfo.EmailAddress) &&
                        (memberInfo.EmailAddress.Length <= 40) &&
                        (!String.IsNullOrWhiteSpace(memberInfo.CellPhoneNumber) || (!String.IsNullOrWhiteSpace(memberInfo.HomePhoneNumber))) &&
                        Validation_Helper.ParsePhoneNumberLogic(memberInfo.CellPhoneNumber) &&
                        Validation_Helper.ParsePhoneNumberLogic(memberInfo.HomePhoneNumber) &&
                        Validation_Helper.ParsePhoneNumberLogic(memberInfo.AltPhoneNumber) &&
                        !String.IsNullOrWhiteSpace(memberInfo.MailingAddress1) &&
                        (memberInfo.MailingAddress1.Length <= 40) &&
                        (memberInfo.MailingAddress2.Length <= 40) &&
                        !String.IsNullOrWhiteSpace(memberInfo.City) &&
                        (memberInfo.City.Length <= 30) &&
                        !String.IsNullOrWhiteSpace(memberInfo.State) &&
                        (memberInfo.State.Length == 2) &&
                        !String.IsNullOrWhiteSpace(memberInfo.ZipCode) &&
                        Validation_Helper.ParseZipCodeLogic(memberInfo.ZipCode);
            }

        }

        private void InitializeDataGrid()
        {
            var pcontroller = new PersonController();
            ObservableCollection<Person> persons = new ObservableCollection<Person>(pcontroller.QueryPersonsThatAreMembers());
            
            var ccontroller = new MemberController();
            ObservableCollection<Member> members = new ObservableCollection<Member>(ccontroller.GetAllMembers());

            DataGridMembers = MergeMemberInfo(members, persons);
            
        }

        //Make a collection of GameItems with info from both the Item and Game Tables
        private ObservableCollection<MemberInfo> MergeMemberInfo(ObservableCollection<Member> members, ObservableCollection<Person> persons)
        {
            //Temp Collection Definition
            ObservableCollection<MemberInfo> tempMemberInfos = new ObservableCollection<MemberInfo>();

            //Clear DataGridGames
            DataGridMembers.Clear();

            //Make the collection
            IEnumerator<Person> p = persons.GetEnumerator();
            while (p.MoveNext())
            {
                //Get the member that corresponds to the current person
                Member currentMember = members.FirstOrDefault(c => c.Member_Person.Id == p.Current.Id);
                //Create an instance of a MemberInfo with the current member and person info
                var currentMemberInfo = new MemberInfo(currentMember, p.Current);
                //Add the new instance to the collection
                tempMemberInfos.Add(currentMemberInfo);
            }

            return tempMemberInfos;
        }

        #endregion
    }
    
}
