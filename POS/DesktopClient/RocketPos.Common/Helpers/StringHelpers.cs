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
        public static List<string> CombineNames(IEnumerable<Person> list)
        {
            var firstNames = list.Select(p => p.FirstName);
            var lastNames = list.Select(p => p.LastName);
            var names = new List<string>();

            for (int i = 0; i < list.Count(); i++)
            {
                names.Add(firstNames.ElementAt(i) + " " + lastNames.ElementAt(i));
            }

            return names;
        }
    }
}
