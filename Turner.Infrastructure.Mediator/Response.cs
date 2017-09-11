using System.Collections.Generic;
using System.Linq;

namespace Turner.Infrastructure.Mediator
{
    public class Response
    {
        public bool HasErrors => Errors.Any();

        public List<Error> Errors { get; set; } = new List<Error>();
    }

    public class Response<TResult> : Response
    {
        public TResult Data { get; set; }
    }
}