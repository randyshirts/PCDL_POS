using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.TransactionalLayer.Mapping
{
    public class PhoneNumberMap : EntityTypeConfiguration<PhoneNumber>
    {
        public PhoneNumberMap()
        {
            //Key
            HasKey(b => b.Id);

            //Fields
            Property(b => b.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(b => b.HomePhoneNumber).IsOptional().HasMaxLength(10);
            Property(b => b.CellPhoneNumber).IsOptional().HasMaxLength(10);
            Property(b => b.AltPhoneNumber).IsOptional().HasMaxLength(10);
           

            //Table
            ToTable("PhoneNumbers");

        }
    }
}
