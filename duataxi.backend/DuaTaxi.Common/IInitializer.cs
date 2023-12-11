using System.Threading.Tasks;

namespace DuaTaxi.Common
{
    public interface IInitializer
    {
        Task InitializeAsync();
    }
}