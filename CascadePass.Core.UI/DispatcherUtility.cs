using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace CascadePass.Core.UI
{
    public static class DispatcherUtility
    {
        private static Dispatcher _uiDispatcher;
        private static SynchronizationContext _syncContext;

        public static void Initialize()
        {
            _uiDispatcher = Application.Current?.Dispatcher;
            _syncContext = SynchronizationContext.Current;
        }

        public static void RunOnUIThread(Action action, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) return;

            if (_uiDispatcher == null)
                throw new InvalidOperationException("DispatcherUtility is not initialized.");

            if (_uiDispatcher.CheckAccess())
            {
                if (!cancellationToken.IsCancellationRequested) action();
            }
            else
            {
                _uiDispatcher.Invoke(() =>
                {
                    if (!cancellationToken.IsCancellationRequested) action();
                });
            }
        }

        public static async Task RunOnUIThreadAsync(Func<Task> asyncAction, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) return;

            if (_uiDispatcher == null)
                throw new InvalidOperationException("DispatcherUtility is not initialized.");

            await _uiDispatcher.InvokeAsync(async () =>
            {
                if (!cancellationToken.IsCancellationRequested)
                    await asyncAction();
            });
        }

        public static void RunOnCapturedContext(Action action, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) return;

            if (_syncContext == null)
                throw new InvalidOperationException("DispatcherUtility is not initialized.");

            _syncContext.Post(_ =>
            {
                if (!cancellationToken.IsCancellationRequested) action();
            }, null);
        }

        public static Task RunOnCapturedContextAsync(Func<Task> asyncAction, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) return Task.CompletedTask;

            if (_syncContext == null)
                throw new InvalidOperationException("DispatcherUtility is not initialized.");

            var tcs = new TaskCompletionSource<object>();

            _syncContext.Post(async _ =>
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    tcs.SetCanceled();
                    return;
                }

                try
                {
                    await asyncAction();
                    tcs.SetResult(null);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }, null);

            return tcs.Task;
        }
    }
}
