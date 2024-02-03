using FluentValidation;

namespace Application.Tags.Commands.UpdateTag;

public class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommand>
{
    public UpdateTagCommandValidator()
    {
        RuleFor(e => e.Dto)
            .Cascade(CascadeMode.Stop)
            .NotNull();
        
        RuleFor(e => e.Dto.Name)
            .NotNull()
            .NotEmpty();
    }
}