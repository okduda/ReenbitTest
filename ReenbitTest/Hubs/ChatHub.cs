using Microsoft.AspNetCore.SignalR;
using ReenbitTest.Entities;
using ReenbitTest.Hubs.Messages;
using ReenbitTest.Services.Interfaces;

namespace ReenbitTest.Hubs
{
    public class ChatHub : Hub
    {
        private IUserService _userService;
        private IConnectionService _connectionService;
        private IGroupService _groupService;
        private IChatMessageService _chatMessageService;
        private IGroupUserService _groupUserService;

        public ChatHub(IUserService userService,
            IConnectionService connectionService,
            IGroupService groupService,
            IChatMessageService chatMessageService,
            IGroupUserService groupUserService)
        {
            _userService = userService;
            _connectionService = connectionService;
            _groupService = groupService;
            _chatMessageService = chatMessageService;
            _groupUserService = groupUserService;
        }

        public async Task SendMessage(ChatMessage message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task OnConnection(string userName)
        {
            var user = await _userService.GetOrCreateUser(userName);

            await _connectionService.AddConnection(Context.ConnectionId, user.Id);

            var groups = await _groupService.GetGroupsByUserId(user.Id);

            if (groups.Any())
            {
                foreach (var group in groups)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, group.Name);
                }
            }
        }

        public async Task CreateOrUpdateGroup(CreateOrUpdateGroupMessage message)
        {
            var users = await _userService.GetUsersByUserNames(message.UserNames);
            var group = await _groupService.GetOrCreateGroup(message.GroupName);

            var alreadyAddedUserIds = await _userService.GetUsersByGroupId(group.Id);

            var userIds = users
                .Where(u => !alreadyAddedUserIds.Any(eu => eu.Id == u.Id))
                .Select(u => u.Id);

            foreach (var userId in userIds)
            {
                await _groupUserService.AddUserToGroup(userId, group.Id);

                var connections = await _connectionService.GetConnectionsByUserId(userId);
                foreach (var connection in connections)
                {
                    await Groups.AddToGroupAsync(connection.ConnectionId, group.Id.ToString());
                }
            }
        }

        public async Task SendMessageToGroup(SendMessageToGroupMessage message)
        {
            var user = await _userService.GetUserByConnectionId(Context.ConnectionId);

            var group = await _groupService.GetGroupByGroupName(message.GroupName);

            var chatMessage = await _chatMessageService.CreateChatMessage(message.Message, group.Id, user.Id);

            var receiveMessage = new ReceiveMessage
            {
                Id = chatMessage.Id,
                GroupName = group.Name,
                Text = chatMessage.Text
            };

            await Clients.Group(group.Id.ToString()).SendAsync("ReceiveMessage", receiveMessage);
        }

        public async Task GetChatMessages(GetChatMessagesMessage message)
        {
            var user = await _userService.GetUserByConnectionId(Context.ConnectionId);
            var group = await _groupService.GetGroupByGroupName(message.GroupName);
            var chatMessages = await _chatMessageService.GetChatMessagesForUser(group.Id, message.Page, user.Id);

            var messagesToSend = new List<ReceiveMessage>();

            foreach (var chatMessage in chatMessages)
            {
                messagesToSend.Add(
                    new ReceiveMessage
                    {
                        Id = chatMessage.Id,
                        GroupName = group.Name,
                        Text = chatMessage.Text,
                        UserName = chatMessage.User.UserName,
                    }
                );
            }

            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessages", messagesToSend);
        }

        public async Task DeleteChatMessage(DeleteChatMessage message)
        {
            var chatMessage = await _chatMessageService.GetChatMessageById(message.ChatMessageId);

            var currentUser = await _userService.GetUserByConnectionId(Context.ConnectionId);

            if (chatMessage.UserId == currentUser.Id)
            {
                if (message.DeleteForAll)
                {
                    await _chatMessageService.DeleteChatMessageForAll(chatMessage);
                    await Clients.Group(chatMessage.GroupId.ToString()).SendAsync("DeleteMessage", message.ChatMessageId);
                }
                else
                {
                    chatMessage.IsDeletedForUser = true;
                    await _chatMessageService.UpdateChatMessage(chatMessage);

                    var user = await _userService.GetUserByConnectionId(Context.ConnectionId);
                    var connections = await _connectionService.GetConnectionsByUserId(user.Id);
                    foreach (var connection in connections)
                    {
                        await Clients.Group(connection.ConnectionId).SendAsync("DeleteMessage", message.ChatMessageId);
                    }
                }
            }
        }

        public async Task UpdateChatMessage(UpdateChatMessage message)
        {
            var chatMessage = await _chatMessageService.GetChatMessageById(message.ChatMessageId);

            var currentUser = await _userService.GetUserByConnectionId(Context.ConnectionId);

            if (chatMessage.UserId == currentUser.Id)
            {
                chatMessage.Text = message.NewText;
                await _chatMessageService.UpdateChatMessage(chatMessage);
            }

            await Clients.Group(chatMessage.GroupId.ToString()).SendAsync("UpdateChatMessage", message);
        }

        public async Task GetGroups()
        {
            var user = await _userService.GetUserByConnectionId(Context.ConnectionId);
            var groups = await _groupService.GetGroupsByUserId(user.Id);

            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveGroups", groups.Select(g => g.Name).ToList());
        }

    }
}
