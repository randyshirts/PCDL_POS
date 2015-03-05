using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace RocketPos.Common.Foundation
{
    public abstract class ViewModel : ObservableObjects, IDataErrorInfo
    {
        public ViewModel()
        {
        }


#region IDataErrorInfo

        public string this[String ColumnName]
        {
            get { return OnValidate(ColumnName); }
        }
        
        [Obsolete]
        public string Error
        {
            get { throw new NotSupportedException();} 
        }
#endregion


        protected virtual string OnValidate(string propertyName)
        {
            var context = new ValidationContext(this)
            {
                MemberName = propertyName
            };

            var results = new Collection<ValidationResult>();
            var isValid = Validator.TryValidateObject(this, context, results, true);

            //return null if valid, return the errormessage for the appropriate property if not
            if (!isValid)
            {
                ValidationResult result = results.SingleOrDefault(p =>
                                                                  p.MemberNames.Any(memberName =>
                                                                                    memberName == propertyName));

                return result == null ? null : result.ErrorMessage;
            }
            return null;
        }

        
        
    }
    
}
