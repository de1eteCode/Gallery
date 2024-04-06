using Application.Common.Extensions;
using FluentValidation;

namespace Application.Tags.Commands.CreateTag;

public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
    public CreateTagCommandValidator()
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
                    .IsSearchKey();
            });
    }
}