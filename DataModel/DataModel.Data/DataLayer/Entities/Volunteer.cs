using DataModel.Data.DataLayer.Repositories;
using Newtonsoft.Json;

namespace DataModel.Data.DataLayer.Entities
{
    [JsonObject(IsReference = true)]
    public class Volunteer : Entity
    {

        #region Define Members
        //Define members
        public virtual Person Volunteer_Person { get; set; }
        #endregion




    }
}
