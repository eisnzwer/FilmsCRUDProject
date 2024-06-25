using FilmsCRUDProject.Dto;
using FluentValidation;

namespace FilmsCRUDProject.Validators;

public class FilmDtoValidator : AbstractValidator<FilmDto>
{
	public FilmDtoValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty()
			.WithMessage("Название фильма должно быть указано");
		
		RuleFor(x => x.Producer)
			.NotEmpty()
			.WithMessage("Имя режиссера должно быть указано");
		
		RuleFor(x => x.Year)
			.NotEmpty()
			.WithMessage("Год выпуска фильма должен быть указан");
		
		RuleFor(x => x.Year)
			.InclusiveBetween(1800, DateTime.UtcNow.Year)
			.WithMessage("Год выпуска фильма должен быть валидным");
	}
}