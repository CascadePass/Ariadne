﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace CascadePass.Core.UI.Tests
{
    [TestClass]
    public class AsyncDelegateCommandTests
    {
        [TestMethod]
        public async Task ExecuteAsync_WithParameter_CallsExecuteAsyncDelegate()
        {
            bool executed = false;
            var command = new AsyncDelegateCommand<int>(
                async (param, token, progress) =>
                {
                    await Task.Delay(10);
                    executed = true;
                });

            await command.ExecuteAsync(null, new Progress<int>());

            Assert.IsTrue(executed);
        }

        [TestMethod]
        public async Task ExecuteAsync_ReportsProgress()
        {
            var tcs = new TaskCompletionSource<int>();
            var command = new AsyncDelegateCommand<int>(
                async (param, token, progress) =>
                {
                    progress.Report(42);
                    await Task.CompletedTask;
                });

            var progress = new Progress<int>(val => tcs.SetResult(val));

            await command.ExecuteAsync(null, progress);

            var reportedProgress = await tcs.Task; // this guarantees value receipt
            Assert.AreEqual(42, reportedProgress);
        }

        [TestMethod]
        public void Cancel_ShouldCancelToken()
        {
            var command = new AsyncDelegateCommand<int>(
                async (param, token, progress) =>
                {
                    while (!token.IsCancellationRequested)
                    {
                        await Task.Delay(10);
                    }

                    token.ThrowIfCancellationRequested();
                });

            var task = command.ExecuteAsync(null, new Progress<int>());
            command.Cancel();

            Assert.ThrowsExceptionAsync<TaskCanceledException>(async () => await task);
        }

        [TestMethod]
        public void CanExecute_ReturnsExpectedValue()
        {
            bool canExecute = false;
            var command = new AsyncDelegateCommand<int>(
                async (p, t, prog) => await Task.CompletedTask,
                _ => canExecute);

            Assert.IsFalse(command.CanExecute(null));
            canExecute = true;
            Assert.IsTrue(command.CanExecute(null));
        }

    }
}
