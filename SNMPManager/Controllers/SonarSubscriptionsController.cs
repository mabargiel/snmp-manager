using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SNMPManager.Core.Services;

namespace SNMPManager.Controllers
{
    [Produces("application/json")]
    [Route("api/sonarSubscriptions")]
    public class SonarSubscriptionsController : Controller
    {
        private readonly SonarsHub _hub;
        private readonly SnmpSonarService _snmpSonar;
        private readonly IMapper _mapper;
        private readonly Dictionary<string, (Task task, CancellationTokenSource tokenSource)> _subscriptions = new Dictionary<string, (Task, CancellationTokenSource)>();

        public SonarSubscriptionsController(SonarsHub hub, SnmpSonarService snmpSonar, IMapper mapper)
        {
            _hub = hub;
            _snmpSonar = snmpSonar;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("{id}")]
        public IActionResult Subscribe(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Id cannot be null or empty");

            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            var sonarTask = Task.Factory.StartNew(async () =>
            {
                token.ThrowIfCancellationRequested();

                await _snmpSonar.Start("cokolwiek", "cokolwiek", SnmpMethod.WALK, 1000,
                    async data => await _hub.SendSonarDataAsync(_mapper.Map<SonarDataDto>(data)), token);
            }, token);
            
            _subscriptions.Add(id, (sonarTask, tokenSource));

            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult UnSubscribe(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Id cannot be null or empty");

            if (!_subscriptions.ContainsKey(id))
                return NoContent();

            var sonarTask = _subscriptions[id];

            try
            {
                sonarTask.tokenSource.Cancel();
                _subscriptions.Remove(id);
            }
            finally
            {
                sonarTask.tokenSource.Dispose();
            }

            return NoContent();
        }
    }
}