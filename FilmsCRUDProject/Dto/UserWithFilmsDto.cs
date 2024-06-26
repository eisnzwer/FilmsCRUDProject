namespace FilmsCRUDProject.Dto;

public record UserWithFilmsDto
{
	public List<FilmDto> Films { get; init; }
}