using Abp.Application.Services;
using DataModel.Data.ApplicationLayer.DTO;

namespace DataModel.Data.ApplicationLayer.Services
{
    public interface IStoreCreditPmtAppservice : IApplicationService
    {
        GetStoreCreditPmtsByConsignorIdOutput GetStoreCreditPmtsByConsignorId(GetStoreCreditPmtsByConsignorIdInput input);
    }
}
