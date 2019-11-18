using System.Collections.Generic;
using System.Threading.Tasks;

namespace Turner.Infrastructure.Mediator
{
    public static class ErrorExtensions
    {
        public static Response AsErrorResponse(this Error error)
        {
            var result = new Response
            {
                Errors = new List<Error> { error }
            };

            return result;
        }

        public static Response<TResult> AsErrorResponse<TResult>(this Error error)
        {
            var result = new Response<TResult>
            {
                Errors = new List<Error> {error}
            };

            return result;
        }

        public static Task<Response> AsErrorResponseAsync(this Error error)
        {
            return Task.FromResult(error.AsErrorResponse());
        }

        public static Task<Response<TResult>> AsErrorResponseAsync<TResult>(this Error error)
        {
            return Task.FromResult(error.AsErrorResponse<TResult>());
        }

        public static Response AsErrorResponse(this IEnumerable<Error> errors)
        {
            var result = new Response
            {
                Errors = new List<Error>(errors)
            };

            return result;
        }

        public static Response<TResult> AsErrorResponse<TResult>(this IEnumerable<Error> errors)
        {
            var result = new Response<TResult>
            {
                Errors = new List<Error>(errors)
            };

            return result;
        }

        public static Task<Response> AsErrorResponseAsync(this IEnumerable<Error> errors)
        {
            return Task.FromResult(errors.AsErrorResponse());
        }

        public static Task<Response<TResult>> AsErrorResponseAsync<TResult>(this IEnumerable<Error> errors)
        {
            return Task.FromResult(errors.AsErrorResponse<TResult>());
        }
    }
}
