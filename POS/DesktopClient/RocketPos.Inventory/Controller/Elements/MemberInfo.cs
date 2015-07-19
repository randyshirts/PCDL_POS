using System;
using System.Linq;
using System.Windows;
using DataModel.Data.DataLayer.Entities;

namespace RocketPos.Inventory.Resources
{
    public class MemberInfo
    {
        public MemberInfo()
        { }

        public MemberInfo(Member member, Person person)
        {
            //Member Info
            Id = member.Id;
            DateAdded = member.DateAdded;
            RenewDate = member.RenewDate;
            StartDate = member.StartDate;
            MemberType = member.MemberType;

            //Person Info
            Member_Person = person;
            FirstName = person.FirstName;
            LastName = person.LastName;

            //Email Address
            if (person.EmailAddresses.Any())
            {
                EmailAddress = person.EmailAddresses.FirstOrDefault().EmailAddress;
            }

            //Mailing Address
            if (person.MailingAddresses.Any())
            {
                MailingAddress1 = person.MailingAddresses.FirstOrDefault().MailingAddress1;
                MailingAddress2 = person.MailingAddresses.FirstOrDefault().MailingAddress2;
                City = person.MailingAddresses.FirstOrDefault().City;
                State = person.MailingAddresses.FirstOrDefault().State;
                ZipCode = person.MailingAddresses.FirstOrDefault().ZipCode;
            }
            //Phone Numbers
            CellPhoneNumber = person.PhoneNumbers.CellPhoneNumber;
            HomePhoneNumber = person.PhoneNumbers.HomePhoneNumber;
            AltPhoneNumber = person.PhoneNumbers.AltPhoneNumber;
        }
        
        
        //Define members
        public int Id { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime RenewDate { get; set; }
        public DateTime StartDate { get; set; }
        public int MemberType { get; set; }
        public Person Member_Person { get; set; }
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
                            FirstName = text;

                            //change the value inside the Person class
                            Member_Person.FirstName = text;

                            success = true;
                        }
     
                        break;
                    }
                case "Last Name":
                    {
                        if (!String.IsNullOrWhiteSpace(text))
                        {
                            //Change the member property that is displayed in grid
                            LastName = text;

                            //change the value inside the Person class
                            Member_Person.LastName = text;

                            success = true;
                        }
                  
                        break;
                    }
                case "Member Type":
                    {
                        //if (!String.IsNullOrWhiteSpace(text))
                        //{
                        //    //Change the member property that is displayed in grid
                        //    LastName = text;

                        //    //change the value inside the Person class
                        //    Member_Person.LastName = text;

                        //    success = true;
                        //}

                        break;
                    }
                case "Email":
                    {
                        if (!String.IsNullOrWhiteSpace(text))
                        {
                            //Change the member property that is displayed in grid
                            EmailAddress = text;
                            
                            //change the value inside the Person class
                            Member_Person.EmailAddresses.FirstOrDefault().EmailAddress = text;
                            
                            success = true;
                        }
   
                        
                        break;
                    }
                case "Mailing Address 1":
                    {
                        if (!String.IsNullOrWhiteSpace(text))
                        {
                            //Change the member property that is displayed in grid
                            MailingAddress1 = text;

                            //change the value inside the Person class
                            Member_Person.MailingAddresses.FirstOrDefault().MailingAddress1 = text;

                            success = true;
                        }
                    
                        break;
                    }
                case "Mailing Address 2":
                    {
                        if (!String.IsNullOrWhiteSpace(text))
                        {
                            //Change the member property that is displayed in grid
                            MailingAddress2 = text;

                            //change the value inside the Person class
                            Member_Person.MailingAddresses.FirstOrDefault().MailingAddress2 = text;

                            success = true;
                        }
                        else
                        {
                            //Change the member property that is displayed in grid
                            MailingAddress2 = null;

                            //change the value inside the Person class
                            Member_Person.MailingAddresses.FirstOrDefault().MailingAddress2 = null;

                            success = true;     //This field is nullable
                        }
                        break;
                    }
                
                case "City":
                    {
                        if (!String.IsNullOrWhiteSpace(text))
                        {
                            //Change the member property that is displayed in grid
                            City = text;

                            //change the value inside the Person class
                            Member_Person.MailingAddresses.FirstOrDefault().City = text;

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
                            State = text;

                            //change the value inside the Person class
                            Member_Person.MailingAddresses.FirstOrDefault().State = text;

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
                            ZipCode = text;

                            //change the value inside the Person class
                            Member_Person.MailingAddresses.FirstOrDefault().ZipCode = text;

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
                            var orig = DateAdded;
                            DateTime d;
                            DateAdded = DateTime.TryParse(text, out d) ? d : DateTime.MinValue;
                            if (DateAdded == DateTime.MinValue)
                            {
                                DateAdded = orig;
                            }
                            else
                                success = true;
                        } //Non nullable
                        break;
                    }
                case "Renew Date":
                    {
                        if (!String.IsNullOrEmpty(text))
                        {
                            var orig = RenewDate;
                            DateTime d;
                            RenewDate = DateTime.TryParse(text, out d) ? d : DateTime.MinValue;
                            if (RenewDate == DateTime.MinValue)
                            {
                                RenewDate = orig;
                            }
                            else
                                success = true;
                        } //Non nullable
                        break;
                    }
                case "Start Date":
                    {
                        if (!String.IsNullOrEmpty(text))
                        {
                            var orig = StartDate;
                            DateTime d;
                            StartDate = DateTime.TryParse(text, out d) ? d : DateTime.MinValue;
                            if (StartDate == DateTime.MinValue)
                            {
                                StartDate = orig;
                            }
                            else
                                success = true;
                        } //Non nullable
                        break;
                    }
                case "Cell Phone":
                    {
                        if (!String.IsNullOrWhiteSpace(text))
                        {
                            //Change the member property that is displayed in grid
                            CellPhoneNumber = text;

                            //change the value inside the Person class
                            Member_Person.PhoneNumbers.CellPhoneNumber = text;

                            success = true;
                        }

                        break;

                    }
                case "Home Phone":
                    {
                        if (!String.IsNullOrWhiteSpace(text))
                        {
                            //Change the member property that is displayed in grid
                            HomePhoneNumber = text;

                            //change the value inside the Person class
                            Member_Person.PhoneNumbers.HomePhoneNumber = text;

                            success = true;
                        }
                        else
                        {
                            //If empty set to null 
                            //Change the member property that is displayed in grid
                            HomePhoneNumber = null;

                            //change the value inside the Person class
                            Member_Person.PhoneNumbers.HomePhoneNumber = null;

                            success = true;     //This field is nullable
                     
                        }
                        
                        break;
                    }
                case "Alt Phone":
                    {
                        if (!String.IsNullOrWhiteSpace(text))
                        {
                            //Change the member property that is displayed in grid
                            AltPhoneNumber = text;

                            //change the value inside the Person class
                            Member_Person.PhoneNumbers.AltPhoneNumber = text;

                            success = true;
                        }
                        else
                        {                       
                            //Change the member property that is displayed in grid
                            AltPhoneNumber = null;

                            //change the value inside the Person class
                            Member_Person.PhoneNumbers.AltPhoneNumber = null;

                            success = true;     //This field is nullable
                        }
                        
                        break;
                    }

            }
            return success;
        }

        public Member ConvertMemberInfoToMember(MemberInfo memberInfo)
        {
            var newMember = new Member
            {
                Id = memberInfo.Id,
                DateAdded = memberInfo.DateAdded,
                RenewDate = memberInfo.RenewDate,
                MemberType = memberInfo.MemberType,
                Member_Person = memberInfo.Member_Person
            };

            return newMember;
        }

        public Person ConvertMemberInfoToPerson(MemberInfo memberInfo)
        {
            return Member_Person;
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
