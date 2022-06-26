using DMConnect.Server;
using Domain.Services;

namespace Server.API.Services;

public class HostedHubService : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILoggerFactory _loggerFactory;
    private readonly int _port;

    private IServiceScope _scope = null!;
    private DedicatedMachineHub _hub = null!;

    public HostedHubService(IServiceScopeFactory scopeFactory, ILoggerFactory loggerFactory, Settings settings)
    {
        _scopeFactory = scopeFactory;
        _loggerFactory = loggerFactory;
        _port = settings.DmConnectHubPort;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _scope = _scopeFactory.CreateScope();
        _hub = new DedicatedMachineHub(_loggerFactory,
            _scope.ServiceProvider.GetRequiredService<IDedicatedMachineService>(),
            _scope.ServiceProvider.GetRequiredService<IInstanceService>(),
            _port);

        _hub.Start();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _hub.Stop();
        _scope.Dispose();
        return Task.CompletedTask;
    }
}