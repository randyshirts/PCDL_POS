using System;
using System.Linq;
using System.Windows;
using DataModel.Data.DataLayer.Entities;

namespace RocketPos.Inventory.Resources
{
    public class ConsignorInfo
    {
        public ConsignorInfo()
        { }

        public ConsignorInfo(Consignor consignor, Person person)
        {
            //Consignor Info
            Id = consignor.Id;
            DateAdded = consignor.DateAdded;

            //Person Info
            Consignor_Person = person;
            FirstName = person.FirstName;
            LastName = person.LastName;

            //Email Address
            EmailAddress = person.EmailAddresses.FirstOrDefault().EmailAddress;

            //Mailing Address
            MailingAddress1 = person.MailingAddresses.FirstOrDefault().MailingAddress1;
            MailingAddress2 = person.MailingAddresses.FirstOrDefault().MailingAddress2;
            City = person.MailingAddresses.FirstOrDefault().City;
            State = person.MailingAddresses.FirstOrDefault().State;
            ZipCode = person.MailingAddresses.FirstOrDefault().ZipCode;

            //Phone Numbers
            CellPhoneNumber = person.PhoneNumbers.CellPhoneNumber;
            HomePhoneNumber = person.PhoneNumbers.HomePhoneNumber;
            AltPhoneNumber = person.PhoneNumbers.AltPhoneNumber;
        }
        
        
        //Define members
        public int Id { get; set; }
        public DateTime DateAdded { get; set; }
        public Person Consignor_Person { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string MailingAddress1 { get; set; }
        public string MailingAddress2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string CellPhoneNumber { get; set; }
        public string HomePhoneNumber { get; set; }
        public string AltPhoneNumber { get; set; }

        public bool SetProperty(string column, string text)
        {
            bool success = false;
            
            switch(column)
            {
                case "First Name":
                    {
                        if (!String.IsNullOrWhiteSpace(text))
                        {
                            //Change the member property that is displayed in grid
                            this.FirstName = text;

                            //change the value inside the Person class
                            this.Consignor_Person.FirstName = text;

                            success = true;
                        }
                        else
                            success = false;
                        break;
                    }
                case "Last Name":
                    {
                        if (!String.IsNullOrWhiteSpace(text))
                        {
                            //Change the member property that is displayed in grid
                            this.LastName = text;

                            //change the value inside the Person class
                            this.Consignor_Person.LastName = text;

                            success = true;
                        }
                        else
                            success = false;                   
                        break;
                    }
                case "Email":
                    {
                        if (!String.IsNullOrWhiteSpace(text))
                        {
                            //Change the member property that is displayed in grid
                            this.EmailAddress = text;
                            
                            //change the value inside the Person class
                            this.Consignor_Person.EmailAddresses.FirstOrDefault().EmailAddress = text;
                            
                            success = true;
                        }
                        else
                            success = false;   
                        
                        break;
                    }
                case "Mailing Address 1":
                    {
                        if (!String.IsNullOrWhiteSpace(text))
                        {
                            //Change the member property that is displayed in grid
                            this.MailingAddress1 = text;

                            //change the value inside the Person class
                            this.Consignor_Person.MailingAddresses.FirstOrDefault().MailingAddress1 = text;

                            success = true;
                        }
                        else
                            success = false;                     
                        break;
                    }
                case "Mailing Address 2":
                    {
                        if (!String.IsNullOrWhiteSpace(text))
                        {
                            //Change the member property that is displayed in grid
                            this.MailingAddress2 = text;

                            //change the value inside the Person class
                            this.Consignor_Person.MailingAddresses.FirstOrDefault().MailingAddress2 = text;

                            success = true;
                        }
                        else
                        {
                            //Change the member property that is displayed in grid
                            this.MailingAddress2 = null;

                            //change the value inside the Person class
                            this.Consignor_Person.MailingAddresses.FirstOrDefault().MailingAddress2 = null;

                            success = true;     //This field is nullable
                        }
                        break;
                    }
                
                case "City":
                    {
                        if (!String.IsNullOrWhiteSpace(text))
                        {
                            //Change the member property that is displayed in grid
                            this.City = text;

                            //change the value inside the Person class
                            this.Consignor_Person.MailingAddresses.FirstOrDefault().City = text;

                            success = true;
                        }
                        else 
                            success = false;
                                 
                        break;
                    }
                case "State":
                    {
                        if (!String.IsNullOrWhiteSpace(text))
                        {
                            //Change the member property that is displayed in grid
                            this.State = text;

                            //change the value inside the Person class
                            this.Consignor_Person.MailingAddresses.FirstOrDefault().State = text;

                            success = true;
                        }
                        else
                            success = false;

                        break;
                    }
                case "ZipCode":
                    {
                        if (!String.IsNullOrWhiteSpace(text))
                        {
                            //Change the member property that is displayed in grid
                            this.ZipCode = text;

                            //change the value inside the Person class
                            this.Consignor_Person.MailingAddresses.FirstOrDefault().ZipCode = text;

                            success = true;
                        }
                        else
                            success = false;

                        break;
                    }
                
                case "DateAdded":
                    {
                        if (!String.IsNullOrEmpty(text))
                        {
                            var orig = this.DateAdded;
                            DateTime d;
                            this.DateAdded = DateTime.TryParse(text, out d) ? d : DateTime.MinValue;
                            if (this.DateAdded == DateTime.MinValue)
                            {
                                success = false;
                                this.DateAdded = orig;
                            }
                            else
                                success = true;
                        }
                        else
                            success = false;    //Non nullable
                        break;
                    }
                case "Cell Phone":
                    {
                        if (!String.IsNullOrWhiteSpace(text))
                        {
                            //Change the member property that is displayed in grid
                            this.CellPhoneNumber = text;

                            //change the value inside the Person class
                            this.Consignor_Person.PhoneNumbers.CellPhoneNumber = text;

                            success = true;
                        }
                        else
                            success = false;

                        break;

                    }
                case "Home Phone":
                    {
                        if (!String.IsNullOrWhiteSpace(text))
                        {
                            //Change the member property that is displayed in grid
                            this.HomePhoneNumber = text;

                            //change the value inside the Person class
                            this.Consignor_Person.PhoneNumbers.HomePhoneNumber = text;

                            success = true;
                        }
                        else
                        {
                            //If empty set to null 
                            //Change the member property that is displayed in grid
                            this.HomePhoneNumber = null;

                            //change the value inside the Person class
                            this.Consignor_Person.PhoneNumbers.HomePhoneNumber = null;

                            success = true;     //This field is nullable
                     
                        }
                        
                        break;
                    }
                case "Alt Phone":
                    {
                        if (!String.IsNullOrWhiteSpace(text))
                        {
                            //Change the member property that is displayed in grid
                            this.AltPhoneNumber = text;

                            //change the value inside the Person class
                            this.Consignor_Person.PhoneNumbers.AltPhoneNumber = text;

                            success = true;
                        }
                        else
                        {                       
                            //Change the member property that is displayed in grid
                            this.AltPhoneNumber = null;

                            //change the value inside the Person class
                            this.Consignor_Person.PhoneNumbers.AltPhoneNumber = null;

                            success = true;     //This field is nullable
                        }
                        
                        break;
                    }

            }
            return success;
        }

        public Consignor ConvertConsignorInfoToConsignor(ConsignorInfo consignorInfo)
        {
            var newConsignor = new Consignor
            {
                Id = consignorInfo.Id,
                DateAdded = consignorInfo.DateAdded,
                Consignor_Person = consignorInfo.Consignor_Person
            };

            return newConsignor;
        }

        public Person ConvertConsignorInfoToPerson(ConsignorInfo consignorInfo)
        {
            return Consignor_Person;
        }

        public void ReportError(string column, string text)
        {
            MessageBoxResult result = new MessageBoxResult();
            if (!String.IsNullOrEmpty(text))
            {
                switch (column)
                {
                    case "First Name":
                    case "Last Name":
                        {
                            result = MessageBox.Show("Input Error - '" + text +
                                                        "' - Name must be 20 characters or less - Try again", "Error");
                            break;
                        }
                    case "Email":
                        {
                            result = MessageBox.Show("Input Error - '" + text +
                                                        "' - Email must contain '@','.' characters (e.g. johndoe@gmail.com) " +
                                                        "and it must be less than 40 characters in length - Try again", "Error");
                            break;
                        }
                    case "Mailing Address 1":
                    case "Mailing Address 2":
                        {
                            result = MessageBox.Show("Input Error - '" + text +
                                                        "' - Mailing Address must be 40 characters or less - Try again", "Error");
                            break;
                        }
                    case "Cell Phone":
                    case "Home Phone":
                    case "Alt Phone":
                        {
                            result = MessageBox.Show("Input Error - '" + text +
                                                        "' - Phone Number must be 10 characters and it must contain only numbers (e.g. 2565559999) - Try again", "Error");
                            break;
                        }
                    case "City":
                        {
                            result = MessageBox.Show("Input Error - '" + text +
                                                        "' - City must be 30 characters or less - Try again", "Error");
                            break;
                        }
                    case "State":
                        {
                            result = MessageBox.Show("Input Error - '" + text +
                                                        "' - State Abbreviation must be 2 characters - Try again", "Error");
                            break;
                        }
                    case "ZipCode":
                        {
                            result = MessageBox.Show("Input Error - '" + text +
                                                        "' - Zip Code must be 5 characters and must contain only numbers (e.g. 35824) - Try again", "Error");
                            break;
                        }
                    default:
                        {
                            result = MessageBox.Show("Input Error - '" + text + "' - Try again", "Error");
                            break;
                        }
                }
            }
        }
    }


}
