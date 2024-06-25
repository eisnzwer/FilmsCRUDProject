using FilmsCRUDProject.Dto;
using FilmsCRUDProject.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FilmsCRUDProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilmsController : ControllerBase
{
	private readonly FilmsService _filmsService;
	private readonly IValidator<FilmDto> _filmDtoValidator;

	public FilmsController(FilmsService filmsService, IValidator<FilmDto> filmDtoValidator)
	{
		_filmsService = filmsService;
		_filmDtoValidator = filmDtoValidator;
	}

	[HttpGet("all")]
	public async Task<ActionResult<List<FilmDto>>> GetAllFilms()
	{
		var films = await _filmsService.GetAllFilms();
		return Ok(films);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<FilmDto>> GetFilmById(int id)
	{
		var film = await _filmsService.GetFilmById(id);
		return Ok(film);
	}
	
	[HttpGet("user/{userId}")]
	public async Task<IActionResult> GetFilmsByUserId(int userId)
	{
		var films = await _filmsService.GetFilmsByUserId(userId);
		return Ok(films);
	}

	[HttpPost]
	public async Task<IActionResult> AddFilm(FilmDto filmDto)
	{
		var validationResult = await _filmDtoValidator.ValidateAsync(filmDto);
        
		if (!validationResult.IsValid)
		{
			throw new ValidationException(validationResult.Errors);
		}
		
		await _filmsService.AddFilm(filmDto);
		return Ok();
	}
	
	[HttpPost("AddFilmToUser")]
	public async Task<IActionResult> AddFilmToUser(FilmsForUserDto filmsForUserDto)
	{
		await _filmsService.AddFilmToUser(filmsForUserDto);
		return Ok();
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateFilm(int id, FilmDto filmDto)
	{
		var validationResult = await _filmDtoValidator.ValidateAsync(filmDto);
        
		if (!validationResult.IsValid)
		{
			throw new ValidationException(validationResult.Errors);
		}
		
		await _filmsService.UpdateFilm(id, filmDto);
		return Ok();
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteFilm(int id)
	{
		await _filmsService.DeleteFilm(id);
		return Ok();
	}
}