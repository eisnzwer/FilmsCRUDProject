using FilmsCRUDProject.Dto;
using FluentValidation;

namespace FilmsCRUDProject.Validators;

public class UserDtoValidator : AbstractValidator<UserDto>
{
	public UserDtoValidator()
	{
		RuleFor(x => x.Email)
			.EmailAddress()
			.WithMessage("Некорректный формат email");

		RuleFor(x => x.Phone)
			.Matches(@"^(\+7|8)?[\s\-]?\(?\d{3}\)?[\s\-]?\d{3}[\s\-]?\d{2}[\s\-]?\d{2}$")
			.WithMessage("Некорректный формат номера телефона");

		RuleFor(x => x.Login)
			.NotEmpty()
			.WithMessage("Логин пользователя должен быть указан");

		RuleFor(x => x.Password)
			.NotEmpty()
			.WithMessage("Пароль пользователя должен быть указан");
	}
}