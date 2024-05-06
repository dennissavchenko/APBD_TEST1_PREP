namespace Hospital.Models;

public class Doctor
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public List<Prescription>? Prescriptions { get; set; }
}