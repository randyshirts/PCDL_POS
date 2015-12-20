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

        public static string SwitchNameOrder(string fullName)
        {
            //Split full name
            if (String.IsNullOrEmpty(fullName)) return null;
            var names = fullName.Split(' ');
            var last = names[0];
            var first = names[1];

            return first + " " + last;
        }

        public static List<string> SanitizeNames(string first, string last)
        {
            var list = new List<string>();
            list.Add(first.Replace(" ", String.Empty));
            list.Add(last.Replace(" ", String.Empty));

            return list;
        }

        public static string SanitizeName(string name)
        {
            return name.Replace(" ", String.Empty);
        }
    }
}
