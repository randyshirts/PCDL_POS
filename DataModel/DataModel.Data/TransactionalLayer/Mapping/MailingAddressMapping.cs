using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.TransactionalLayer.Mapping
{
    public class MailingAddressMap : EntityTypeConfiguration<MailingAddress>
    {
        public MailingAddressMap()
        {
            //Key
            HasKey(b => b.Id);

            //Fields
            Property(b => b.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(b => b.MailingAddress1).IsRequired().HasMaxLength(40);
            Property(b => b.MailingAddress2).IsOptional().HasMaxLength(40);
            Property(b => b.City).IsRequired().HasMaxLength(30);
            Property(b => b.ZipCode).IsRequired().HasMaxLength(10);
            Property(b => b.State).IsRequired().HasMaxLength(2);

            //Table
            ToTable("MailingAddresses");
            
        }
    }
}
