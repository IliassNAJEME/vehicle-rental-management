using concessionnaireVoituesGrA.Models;
using concessionnaireVoituesGrA.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace concessionnaireVoituesGrA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsAPIController : ControllerBase
    {
        InterfaceClients service;
        public ClientsAPIController(InterfaceClients service) //DI
        { this.service = service; }


        // GET: api/<ClientsAPIController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(service.GetAllClients());
        }

        // GET api/<ClientsAPIController>/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var client = service.GetClient(id);
            if (client == null)
            {
                return NotFound("Client not found!");
            }
            return Ok(client)   ;
        }

        // POST api/<ClientsAPIController>
        [HttpPost]  
        public void Post(ClientDto model)
        {
            service.Ajouter(model);
        }

        // PUT api/<ClientsAPIController>/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] ClientDto model)
        {
            service.Modifier(id, model);
        }

        // DELETE api/<ClientsAPIController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            service.Supprimer(id);
        }
    }
}
