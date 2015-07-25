using System;
using System.Collections.Generic;
using System.Linq;
using DataModel.Data.ApplicationLayer.DTO;
using DataModel.Data.ApplicationLayer.Services;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.TransactionalLayer.Repositories;

namespace DataModel.Data.ApplicationLayer.WpfControllers
{
    public class MemberController : IMemberController
    {
        public int AddNewMember(Member member)
        {
            var input = new AddNewMemberInput
            {
                Member = new MemberDto(member)
            };

            var emailInput = new GetPersonsByEmailInput
            {
                EmailAddress = member.Member_Person.EmailAddresses.FirstOrDefault().EmailAddress
            };

            var person = new Person();

            using (var repo = new EmailRepository())
            {
                var app = new EmailAppService(repo);
                person = app.GetPersonsByEmail(emailInput).EmailPersons
                    .Select(p => p.ConvertToPerson()).FirstOrDefault();
            }

            if (person != null)
            {
                //Add member to existing person if name matches
                if (person.FirstName == member.Member_Person.FirstName &&
                    (person.LastName == member.Member_Person.LastName))
                {
                    member.Member_Person = null;

                    var existingInput = new AddMemberToPersonInput
                    {
                        Member = new MemberDto(member),
                        Person = new PersonDto(person)
                    };
                    
                    using (var repo = new MemberRepository())
                    {
                        var app = new MemberAppService(repo);
                        if (app.AddMemberToPerson(existingInput).Result)
                        {
                            return person.Id;
                        }
                    }
                }
                throw new Exception("First and Last names do not match email address on record");
            }

            using (var repo = new MemberRepository())
            {
                var app = new MemberAppService(repo);
                return app.AddNewMember(input).Id;
            }
        }

        public void DeleteMemberById(int id)
        {
            var input = new DeleteMemberByIdInput
            {
                Id = id
            };
            using (var repo = new MemberRepository())
            {
                var app = new MemberAppService(repo);
                app.DeleteMemberById(input);
            }
        }

        public void UpdateMember(Member updatedMember)
        {
            var input = new UpdateMemberInput
            {
                Member = new MemberDto(updatedMember)
            };
            using (var repo = new MemberRepository())
            {
                var app = new MemberAppService(repo);
                app.UpdateMember(input);
            }
        }

        public Member GetMemberByName(string firstName, string lastName)
        {
            var input = new GetMemberByNameInput
            {
                FirstName = firstName,
                LastName = lastName
            };
            using (var repo = new MemberRepository())
            {
                var app = new MemberAppService(repo);
                var output = app.GetMemberByName(input);

                if(output.Member != null)
                    return output.Member.ConvertToMember();

                return null;
            }
        }

        public List<string> GetMemberNames()
        {
            using (var repo = new MemberRepository())
            {
                var app = new MemberAppService(repo);
                var output = app.GetMemberNames();

                return output.MemberNames;
            }
        }

        public List<string> GetMemberNamesLastnameFirst()
        {
            using (var repo = new MemberRepository())
            {
                var app = new MemberAppService(repo);
                var output = app.GetMemberNamesLastnameFirst();

                return output.MemberNames;
            }
        }

        public Member GetMemberByFullName(string fullName)
        {
            var input = new GetMemberByFullNameInput
            {
                FullName = fullName
            };
            using (var repo = new MemberRepository())
            {
                var app = new MemberAppService(repo);
                var output = app.GetMemberByFullName(input);

                return output.Member.ConvertToMember();
            }
        }

        public double GetMemberCreditBalance(string name)
        {
            var input = new GetMemberCreditBalanceInput
            {
                FullName = name
            };
            using (var repo = new MemberRepository())
            {
                var app = new MemberAppService(repo);
                var output = app.GetMemberCreditBalance(input);

                return output.Balance;
            }
        }

        public IEnumerable<Member> GetAllMembers()
        {
            using (var repo = new MemberRepository())
            {
                var app = new MemberAppService(repo);
                var output = app.GetAllMembers();

                return output.Members.Select(c => c.ConvertToMember()).ToList();
                
            }
        }
    }
}
