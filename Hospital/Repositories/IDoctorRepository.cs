using Hospital.Models;

namespace Hospital.Repositories;

public interface IDoctorRepository
{
    public Doctor GetDoctor(int id);
    public int DeleteDoctor(int id);
}