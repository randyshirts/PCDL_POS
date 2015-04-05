using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.TransactionalLayer.Mapping
{
    public class StoreCreditPmtMap : EntityTypeConfiguration<StoreCreditPmt>
    {
        public StoreCreditPmtMap()
        {
            //Key
            HasKey(b => b.Id);

            //Fields
            Property(b => b.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(sc => sc.StoreCreditPmtAmount).IsRequired();

            //Table
            ToTable("StoreCreditPmts");

            //Relationships
            HasRequired(sc => sc.Consignor_StoreCreditPmt).WithMany(c => c.StoreCreditPmts_Consignor).HasForeignKey(cp => cp.ConsignorId).WillCascadeOnDelete(false);
        }
    }
}
