namespace FilmsCRUDProject.Dto;

public record FilmFilterDto
{
	public string? Name { get; init; }
	public string? Producer { get; init; }
	public int? Year { get; init; }
	public string? SortBy { get; init; }
	public string? SortOrder { get; init; }
	public int Page { get; init; } = 1;
	public int PageSize { get; init; } = 10;
}