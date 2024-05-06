using Hospital.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers;

[ApiController]
[Route("/api/doctors")]
public class DoctorController : ControllerBase
{
    private IDoctorRepository _doctorRepository;

    public DoctorController(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }
    
    [HttpGet("{id:int}")]
    public IActionResult GetDoctor(int id)
    {
        var doctor = _doctorRepository.GetDoctor(id);
        return Ok(doctor);
    }
    
    [HttpDelete("{id:int}")]
    public IActionResult DeleteDoctor(int id)
    {
        var affectedRows = _doctorRepository.DeleteDoctor(id);
        Console.WriteLine("Rows affected: " + affectedRows);
        return NoContent();
    }
    
}