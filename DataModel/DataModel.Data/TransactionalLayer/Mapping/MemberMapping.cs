using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.TransactionalLayer.Mapping
{
    public class MemberMap : EntityTypeConfiguration<Member>
    {
        public MemberMap()
        {
            //Key
            HasKey(m => m.Id);

            //Fields
            Property(m => m.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(m => m.DateAdded).IsRequired();
            Property(m => m.StartDate);
            Property(m => m.RenewDate).IsRequired();
            Property(m => m.MemberType).IsRequired();

            //Table
            ToTable("Members");


        }
    }
}
