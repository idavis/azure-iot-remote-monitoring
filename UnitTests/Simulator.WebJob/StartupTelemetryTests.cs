
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Simulator.WebJob.SimulatorCore.Devices;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Simulator.WebJob.Cooler.Telemetry;
using Xunit;
using Moq;
using Ploeh.AutoFixture;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.UnitTests.Simulator.WebJob
{
    class StartupTelemetryTests
    {
        private StartupTelemetry telemetry;
        private IDevice _device;
        private Fixture _fixture;


        public StartupTelemetryTests()
        {
            this._fixture = new Fixture();

            this._device = this._fixture.Create<DeviceBase>();

            this.telemetry = new StartupTelemetry(this._device);
        }

        [Fact]
        public async void SendEventsAsyncUncancelledTest()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await this.telemetry.SendEventsAsync(cancellationTokenSource.Token, async (obj) =>
            {
                Assert.Equal(obj, this._device.GetDeviceInfo());
                await Task.Delay(0);
            });
        }

        [Fact]
        public async void SendEventsAsyncCancelledTest()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            var count = 0;

            await this.telemetry.SendEventsAsync(cancellationTokenSource.Token, async (obj) =>
            {
                count++;
                await Task.Delay(0);
            });

            Assert.Equal(count, 0);
        }
    }
}
