using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FUNAPI.Models;
using FUNAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FUNAPI.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class RoomsController : Controller
    {
        private IReadOnlyRepository<Room> repository;
        public RoomsController(IReadOnlyRepository<Room> _repository)
        {
            this.repository = _repository;
        }
        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var entity = await repository.GetAllAsync();
            if (entity == null || !entity.Any())
            {
                return NotFound();
            }
            return Ok(entity);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await repository.GetSingleAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }
    }
}