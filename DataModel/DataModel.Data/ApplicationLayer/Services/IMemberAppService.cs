using System.Collections.Generic;
using DataModel.Data.ApplicationLayer.DTO;

namespace DataModel.Data.ApplicationLayer.Services
{
    public interface IMemberAppService
    {
        AddNewMemberOutput AddNewMember(AddNewMemberInput input);
        AddMemberToPersonOutput AddMemberToPerson(AddMemberToPersonInput input);
        void DeleteMemberById(DeleteMemberByIdInput input);
        void UpdateMember(UpdateMemberInput input);
        GetMemberByNameOutput GetMemberByName(GetMemberByNameInput input);
        GetMemberNamesOutput GetMemberNames();
        GetMemberNamesLastnameFirstOutput GetMemberNamesLastnameFirst();
        GetMemberByFullNameOutput GetMemberByFullName(GetMemberByFullNameInput input);
        GetMemberCreditBalanceOutput GetMemberCreditBalance(GetMemberCreditBalanceInput input);
        GetAllMembersOutput GetAllMembers();
    }
}
