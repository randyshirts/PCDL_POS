using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataModel.Data.DataLayer.Entities;

namespace RocketPos.Data.DataLayer.Mapping
{
    public class DebitTransactionMap : EntityTypeConfiguration<DebitTransaction>
    {
        public DebitTransactionMap()
        {
            //Key
            HasKey(t => t.Id);

            //Fields
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.DebitTotal).IsRequired();
            Property(t => t.DebitTransactionDate).IsRequired();

            //Table
            ToTable("DebitTransactions");

            //Relationships
            HasOptional(dt => dt.ConsignorPmt).WithRequired(cp => cp.DebitTransaction_ConsignorPmt).WillCascadeOnDelete(false);
            HasOptional(dt => dt.StoreCreditPmt).WithRequired(cp => cp.DebitTransaction_StoreCreditPmt).WillCascadeOnDelete(false);
        }
    }
}
