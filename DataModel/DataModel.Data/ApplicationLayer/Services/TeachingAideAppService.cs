using DataModel.Data.DataLayer.Entities;
using DataModel.Data.TransactionalLayer.Repositories;

namespace DataModel.Data.ApplicationLayer.Services
{
    public class TeachingAideAppService : GenericItemAppService<TeachingAide>
    {
        private readonly ITeachingAideRepository _teachingAideRepository;

        public TeachingAideAppService(ITeachingAideRepository teachingAideRepository)
            : base(teachingAideRepository)
        {
            _teachingAideRepository = teachingAideRepository;
        }

        
    }
}
