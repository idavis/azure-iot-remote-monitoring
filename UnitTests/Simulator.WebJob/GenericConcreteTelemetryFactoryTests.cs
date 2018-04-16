
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Simulator.WebJob.SimulatorCore.Devices;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Simulator.WebJob.SimulatorCore.Telemetry;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Simulator.WebJob.SimulatorCore.Telemetry.Factory;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.UnitTests.Simulator.WebJob
{
    public class GenericConcreteTelemetryFactoryTests
    {
        private GenericConcreteTelemetryFactory telemetryFactory;
        private Mock<IDevice> deviceMock;

        public GenericConcreteTelemetryFactoryTests()
        {
            this.telemetryFactory = new GenericConcreteTelemetryFactory();
            this.deviceMock = new Mock<IDevice>();
        }

        [Fact]
        public void PopulateDeviceWithTelemetryEventsTest()
        {
            List<ITelemetry> list = new List<ITelemetry>();
            this.deviceMock.SetupGet<List<ITelemetry>>(mock => mock.TelemetryEvents).Returns(list);

            this.telemetryFactory.PopulateDeviceWithTelemetryEvents(this.deviceMock.Object);

            Assert.Equal(list.Count, 2);
        }
    }
}