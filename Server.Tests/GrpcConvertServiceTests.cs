using Grpc.Core;
using Grpc.Core.Testing;
using Microsoft.Extensions.Logging;
using Moq;

namespace Server.Services{

    [TestClass]
    public class GrpcConvertServiceTests
    {
        private Mock<ILogger<GrpcConvertService>>? loggerMock;
        private GrpcConvertService? service;

        [TestInitialize]
        public void Initialize()
        {
            loggerMock = new Mock<ILogger<GrpcConvertService>>();
            service = new GrpcConvertService(loggerMock.Object);
        }

        private async Task<ConvertResponse> PerformConversion(double input)
        {
            var request = new ConvertRequest { Input = input };
            var context = TestServerCallContext.Create(
                method: nameof(GrpcConvertService.Convert),
                host: "localhost",
                deadline: DateTime.Now.AddMinutes(30),
                requestHeaders: [],
                cancellationToken: CancellationToken.None,
                peer: "10.0.0.25:5001",
                authContext: null,
                contextPropagationToken: null,
                writeHeadersFunc: _ => Task.CompletedTask,
                writeOptionsGetter: () => new WriteOptions(),
                writeOptionsSetter: _ => { });

            return await service.Convert(request, context);
        }

        [DataTestMethod]  
        [DataRow(0, "zero dollars")]  
        [DataRow(1, "one dollar")]  
        [DataRow(25.1, "twenty-five dollars and ten cents")]  
        [DataRow(0.01, "one cent")]  
        [DataRow(45100, "forty-five thousand one hundred dollars")]   
        [DataRow(99999999.99, "ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine dollars and ninety-nine cents")]  
        public async Task TestConvert(double before, string expected)
        {
            var actual = await PerformConversion(before);

            Assert.AreEqual(expected, actual.Output);
        }

        [DataTestMethod]  
        [DataRow(1.10, true)]   
        [DataRow(1.105325325235, false)] 
        [DataRow(1.1, true)]  
        [DataRow(100, true)] 
        [DataRow(0.01, true)] 
        [DataRow(999999999.99, true)] 
        [DataRow(100.1234, false)] 
        [DataRow(235235.235235, false)] 
        public void TestDigits(double before, bool expected)
        {
            bool actual = service.HasUpToTwoDecimalPlaces(before);

            Assert.AreEqual(expected, actual);
        }
    }
}
