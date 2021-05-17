using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Gruppe19_NGK_Aflevering3_OpgaveA.Hubs
{
    public class WeatherObservationHub : Hub
    {
        public async Task SendMessage(string user, string message)
            {
                await Clients.All.SendAsync("SendObservation", message);
            }
    }
}
