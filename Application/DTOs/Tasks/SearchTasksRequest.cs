namespace Application.DTOs.Tasks;

public class SearchTasksRequest
{
    public string? Query { get; set; }
    public int? Color { get; set; }
    public int? StatCategory { get; set; }
    public int? Status { get; set; }
    public bool? IsRequired { get; set; }
    public int? Priority { get; set; }
}
