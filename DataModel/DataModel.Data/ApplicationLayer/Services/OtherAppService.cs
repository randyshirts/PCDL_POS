using DataModel.Data.DataLayer.Entities;
using DataModel.Data.TransactionalLayer.Repositories;

namespace DataModel.Data.ApplicationLayer.Services
{
    public class OtherAppService : GenericItemAppService<Other>
    {
        private readonly IOtherRepository _otherRepository;

        public OtherAppService(IOtherRepository otherRepository)
            : base(otherRepository)
        {
            _otherRepository = otherRepository;
        }

        
    }
}
