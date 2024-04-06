using FluentValidation;

namespace Application.Posts.Commands.CreatePost;

public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(e => e.Dto)
            .NotNull()
            .ChildRules(dto =>
            {
                dto.RuleFor(e => e.File)
                    .NotNull();
            });
    }
}