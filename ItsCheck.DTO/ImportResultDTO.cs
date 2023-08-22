namespace ItsCheck.DTO;

public class ImportResultDTO
{
    public object Object { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
}