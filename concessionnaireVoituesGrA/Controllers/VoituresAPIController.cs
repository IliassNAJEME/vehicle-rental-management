using concessionnaireVoituesGrA.Models;
using concessionnaireVoituesGrA.Services;
using Microsoft.AspNetCore.Mvc;

namespace concessionnaireVoituesGrA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoituresAPIController : ControllerBase
    {
        InterfaceVoitures service;
        public VoituresAPIController(InterfaceVoitures service)
        {
            this.service = service;
        }

        // GET: api/<VoituresAPIController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(service.GetAllVoitures());
        }

        // GET api/<VoituresAPIController>/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var voiture = service.GetVoiture(id);
            if (voiture == null)
            {
                return NotFound("Voiture not found!");
            }
            return Ok(voiture);
        }
    }
}
