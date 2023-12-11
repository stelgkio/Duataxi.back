using System;
using System.Threading.Tasks;
using DuaTaxi.Services.TaxiApi.Framework;

using Microsoft.AspNetCore.SignalR;

namespace DuaTaxi.Services.TaxiApi.Hubs.Services
{
    public class HubWrapper : IHubWrapper
    {
        private readonly IHubContext<TaxiDriverHub> _hubContext;

        public HubWrapper(IHubContext<TaxiDriverHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task PublishToUserAsync(Guid userId, string message, object data)
            => await _hubContext.Clients.Group(userId.ToUserGroup()).SendAsync(message, data);

        public async Task PublishToAllAsync(string message, object data)
            => await _hubContext.Clients.All.SendAsync(message, data);
    }
}