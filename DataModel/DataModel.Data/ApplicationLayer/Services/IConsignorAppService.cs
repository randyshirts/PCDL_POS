using System.Collections.Generic;
using DataModel.Data.ApplicationLayer.DTO;

namespace DataModel.Data.ApplicationLayer.Services
{
    public interface IConsignorAppService
    {
        AddNewConsignorOutput AddNewConsignor(AddNewConsignorInput input);
        AddConsignorToPersonOutput AddConsignorToPerson(AddConsignorToPersonInput input);
        void DeleteConsignerById(DeleteConsignerByIdInput input);
        void UpdateConsignor(UpdateConsignorInput input);
        GetConsignorByNameOutput GetConsignorByName(GetConsignorByNameInput input);
        GetConsignorNamesOutput GetConsignorNames();
        GetConsignorNamesLastnameFirstOutput GetConsignorNamesLastnameFirst();
        GetConsignorByFullNameOutput GetConsignorByFullName(GetConsignorByFullNameInput input);
        GetConsignorCreditBalanceOutput GetConsignorCreditBalance(GetConsignorCreditBalanceInput input);
        GetAllConsignorsOutput GetAllConsignors();
    }
}
