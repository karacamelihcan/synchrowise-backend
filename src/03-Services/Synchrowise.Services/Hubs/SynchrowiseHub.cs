using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Synchrowise.Services.Hubs
{
    public class SynchrowiseHub : Hub
    {
        public async override Task OnConnectedAsync()
        {
            var data = Context.GetHttpContext();
            var guid = data.Request.Headers["guid"].ToString();
            await Clients.All.SendAsync("Greetings",Context.ConnectionId,guid);
            await base.OnConnectedAsync();
        }
    }
}