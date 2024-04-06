using Application.Common.Extensions;
using FluentValidation;

namespace Application.Tags.Commands.UpdateTag;

public class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommand>
{
    public UpdateTagCommandValidator()
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