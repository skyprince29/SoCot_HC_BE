using Microsoft.AspNetCore.SignalR;

namespace SoCot_HC_BE.Hub
{
    public class AppHub: Microsoft.AspNetCore.SignalR.Hub
    {

        public async Task NotifyPatientDepartmentTransactionReload(string message = "Reload page.")
        {
            await Clients.All.SendAsync("ReloadPageAsyncSignalR", message);
         }

        // Optional: Connection Lifecycle Methods
        public override async Task OnConnectedAsync()
        {
            //Console.WriteLine($"[SignalR] Patient Department Transaction Hub client connected: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            //Console.WriteLine($"[SignalR] Patient Department Transaction Hub client disconnected: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }

    }
}
