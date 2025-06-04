using Microsoft.AspNetCore.SignalR;

namespace SoCot_HC_BE.Hub
{
    public class AppHub: Microsoft.AspNetCore.SignalR.Hub
    {

        public async Task NotifyPatientDepartmentTransactionReload(string message = "Patient department transaction list refreshed!")
        {
            await Clients.All.SendAsync("ReloadPatientDepartmentTransactions", message);
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
