using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Http;
namespace SL.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CineController : ControllerBase
    {
        [HttpGet(Name = "GetAll")]
        public IActionResult GetAll()
        {
            ML.Result result = BL.Cine.GetAll();
            if(result.Correct)
            {
                return StatusCode(200, result);
            }
            else
            {
                return StatusCode(400, result);
            }
        }
        [HttpDelete(Name ="Delete")]
        public IActionResult Delete(int idCine)
        {
            ML.Result result = BL.Cine.Delete(idCine);
            if(result.Correct)
            {
                return StatusCode(200, result);
            }
            else
            {
                return StatusCode(400, result);
            }
        }
        [HttpPost]
        public IActionResult Add(ML.Cine cine)
        {
            ML.Result result = BL.Cine.Add(cine);
            if (result.Correct)
            {
                return StatusCode(200, result);
            }
            else
            {
                return StatusCode(400, result);
            }
        }
        [HttpGet("{idCine}",Name = "GetById")]
        public IActionResult GetById(int idCine)
        {
            ML.Result result = BL.Cine.GetById(idCine);
            if (result.Correct)
            {
                return StatusCode(200, result);
            }
            else
            {
                return StatusCode(400, result);
            }
        }
        [HttpPut("{idCine}")]
        public IActionResult Update(int idCine,[FromBody]ML.Cine cine)
        {
            cine.IdCine = idCine;
            ML.Result result = BL.Cine.Update(cine);
            if (result.Correct)
            {
                return StatusCode(200, result);
            }
            else
            {
                return StatusCode(400, result);
            }
        }
    }
}
