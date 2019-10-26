using System.Linq;
using Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly DatingAppContext datingAppContext;
        public ValuesController(DatingAppContext datingAppContext)
        {
            this.datingAppContext = datingAppContext;
        }
         
        [HttpGet]
        public IActionResult Get()
        {
            var Values = datingAppContext.Values.ToList();
            return Ok(Values);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetId(int id)
        {
            var Value = datingAppContext.Values.FirstOrDefault(X => X.Id == id);
            return Ok(Value);
        }
    }
}

