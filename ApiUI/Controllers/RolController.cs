using Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RolController : ControllerBase
    {
        private readonly IRolService _rolService;

        public RolController(IRolService rolService)
        {
            _rolService = rolService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var roller = _rolService.GetRoller();
            return Ok(roller);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var rol = _rolService.GetRolById(id);
            if (rol == null)
            {
                return NotFound();
            }
            return Ok(rol);
        }
    }
}
