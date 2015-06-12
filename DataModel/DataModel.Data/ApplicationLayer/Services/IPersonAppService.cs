using Abp.Application.Services;
using DataModel.Data.ApplicationLayer.DTO;

namespace DataModel.Data.ApplicationLayer.Services
{
    public interface IPersonAppService : IApplicationService
    {
        void UpdatePerson(UpdatePersonInput input);
        QueryPersonsThatAreConsignersOutput QueryPersonsThatAreConsigners();
        QueryPersonsThatAreMembersOutput QueryPersonsThatAreMembers();
        GetPersonOutput GetPerson(GetPersonInput input);
    }
}
