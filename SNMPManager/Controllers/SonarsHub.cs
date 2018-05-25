using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SNMPManager.Controllers
{
    public class SonarsHub : Hub
    {
        public string Activate()
        {
            return "SonarsHub Activated";
        }

        public async Task SendSonarDataAsync(SonarDataDto sonarDataDto)
        {
            await Clients.All.SendAsync("signalR-sonar-data", sonarDataDto);
        }
    }

    public class SonarDataDto
    {
        public string Id { get; set; }
        public SonarProperties[] Data { get; set; }

        public class SonarProperties
        {
            public string PropName { get; set; }
            public object Value { get; set; }
        }
    }
}
