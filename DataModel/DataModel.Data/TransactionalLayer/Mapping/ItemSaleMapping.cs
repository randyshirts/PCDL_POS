using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataModel.Data.DataLayer.Entities;

namespace RocketPos.Data.DataLayer.Mapping
{
    public class ItemSaleMap : EntityTypeConfiguration<ItemSaleTransaction>
    {
        public ItemSaleMap()
        {
            //Key
            HasKey(b => b.Id);

            //Fields
            Property(b => b.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            //Table
            ToTable("ItemSaleTransactions");

            //Relationships
            //HasRequired(cp => cp.Consignor_ConsignorPmt).WithMany(c => c.ConsignorPmts_Consignor).HasForeignKey(cp => cp.ConsignorId).WillCascadeOnDelete(false);
        }
    }
}
