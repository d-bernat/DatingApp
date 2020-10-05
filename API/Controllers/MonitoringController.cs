using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class MonitoringController : ControllerBase
    {
        [HttpGet]
        [Route("Alive")]
        public string Get() => "OK";
    }
}
