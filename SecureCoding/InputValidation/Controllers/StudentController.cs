using InputValidation.Data;
using InputValidation.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InputValidation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class StudentController : ControllerBase
{
    // IDataProtector instance for encrypting and decrypting sensitive data (e.g., student IDs)

    private readonly IDataProtector _dataProtector;

    private readonly ApplicationDbContext _context;


    private static List<Student> _students = new List<Student>();

    public StudentController(IDataProtectionProvider dataProtectionProvider, ApplicationDbContext context)
    {
        // Create an IDataProtector with a specific purpose ("SecureCoding") to encrypt/decrypt student IDs
        // The purpose string ensures data encrypted here is only decryptable by this protector

        _dataProtector = dataProtectionProvider.CreateProtector("SecureCoding");

        _context = context;
    }


    [HttpPost]
    public IActionResult AddStudent([FromBody] Student student)
    {
        student.Id = _students.Count + 1;
        _students.Add(student);

        // Create a response object with the student ID encrypted using IDataProtector
        // The plain ID is never exposed; instead, a protected (encrypted) ID is sent to the client

        var response = new
        {
            Id = _dataProtector.Protect(student.Id.ToString()), // Encrypt the ID to protect it in transit ex (CfDJ8P2ZTXNE4L1GnuQq8eMH4k71PEr3aZ6skF5pob_OTbG0NdDgrQZktQJ0zCL3z2F6obL1T_zjUr7-WSa9hfma08UwZ6RXnjHZwxVuXUvWoVeXmQyujKfPiVhafPBNqkoPhQ)
            Name = $"{student.FirstName} {student.LastName}",
            Address = student.Address,
            Email = student.Email,
            Gender = student.Gender,

        };

        return Ok(response);
    }



    // Retrieves a student by their encrypted ID

    [HttpGet("{protectedId}")]
    public IActionResult GetStudent([FromRoute] string protectedId)  //ex=> protectedId (CfDJ8P2ZTXNE4L1GnuQq8eMH4k71PEr3aZ6skF5pob_OTbG0NdDgrQZktQJ0zCL3z2F6obL1T_zjUr7-WSa9hfma08UwZ6RXnjHZwxVuXUvWoVeXmQyujKfPiVhafPBNqkoPhQ)
    {
        // Decrypt the protected ID received from the route using IDataProtector
        // Note: This assumes the protected ID was previously encrypted by the same protector

        var studentId = int.Parse(_dataProtector.Unprotect(protectedId));

        var student = _students.FirstOrDefault(s => s.Id == studentId);        // Find the student with the decrypted ID
        return Ok(student);
    }


    [HttpGet()]
    public IActionResult GetAllStudents(string searchValue)
    {
        List<Student> students = [];
        if (!string.IsNullOrEmpty(searchValue))
        {
            // don't use FromSqlRaw because of the sql injection

            //students = _context.Students
            //    .FromSqlRaw($"Select * From Students Where LastName Like '%{searchValue}%'").ToList();

            // students = _context.Students.Where(s => s.LastName.Contains(searchValue)).ToList();

            students = _context.Students
                .FromSqlInterpolated($"Select * From Students Where LastName Like {"%" + searchValue + "%"}").ToList();

        }
        else
        {
            students = _context.Students.FromSqlRaw("Select * From Students").ToList();
        }
        return Ok(students);
    }


}
