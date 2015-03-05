using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataModel.Data.DataLayer.Entities;

namespace RocketPos.Data.DataLayer.Mapping
{
    public class CreditTransactionMap : EntityTypeConfiguration<CreditTransaction>
    {
        public CreditTransactionMap()
        {
            //Key
            HasKey(t => t.Id);

            //Fields
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.TransactionTotal).IsRequired();
            Property(t => t.StateSalesTaxTotal).IsRequired();
            Property(t => t.LocalSalesTaxTotal).IsRequired();
            Property(t => t.DiscountTotal).IsOptional();
            Property(t => t.TransactionDate).IsRequired();

            //Table
            ToTable("CreditTransactions");

            //Relationships
            HasOptional(ct => ct.ItemSaleTransaction).WithRequired(its => its.CreditTransaction_ItemSale).WillCascadeOnDelete(false);
            HasOptional(ct => ct.ClassPmtTransaction).WithRequired(cp => cp.CreditTransaction_ClassPmt).WillCascadeOnDelete(false);
            HasOptional(ct => ct.SpaceRentalTransaction).WithRequired(sr => sr.CreditTransaction_SpaceRental).WillCascadeOnDelete(false);
            HasOptional(ct => ct.StoreCreditTransaction).WithRequired(sc => sc.CreditTransaction_StoreCredit).WillCascadeOnDelete(false);
        }
    }
}
