namespace FilmsCRUDProject.Dto;

public record UserDto
{
	public string Email { get; init; }
	public string Phone { get; init; }
	public string Login { get; init; }
	public string Password { get; init; }
}