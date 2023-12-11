using System.Threading.Tasks;

namespace DuaTaxi.Common.Consul
{
    public interface IConsulHttpClient
    {
        Task<T> GetAsync<T>(string requestUri);
    }
}

