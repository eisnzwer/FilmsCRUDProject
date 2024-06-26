using FilmsCRUDProject.Dto;
using FluentValidation;

namespace FilmsCRUDProject.Validators;

public class FilmDtoValidator : AbstractValidator<FilmDto>
{
	public FilmDtoValidator()
	{
		RuleFor(filmDto => filmDto.Name)
			.NotEmpty()
			.WithMessage("Название фильма должно быть указано");
		
		RuleFor(filmDto => filmDto.Producer)
			.NotEmpty()
			.WithMessage("Имя режиссера должно быть указано");
		
		RuleFor(filmDto => filmDto.Year)
			.NotEmpty()
			.WithMessage("Год выпуска фильма должен быть указан");
		
		RuleFor(filmDto => filmDto.Year)
			.InclusiveBetween(1800, DateTime.UtcNow.Year)
			.WithMessage("Год выпуска фильма должен быть валидным");
	}
}