using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Turner.Infrastructure.Mediator.Decorators
{
    public class TransactionBaseHandler<TRequest, TResult> where TResult : Response, new()
    {
        private readonly DbContext _context;

        public TransactionBaseHandler(DbContext context)
        {
            _context = context;
        }

        public async Task<TResult> HandleAsync(TRequest request, Func<Task<TResult>> processRequest)
        {
            if (_context.Database.CurrentTransaction != null)
            {
                return await processRequest();
            }
            
            using (var transaction = _context.Database.BeginTransaction())
            {
                var result = await processRequest();
                if (result.HasErrors)
                {
                    transaction.Rollback();
                    return result;
                }
                
                transaction.Commit();
                return result;
            }
        }
    }

    public class TransactionHandler<TRequest> : IRequestHandler<TRequest> where TRequest : IRequest
    {
        private readonly Func<IRequestHandler<TRequest>> _decorateeFactory;
        private readonly TransactionBaseHandler<TRequest, Response> _transactionHandler;

        public TransactionHandler(Func<IRequestHandler<TRequest>> decorateeFactory,
            TransactionBaseHandler<TRequest, Response> transactionHandler)
        {
            _decorateeFactory = decorateeFactory;
            _transactionHandler = transactionHandler;
        }

        public Task<Response> HandleAsync(TRequest request)
        {
            return _transactionHandler.HandleAsync(request, () => _decorateeFactory().HandleAsync(request));
        }
    }

    public class TransactionHandler<TRequest, TResult> : IRequestHandler<TRequest, TResult> where TRequest : IRequest<TResult>
    {
        private readonly Func<IRequestHandler<TRequest, TResult>> _decorateeFactory;
        private readonly TransactionBaseHandler<TRequest, Response<TResult>> _transactionHandler;

        public TransactionHandler(Func<IRequestHandler<TRequest, TResult>> decorateeFactory,
            TransactionBaseHandler<TRequest, Response<TResult>> transactionHandler)
        {
            _decorateeFactory = decorateeFactory;
            _transactionHandler = transactionHandler;
        }

        public Task<Response<TResult>> HandleAsync(TRequest request)
        {
            return _transactionHandler.HandleAsync(request, () => _decorateeFactory().HandleAsync(request));
        }
    }
}
