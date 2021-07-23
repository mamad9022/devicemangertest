using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Sepid.DeviceManagerTest.Api.Controllers
{
    [Route("[Controller]")]
    [EnableCors("MyPolicy")]
    [ApiController]
    public class BaseController : Controller
    {
    }
}