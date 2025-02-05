using Grpc.Health.V1;
using Grpc.HealthCheck;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Inventory.gRPC.Extensitons
{
    public class StatusService : BackgroundService
    {
        private readonly HealthCheckService _healthCheckService;
        private readonly HealthServiceImpl _healthService;
        private readonly ILogger<StatusService> _logger;

        public StatusService(
            HealthServiceImpl healthService,
            HealthCheckService healthCheckService,
            ILogger<StatusService> logger)
        {
            _healthService = healthService;
            _healthCheckService = healthCheckService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var health = await _healthCheckService.CheckHealthAsync(stoppingToken);

                    foreach (var h in health.Entries)
                    {
                        _healthService.SetStatus(h.Key,
                            health.Status == HealthStatus.Healthy
                            ? HealthCheckResponse.Types.ServingStatus.Serving
                            : HealthCheckResponse.Types.ServingStatus.NotServing);
                    }

                    // Kiểm tra trước khi delay
                    if (stoppingToken.IsCancellationRequested)
                        break;

                    //await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
                }
            }
            catch (TaskCanceledException)
            {
                // Ghi log nếu cần, nhưng không cần xử lý mạnh tay
                Console.WriteLine("Task bị hủy do yêu cầu dừng.");
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ khác nếu có
                Console.WriteLine($"Lỗi không mong muốn: {ex.Message}");
            }
        }

    }
}