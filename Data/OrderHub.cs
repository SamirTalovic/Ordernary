using Microsoft.AspNetCore.SignalR;

namespace Ordernary.Data
{
    public class OrderHub : Hub
    {
        public async Task SendOrderUpdate()
        {
            await Clients.All.SendAsync("ReceiveOrderUpdate");
        }
    }
}
