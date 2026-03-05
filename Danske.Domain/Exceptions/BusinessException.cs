
using System.Net;

namespace Danske.Domain.Exceptions
{
    public class BusinessException : Exception
    {
        public HttpStatusCode ErrorCode { get; init; }

        public BusinessException(string message, HttpStatusCode errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
