using FilmsCRUDProject.Data;
using FilmsCRUDProject.Dto;
using FilmsCRUDProject.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FilmsCRUDProject.Services;

public class UsersService
{
    private readonly IValidator<UserDto> _userDtoValidator;
    private readonly AppDbContext _context;

    public UsersService(IValidator<UserDto> userDtoValidator, AppDbContext context)
    {
        _userDtoValidator = userDtoValidator;
        _context = context;
    }

    public async Task<List<UserDto>> GetAllUsers()
    {
        var users = await _context.Users.ToListAsync();
        return users.Select(user => new UserDto
        {
            Email = user.Email,
            Phone = user.Phone,
            Login = user.Login,
            Password = user.Password
        }).ToList();
    }

    public async Task AddUser(UserDto userDto)
    {
        var validationResult = await _userDtoValidator.ValidateAsync(userDto);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var newUser = new User
        {
            Email = userDto.Email,
            Phone = userDto.Phone,
            Login = userDto.Login,
            Password = userDto.Password
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
    }

    public async Task<UserDto> GetUserById(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            throw new KeyNotFoundException($"Пользователь с id {id} не найден");
        }

        return new UserDto
        {
            Email = user.Email,
            Phone = user.Phone,
            Login = user.Login,
            Password = user.Password
        };
    }

    public async Task UpdateUser(int id, UserDto userDto)
    {
        var existingUser = await _context.Users.FindAsync(id);

        if (existingUser == null)
        {
            throw new KeyNotFoundException($"Пользователь с id {id} не найден");
        }

        existingUser.Email = userDto.Email;
        existingUser.Phone = userDto.Phone;
        existingUser.Login = userDto.Login;
        existingUser.Password = userDto.Password;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            throw new KeyNotFoundException($"Пользователь с id {id} не найден");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
    
    public async Task<UserWithFilmsDto> GetUserWithFilms(int id)
    {
        var user = await _context.Users
            .Include(u => u.Films)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            throw new KeyNotFoundException($"Пользователь с id {id} не найден");
        }

        return new UserWithFilmsDto
        {
            Films = user.Films.Select(film => new FilmDto
            {
                Name = film.Name,
                Producer = film.Producer,
                Year = film.Year
            }).ToList()
        };
    }
}
