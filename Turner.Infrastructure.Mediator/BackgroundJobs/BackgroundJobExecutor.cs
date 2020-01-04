using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Turner.Infrastructure.Mediator.BackgroundJobs
{
    public class BackgroundJobExecutor<T> where T : IRequest
    {
        private readonly IMediator _mediator;

        public BackgroundJobExecutor(IMediator mediator)
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
