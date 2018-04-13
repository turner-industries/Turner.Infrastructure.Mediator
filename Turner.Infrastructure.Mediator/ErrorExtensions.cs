using System.Collections.Generic;
using System.Threading.Tasks;

namespace Turner.Infrastructure.Mediator
{
    public static class ErrorExtensions
    {
        public static Response AsResponse(this Error error)
        {
            var result = new Response
            {
                Errors = new List<Error> { error }
            };

            return result;
        }

        public static Response<TResult> AsResponse<TResult>(this Error error)
        {
            var result = new Response<TResult>
            {
                Errors = new List<Error> {error}
            };

            return result;
        }

        public static Task<Response> AsResponseAsync(this Error error)
        {
            return Task.FromResult(error.AsResponse());
        }

        public static Task<Response<TResult>> AsResponseAsync<TResult>(this Error error)
        {
            return Task.FromResult(error.AsResponse<TResult>());
        }
    }
}
