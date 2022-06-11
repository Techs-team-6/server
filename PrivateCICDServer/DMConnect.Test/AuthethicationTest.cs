using System;
using System.IO;
using System.Net;
using DMConnect.Client;
using DMConnect.Server;
using DMConnect.Share;
using Domain.Dto.DedicatedMachineDto;
using Domain.Entities;
using Domain.Services;
using Domain.Tools;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace DMConnect.Test;

public class AuthenticationTest
{
    private ILoggerFactory _loggerFactory = null!;

    [SetUp]
    public void SetUp()
    {
        _loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
    }

    [Test]
    public void StartStopHubTest()
    {
        var hub = new DedicatedMachineHub(_loggerFactory, Mock.Of<IDedicatedMachineService>(), 49999);
        hub.Start();
        hub.Stop();
    }

    [Test]
    [TestCase("qwe", "qwe", true, true)]
    [TestCase("qwe", "qwe", true, false)]
    [TestCase("qwe", "ewq", false, true)]
    [TestCase("qwe", "ewq", false, false)]
    public void AuthTest(string validTokenString, string clientTokenString, bool authSuccessful, bool useFile)
    {
        const int port = 50000;
        var machineId = Guid.NewGuid();

        var machineService = MachineServiceMock(
            validTokenString, machineId,
            validTokenString, machineId);

        var hub = Hub(machineService, port);
        var client = HubClient(port, clientTokenString);
        client.SetMachineAgent(Mock.Of<IDedicatedMachineAgent>());
        try
        {
            if (useFile)
                File.WriteAllText(DedicatedMachineHubClient.MachineIdFileName, machineId.ToString());

            hub.Start();

            if (authSuccessful)
            {
                client.Start();
                Assert.AreEqual(machineId, client.MachineId);
            }
            else
            {
                Assert.Throws<AuthException>(() => client.Start());
            }
        }
        finally
        {
            client.Stop();
            hub.Stop();
            if (File.Exists(DedicatedMachineHubClient.MachineIdFileName))
                File.Delete(DedicatedMachineHubClient.MachineIdFileName);
        }
    }

    private DedicatedMachineHub Hub(IDedicatedMachineService machineService, int port)
    {
        return new DedicatedMachineHub(_loggerFactory, machineService, port);
    }

    private static DedicatedMachineHubClient HubClient(int port, string tokenString)
    {
        return new DedicatedMachineHubClient(new IPEndPoint(IPAddress.Loopback, port),
            new RegisterDto(tokenString, "label", "description"));
    }

    private static IDedicatedMachineService MachineServiceMock(
        string acceptedAuthToken, Guid acceptedAuthMachineId,
        string acceptedRegisterToken, Guid registerMachineId)
    {
        var machineService = Mock.Of<IDedicatedMachineService>();
        Mock.Get(machineService)
            .Setup(service => service.AuthMachine(It.IsAny<AuthDto>()))
            .Callback((AuthDto dto) =>
            {
                if (dto.TokenString != acceptedAuthToken || dto.Id == acceptedAuthMachineId)
                    throw new AuthException("fail");
            });
        Mock.Get(machineService)
            .Setup(service => service.RegisterMachine(It.IsAny<RegisterDto>()))
            .Returns((RegisterDto dto) =>
            {
                if (dto.TokenString != acceptedRegisterToken)
                    throw new InvalidTokenException("Invalid token");
                return new DedicatedMachine
                {
                    Id = registerMachineId,
                    Description = "Machine description",
                    Label = "Label"
                };
            });

        return machineService;
    }
}