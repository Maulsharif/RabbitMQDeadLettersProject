using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ProducerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IMessageProducer _messagePublisher;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMessageProducer messageProvider)
        {
            _logger = logger;
            _messagePublisher = messageProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrder()
        {
            Order order = new()
            {
                ProductName = "Nintendo 2",
                Price = 120M,
                Quantity = 1
            };
            _messagePublisher.SendMessage(order);
            return Ok(order);
        }
    }
}
