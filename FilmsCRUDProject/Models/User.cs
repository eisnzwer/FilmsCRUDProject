namespace FilmsCRUDProject.Models;

public record User
{
    public int Id { get; init; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public List<Film> Films { get; init; }
}