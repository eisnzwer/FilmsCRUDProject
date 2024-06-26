namespace FilmsCRUDProject.Dto;

public record FilmDto
{
	public string Name { get; init; }
	public string Producer { get; init; }
	public int Year { get; init; }
}