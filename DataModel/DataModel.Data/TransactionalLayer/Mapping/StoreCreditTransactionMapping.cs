using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.TransactionalLayer.Mapping
{
    public class StoreCreditTransactionMap : EntityTypeConfiguration<StoreCreditTransaction>
    {
        public StoreCreditTransactionMap()
        {
            //Key
            HasKey(b => b.Id);

            //Fields
            Property(b => b.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(sc => sc.StoreCreditTransactionAmount).IsRequired();
            Property(sc => sc.TransactionType).IsOptional();
            //Table
            ToTable("StoreCreditTransactions");

            //Relationships
            HasRequired(sc => sc.Consignor_StoreCreditTransaction).WithMany(c => c.StoreCreditTransactions_Consignor).HasForeignKey(cp => cp.ConsignorId).WillCascadeOnDelete(false);
            
        }
    }
}
