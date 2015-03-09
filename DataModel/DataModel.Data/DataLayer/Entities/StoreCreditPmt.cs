using DataModel.Data.DataLayer.Repositories;
using Newtonsoft.Json;

namespace DataModel.Data.DataLayer.Entities
{
    [JsonObject(IsReference = true)]
    public class StoreCreditPmt : Entity
    {

        #region Define Members
        //Define members
        //public int Id { get; set; }
        public virtual DebitTransaction DebitTransaction_StoreCreditPmt { get; set; }
        public virtual int ConsignorId { get; set; }
        public virtual Consignor Consignor_StoreCreditPmt { get; set; }
        public double StoreCreditPmtAmount { get; set; }

        #endregion

    }
}
