using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Synchrowise.Core.Dtos;
using Synchrowise.Core.Models;
using Synchrowise.Database.Repositories.GroupRepositories;
using Synchrowise.Database.Repositories.UserRepositories;
using Synchrowise.Database.UnitOfWorks;
using Synchrowise.Services.MappingProfile;

namespace Synchrowise.Services.Hubs
{
    public class SynchrowiseHub : Hub
    {
        private readonly IGroupRepository _groupRepo;
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _unitOfWork;

        public SynchrowiseHub(IGroupRepository groupRepo, IUserRepository userRepo, IUnitOfWork unitOfWork)
        {
            _groupRepo = groupRepo;
            _userRepo = userRepo;
            _unitOfWork = unitOfWork;
        }

        public async override Task OnConnectedAsync()
        {
            var context = Context.GetHttpContext();

            if (context != null)
            {
                var guid = Guid.Parse(context.Request.Headers["guid"].ToString());
                var user = await _userRepo.GetByGuidAsync(guid);
                if (user != null && user.SignalRConnectionId != Context.ConnectionId)
                {
                    user.SignalRConnectionId = Context.ConnectionId;
                    await _unitOfWork.CommitAsync();
                }
            }
            await Clients.Client(Context.ConnectionId).SendAsync("ConnectionStart", "Connected successfully");
            await base.OnConnectedAsync();
        }
        public async override Task OnDisconnectedAsync(Exception? exception)
        {
            await Clients.Client(Context.ConnectionId).SendAsync("ConnectionEnd", "Connection ended");
            await base.OnDisconnectedAsync(exception);
        }

        //Invoke after join group from api
        public async Task JoinGroup()
        {
            var context = Context.GetHttpContext();

            if (context != null)
            {
                var guid = Guid.Parse(context.Request.Headers["guid"].ToString());
                var user = await _userRepo.GetByGuidAsync(guid);
                var group = await _groupRepo.GetGroupWithRelations(user.GroupId);
                if (group != null)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, group.Guid.ToString());
                    await Clients.Group(group.Guid.ToString()).SendAsync("JoinGroup", group, user.Username + " joined to group");
                }
            }
        }

        //Invoke before left group from api
        public async Task LeftFromGroup()
        {
            var httpContext = Context.GetHttpContext();
            if (httpContext != null)
            {
                var guid = Guid.Parse(httpContext.Request.Headers["guid"].ToString());
                var user = await _userRepo.GetByGuidAsync(guid);
                var group = await _groupRepo.GetGroupWithRelations(user.GroupId);
                if (group != null)
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, group.Guid.ToString());
                    await Clients.Group(group.Guid.ToString()).SendAsync("JoinGroup", group, user.Username + " joined to group");
                }
            }
        }


        //Invoke after upload filed from api
        public async Task SendGroupFiles()
        {
            var httpContext = Context.GetHttpContext();
            if (httpContext != null)
            {
                var guid = Guid.Parse(httpContext.Request.Headers["guid"].ToString());
                var user = await _userRepo.GetByGuidAsync(guid);
                var group = await _groupRepo.GetGroupWithRelations(user.GroupId);
                if (group != null)
                {
                    var result = new List<GroupFileDto>();
                    foreach (var file in group.GroupFiles)
                    {
                        result.Add(ObjectMapper.Mapper.Map<GroupFileDto>(file));
                    }
                    await Clients.Group(group.Guid.ToString()).SendAsync("GetGroupFile", result);
                }
            }
        }

        // Send new message
        public async Task SendMessage(string message)
        {
            if (message != null)
            {
                var httpContext = Context.GetHttpContext();
                if (httpContext != null)
                {
                    var guid = Guid.Parse(httpContext.Request.Headers["guid"].ToString());
                    var user = await _userRepo.GetByGuidAsync(guid);
                    var group = await _groupRepo.GetGroupMessages(user.GroupId);
                    if (group != null)
                    {
                        var msg = new GroupMessage()
                        {
                            Message = message,
                            Sender = user,
                            Group = group
                        };
                        group.Messages.Add(msg);
                        await _unitOfWork.CommitAsync();
                        var result = ObjectMapper.Mapper.Map<GroupMessageDto>(msg);
                        await Clients.Group(group.Guid.ToString()).SendAsync("GetNewMessage", result);
                    }
                }
            }
        }

        //GetAllMessage
        public async Task GetAllMessages()
        {
            var httpContext = Context.GetHttpContext();
                if (httpContext != null)
                {
                    var guid = Guid.Parse(httpContext.Request.Headers["guid"].ToString());
                    var user = await _userRepo.GetByGuidAsync(guid);
                    var group = await _groupRepo.GetGroupMessages(user.GroupId);
                    if (group != null)
                    {
                        var result = new List<GroupMessageDto>();

                        if(group.Messages.Any()){
                            foreach (var msg in group.Messages)
                            {   
                                result.Add(ObjectMapper.Mapper.Map<GroupMessageDto>(msg));
                            }
                        }
                        await Clients.Client(Context.ConnectionId).SendAsync("GetAllMessage", result);
                    }
                }
        }
    }
}