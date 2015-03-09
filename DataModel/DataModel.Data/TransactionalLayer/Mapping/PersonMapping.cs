using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.TransactionalLayer.Mapping
{
    public class PersonMap : EntityTypeConfiguration<Person>
    {
        public PersonMap()
        {
            //Key
            HasKey(b => b.Id);

            //Fields
            Property(b => b.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(b => b.FirstName).IsRequired().HasMaxLength(20);
            Property(b => b.LastName).IsRequired().HasMaxLength(20);

            //Table
            ToTable("Persons");

            //Relationships
            HasOptional(p => p.User).WithRequired(u => u.PersonUser).WillCascadeOnDelete(false);
            HasOptional(p => p.Consignor).WithRequired(co => co.Consignor_Person).WillCascadeOnDelete(false);
            HasOptional(p => p.Member).WithRequired(me => me.Member_Person).WillCascadeOnDelete(false);
            HasOptional(p => p.Volunteer).WithRequired(vo => vo.Volunteer_Person).WillCascadeOnDelete(false);
            HasMany(p => p.MailingAddresses).WithMany(m => m.MailingAddress_Person).Map(m => m.MapLeftKey("MailingAddressId").MapRightKey("PersonId").ToTable("MailingAddresses_Persons"));
            HasMany(p => p.EmailAddresses).WithMany(e => e.EmailAddress_Person).Map(m => m.MapLeftKey("EmailAddressId").MapRightKey("PersonId").ToTable("EmailAddresses_Persons"));
            HasRequired(pe => pe.PhoneNumbers).WithRequiredPrincipal(pn => pn.PhoneNumber_Person).WillCascadeOnDelete(false);
        }
    }
}
