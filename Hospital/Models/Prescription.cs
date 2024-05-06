namespace Hospital.Models;

public class Prescription
{
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public Patient Patient { get; set; }
    public List<Usage> Usages { get; set; }
}