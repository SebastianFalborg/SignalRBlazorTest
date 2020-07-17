using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingalRBlazor.Server.Hubs
{
    public class PollHub : Hub
    {
        public async Task SubscribeToPollList()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "PollListUpdates");
        }

        public async Task SubscribeToPoll(Guid pollId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, pollId.ToString());
        }
    }
}
