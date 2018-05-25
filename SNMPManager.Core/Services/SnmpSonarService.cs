using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SNMPManager.Core.Services
{
    public class SnmpSonarService
    {
        private readonly ISnmpService _snmpService;

        public SnmpSonarService(ISnmpService snmpService)
        {
            _snmpService = snmpService;
        }

        public virtual async Task Start(string ip, string mib, SnmpMethod method, int pingEveryMs, Action<SnmpData> onResponse, CancellationToken token = default(CancellationToken))
        {
            while (!token.IsCancellationRequested)
            {
                onResponse(_snmpService.Request(ip, mib, method));
                try
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(pingEveryMs), token);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }
    }

    public enum SnmpMethod
    {
        WALK
    }

    public interface ISnmpService
    {
        SnmpData Request(string ip, string mib, SnmpMethod method);
    }

    public class SnmpData : Dictionary<string, object>
    {
    }
}
