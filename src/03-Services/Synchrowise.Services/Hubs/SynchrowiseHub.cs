using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Synchrowise.Database.Repositories.GroupRepositories;
using Synchrowise.Database.Repositories.UserRepositories;

namespace Synchrowise.Services.Hubs
{
    public class SynchrowiseHub : Hub
    {
        private readonly IGroupRepository _groupRepo;
        private readonly IUserRepository _userRepo;

        public SynchrowiseHub(IGroupRepository groupRepo, IUserRepository userRepo)
        {
            _groupRepo = groupRepo;
            _userRepo = userRepo;
        }

        public async override Task OnConnectedAsync()
        {
            await Clients.Client(Context.ConnectionId).SendAsync("ConnectionStart","Connected successfully");
            await base.OnConnectedAsync();
        }
        public async override Task OnDisconnectedAsync(Exception? exception)
        {
            await Clients.Client(Context.ConnectionId).SendAsync("ConnectionEnd","Connection ended");
            await base.OnDisconnectedAsync(exception);
        }
    }
}