using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Simulator.WebJob.Cooler.Telemetry;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Simulator.WebJob.SimulatorCore.Devices;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Simulator.WebJob.Cooler.Telemetry.Factory;
using Moq;
using Xunit;
using Ploeh.AutoFixture;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.UnitTests.Simulator.WebJob
{
    class CoolerTelemetryFactoryTests
    {
        private CoolerTelemetryFactory factory;
        private IFixture _fixture;

        public CoolerTelemetryFactoryTests()
        {
            this.factory = new CoolerTelemetryFactory();
            this._fixture = new Fixture();
        }

        [Fact]
        public void PopulateDeviceWithTelemetryEventsTest()
        {
            DeviceBase device = this._fixture.Create<DeviceBase>();

            this.factory.PopulateDeviceWithTelemetryEvents(device);

            Assert.Equal(device.TelemetryEvents.Count, 2);
            Assert.IsType<StartupTelemetry>(device.TelemetryEvents[0]);
            Assert.IsType<RemoteMonitorTelemetry>(device.TelemetryEvents[1]);
        }
    }
}
