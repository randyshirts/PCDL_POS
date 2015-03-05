using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.DataLayer.Repositories;

namespace DataModel.Data.ApplicationLayer.DTO
{
    public class GenericItemDto<T> : EntityDto
        where T : class, IGenericItem
    {
        public GenericItemDto(T item)
        {
            Id = item.Id;
            Title = item.Title;
            Manufacturer = item.Manufacturer;
            Ean = item.EAN;
            ItemImage = item.ItemImage;
            Items_TItems = item.Items_TItems;
        }

        

        [Required]
        public string Title { get; set; }
        [Required]
        public string Manufacturer { get; set; }
        public string Ean { get; set; }
        public string ItemImage { get; set; }
        public ICollection<Item> Items_TItems { get; set; }

        public T ConvertToTItem()
        {
            IGenericItem item = (IGenericItem) Activator.CreateInstance(typeof(T));
            
            item.Id = Id;
            item.Title = Title;
            item.Manufacturer = Manufacturer;
            item.EAN = Ean;
            item.ItemImage = ItemImage;
            item.Items_TItems = Items_TItems;
            
            return item as T;
        }

        

    }

    public class GetAllTItemsOutput<T> : IOutputDto
        where T : class, IGenericItem
    {
        public List<GenericItemDto<T>> Items { get; set; }
    }

    public class GetItemByTitleOutput<T> : IOutputDto
        where T : class, IGenericItem
    {
        public GenericItemDto<T> Item { get; set; }
    }

    public class GetItemByTitleInput : IInputDto
    {
        public string Title { get; set; }
    }

    public class AddNewItemOutput : IOutputDto
    {
        public int Id { get; set; }
    }

    public class AddNewItemInput<T> : IInputDto
        where T : class, IGenericItem
    {
        public GenericItemDto<T> Item { get; set; }
    }

    public class UpdateTItemOutput : IOutputDto
    {
        public int Id { get; set; }
    }

    public class UpdateTItemInput<T> : IInputDto
        where T : class, IGenericItem
    {
        public GenericItemDto<T> Item { get; set; }
    }

    public class DeleteItemByIdInput<T> : IInputDto
        where T : class, IGenericItem
    {
        public int Id { get; set; }
    }
}
