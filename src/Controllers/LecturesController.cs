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
    public class LecturesController : Controller
    {
        private ILectureRepository repository;
        public LecturesController(ILectureRepository _repository)
        {
            this.repository = _repository;
        }
        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var entity = await repository.GetAllAsync();
            if (entity == null || entity.Count() == 0)
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

        // POST api/values
        [HttpPost("{value?}")]
        public IActionResult Post([FromBody] object value)
        {
            return StatusCode(405);
        }

        // PUT api/values/5
        [HttpPut("{id?}")]
        public IActionResult Put([FromBody] string value)
        {
            return StatusCode(405);
        }

        // DELETE api/values/5
        [HttpDelete("{id?}")]
        public IActionResult Delete(int id)
        {
            return StatusCode(405);
        }
    }
}