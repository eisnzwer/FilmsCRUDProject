namespace FilmsCRUDProject.Dto;

public record UserFilterDto
{
	public string? Email { get; init; }
	public string? Phone { get; init; }
	public string? Login { get; init; }
	public string? SortBy { get; init; }
	public string? SortOrder { get; init; }
	public int Page { get; init; } = 1;
	public int PageSize { get; init; } = 10;
}