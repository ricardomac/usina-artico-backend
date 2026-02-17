using Microsoft.Extensions.Logging;
using UsinaArtico.Application.Abstractions.Data;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Application.Abstractions.Behaviors;

internal static class TransactionalDecorator
{
    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        IApplicationDbContext dbContext,
        ILogger<CommandHandler<TCommand, TResponse>> logger)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken)
        {
            string commandName = typeof(TCommand).Name;
            logger.LogInformation("Starting transaction for command {CommandName}", commandName);

            using var transaction = await dbContext.BeginTransactionAsync(cancellationToken);

            try
            {
                Result<TResponse> result = await innerHandler.Handle(command, cancellationToken);

                if (result.IsSuccess)
                {
                    logger.LogInformation("Committing transaction for command {CommandName}", commandName);
                    await transaction.CommitAsync(cancellationToken);
                }
                else
                {
                    logger.LogWarning("Rolling back transaction for command {CommandName} due to failure result", commandName);
                    await transaction.RollbackAsync(cancellationToken);
                }

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error handling command {CommandName} in transaction. Rolling back.", commandName);
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }

    internal sealed class CommandBaseHandler<TCommand>(
        ICommandHandler<TCommand> innerHandler,
        IApplicationDbContext dbContext,
        ILogger<CommandBaseHandler<TCommand>> logger)
        : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public async Task<Result> Handle(TCommand command, CancellationToken cancellationToken)
        {
            string commandName = typeof(TCommand).Name;
            logger.LogInformation("Starting transaction for command {CommandName}", commandName);

            using var transaction = await dbContext.BeginTransactionAsync(cancellationToken);

            try
            {
                Result result = await innerHandler.Handle(command, cancellationToken);

                if (result.IsSuccess)
                {
                    logger.LogInformation("Committing transaction for command {CommandName}", commandName);
                    await transaction.CommitAsync(cancellationToken);
                }
                else
                {
                    logger.LogWarning("Rolling back transaction for command {CommandName} due to failure result", commandName);
                    await transaction.RollbackAsync(cancellationToken);
                }

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error handling command {CommandName} in transaction. Rolling back.", commandName);
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
