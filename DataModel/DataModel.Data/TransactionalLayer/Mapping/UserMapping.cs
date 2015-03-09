using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.Data.DataLayer.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DataModel.Data.TransactionalLayer.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {

        public UserMap()
            {
                //Key
                HasKey(b => b.Id);

                //Properties
                Property(b => b.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None); 

                //Table
                ToTable("Users");

            }

        


        
    }
}
