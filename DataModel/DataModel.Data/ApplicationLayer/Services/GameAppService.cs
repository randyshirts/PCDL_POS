using System;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.TransactionalLayer.Repositories;

namespace DataModel.Data.ApplicationLayer.Services
{
    public class GameAppService : GenericItemAppService<Game>
    {
        private readonly IGameRepository _gameRepository;

        public GameAppService(IGameRepository gameRepository)
            : base(gameRepository)
        {
            _gameRepository = gameRepository;
        }

        
    }
}
