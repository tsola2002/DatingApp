using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{

    //this is the base uri
    //http://localhost:5000/api/
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    //inherits controller base class without view support
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _context;

        //we need to inject our DataContext into out class through our constructor
        //and give it a name of context
        public ValuesController(DataContext context)
        {
        //we refer to context as _context to have access throughout our entire class    
        _context = context;
        
        }

        //IActionResult allows us return http responses like 200 ok e.t.c
        //we tell the method that its an async method
        //we return a task of type IActionResult
        //task represents asychronous operation that returns a value & open the thread to allow other requests
        // GET api/values
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetValues()
        {
            //throw new Exception("Test Exception");
            //_context gives us access to entity framework methods(dbset)
            //we store database values in a variable
            //inorder to get the values as a list we use to ToList method
            var values = await _context.Values.ToListAsync();

            //we return http 200 ok response along with our values
            return Ok(values);
        }

        // GET api/values/5
        //because we want to get aspecific value we use firstordefault
        //this returns null rather than exception if the value isnt found
        //firstordefault takes in a lambda expression
        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetValue(int id)
        {
            var value = _context.Values.FirstOrDefault(x => x.Id == id);

            return Ok(value);
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
