using System.Net;
using Application.Common.Interfaces;

namespace Application.Common.Exceptions;

public class WrongFileLengthException : Exception, IRestException
{
    public int Code => (int)HttpStatusCode.BadRequest;

    public WrongFileLengthException(string message)
        : base(message)
    {
    }

    public WrongFileLengthException(long length)
        : base($"Недопустимый размер файла: {length}.")
    {
    }

    public WrongFileLengthException(string fileSize, string maxSize)
        : base($"Недопустимый размер файла: {fileSize}. Максимальный допустимый размер: {maxSize}.")
    {
    }
}