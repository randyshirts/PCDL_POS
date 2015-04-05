using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.TransactionalLayer.Mapping
{
    public class GameMap : EntityTypeConfiguration<Game>
    {
        public GameMap()
        {
            //Key
            HasKey(b => b.Id);

            //Fields
            Property(b => b.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(b => b.Title).IsRequired().HasMaxLength(150);
            Property(b => b.Manufacturer).HasMaxLength(100);
            Property(b => b.EAN).IsOptional().HasMaxLength(13);
            Property(b => b.ItemImage).IsOptional().HasMaxLength(300);
            
            //Table
            ToTable("Games");

        }
    }
}
