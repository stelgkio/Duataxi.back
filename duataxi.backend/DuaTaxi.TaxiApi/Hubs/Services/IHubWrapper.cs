using System;
using System.Threading.Tasks;

namespace DuaTaxi.Services.TaxiApi.Hubs.Services
{
    public interface IHubWrapper
    {
        Task PublishToUserAsync(Guid userId, string message, object data);
        Task PublishToAllAsync(string message, object data);
    }
}