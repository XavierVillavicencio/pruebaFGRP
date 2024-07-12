using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using pruebaFGRP.Data;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly pruebaFGRPContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HealthController(
        pruebaFGRPContext context,
        IHttpClientFactory httpClientFactory,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public class HealthCheckResult
    {
        public bool IsHealthy { get; set; }
        public string Message { get; set; }
    }

    [HttpGet]
    public async Task<IActionResult> CheckHealth()
    {
        var healthStatus = new
        {
            Database = await CheckDatabaseConnection(),
            ConfigController = await CheckControllerStatus("config"),
            LogsController = await CheckControllerStatus("logs"),
            OrdersController = await CheckControllerStatus("orders")
        };

        bool isOverallHealthy = healthStatus.Database.IsHealthy &&
                                healthStatus.ConfigController.IsHealthy &&
                                healthStatus.LogsController.IsHealthy &&
                                healthStatus.OrdersController.IsHealthy;

        if (isOverallHealthy)
        {
            return Ok(new { Status = "Healthy", Details = healthStatus });
        }
        else
        {
            return StatusCode(503, new { Status = "Unhealthy", Details = healthStatus });
        }
    }

    private async Task<HealthCheckResult> CheckDatabaseConnection()
    {
        try
        {
            bool canConnect = await _context.Database.CanConnectAsync();
            return new HealthCheckResult
            {
                IsHealthy = canConnect,
                Message = canConnect ? "Conexión a la base de datos correcta." : "No se puede conectar a la base de datos."
            };
        }
        catch (Exception ex)
        {
            return new HealthCheckResult
            {
                IsHealthy = false,
                Message = $"Error al verificar la conexión a la base de datos: {ex.Message}"
            };
        }
    }

    private async Task<HealthCheckResult> CheckControllerStatus(string controllerName)
    {
        try
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{baseUrl}/api/{controllerName}");
            bool isHealthy = response.IsSuccessStatusCode;
            return new HealthCheckResult
            {
                IsHealthy = isHealthy,
                Message = isHealthy ? $"El controlador {controllerName} está funcionando correctamente." : $"El controlador {controllerName} no está respondiendo correctamente."
            };
        }
        catch (Exception ex)
        {
            return new HealthCheckResult
            {
                IsHealthy = false,
                Message = $"Error al verificar el controlador {controllerName}: {ex.Message}"
            };
        }
    }
}