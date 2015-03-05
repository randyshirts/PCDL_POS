using System.Data.Entity;


namespace DataModel.Data.DataLayer.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public interface IDbContextProvider<out TDbContext>
        where TDbContext : DbContext
    {
        TDbContext DbContext { get; }
    }
}
