using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using soapApi.Data;
using soapApi.ViewModels;

namespace soapApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IDataRepository _repo;
        private readonly IMapper _mapper;
        public ValuesController(IDataRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;

        }
        // GET api/values
        [HttpGet]
        [EnableQuery()]
        public async Task<IActionResult> GetAllValues()
        {
            var result = await _repo.GetAllValuesAsync();

            List<ValueViewModel> vm = _mapper.Map<List<ValueViewModel>>(result);

            return Ok(vm);
        }

         [HttpGet("directvalues")]
        public async Task<List<ValueViewModel>> GetValuesDirectly()
        {
            var result = await _repo.GetAllValuesAsync();

            List<ValueViewModel> vm = _mapper.Map<List<ValueViewModel>>(result);

            return vm;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _repo.GetValueAsync(id);

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
