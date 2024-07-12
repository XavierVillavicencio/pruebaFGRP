using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using pruebaFGRP.Interfaces;
using System.Linq;
namespace pruebaFGRP.Data
{
    public class OrdersMaintenance : IConfigData
    {
        
        private readonly ILogger<Worker> _logger;
        private pruebaFGRPContext _context;
        private readonly IServiceProvider _serviceProvider;
        public OrdersMaintenance(ILogger<Worker> logger, IServiceProvider serviceProvider) {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task CheckOrderStatus()
        {

            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<pruebaFGRPContext>();

                    if (dbContext == null)
                    {
                        throw new InvalidOperationException("Database context no se ha inicializado");
                    }

                    try
                    {
                        _logger.LogInformation("Validando cantidad de ordenes...");

                        List<Models.Orders> orders = await dbContext.Orders
                        .Where(o =>o.IsActive.Equals("1"))
                        .OrderByDescending(o => o.CreatedAt)
                        .Take(10)
                        .ToListAsync();
                        //string? value = config.Value.ToString();
                        _logger.LogInformation("Hemos realizado la actualización de ordenes");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error al consultar la base de datos");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "No se pudo traer la data de la base de datos.");
                throw; // Re-throw the exception or handle it as appropriate for your application
            }
        }

        public async Task GetConfigVar(string Key)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<pruebaFGRPContext>();
                    
                    if (dbContext == null)
                    {
                        throw new InvalidOperationException("Database context no se ha inicializado");
                    }

                    try
                    {
                        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                        var searchTextLower = Key.ToLower();
                        var config = await dbContext.Config
                            .Where(c => c.Key.ToLower().Contains(searchTextLower))
                            .FirstOrDefaultAsync();
                        string? value = config.Value.ToString();
                        // CheckOrderStatus(); // Ejecutamos el servicio para la comprobación de funciones.
                        _logger.LogInformation("Cantidad de segundos para ejecutar la validación:" + value);

                        await Task.Delay(Convert.ToInt32(value));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error al consultar la base de datos");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "No se pudo traer la data de la base de datos.");
                throw; // Re-throw the exception or handle it as appropriate for your application
            }
        }
    }
}
