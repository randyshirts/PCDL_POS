using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataModel.Data.DataLayer.Entities;

namespace RocketPos.Data.DataLayer.Mapping
{
    public class BookMap : EntityTypeConfiguration<Book>
    {
        public BookMap()
        {
            //Key
            HasKey(b => b.Id);

            //Fields
            Property(b => b.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(b => b.Title).IsRequired().HasMaxLength(150);
            Property(b => b.Author).HasMaxLength(100);
            Property(b => b.ISBN).IsRequired().HasMaxLength(13);
            Property(b => b.Binding).HasMaxLength(25);
            Property(b => b.NumberOfPages).IsOptional();
            Property(b => b.PublicationDate).IsOptional();
            Property(b => b.TradeInValue).IsOptional();
            Property(b => b.ItemImage).IsOptional().HasMaxLength(200);

            //Table
            ToTable("Books");


        }
    }
}
