using DataModel.Data.DataLayer.Repositories;

namespace DataModel.Data.DataLayer.Entities
{
    public class Volunteer : Entity
    {

        #region Define Members
        //Define members
        public virtual Person Volunteer_Person { get; set; }
        #endregion




    }
}
