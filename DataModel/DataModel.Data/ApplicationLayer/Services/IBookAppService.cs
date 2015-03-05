using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using DataModel.Data.ApplicationLayer.DTO;

namespace DataModel.Data.ApplicationLayer.Services
{
    public interface IBookAppService : IApplicationService
    {
        GetAllBooksOutput GetAllBooks();
        GetBookByIsbnOutput GetBookByIsbn(GetBookByIsbnInput input);
        AddNewBookOutput AddNewBook(AddNewBookInput input);
        UpdateBookOutput UpdateBook(UpdateBookInput input);
    }
}
