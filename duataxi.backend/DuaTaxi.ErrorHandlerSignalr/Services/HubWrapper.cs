using System;
using System.Threading.Tasks;
using DuaTaxi.Services.Signalr.Framework;
using DuaTaxi.Services.Signalr.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace DuaTaxi.Services.Signalr.Services
{
    public class HubWrapper : IHubWrapper
    {
        private readonly IHubContext<DShopHub> _hubContext;

        public HubWrapper(IHubContext<DShopHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task PublishToUserAsync(Guid userId, string message, object data)
            => await _hubContext.Clients.Group(userId.ToUserGroup()).SendAsync(message, data);

        public async Task PublishToAllAsync(string message, object data)
            => await _hubContext.Clients.All.SendAsync(message, data);
    }
}