using System.Data.Entity;
using MySql.Data.Entity;
//using System.Data.Entity.SqlServer;

namespace DataModel.Data.DataLayer
{
    
    public class DataConfiguration : DbConfiguration
    {
        
        public DataConfiguration()
        {
            DbConfiguration.SetConfiguration(new MySqlEFConfiguration());
            //SetTransactionHandler(MySqlProviderServices, () => new CommitFailureHandler());
        }

        
    }
}
