using InputValidation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InputValidation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class StudentController : ControllerBase
{
    [HttpPost]
    public IActionResult AddStudent([FromBody] Student student)
    {
        return Ok(student);
    }
}
