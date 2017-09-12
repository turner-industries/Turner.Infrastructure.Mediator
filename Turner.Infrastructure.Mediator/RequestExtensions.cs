using System.Threading.Tasks;

namespace Turner.Infrastructure.Mediator
{
    public static class RequestExtensions
    {
        public static Response<TResult> AsResponse<TResult>(this TResult item)
        {
            return new Response<TResult> { Data = item };
        }

        public static Task<Response<TResult>> AsResponseAsync<TResult>(this TResult item)
        {
            return Task.FromResult(AsResponse(item));
        }

        public static Response Success(this IRequest item)
        {
            return new Response();
        }

        public static Task<Response> SuccessAsync(this IRequest item)
        {
            return Task.FromResult(Success(item));
        }

        public static Response<TResult> HasError<TResult>(this IRequest<TResult> item, string errorMessage, string property = "")
        {
            var result = new Response<TResult>();
            result.Errors.Add(new Error
            {
                PropertyName = property,
                ErrorMessage = errorMessage
            });

            return result;
        }

        public static Response HasError(this IRequest item, string errorMessage, string property = "")
        {
            var result = new Response();
            result.Errors.Add(new Error
            {
                PropertyName = property,
                ErrorMessage = errorMessage
            });

            return result;
        }

        public static Task<Response<TResult>> HasErrorAsync<TResult>(this IRequest<TResult> item, string errorMessage, string property = "")
        {
            return Task.FromResult(HasError(item, errorMessage, property));
        }

        public static Task<Response> HasErrorAsync(this IRequest item, string errorMessage, string property = "")
        {
            return Task.FromResult(HasError(item, errorMessage, property));
        }
    }
}
