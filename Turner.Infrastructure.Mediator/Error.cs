using System.Threading.Tasks;

namespace Turner.Infrastructure.Mediator
{
    public class Error
    {
        public string PropertyName { get; set; }

        public string ErrorMessage { get; set; }
        
        public static Response AsResponse(string errorMessage, string propertyName = "")
        {
            var error = new Error
            {
                PropertyName = propertyName,
                ErrorMessage = errorMessage
            };

            return error.AsResponse();
        }

        public static Response<TResult> AsResponse<TResult>(string errorMessage, string propertyName = "")
        {
            var error = new Error
            {
                PropertyName = propertyName,
                ErrorMessage = errorMessage
            };

            return error.AsResponse<TResult>();
        }

        public static Task<Response> AsResponseAsync(string errorMessage, string propertyName = "")
        {
            return Task.FromResult(AsResponse(errorMessage, propertyName));
        }

        public static Task<Response<TResult>> AsResponseAsync<TResult>(string errorMessage, string propertyName = "")
        {
            return Task.FromResult(AsResponse<TResult>(errorMessage, propertyName));
        }
    }
}