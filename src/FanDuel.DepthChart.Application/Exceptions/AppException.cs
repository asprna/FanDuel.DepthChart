using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Application.Exceptions
{
    /// <summary>
    /// Application specific exceptions.
    /// </summary>
    public abstract class AppException : Exception
    {
        public AppException(int statusCode, string title, string details) : base(details)
        {
            StatusCode = statusCode;
            Title = title;
            Details = details;
        }

        public AppException(int statusCode, string title, string details, Exception innerException) : base(details, innerException)
        {
            StatusCode = statusCode;
            Title = title;
            Details = details;
        }

        public int StatusCode { get; }
        public string Title { get; }
        public string Details { get; }
    }

    /// <summary>
    /// Return internal server error when there is an issue with query execution on the database.
    /// </summary>
    public class DataContextException : AppException
    {
        public DataContextException(string details) :
            base((int)HttpStatusCode.InternalServerError, "Internal Server Error", details)
        {
        }
    }

    /// <summary>
    /// Return internal server error when unexpected behaviour happens.
    /// </summary>
    public class UnhandleException : AppException
    {
        public UnhandleException(string details) :
            base((int)HttpStatusCode.InternalServerError, "Internal Server Error", details)
        {
        }
    }

    /// <summary>
    /// Return Http No Content when the entity is not found.
    /// </summary>
    public class NoContentException(string details) : AppException((int)HttpStatusCode.NoContent, "No Content", details)
    {
    }
}
