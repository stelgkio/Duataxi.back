using System.Threading.Tasks;

namespace DuaTaxi.Common.Mongo
{
    public interface IMongoDbSeeder
    {
        Task SeedAsync();
    }
}