using CSharpCodeCrew.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CSharpCodeCrew.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly IChartService _pieService;
        public ChartController(IChartService pieService)
        {
            _pieService = pieService;
        }
        [HttpPost("pie")]
        public async Task<IActionResult> GetChartImage([FromBody]string jsonData)
        {
            var res = await _pieService.GenerateChart(jsonData);
            return File(res, "image/png");
        }
    }
}
