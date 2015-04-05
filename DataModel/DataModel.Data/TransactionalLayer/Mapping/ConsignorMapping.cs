using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.TransactionalLayer.Mapping
{
    public class ConsignorMap : EntityTypeConfiguration<Consignor>
    {
        public ConsignorMap()
        {
            //Key
            HasKey(b => b.Id);

            //Fields
            Property(b => b.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(c => c.DateAdded).IsRequired();

            //Table
            ToTable("Consignors");

        }
    }
}
