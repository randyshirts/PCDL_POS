using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataModel.Data.ApplicationLayer.DTO;
using DataModel.Data.ApplicationLayer.Services;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.TransactionalLayer.Repositories;

namespace DataModel.Data.ApplicationLayer.WpfControllers
{
    public class PersonController : IPersonController
    {
        public bool UpdatePerson(Person updatedPerson)
        {
            var input = new UpdatePersonInput
            {
                PersonDto = new PersonDto(updatedPerson)
            };

            try
            {
                using (var repo = new PersonRepository())
                {
                    var app = new PersonAppService(repo);
                    app.UpdatePerson(input);
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "UpdatePerson - Controller");
                return false;
            }
        }

        public IEnumerable<Person> QueryPersonsThatAreConsigners()
        {
            try
            {
                using (var repo = new PersonRepository())
                {
                    var app = new PersonAppService(repo);
                    var output = app.QueryPersonsThatAreConsigners();
                    return output.Persons.Select(person => person.ConvertToPerson()).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "QueryPersonsThatAreConsigners - Controller");
                return null;
            }
        }
    }
}
