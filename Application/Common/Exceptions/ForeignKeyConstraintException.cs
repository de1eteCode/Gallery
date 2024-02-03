namespace Application.Common.Exceptions;

public class ForeignKeyConstraintException : Exception
{
    public ForeignKeyConstraintException(string message)
        : base(message)
    {
    }
}