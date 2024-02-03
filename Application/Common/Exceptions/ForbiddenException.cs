using System.Net;

namespace Application.Common.Exceptions
{
    public class ForbiddenException : Exception
    {
        public int Code => (int)HttpStatusCode.Forbidden;

        public ForbiddenException()
            : base("Доступ запрещен")
        {
        }
    }
}
