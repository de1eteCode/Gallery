using FluentValidation;

namespace Application.Tags.Commands.CreateTag;

public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
    public CreateTagCommandValidator()
    {
        RuleFor(e => e.Dto)
            .Cascade(CascadeMode.Stop)
            .NotNull();
        
        RuleFor(e => e.Dto.Name)
            .NotNull()
            .NotEmpty();
    }
}