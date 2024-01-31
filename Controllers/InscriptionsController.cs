using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SellIntegro.Models;
using SellIntegro.Services;

namespace SellIntegro.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class InscriptionsController : ControllerBase
    {
        private readonly IInscriptionsService _inscriptionsService;
        public InscriptionsController(IInscriptionsService inscriptionsService) 
        {
            _inscriptionsService = inscriptionsService;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Inscription>>> GetInscriptions()
        {
            return Ok(await _inscriptionsService.GetInscriptions());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Inscription>> GetSingleInscription(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var inscription = await _inscriptionsService.GetSingleInscription(id);
            if(inscription == null)
            {
                return NotFound();
            }

            return Ok(inscription);
        }

        [HttpPost()]
        public async Task<ActionResult> AddInscription(Inscription inscription)
        {
            await _inscriptionsService.AddInscription(inscription);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateInscription(int id, Inscription inscription)
        {
            if(id == 0)
            {
                return BadRequest();
            }

            if ((await _inscriptionsService.GetSingleInscription(id)) == null)
            {
                return NotFound();
            }

            inscription.Id = id;
            await _inscriptionsService.UpdateInscription(inscription);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInscription(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var inscription = await _inscriptionsService.GetSingleInscription(id);
            if (inscription == null)
            {
                return NotFound();
            }

            await _inscriptionsService.DeleteInscription(inscription);
            return NoContent();
        }
    }
}
