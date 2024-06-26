using FilmsCRUDProject.Dto;
using FilmsCRUDProject.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FilmsCRUDProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
	private readonly UsersService _usersService;
	private readonly IValidator<UserDto> _userDtoValidator;

	public UsersController(UsersService usersService, IValidator<UserDto> userDtoValidator)
	{
		_usersService = usersService;
		_userDtoValidator = userDtoValidator;
	}

	[HttpGet("all")]
	public async Task<ActionResult<List<UserDto>>> GetAllUsers([FromQuery] UserFilterDto filter)
	{
		var users = await _usersService.GetAllUsers(filter);
		return Ok(users);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<UserDto>> GetUserById(int id)
	{
		var user = await _usersService.GetUserById(id);
		return Ok(user);
	}
	
	[HttpGet("{id}/films")]
	public async Task<ActionResult<UserWithFilmsDto>> GetUserWithFilms(int id)
	{
		var user = await _usersService.GetUserWithFilms(id);
		return Ok(user);
	}

	[HttpPost]
	public async Task<IActionResult> AddUser(UserDto userDto)
	{
		var validationResult = await _userDtoValidator.ValidateAsync(userDto);

		if (!validationResult.IsValid)
		{
			throw new ValidationException(validationResult.Errors);
		}
		await _usersService.AddUser(userDto);
		return Ok(userDto);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateUser(int id, UserDto userDto)
	{
		var validationResult = await _userDtoValidator.ValidateAsync(userDto);

		if (!validationResult.IsValid)
		{
			throw new ValidationException(validationResult.Errors);
		}
		await _usersService.UpdateUser(id, userDto);
		return Ok();
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteUser(int id)
	{
		await _usersService.DeleteUser(id);
		return Ok();
	}
}