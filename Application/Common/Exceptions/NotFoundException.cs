namespace Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message)
        : base(message)
    {
    }

    public NotFoundException(string name, object key)
        : this($"Объект \"{name}\" ({key}) не был найден.")
    {
    }
}