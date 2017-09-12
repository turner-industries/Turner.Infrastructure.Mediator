using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Turner.Infrastructure.Mediator.BackgroundJobs
{
    public class BackgoundJobExecutor<T> where T : IRequest
    {
        private readonly IMediator _mediator;

        public BackgoundJobExecutor(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(T command)
        {
            var result = await _mediator.HandleAsync(command);
            if (result.HasErrors)
            {
                throw new Exception(JsonConvert.SerializeObject(result.Errors));
            }
        }
    }
}
