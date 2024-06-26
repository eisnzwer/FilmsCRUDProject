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

    public async Task<List<UserDto>> GetAllUsers(UserFilterDto filter)
    {
        var query = _context.Users.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Login))
        {
            query = query.Where(film => film.Login.Contains(filter.Login));
        }

        if (!string.IsNullOrEmpty(filter.Email))
        {
            query = query.Where(film => film.Email.Contains(filter.Email));
        }

        if (!string.IsNullOrEmpty(filter.Phone))
        {
            query = query.Where(film => film.Phone.Contains(filter.Phone));
        }

        if (!string.IsNullOrEmpty(filter.SortBy))
        {
            if (filter.SortOrder?.ToUpper() == "DESC")
            {
                switch (filter.SortBy.ToLower())
                {
                    case "login":
                        query = query.OrderByDescending(user => user.Login);
                        break;
                    case "email":
                        query = query.OrderByDescending(user => user.Email);
                        break;
                    case "phone":
                        query = query.OrderByDescending(user => user.Phone);
                        break;
                    default:
                        query = query.OrderByDescending(user => user.Id);
                        break;
                }
            }
            else if (filter.SortOrder?.ToUpper() == "ASC")
            {
                switch (filter.SortBy.ToLower())
                {
                    case "login":
                        query = query.OrderBy(user => user.Login);
                        break;
                    case "email":
                        query = query.OrderBy(user => user.Email);
                        break;
                    case "phone":
                        query = query.OrderBy(user => user.Phone);
                        break;
                    default:
                        query = query.OrderBy(user => user.Id);
                        break;
                }
            }
        }
        
        var skipNumber = (filter.Page - 1) * filter.PageSize;
        query = query.Skip(skipNumber).Take(filter.PageSize);

        var users = await query.ToListAsync();
        return users.Select(user => new UserDto
        {
            Login = user.Login,
            Email = user.Email,
            Phone = user.Phone,
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
