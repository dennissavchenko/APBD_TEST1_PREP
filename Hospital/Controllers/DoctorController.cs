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
    public IActionResult GetDoctors(int id)
    {
        var doctor = _doctorRepository.GetDoctor(id);
        return Ok(doctor);
    }
    
}