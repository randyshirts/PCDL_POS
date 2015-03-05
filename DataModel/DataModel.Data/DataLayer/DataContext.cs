using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Abp.EntityFramework;
using DataModel.Data.DataLayer.Entities;
using MySql.Data.Entity;

namespace DataModel.Data.DataLayer
{
    

    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public partial class DataContext : DbContext
    {
        
        public DataContext()
            : base("name=DataContext")    //name= syntax ensures that if the connection string is not present 
                                          //      then EF will throw rather than creating a new database by convention
        {
            
            //try
            //{
                Debug.Write(Database.Connection.ConnectionString);
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    throw;
            //}
        }

        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<TeachingAide> TeachingAides { get; set; }
        public virtual DbSet<Other> Others { get; set; }
        public virtual DbSet<Video> Videos { get; set; }
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<Consignor> Consignors { get; set; }
        public virtual DbSet<MailingAddress> MailingAddresses { get; set; }
        public virtual DbSet<Email> Emails { get; set; }
        public virtual DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public virtual DbSet<CreditTransaction> CreditTransactions { get; set; }
        public virtual DbSet<DebitTransaction> DebitTransactions { get; set; }
        public virtual DbSet<ConsignorPmt> ConsignorPmts { get; set; }
        public virtual DbSet<StoreCreditPmt> StoreCreditPmts { get; set; }
        public virtual DbSet<ItemSaleTransaction> ItemSaleTransactions { get; set; }
        public virtual DbSet<ClassPmtTransaction> ClassPmtTransactions { get; set; }
        public virtual DbSet<SpaceRentalTransaction> SpaceRentalTransactions { get; set; }
        public virtual DbSet<StoreCreditTransaction> StoreCreditTransactions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
           .Where(type => !String.IsNullOrEmpty(type.Namespace))
           .Where(type => type.BaseType != null && type.BaseType.IsGenericType
                && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
            base.OnModelCreating(modelBuilder); 
        }

        
    }

}
