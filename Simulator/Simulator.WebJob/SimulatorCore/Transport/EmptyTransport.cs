using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.Logging;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.Simulator.WebJob.SimulatorCore.Transport
{
    public class EmptyTransport : ITransport
    {
        private readonly ILog _logger = LogProvider.GetCurrentClassLogger();

        public EmptyTransport()
        {
        }

        public async Task OpenAsync()
        {
            await Task.FromResult(0);
        }

        public async Task CloseAsync()
        {
            await Task.FromResult(0);
        }

        public async Task SendEventAsync(dynamic eventData)
        {
            var eventId = Guid.NewGuid();
            await SendEventAsync(eventId, eventData);
        }

        public async Task SendEventAsync(Guid eventId, dynamic eventData)
        {
            _logger.Info("SendEventAsync called:");
            _logger.InfoFormat("SendEventAsync: EventId: {0}", eventId);
            string eventString = eventData.ToString();
            _logger.InfoFormat("SendEventAsync: message: {0}", eventString);

            await Task.FromResult(0);
        }

        public async Task SendEventBatchAsync(IEnumerable<Client.Message> messages)
        {
            _logger.Info("SendEventBatchAsync called");

            await Task.FromResult(0);
        }

        public async Task<DeserializableCommand> ReceiveAsync()
        {
            _logger.Info("ReceiveAsync: waiting...");

            return await Task.FromResult(new DeserializableCommand(new Client.Message()));
        }

        public async Task SignalAbandonedCommand(DeserializableCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            await Task.FromResult(0);
        }

        public async Task SignalCompletedCommand(DeserializableCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            await Task.FromResult(0);
        }

        public async Task SignalRejectedCommand(DeserializableCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            await Task.FromResult(0);
        }

        public async Task UpdateReportedPropertiesAsync(TwinCollection reportedProperties)
        {
            _logger.Info("UpdateReportedPropertiesAsync called");
            _logger.Info($"UpdateReportedPropertiesAsync: reportedProperties: {reportedProperties.ToJson(Formatting.Indented)}");

            await Task.FromResult(0);
        }

        public async Task<Twin> GetTwinAsync()
        {
            _logger.Info("GetTwinAsync called");

            return await Task.FromResult(new Twin());
        }

        public Task SetMethodHandlerAsync(string methodName, MethodCallback callback)
        {
            _logger.Info(FormattableString.Invariant($"SetMethodHandler called: {methodName} -> {callback.Method.Name}"));

            return Task.FromResult(0);
        }

        public void SetDesiredPropertyUpdateCallback(DesiredPropertyUpdateCallback callback)
        {
            _logger.Info(FormattableString.Invariant($"SetDesiredPropertyUpdateCallback called, callback = {callback.Method.Name}"));
        }
    }
}
