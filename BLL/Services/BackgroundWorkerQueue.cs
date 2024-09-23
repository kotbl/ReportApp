using Domain.Dto;
using System.Collections.Concurrent;

namespace BLL.Services;

public class BackgroundWorkerQueue
{
    private ConcurrentQueue<QueueItem> queueItems = new ConcurrentQueue<QueueItem>();
    private SemaphoreSlim _signal = new SemaphoreSlim(0);

    public async Task<QueueItem> DequeueAsync(CancellationToken token)
    {
        await _signal.WaitAsync(token);
        queueItems.TryDequeue(out var queueItem);

        if (queueItem is null)
            throw new ArgumentNullException(nameof(queueItem));

        return queueItem;
    }

    public void QueueBackgroundWorkItem(QueueItem queueItem)
    {
        if (queueItem is null)
            throw new ArgumentNullException(nameof(queueItem));

        queueItems.Enqueue(queueItem);
        _signal.Release();
    }
}
