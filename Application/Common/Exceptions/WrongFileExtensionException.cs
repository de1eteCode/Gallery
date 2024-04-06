using System.Net;
using Application.Common.Interfaces;

namespace Application.Common.Exceptions;

public class WrongFileExtensionException : Exception, IRestException
{
    public int Code => (int)HttpStatusCode.BadRequest;

    public WrongFileExtensionException(string extension)
        : base($"Недопустимое расширение файла: {extension}.")
    {
    }
}