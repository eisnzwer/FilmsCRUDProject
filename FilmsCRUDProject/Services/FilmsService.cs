using FilmsCRUDProject.Data;
using FilmsCRUDProject.Dto;
using FilmsCRUDProject.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FilmsCRUDProject.Services;

public class FilmsService
{
    private readonly IValidator<FilmDto> _filmDtoValidator;
    private readonly AppDbContext _context;

    public FilmsService(IValidator<FilmDto> filmDtoValidator, AppDbContext context)
    {
        _filmDtoValidator = filmDtoValidator;
        _context = context;
    }

    public async Task<List<FilmDto>> GetAllFilms()
    {
        var films = await _context.Films.ToListAsync();
        return films.Select(film => new FilmDto
        {
            Name = film.Name,
            Producer = film.Producer,
            Year = film.Year
        }).ToList();
    }

    public async Task<FilmDto> GetFilmById(int id)
    {
        var film = await _context.Films.FindAsync(id);

        if (film == null)
        {
            throw new KeyNotFoundException($"Фильм с id {id} не найден");
        }

        return new FilmDto
        {
            Name = film.Name,
            Producer = film.Producer,
            Year = film.Year
        };
    }

    public async Task UpdateFilm(int id, FilmDto filmDto)
    {
        var existingFilm = await _context.Films.FindAsync(id);

        if (existingFilm == null)
        {
            throw new KeyNotFoundException($"Фильм с id {id} не найден");
        }

        var validationResult = await _filmDtoValidator.ValidateAsync(filmDto);

        if (!validationResult.IsValid)
        {
            throw new ValidationException("Ошибка валидации", validationResult.Errors);
        }

        existingFilm.Name = filmDto.Name;
        existingFilm.Producer = filmDto.Producer;
        existingFilm.Year = filmDto.Year;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteFilm(int id)
    {
        var film = await _context.Films.FindAsync(id);

        if (film == null)
        {
            throw new KeyNotFoundException($"Фильм с id {id} не найден");
        }

        _context.Films.Remove(film);
        await _context.SaveChangesAsync();
    }

    public async Task<List<FilmDto>> GetFilmsByUserId(int userId)
    {
        var user = await _context.Users
            .Include(u => u.Films)
            .FirstOrDefaultAsync(u => u.Id == userId);
    
        if (user == null)
        {
            throw new KeyNotFoundException($"Пользователь с id {userId} не найден");
        }

        return user.Films.Select(film => new FilmDto
        {
            Name = film.Name,
            Producer = film.Producer,
            Year = film.Year
        }).ToList();
    }

    public async Task AddFilm(FilmDto filmDto)
    {
        var validationResult = await _filmDtoValidator.ValidateAsync(filmDto);
    
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
    
        var newFilm = new Film
        {
            Name = filmDto.Name,
            Producer = filmDto.Producer,
            Year = filmDto.Year
        };
    
        _context.Add(newFilm);
        await _context.SaveChangesAsync();
    }
    
    public async Task AddFilmToUser(FilmsForUserDto filmsForUserDto)
    {
        var user = await _context.Users
            .Include(u => u.Films)
            .FirstOrDefaultAsync(u => u.Id == filmsForUserDto.UserId);

        if (user == null)
        {
            throw new KeyNotFoundException($"Пользователь с id {filmsForUserDto.UserId} не найден");
        }

        var validationResult = await _filmDtoValidator.ValidateAsync(filmsForUserDto.Film);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var newFilm = new Film
        {
            Name = filmsForUserDto.Film.Name,
            Producer = filmsForUserDto.Film.Producer,
            Year = filmsForUserDto.Film.Year
        };

        user.Films.Add(newFilm);
        await _context.SaveChangesAsync();
    }
}
