using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Interfaces.IServices;
using System.Threading.Tasks;

namespace OngProject.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class RolController : ControllerBase
    {
        private readonly IRolService _rolService;
                
        public RolController(IRolService rolService)
        {
            _rolService = rolService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {            
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById()
        {           
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Insert()
        {
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update()
        {
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            return Ok();
        }
    }
}
