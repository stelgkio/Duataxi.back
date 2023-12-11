using System.Threading.Tasks;

namespace DuaTaxi.Common.Fabio
{
    public interface IFabioHttpClient
    {
        Task<T> GetAsync<T>(string requestUri);
    }
}