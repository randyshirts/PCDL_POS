using System.Linq;
using DataModel.Data.ApplicationLayer.DTO;
using DataModel.Data.TransactionalLayer.Repositories;

namespace DataModel.Data.ApplicationLayer.Services
{
    public class MemberAppService : PcdAppServiceBase, IMemberAppService
    {
        private readonly IMemberRepository _memberRepository;

        public MemberAppService(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }
        
        public AddNewMemberOutput AddNewMember(AddNewMemberInput input)
        {
            return new AddNewMemberOutput
            {
                Id = _memberRepository.AddNewMember(input.Member.ConvertToMember())
            };
        }

        public AddMemberToPersonOutput AddMemberToPerson(AddMemberToPersonInput input)
        {
            return new AddMemberToPersonOutput
            {
                Result = _memberRepository.AddMemberToPerson(input.Person.ConvertToPerson(), input.Member.ConvertToMember())   
            };
        }

        public void DeleteMemberById(DeleteMemberByIdInput input)
        {
            _memberRepository.DeleteMemberById(input.Id);
        }

        public void UpdateMember(UpdateMemberInput input)
        {
            _memberRepository.UpdateMember(input.Member.ConvertToMember());
        }

        public GetMemberByNameOutput GetMemberByName(GetMemberByNameInput input)
        {
            var member = new MemberDto(_memberRepository.GetMemberByName(input.FirstName, input.LastName));

            if (member.Member_Person == null)
                return new GetMemberByNameOutput
                {
                    Member = null
                };

            var person = new PersonDto(member.Member_Person);
            member.Member_Person = person.ConvertToPerson();
            
            return new GetMemberByNameOutput
            {
                Member = member
            };
        }

        public GetMemberNamesOutput GetMemberNames()
        {
            return new GetMemberNamesOutput
            {
                MemberNames = _memberRepository.GetMemberNames().ToList()
            };
        }

        public GetMemberNamesLastnameFirstOutput GetMemberNamesLastnameFirst()
        {
            return new GetMemberNamesLastnameFirstOutput
            {
                MemberNames = _memberRepository.GetMemberNamesLastnameFirst().ToList()
            };
        }

        public GetMemberByFullNameOutput GetMemberByFullName(GetMemberByFullNameInput input)
        {
            var member = new MemberDto(_memberRepository.GetMemberByFullName(input.FullName));
            var person = new PersonDto(member.Member_Person);
            member.Member_Person = person.ConvertToPerson();
            
            return new GetMemberByFullNameOutput
            {
                Member = member
            };
        }

        public GetMemberCreditBalanceOutput GetMemberCreditBalance(GetMemberCreditBalanceInput input)
        {
            return new GetMemberCreditBalanceOutput
            {
                Balance = _memberRepository.GetMemberCreditBalance(input.FullName)
            };
        }

        public GetAllMembersOutput GetAllMembers()
        {
            var output = _memberRepository.GetAllList();
            var list = output.Select(c => (new MemberDto(c))).ToList();
            
            return new GetAllMembersOutput
            {
                Members = list
            };
        }

        public GetMemberByEmailOutput GetMemberByEmail(GetMemberByEmailInput input)
        {
            return new GetMemberByEmailOutput
            {
                Member = new MemberDto(_memberRepository.GetMemberByEmail(input.Email))
            };
        }
    }
}
