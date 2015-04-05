using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;

//using MySql.Data.Entity;
//using System.Data.Entity.SqlServer;

namespace DataModel.Data.DataLayer
{
    
    public class DataConfiguration : DbConfiguration
    {
        
        public DataConfiguration()
        {
            //MySql
            //DbConfiguration.SetConfiguration(new MySqlEFConfiguration());
            
            //Sql
            SetTransactionHandler(SqlProviderServices.ProviderInvariantName, () => new CommitFailureHandler());
        }

        
    }
}
