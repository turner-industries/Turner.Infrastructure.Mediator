using System.Threading.Tasks;

namespace Turner.Infrastructure.Mediator
{
    public static class StringExtensions
    {
        public static Error AsError(this string errorMessage, string propertyName = "")
            => new Error { ErrorMessage = errorMessage, PropertyName = propertyName };

        public static Response AsErrorResponse(this string errorMessage, string propertyName = "")
            => errorMessage.AsError(propertyName).AsErrorResponse();

        public static Response<TResult> AsErrorResponse<TResult>(this string errorMessage, string propertyName = "")
            => errorMessage.AsError(propertyName).AsErrorResponse<TResult>();

        public static Task<Response> AsErrorResponseAsync(this string errorMessage, string propertyName = "")
            => errorMessage.AsError(propertyName).AsErrorResponseAsync();

        public static Task<Response<TResult>> AsErrorResponseAsync<TResult>(this string errorMessage, string propertyName = "")
            => errorMessage.AsError(propertyName).AsErrorResponseAsync<TResult>();
    }
}
