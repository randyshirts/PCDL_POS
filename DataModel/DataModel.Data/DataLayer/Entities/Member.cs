using DataModel.Data.DataLayer.Repositories;

namespace DataModel.Data.DataLayer.Entities
{
    public class Member : Entity
    {

        #region Define Members
        //Define members
        //public int Id { get; set; }
        public virtual Person Member_Person { get; set; }
        #endregion




    }
}
