﻿namespace Application.Common.Exceptions;

public class UniqueConstraintException : Exception
{
    public UniqueConstraintException(string message)
        : base(message)
    {
    }
}