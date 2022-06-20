using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
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

                // var userDto = ObjectMapper.Mapper.Map<UserDto>(user);

                // Dictionary<string, object> data = new Dictionary<string, object>();

                // data["message"] = "Connected";
                // data["user"] = userDto;

                // string json = JsonConvert.SerializeObject(data);

                // await Clients.Client(Context.ConnectionId).SendAsync("ConnectionStart", json);
                await base.OnConnectedAsync();
            }
        }
        public async override Task OnDisconnectedAsync(Exception? exception)
        {
            await Clients.Client(Context.ConnectionId).SendAsync("ConnectionEnded", "Connection ended");
            await base.OnDisconnectedAsync(exception);
        }

        //Invoke after join group from api
        public async Task JoinGroup()
        {
            try
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

                        Dictionary<string, object> data = new Dictionary<string, object>();

                        data["groupId"] = group.Guid.ToString();
                        data["user"] = ObjectMapper.Mapper.Map<UserDto>(user);


                        await Clients.All.SendAsync("JoinedGroup", JsonConvert.SerializeObject(data));
                    }
                    else
                    {
                        await Clients.All.SendAsync("JoinGroupError", "There is no such a group");
                    }
                }
                else
                {
                    await Clients.All.SendAsync("JoinGroupError", "Http Context cannot be null");
                }
            }
            catch (System.Exception ex)
            {

                await Clients.All.SendAsync("JoinGroupError", ex.Message);
            }
        }

        //Invoke before left group from api
        public async Task LeaveGroup()
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


                    Dictionary<string, object> data = new Dictionary<string, object>();

                    data["groupId"] = group.Guid.ToString();
                    data["userId"] = user.Guid.ToString();

                    await Clients.All.SendAsync("LeftGroup", JsonConvert.SerializeObject(data));
                }
            }
        }


        //Invoke after upload filed from api
        public async Task UploadGroupFile(Guid guid)
        {
            var httpContext = Context.GetHttpContext();
            if (httpContext != null)
            {
                var userId = Guid.Parse(httpContext.Request.Headers["guid"].ToString());
                var user = await _userRepo.GetByGuidAsync(userId);
                var group = await _groupRepo.GetGroupWithRelations(user.GroupId);
                if (group != null)
                {
                    var file = group.GroupFiles.Where(file => file.Guid == guid).FirstOrDefault();
                    if (file != null)
                    {
                        var result = ObjectMapper.Mapper.Map<GroupFileDto>(file);
                        Dictionary<string, object> data = new Dictionary<string, object>();

                        data["groupId"] = group.Guid.ToString();
                        data["filePath"] = file.Path;
                        await Clients.All.SendAsync("GroupFileUploaded", JsonConvert.SerializeObject(data));
                    }

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

                    if (group.Messages.Any())
                    {
                        foreach (var msg in group.Messages)
                        {
                            result.Add(ObjectMapper.Mapper.Map<GroupMessageDto>(msg));
                        }
                    }
                    await Clients.Client(Context.ConnectionId).SendAsync("GetAllMessage", result);
                }
            }
        }

        // Start Video . Send video guid that you want to start
        public async Task PlayVideo(Guid guid, int playTimeMs)
        {
            var httpContext = Context.GetHttpContext();
            if (httpContext != null)
            {
                var userGuid = Guid.Parse(httpContext.Request.Headers["guid"].ToString());
                var user = await _userRepo.GetByGuidAsync(userGuid);
                var group = await _groupRepo.GetGroupWithRelations(user.GroupId);
                if (group != null)
                {
                    var file = group.GroupFiles.Where(file => file.Guid == guid).FirstOrDefault();
                    if (file != null)
                    {
                        var result = new Dictionary<string, object>();
                        result["groupId"] = group.Guid.ToString();
                        result["playTimeMs"] = playTimeMs;

                        await Clients.Group(group.Guid.ToString()).SendAsync("PlayVideo", JsonConvert.SerializeObject(result));
                    }
                }
            }
        }

        // Stop Video . Send video guid that you want to stop
        public async Task StopVideo(Guid guid, int stopTimeMs)
        {
            var httpContext = Context.GetHttpContext();
            if (httpContext != null)
            {
                var userGuid = Guid.Parse(httpContext.Request.Headers["guid"].ToString());
                var user = await _userRepo.GetByGuidAsync(userGuid);
                var group = await _groupRepo.GetGroupWithRelations(user.GroupId);
                if (group != null)
                {
                    var file = group.GroupFiles.Where(file => file.Guid == guid).FirstOrDefault();
                    if (file != null)
                    {
                        Dictionary<string, object> data = new Dictionary<string, object>();

                        data["groupId"] = group.Guid.ToString();
                        data["stopTimeMs"] = stopTimeMs;

                        await Clients.Group(group.Guid.ToString()).SendAsync("StopVideo", JsonConvert.SerializeObject(data));
                    }
                }
            }
        }
        // Skip or back forward Video . Send video guid that you want to start and time ypu forward
        public async Task SkipForwardVideo(Guid guid, int forwardTimeMs)
        {
            var httpContext = Context.GetHttpContext();
            if (httpContext != null)
            {
                var userGuid = Guid.Parse(httpContext.Request.Headers["guid"].ToString());
                var user = await _userRepo.GetByGuidAsync(userGuid);
                var group = await _groupRepo.GetGroupWithRelations(user.GroupId);
                if (group != null)
                {
                    var file = group.GroupFiles.Where(file => file.Guid == guid).FirstOrDefault();
                    if (file != null)
                    {
                        Dictionary<string, object> data = new Dictionary<string, object>();

                        data["groupId"] = group.Guid.ToString();
                        data["forwardTimeMs"] = forwardTimeMs;

                        await Clients.Group(group.Guid.ToString()).SendAsync("SkipForward", JsonConvert.SerializeObject(data));
                    }
                }
            }
        }

        public async Task ReceivedVideo()
        {
            var httpContext = Context.GetHttpContext();
            if (httpContext != null)
            {
                var userGuid = Guid.Parse(httpContext.Request.Headers["guid"].ToString());
                var user = await _userRepo.GetByGuidAsync(userGuid);
                var group = await _groupRepo.GetGroupWithRelations(user.GroupId);
                if (group != null)
                {
                    await Clients.Group(group.Guid.ToString()).SendAsync("ReadyToPlay", Context.ConnectionId);
                }
            }
        }

        public async Task RemoveFromGroup(Guid userGuid){
            var httpContext = Context.GetHttpContext();
            if (httpContext != null)
            {
                var OwnerGuid = Guid.Parse(httpContext.Request.Headers["guid"].ToString());
                var owner = await _userRepo.GetByGuidAsync(OwnerGuid);
                var group = await _groupRepo.GetGroupWithRelations(owner.GroupId);
                if (group != null)
                {
                    var removedUser = group.Users.Where(usr => usr.Guid ==userGuid).FirstOrDefault();
                    Dictionary<string, object> data = new Dictionary<string, object>();

                    data["groupId"] = group.Guid.ToString();
                    data["user"] = ObjectMapper.Mapper.Map<UserDto>(removedUser);
                    await Clients.All.SendAsync("RemoveFromGroup", data);
                }
            }
        }



    }
}