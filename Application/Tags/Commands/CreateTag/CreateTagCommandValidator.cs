using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Tags.Commands.CreateTag;

public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
    public CreateTagCommandValidator(IApplicationDbContext context)
    {
        RuleFor(e => e.Dto)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .ChildRules(dto =>
            {
                dto.RuleFor(e => e.Name)
                    .NotNull()
                    .NotEmpty();

                dto.RuleFor(e => e.SearchKey)
                    .NotNull()
                    .NotEmpty()
                    .Must(e => !e.Contains(' '))
                    .WithMessage("Поисковый ключ не должен содержать пробелы")
                    .MustAsync(async (value, ctx) =>
                        await context.Tags.AnyAsync(s =>
                            !string.Equals(s.SearchKey, value, StringComparison.OrdinalIgnoreCase), ctx))
                    .WithMessage("Поисковый ключ должен быть уникальным");
            });
    }
}