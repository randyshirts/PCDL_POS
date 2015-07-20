using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.Data.DataLayer.Entities;

namespace RocketPos.Common.Helpers
{
    public static class StringHelpers
    {
        public static IEnumerable<string> CombineNames(IEnumerable<Person> persons)
        {
            return persons.Select(person => person.FirstName + " " + person.LastName).ToList();
        }
    }
}
