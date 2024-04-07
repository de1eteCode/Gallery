using Gridify;

namespace Application.Posts.Queries.GetPostGrid;

/// <summary>
/// Модель запроса таблицы постов
/// </summary>
public class PostGridifyQuery : IGridifyPagination
{
    private int _pageSize = MinPageSize;
    private const int MinPageSize = 10;
    private const int MaxPageSize = 100;

    /// <summary>
    /// Страница
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Размер страницы
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize =
            value < MinPageSize
                ? MinPageSize
                : value > MaxPageSize
                    ? MaxPageSize
                    : value;
    }

    /// <summary>
    /// Поисковые ключи
    /// </summary>
    public string? SearchKeys { get; set; }
}