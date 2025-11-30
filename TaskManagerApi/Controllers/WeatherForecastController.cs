using Microsoft.AspNetCore.Mvc;

namespace TaskManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get([FromQuery] int range = 5)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return Enumerable.Range(1, range).Select(index => new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                    })
                    .ToArray();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"the exception is: {e.Message}");
                    _logger.LogError($"the exception is: {e.Message}");
                return Array.Empty<WeatherForecast>();
                }
            }
            return Array.Empty<WeatherForecast>();
        }
    }
}
