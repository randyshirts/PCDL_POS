using Abp.Application.Services;
using DataModel.Data.ApplicationLayer.DTO;

namespace DataModel.Data.ApplicationLayer.Services
{
    public interface IStoreCreditTransactionAppService : IApplicationService
    {
        GetStoreCreditTransactionsByConsignorIdOutput GetStoreCreditTransactionsByConsignorId(GetStoreCreditTransactionsByConsignorIdInput input);

    }
}
