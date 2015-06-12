using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.ApplicationLayer.DTO
{
    public class MemberDto : EntityDto
    {
        public MemberDto(Member member)
        {
            if (member != null)
            {
                Id = member.Id;
                DateAdded = member.DateAdded;
                Member_Person = member.Member_Person;
                RenewDate = member.RenewDate;
                StartDate = member.StartDate;
                MemberType = member.MemberType;
            }
        }
           
        public DateTime DateAdded { get; set; }
        public Person Member_Person { get; set; }
        public DateTime RenewDate { get; set; }
        public DateTime StartDate { get; set; }
        public int MemberType { get; set; }

        public Member ConvertToMember()
        {
            return new Member()
            {
                Id = Id,
                DateAdded = DateAdded,
                RenewDate = RenewDate,
                StartDate = StartDate,
                Member_Person = Member_Person,
                MemberType = MemberType
            };
        }

    }

    public class AddNewMemberOutput : IOutputDto
    {
        public int Id { get; set; }
    }

    public class AddNewMemberInput : IInputDto
    {
        public MemberDto Member { get; set; }
    }

    public class AddMemberToPersonOutput : IOutputDto
    {
        public bool Result { get; set; }
    }

    public class AddMemberToPersonInput : IInputDto
    {
        public PersonDto Person { get; set; }
        public MemberDto Member { get; set; }
    }

    public class DeleteMemberByIdInput : IInputDto
    {
        public int Id { get; set; }
    }

    public class UpdateMemberInput : IInputDto
    {
        public MemberDto Member { get; set; }
    }

    public class GetMemberByNameOutput : IOutputDto
    {
        public MemberDto Member { get; set; }
    }

    public class GetMemberByNameInput : IInputDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class GetMemberNamesOutput : IOutputDto
    {
        public List<string> MemberNames { get; set; } 
    }

    public class GetMemberNamesLastnameFirstOutput : IOutputDto
    {
        public List<string> MemberNames { get; set; } 
    }

    public class GetMemberByFullNameOutput : IOutputDto
    {
        public MemberDto Member { get; set; }
    }

    public class GetMemberByFullNameInput : IInputDto
    {
        public string FullName { get; set; }
    }

    public class GetMemberCreditBalanceOutput : IOutputDto
    {
        public double Balance { get; set; }
    }

    public class GetMemberCreditBalanceInput : IInputDto
    {
        public string FullName { get; set; }
    }

    public class GetAllMembersOutput : IOutputDto
    {
        public List<MemberDto> Members { get; set; } 
    }

    public class GetMemberByEmailInput : IInputDto
    {
        public string Email { get; set; }
    }

    public class GetMemberByEmailOutput : IOutputDto
    {
        public MemberDto Member { get; set; }
    }
}
