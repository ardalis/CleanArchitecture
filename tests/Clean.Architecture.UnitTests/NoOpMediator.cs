namespace Clean.Architecture.UnitTests;

public class NoOpMediator : IMediator
{
  public async Task<IAsyncEnumerable<TResponse>> CreateStream<TResponse>(IStreamQuery<TResponse> query, CancellationToken cancellationToken = default)
  {
    await Task.Delay(1);
    return AsyncEnumerable.Empty<TResponse>();
  }

  public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
  {
    return AsyncEnumerable.Empty<TResponse>();
  }

  public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamCommand<TResponse> command, CancellationToken cancellationToken = default)
  {
    return AsyncEnumerable.Empty<TResponse>();
  }

  public IAsyncEnumerable<object?> CreateStream(object message, CancellationToken cancellationToken = default)
  {
    return AsyncEnumerable.Empty<object?>();
  }

  public ValueTask Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
  {
    return ValueTask.CompletedTask;
  }

  public ValueTask Publish(object notification, CancellationToken cancellationToken = default)
  {
    return ValueTask.CompletedTask;
  }

  public ValueTask<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
  {
    return ValueTask.FromResult(default(TResponse)!);
  }

  public ValueTask<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
  {
    return ValueTask.FromResult(default(TResponse)!);
  }

  public ValueTask<TResponse> Send<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
  {
    return ValueTask.FromResult(default(TResponse)!);
  }

  public ValueTask<object?> Send(object message, CancellationToken cancellationToken = default)
  {
    return ValueTask.FromResult<object?>(null);
  }

  IAsyncEnumerable<TResponse> ISender.CreateStream<TResponse>(IStreamQuery<TResponse> query, CancellationToken cancellationToken)
  {
    return AsyncEnumerable.Empty<TResponse>();
  }
}
