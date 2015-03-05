using Abp.Application.Services;
using DataModel.Data.ApplicationLayer.DTO;

namespace DataModel.Data.ApplicationLayer.Services
{
    public interface IConsignorPmtAppService : IApplicationService
    {
        AddNewConsignorPmtOutput AddNewConsignorPmt(AddNewConsignorPmtInput input);
        GetConsignorPmtByIdOutput GetConsignorPmtById(GetConsignorPmtByIdInput input);
        GetConsignorPmtsByConsignorIdOutput GetConsignorPmtsByConsignorId(GetConsignorPmtsByConsignorIdInput input);
    }
}
