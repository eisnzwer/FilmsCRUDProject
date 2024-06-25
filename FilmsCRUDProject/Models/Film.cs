namespace FilmsCRUDProject.Models;

public class Film
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Producer { get; set; }
	public int Year { get; set; }
	public List<User> Users { get; set; }
}