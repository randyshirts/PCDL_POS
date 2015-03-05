using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataModel.Data.ApplicationLayer.DTO;
using DataModel.Data.ApplicationLayer.Services;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.TransactionalLayer.Repositories;

namespace DataModel.Data.ApplicationLayer.WpfControllers
{
    public class ConsignorPmtController : IConsignorPmtController
    {
        public int AddNewConsignorPmt(ConsignorPmt consignorPmt)
        {
            var input = new AddNewConsignorPmtInput
            {
                PmtDto = new ConsignorPmtDto(consignorPmt)
            };

            using (var repo = new ConsignorPmtRepository())
            {
                var app = new ConsignorPmtAppService(repo);
                return app.AddNewConsignorPmt(input).Id;
            }
        }

        public ConsignorPmt GetConsignorPmtById(int id)
        {
            var input = new GetConsignorPmtByIdInput
            {
                Id = id
            };

            using (var repo = new ConsignorPmtRepository())
            {
                var app = new ConsignorPmtAppService(repo);
                return app.GetConsignorPmtById(input).PmtDto.ConvertToConsignorPmt();
            }
        }

        public IEnumerable<ConsignorPmt> GetConsignorPmtsByConsignorId(int id)
        {
            var input = new GetConsignorPmtsByConsignorIdInput
            {
                 ConsignorId = id
            };

            using (var repo = new ConsignorPmtRepository())
            {
                var app = new ConsignorPmtAppService(repo);
                return app.GetConsignorPmtsByConsignorId(input).PmtDtos.Select(c => c.ConvertToConsignorPmt()).ToList();
            }
        }
    }
}
