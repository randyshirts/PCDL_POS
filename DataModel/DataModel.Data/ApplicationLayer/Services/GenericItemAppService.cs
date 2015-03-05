using System.Collections.Generic;
using System.Linq;
using Abp.Application.Services;
using AutoMapper;
using DataModel.Data.ApplicationLayer.DTO;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.DataLayer.Repositories;
using DataModel.Data.TransactionalLayer.Repositories;

namespace DataModel.Data.ApplicationLayer.Services
{
    public class GenericItemAppService<T> : PcdAppServiceBase, IGenericItemAppService<T> 
        where T : class, IGenericItem
    {
        private readonly GenericItemRepositoryBase<T> _genericItemRepository;

        public GenericItemAppService(IGenericItemRepository<T> genericItemRepository)
        {
            _genericItemRepository = (GenericItemRepositoryBase<T>)genericItemRepository;
        }

        public virtual GetAllTItemsOutput<T> GetAllTItems()
        {
            var output = _genericItemRepository.GetAllList();
            var list = output.Select(i => (new GenericItemDto<T>(i))).ToList();
            
            return new GetAllTItemsOutput<T>
            {
                Items = list
            };
        }

        public virtual GetItemByTitleOutput<T> GetItemByTitle(GetItemByTitleInput input)
        {
            return new GetItemByTitleOutput<T>
            {
                 Item = new GenericItemDto<T>(_genericItemRepository.GetItemByTitle(input.Title))
            };
        }

        public virtual AddNewItemOutput AddNewItem(AddNewItemInput<T> input)
        {
            return new AddNewItemOutput
            {
                Id = _genericItemRepository.AddNewItem(input.Item.ConvertToTItem())
            };
        }

        public virtual UpdateTItemOutput UpdateTItem(UpdateTItemInput<T> input)
        {
            return new UpdateTItemOutput
            {
                Id = _genericItemRepository.UpdateItem(input.Item.ConvertToTItem())
            };
        }

        public virtual void DeleteItemById(DeleteItemByIdInput<T> input)
        {
            _genericItemRepository.Delete(input.Id);
        }
    }
}
