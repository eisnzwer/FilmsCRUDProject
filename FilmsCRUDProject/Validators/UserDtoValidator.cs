using FilmsCRUDProject.Dto;
using FluentValidation;

namespace FilmsCRUDProject.Validators;

public class UserDtoValidator : AbstractValidator<UserDto>
{
	public UserDtoValidator()
	{
		RuleFor(userDto => userDto.Email)
			.EmailAddress()
			.WithMessage("Некорректный формат email");

		RuleFor(userDto => userDto.Phone)
			.Matches(@"^(\+7|8)?[\s\-]?\(?\d{3}\)?[\s\-]?\d{3}[\s\-]?\d{2}[\s\-]?\d{2}$")
			.WithMessage("Некорректный формат номера телефона");

		RuleFor(userDto => userDto.Login)
			.NotEmpty()
			.WithMessage("Логин пользователя должен быть указан");

		RuleFor(userDto => userDto.Password)
			.NotEmpty()
			.WithMessage("Пароль пользователя должен быть указан");
	}
}