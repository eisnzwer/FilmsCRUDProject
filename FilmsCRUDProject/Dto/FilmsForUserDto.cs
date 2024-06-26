namespace FilmsCRUDProject.Dto;

public record FilmsForUserDto
{
	public int UserId { get; init; }
	public FilmDto Film { get; init; }
}