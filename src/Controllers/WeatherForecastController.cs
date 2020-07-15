using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.MariaDB.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspNetCore.MariaDB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMariaDbWeatherForecastService _forecastDbService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMariaDbWeatherForecastService forecastDbService)
        {
            _forecastDbService = forecastDbService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecastDataModel>> Get()
        {
            return await _forecastDbService.FindAll();
        }

        [HttpGet("{id}", Name = "FindOne")]
        public async Task<ActionResult<WeatherForecastDataModel>> Get(int id)
        {
            var result = await _forecastDbService.FindOne(id);
            if (result != default)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<WeatherForecastDataModel>> Insert(WeatherForecastDataModel dto)
        {
            if (dto.Id != null)
            {
                return BadRequest("Id cannot be set for insert action.");
            }

            var id = await _forecastDbService.Insert(dto);
            if (id != default)
                return CreatedAtRoute("FindOne", new { id = id }, dto);
            else
                return BadRequest();
        }

        [HttpPut]
        public async Task<ActionResult<WeatherForecastDataModel>> Update(WeatherForecastDataModel dto)
        {
            if (dto.Id == null)
            {
                return BadRequest("Id should be set for insert action.");
            }

            var result = await _forecastDbService.Update(dto);
            if (result > 0)
                return NoContent();
            else
                return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<WeatherForecastDataModel>> Delete(int id)
        {
            var result = await _forecastDbService.Delete(id);
            if (result > 0)
                return NoContent();
            else
                return NotFound();
        }
    }
}
