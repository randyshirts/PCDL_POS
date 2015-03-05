using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.DataLayer.Repositories;

namespace DataModel.Data.TransactionalLayer.Repositories
{
    public class ConsignorPmtRepository  : PcdRepositoryBase<ConsignorPmt>, IConsignorPmtRepository
    {
        public int AddNewConsignorPmt(ConsignorPmt consignorPmt)
        {
            
            ConsignorPmt oldConsignorPmt = null;
            try
            {
                //Validation if a record already exists for this consignor
                if (consignorPmt != null)
                {
                    oldConsignorPmt = GetConsignorPmtById(consignorPmt.Id);
                }
                if (oldConsignorPmt != null)
                {
                    Validation.ConsignorPmtDuplicate(consignorPmt, oldConsignorPmt);
                }
                //Consignor doesn't exist so add it
                else
                {
                    consignorPmt.Consignor_ConsignorPmt = null;
                    if (consignorPmt.DebitTransaction_ConsignorPmt.StoreCreditPmt != null)
                        consignorPmt.DebitTransaction_ConsignorPmt.StoreCreditPmt.Consignor_StoreCreditPmt = null;

                    var itemList = new Collection<Item>();
                    foreach (var temp in consignorPmt.Items_ConsignorPmt.Select(item => Context.Items.Find(item.Id)))
                    {
                        temp.Consignor = null;
                        temp.Status = "Paid";
                        itemList.Add(temp);
                    }
                    consignorPmt.Items_ConsignorPmt = null;

                    Context.ConsignorPmts.Add(consignorPmt);

                    var cp = Context.ConsignorPmts.Find(consignorPmt.Id);
                    cp.Items_ConsignorPmt = itemList;
                }
                Context.SaveChanges();

                return consignorPmt.Id;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "AddNewConsignorPmt - Repository");
                return -1;
            }
        }

        public ConsignorPmt GetConsignorPmtById(int id)
        {
            ConsignorPmt consignorPmt = (from c in Context.ConsignorPmts
                                         where c.Id == id
                                   select c).FirstOrDefault<ConsignorPmt>();

            return consignorPmt;
        }

        public IEnumerable<ConsignorPmt> GetConsignorPmtsByConsignorId(int id)
        {
            var consignorPmts = (from c in Context.ConsignorPmts
                                         where c.ConsignorId == id
                                         select c);

            return consignorPmts;
        }
    }
}
