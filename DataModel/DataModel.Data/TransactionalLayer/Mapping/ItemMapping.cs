using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataModel.Data.DataLayer.Entities;

namespace RocketPos.Data.DataLayer.Mapping
{
    public class ItemMap : EntityTypeConfiguration<Item>
    {
        public ItemMap()
        {
            //Key
            HasKey(b => b.Id);

            //Fields
            Property(b => b.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(b => b.ItemType).IsRequired().HasMaxLength(20);
            Property(b => b.ListedPrice).IsRequired();
            Property(b => b.SalePrice).IsOptional();
            Property(b => b.ListedDate).IsRequired();
            Property(b => b.IsDiscountable);
            Property(b => b.Subject).IsRequired().HasMaxLength(30);
            Property(b => b.Status).IsRequired().HasMaxLength(30);
            Property(b => b.Condition).IsRequired().HasMaxLength(25);
            Property(b => b.Description).IsOptional();
            Property(i => i.CashPayout).IsOptional();
            Property(i => i.Barcode).IsRequired().IsFixedLength().HasMaxLength(15);
            //Property(i => i.ConsignorPmt).IsOptional();

            //Table
            ToTable("Items");

            //Relationships

            HasOptional(i => i.Book).WithMany(b => b.Items_TItems).HasForeignKey(i => i.BookId).WillCascadeOnDelete(false);
            HasOptional(i => i.Game).WithMany(g => g.Items_TItems).HasForeignKey(i => i.GameId).WillCascadeOnDelete(false);
            HasOptional(i => i.TeachingAide).WithMany(t => t.Items_TItems).HasForeignKey(i => i.TeachingAideId).WillCascadeOnDelete(false);
            HasOptional(i => i.Other).WithMany(o => o.Items_TItems).HasForeignKey(i => i.OtherId).WillCascadeOnDelete(false);
            HasOptional(i => i.Video).WithMany(v => v.Items_TItems).HasForeignKey(i => i.VideoId).WillCascadeOnDelete(false);
            HasOptional(i => i.ItemSaleTransaction).WithMany(t => t.Items_ItemSaleTransaction).HasForeignKey(i => i.ItemSaleTransactionId).WillCascadeOnDelete(false);
            HasOptional(i => i.ConsignorPmt).WithMany(cp => cp.Items_ConsignorPmt).HasForeignKey(i => i.ConsignorPmtId).WillCascadeOnDelete(false);
            HasRequired(i => i.Consignor).WithMany(c => c.Items_Consignor).HasForeignKey(i => i.ConsignorId).WillCascadeOnDelete(false);
        }
    }
}
