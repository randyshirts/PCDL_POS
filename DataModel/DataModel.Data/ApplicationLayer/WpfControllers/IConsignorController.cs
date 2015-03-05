using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.ApplicationLayer.WpfControllers
{
    public interface IConsignorController
    {
        int AddNewConsignor(Consignor consignor);
        void DeleteConsignerById(int id);
        void UpdateConsignor(Consignor updatedConsignor);
        Consignor GetConsignorByName(string firstName, string lastName);
        List<string> GetConsignorNames();
        List<string> GetConsignorNamesLastnameFirst();
        Consignor GetConsignorByFullName(string fullName);
        double GetConsignorCreditBalance(string name);
        IEnumerable<Consignor> GetAllConsignors();
    }
}
