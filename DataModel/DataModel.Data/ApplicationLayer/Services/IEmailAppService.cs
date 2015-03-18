using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.Data.ApplicationLayer.DTO;

namespace DataModel.Data.ApplicationLayer.Services
{
    public interface IEmailAppService
    {
        GetPersonsByEmailOutput GetPersonsByEmail(GetPersonsByEmailInput input);
    }
}
