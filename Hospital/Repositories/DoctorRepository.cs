using System.Data.SqlClient;
using Hospital.Models;

namespace Hospital.Repositories;

public class DoctorRepository : IDoctorRepository
{

    private IConfiguration _configuration;

    public DoctorRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    private Medicament GetMedicament(int id)
    {
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        connection.Open();
        using var command = new SqlCommand($"SELECT * FROM Medicament WHERE IdMedicament = {id}", connection);
        var dr = command.ExecuteReader();
        Medicament medicament = new Medicament();
        while (dr.Read())
        {
            medicament.Name = dr["Name"].ToString() ?? "";
            medicament.Description = dr["Description"].ToString() ?? "";
            medicament.Type = dr["Type"].ToString() ?? "";
        }
        return medicament;
    }

    private List<Usage> GetUsages(int id)
    {
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        connection.Open();
        using var command = new SqlCommand($"SELECT * FROM Prescription_Medicament WHERE IdPrescription = {id}", connection);
        var dr = command.ExecuteReader();
        List<Usage> usages = new List<Usage>();
        while (dr.Read())
        {
            Usage usage = new Usage
            {
                Medicament = GetMedicament((int) dr["IdMedicament"]),
                Dose = (int) dr["Dose"],
                Details = dr["Details"].ToString() ?? ""
            };
            usages.Add(usage);
        }

        return usages;
    }
    
    private Patient GetPatient(int id)
    {
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        connection.Open();
        using var command = new SqlCommand($"SELECT * FROM Patient WHERE IdPatient = {id}", connection);
        var dr = command.ExecuteReader();
        Patient patient = new Patient();
        while (dr.Read())
        {
            patient.Name = dr["FirstName"].ToString() ?? "";
            patient.Surname = dr["LastName"].ToString() ?? "";
            patient.DateOfBirth = (DateTime)dr["Birthdate"];
        }
        return patient;
    }

    private List<Prescription> GetPrescriptions(int id)
    {
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        connection.Open();
        using var command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = $"SELECT * FROM Prescription WHERE IdDoctor = {id} ORDER BY Date DESC";
        var dr = command.ExecuteReader();
        List<Prescription> prescriptions = new List<Prescription>();
        while (dr.Read())
        {
            Prescription prescription = new Prescription
            {
                Date = (DateTime)dr["Date"],
                DueDate = (DateTime)dr["DueDate"],
                Patient = GetPatient((int)dr["IdPatient"]),
                Usages = GetUsages((int)dr["IdPrescription"])
            };
            prescriptions.Add(prescription);
        }

        return prescriptions;
    }
    
    public Doctor GetDoctor(int id)
    {
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        connection.Open();
        using var command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = $"SELECT * FROM DOCTOR WHERE IdDoctor = {id}";
        var dr = command.ExecuteReader();
        Doctor doctor = new Doctor();
        while (dr.Read())
        {
            doctor.Name = dr["FirstName"].ToString() ?? "";
            doctor.Surname = dr["LastName"].ToString() ?? "";
            doctor.Email = dr["Email"].ToString() ?? "";
            doctor.Prescriptions = GetPrescriptions((int) dr["IdDoctor"]);
        }

        return doctor;
    }

    private int DeleteUsage(int id)
    {
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        connection.Open();
        using var command = new SqlCommand($"DELETE FROM Prescription_Medicament WHERE IdPrescription = {id}", connection);
        return command.ExecuteNonQuery();
    }
    
    private int DeleteUsages(int id)
    {
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        connection.Open();
        using var command = new SqlCommand($"SELECT * FROM Prescription WHERE IdDoctor = {id}", connection);
        var dr = command.ExecuteReader();
        int sum = 0;
        while (dr.Read())
        {
            sum += DeleteUsage((int)dr["IdPrescription"]);
        }

        return sum;
    }

    public int DeleteDoctor(int id)
    {
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        connection.Open();
        int del = DeleteUsages(id);
        using var deletePrescriptions = new SqlCommand($"DELETE FROM Prescription WHERE IdDoctor = {id}", connection);
        using var deleteDoctor = new SqlCommand($"DELETE FROM DOCTOR WHERE IdDoctor = {id}", connection);
        return deletePrescriptions.ExecuteNonQuery() + deleteDoctor.ExecuteNonQuery() + del;
    }
}