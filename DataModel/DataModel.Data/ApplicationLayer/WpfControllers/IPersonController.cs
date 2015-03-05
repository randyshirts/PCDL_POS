
using System.Collections.Generic;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.ApplicationLayer.WpfControllers
{
    public interface IPersonController
    {
        bool UpdatePerson(Person updatedPerson);
        IEnumerable<Person> QueryPersonsThatAreConsigners();
    }
}
