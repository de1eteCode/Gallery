using FluentValidation;

namespace Application.Common.Extensions;

public static class ValidatorExtensions
{
    private static readonly char[] AvailableCharacterInSearchKey = "qwertyuiopasdfghjklzxcvbnm()_"
        .Select(e => e)
        .Order()
        .ToArray();

    public static IRuleBuilderOptions<T, string> IsSearchKey<T>(this IRuleBuilderOptions<T, string> ruleBuilder)
        => ruleBuilder.Must(e => e.ToLower().All(s => AvailableCharacterInSearchKey.Contains(s)))
            .WithMessage("Допустимые символы в поисковом ключе: " + string.Join(", ", AvailableCharacterInSearchKey));
}