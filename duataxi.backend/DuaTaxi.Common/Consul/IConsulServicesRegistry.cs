using System.Threading.Tasks;
using Consul;

namespace DuaTaxi.Common.Consul
{
    public interface IConsulServicesRegistry
    {
        Task<AgentService> GetAsync(string name);
    }
}