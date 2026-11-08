#define TRACE
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client.Http;
using Microsoft.AspNet.SignalR.Client.Infrastructure;
using Microsoft.AspNet.SignalR.Client.Transports;
using Microsoft.AspNet.SignalR.Client.Transports.ServerSentEvents;
using Microsoft.AspNet.SignalR.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: ComVisible(false)]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: CompilationRelaxations(8)]
[assembly: AssemblyConfiguration("")]
[assembly: TargetFramework(".NETFramework,Version=v4.0", FrameworkDisplayName = ".NET Framework 4")]
[assembly: AssemblyTitle("Microsoft.AspNet.SignalR.Client (4434168dd1416b4b446f42f40caed46df0c3fac6)")]
[assembly: AssemblyTrademark("")]
[assembly: CLSCompliant(false)]
[assembly: AssemblyFileVersion("1.2.30819.0")]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: AssemblyDescription(".NET client for SignalR")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyInformationalVersion("1.2.2")]
[assembly: AssemblyCopyright("© Microsoft Corporation All rights reserved.")]
[assembly: AssemblyProduct("Microsoft ASP.NET SignalR")]
[assembly: AssemblyMetadata("Serviceable", "True")]
[assembly: AssemblyVersion("1.2.2.0")]
namespace System.Reflection
{
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
	internal sealed class AssemblyMetadataAttribute : Attribute
	{
		public string Key { get; set; }

		public string Value { get; set; }

		public AssemblyMetadataAttribute(string key, string value)
		{
			Key = key;
			Value = value;
		}
	}
}
namespace Microsoft.AspNet.SignalR.Infrastructure
{
	internal static class CancellationTokenExtensions
	{
		private delegate CancellationTokenRegistration RegisterDelegate(ref CancellationToken token, Action<object> callback, object state);

		private class DiposeCancellationState
		{
			private readonly CancellationCallbackWrapper _callbackWrapper;

			private readonly CancellationTokenRegistration _registration;

			public DiposeCancellationState(CancellationCallbackWrapper callbackWrapper, CancellationTokenRegistration registration)
			{
				_callbackWrapper = callbackWrapper;
				_registration = registration;
			}

			public void TryDispose()
			{
				if (_callbackWrapper.TrySetInvoked())
				{
					try
					{
						CancellationTokenRegistration registration = _registration;
						registration.Dispose();
					}
					catch (ObjectDisposedException)
					{
					}
				}
			}
		}

		private class CancellationCallbackWrapper
		{
			private readonly Action<object> _callback;

			private readonly object _state;

			private int _callbackInvoked;

			public CancellationCallbackWrapper(Action<object> callback, object state)
			{
				_callback = callback;
				_state = state;
			}

			public bool TrySetInvoked()
			{
				return Interlocked.Exchange(ref _callbackInvoked, 1) == 0;
			}

			public void TryInvoke()
			{
				if (TrySetInvoked())
				{
					_callback(_state);
				}
			}
		}

		private static readonly RegisterDelegate _tokenRegister = ResolveRegisterDelegate();

		public static IDisposable SafeRegister(this CancellationToken cancellationToken, Action<object> callback, object state)
		{
			CancellationCallbackWrapper cancellationCallbackWrapper = new CancellationCallbackWrapper(callback, state);
			CancellationTokenRegistration registration = _tokenRegister(ref cancellationToken, delegate(object s)
			{
				InvokeCallback(s);
			}, cancellationCallbackWrapper);
			DiposeCancellationState state2 = new DiposeCancellationState(cancellationCallbackWrapper, registration);
			return new DisposableAction(delegate(object s)
			{
				Dispose(s);
			}, state2);
		}

		private static void InvokeCallback(object state)
		{
			((CancellationCallbackWrapper)state).TryInvoke();
		}

		private static void Dispose(object state)
		{
			((DiposeCancellationState)state).TryDispose();
		}

		private static RegisterDelegate ResolveRegisterDelegate()
		{
			RegisterDelegate result = delegate(ref CancellationToken token, Action<object> callback, object state)
			{
				return token.Register(callback, state);
			};
			MethodInfo methodInfo = null;
			try
			{
				methodInfo = typeof(CancellationToken).GetMethod("InternalRegisterWithoutEC", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[2]
				{
					typeof(Action<object>),
					typeof(object)
				}, null);
			}
			catch
			{
			}
			if (methodInfo == null)
			{
				return result;
			}
			try
			{
				return (RegisterDelegate)Delegate.CreateDelegate(typeof(RegisterDelegate), null, methodInfo);
			}
			catch
			{
				return result;
			}
		}
	}
	internal class DisposableAction : IDisposable
	{
		public static readonly DisposableAction Empty = new DisposableAction(delegate
		{
		});

		private Action<object> _action;

		private readonly object _state;

		public DisposableAction(Action action)
			: this(delegate(object state)
			{
				((Action)state)();
			}, action)
		{
		}

		public DisposableAction(Action<object> action, object state)
		{
			_action = action;
			_state = state;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				Interlocked.Exchange(ref _action, delegate
				{
				})(_state);
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
		}
	}
	internal class Disposer : IDisposable
	{
		private static readonly object _disposedSentinel = new object();

		private object _disposable;

		public void Set(IDisposable disposable)
		{
			if (disposable == null)
			{
				throw new ArgumentNullException("disposable");
			}
			object obj = Interlocked.CompareExchange(ref _disposable, disposable, null);
			if (obj != null && obj == _disposedSentinel)
			{
				disposable.Dispose();
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing && Interlocked.Exchange(ref _disposable, _disposedSentinel) is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
		}
	}
	internal static class ExceptionsExtensions
	{
		internal static Exception Unwrap(this Exception ex)
		{
			if (ex == null)
			{
				return null;
			}
			Exception ex2 = ex.GetBaseException();
			while (ex2.InnerException != null)
			{
				ex2 = ex2.InnerException;
			}
			return ex2;
		}
	}
	internal sealed class TaskQueue
	{
		private readonly object _lockObj = new object();

		private Task _lastQueuedTask;

		private volatile bool _drained;

		private readonly int? _maxSize;

		private long _size;

		public bool IsDrained => _drained;

		public TaskQueue()
			: this(TaskAsyncHelper.Empty)
		{
		}

		public TaskQueue(Task initialTask)
		{
			_lastQueuedTask = initialTask;
		}

		public TaskQueue(Task initialTask, int maxSize)
		{
			_lastQueuedTask = initialTask;
			_maxSize = maxSize;
		}

		public Task Enqueue(Func<object, Task> taskFunc, object state)
		{
			lock (_lockObj)
			{
				if (_drained)
				{
					return _lastQueuedTask;
				}
				int? maxSize = _maxSize;
				if (maxSize.HasValue && Interlocked.Increment(ref _size) > _maxSize)
				{
					Interlocked.Decrement(ref _size);
					return null;
				}
				return _lastQueuedTask = _lastQueuedTask.Then((Func<object, Task> next, object nextState) => next(nextState).Finally(delegate(object s)
				{
					TaskQueue taskQueue = (TaskQueue)s;
					int? maxSize2 = taskQueue._maxSize;
					if (maxSize2.HasValue)
					{
						Interlocked.Decrement(ref taskQueue._size);
					}
				}, this), taskFunc, state);
			}
		}

		public Task Enqueue(Func<Task> taskFunc)
		{
			return Enqueue((object state) => ((Func<Task>)state)(), taskFunc);
		}

		public Task Drain()
		{
			lock (_lockObj)
			{
				_drained = true;
				return _lastQueuedTask;
			}
		}
	}
}
namespace Microsoft.AspNet.SignalR
{
	internal static class TaskAsyncHelper
	{
		private static class TaskRunners<T, TResult>
		{
			internal static Task RunTask(Task<T> task, Action<T> successor)
			{
				TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
				task.ContinueWith(delegate(Task<T> t)
				{
					if (t.IsFaulted)
					{
						tcs.SetUnwrappedException(t.Exception);
					}
					else
					{
						if (!t.IsCanceled)
						{
							try
							{
								successor(t.Result);
								tcs.SetResult(null);
								return;
							}
							catch (Exception e)
							{
								tcs.SetUnwrappedException(e);
								return;
							}
						}
						tcs.SetCanceled();
					}
				});
				return tcs.Task;
			}

			internal static Task<TResult> RunTask(Task task, Func<TResult> successor)
			{
				TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>();
				task.ContinueWith(delegate(Task t)
				{
					if (t.IsFaulted)
					{
						tcs.SetUnwrappedException(t.Exception);
					}
					else
					{
						if (!t.IsCanceled)
						{
							try
							{
								tcs.SetResult(successor());
								return;
							}
							catch (Exception e)
							{
								tcs.SetUnwrappedException(e);
								return;
							}
						}
						tcs.SetCanceled();
					}
				});
				return tcs.Task;
			}

			internal static Task<TResult> RunTask(Task<T> task, Func<Task<T>, TResult> successor)
			{
				TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>();
				task.ContinueWith(delegate(Task<T> t)
				{
					if (task.IsFaulted)
					{
						tcs.SetUnwrappedException(t.Exception);
					}
					else
					{
						if (!task.IsCanceled)
						{
							try
							{
								tcs.SetResult(successor(t));
								return;
							}
							catch (Exception e)
							{
								tcs.SetUnwrappedException(e);
								return;
							}
						}
						tcs.SetCanceled();
					}
				});
				return tcs.Task;
			}
		}

		private static class GenericDelegates<T, TResult, T1, T2>
		{
			internal static Task ThenWithArgs(Task task, Action<T1> successor, T1 arg1)
			{
				return RunTask(task, delegate
				{
					successor(arg1);
				});
			}

			internal static Task ThenWithArgs(Task task, Action<T1, T2> successor, T1 arg1, T2 arg2)
			{
				return RunTask(task, delegate
				{
					successor(arg1, arg2);
				});
			}

			internal static Task<TResult> ThenWithArgs(Task task, Func<T1, TResult> successor, T1 arg1)
			{
				return TaskRunners<object, TResult>.RunTask(task, () => successor(arg1));
			}

			internal static Task<TResult> ThenWithArgs(Task task, Func<T1, T2, TResult> successor, T1 arg1, T2 arg2)
			{
				return TaskRunners<object, TResult>.RunTask(task, () => successor(arg1, arg2));
			}

			internal static Task<TResult> ThenWithArgs(Task<T> task, Func<T, T1, TResult> successor, T1 arg1)
			{
				return TaskRunners<T, TResult>.RunTask(task, (Task<T> t) => successor(t.Result, arg1));
			}

			internal static Task<Task> ThenWithArgs(Task task, Func<T1, Task> successor, T1 arg1)
			{
				return TaskRunners<object, Task>.RunTask(task, () => successor(arg1));
			}

			internal static Task<Task> ThenWithArgs(Task task, Func<T1, T2, Task> successor, T1 arg1, T2 arg2)
			{
				return TaskRunners<object, Task>.RunTask(task, () => successor(arg1, arg2));
			}

			internal static Task<Task<TResult>> ThenWithArgs(Task<T> task, Func<T, T1, Task<TResult>> successor, T1 arg1)
			{
				return TaskRunners<T, Task<TResult>>.RunTask(task, (Task<T> t) => successor(t.Result, arg1));
			}

			internal static Task<Task<T>> ThenWithArgs(Task<T> task, Func<Task<T>, T1, Task<T>> successor, T1 arg1)
			{
				return TaskRunners<T, Task<T>>.RunTask(task, (Task<T> t) => successor(t, arg1));
			}
		}

		private static class TaskCache<T>
		{
			public static Task<T> Empty = MakeTask(default(T));
		}

		private static readonly Task _emptyTask = MakeTask<object>(null);

		private static readonly Task<bool> _trueTask = MakeTask(value: true);

		private static readonly Task<bool> _falseTask = MakeTask(value: false);

		public static Task Empty => _emptyTask;

		public static Task<bool> True => _trueTask;

		public static Task<bool> False => _falseTask;

		private static Task<T> MakeTask<T>(T value)
		{
			return FromResult(value);
		}

		public static Task OrEmpty(this Task task)
		{
			return task ?? Empty;
		}

		public static Task<T> OrEmpty<T>(this Task<T> task)
		{
			return task ?? TaskCache<T>.Empty;
		}

		public static Task FromAsync(Func<AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, object state)
		{
			try
			{
				return Task.Factory.FromAsync(beginMethod, endMethod, state);
			}
			catch (Exception e)
			{
				return FromError(e);
			}
		}

		public static Task<T> FromAsync<T>(Func<AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, T> endMethod, object state)
		{
			try
			{
				return Task.Factory.FromAsync(beginMethod, endMethod, state);
			}
			catch (Exception e)
			{
				return FromError<T>(e);
			}
		}

		public static Task Series(Func<object, Task>[] tasks, object[] state)
		{
			Task empty = Empty;
			Task task = Empty;
			for (int i = 0; i < tasks.Length; i++)
			{
				empty = task;
				task = empty.Then(tasks[i], state[i]);
			}
			return task;
		}

		public static TTask Catch<TTask>(this TTask task) where TTask : Task
		{
			return task.Catch(delegate
			{
			});
		}

		public static TTask Catch<TTask>(this TTask task, Action<AggregateException, object> handler, object state) where TTask : Task
		{
			if (task != null && task.Status != TaskStatus.RanToCompletion)
			{
				if (task.Status == TaskStatus.Faulted)
				{
					ExecuteOnFaulted(handler, state, task.Exception);
				}
				else
				{
					AttachFaultedContinuation(task, handler, state);
				}
			}
			return task;
		}

		private static void AttachFaultedContinuation<TTask>(TTask task, Action<AggregateException, object> handler, object state) where TTask : Task
		{
			task.ContinueWith(delegate(Task innerTask)
			{
				ExecuteOnFaulted(handler, state, innerTask.Exception);
			}, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
		}

		private static void ExecuteOnFaulted(Action<AggregateException, object> handler, object state, AggregateException exception)
		{
			Trace.TraceWarning("SignalR exception thrown by Task: {0}", exception);
			handler(exception, state);
		}

		public static TTask Catch<TTask>(this TTask task, Action<AggregateException> handler) where TTask : Task
		{
			return task.Catch(delegate(AggregateException ex, object state)
			{
				((Action<AggregateException>)state)(ex);
			}, handler);
		}

		public static Task ContinueWithNotComplete(this Task task, Action action)
		{
			switch (task.Status)
			{
			case TaskStatus.Canceled:
			case TaskStatus.Faulted:
				try
				{
					action();
					return task;
				}
				catch (Exception e)
				{
					return FromError(e);
				}
			case TaskStatus.RanToCompletion:
				return task;
			default:
			{
				TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
				task.ContinueWith(delegate(Task t)
				{
					if (t.IsFaulted || t.IsCanceled)
					{
						try
						{
							action();
							if (t.IsFaulted)
							{
								tcs.TrySetUnwrappedException(t.Exception);
							}
							else
							{
								tcs.TrySetCanceled();
							}
							return;
						}
						catch (Exception exception)
						{
							tcs.TrySetException(exception);
							return;
						}
					}
					tcs.TrySetResult(null);
				}, TaskContinuationOptions.ExecuteSynchronously);
				return tcs.Task;
			}
			}
		}

		public static void ContinueWithNotComplete(this Task task, TaskCompletionSource<object> tcs)
		{
			task.ContinueWith(delegate(Task t)
			{
				if (t.IsFaulted)
				{
					tcs.SetUnwrappedException(t.Exception);
				}
				else if (t.IsCanceled)
				{
					tcs.SetCanceled();
				}
			}, TaskContinuationOptions.NotOnRanToCompletion);
		}

		public static void ContinueWith(this Task task, TaskCompletionSource<object> tcs)
		{
			task.ContinueWith(delegate(Task t)
			{
				if (t.IsFaulted)
				{
					tcs.TrySetUnwrappedException(t.Exception);
				}
				else if (t.IsCanceled)
				{
					tcs.TrySetCanceled();
				}
				else
				{
					tcs.TrySetResult(null);
				}
			}, TaskContinuationOptions.ExecuteSynchronously);
		}

		public static void ContinueWith<T>(this Task<T> task, TaskCompletionSource<T> tcs)
		{
			task.ContinueWith(delegate(Task<T> t)
			{
				if (t.IsFaulted)
				{
					tcs.TrySetUnwrappedException(t.Exception);
				}
				else if (t.IsCanceled)
				{
					tcs.TrySetCanceled();
				}
				else
				{
					tcs.TrySetResult(t.Result);
				}
			});
		}

		public static Task Return(this Task[] tasks)
		{
			return tasks.Then(delegate
			{
			});
		}

		public static Task Then(this Task task, Action successor)
		{
			switch (task.Status)
			{
			case TaskStatus.Canceled:
			case TaskStatus.Faulted:
				return task;
			case TaskStatus.RanToCompletion:
				return FromMethod(successor);
			default:
				return RunTask(task, successor);
			}
		}

		public static Task<TResult> Then<TResult>(this Task task, Func<TResult> successor)
		{
			return task.Status switch
			{
				TaskStatus.Faulted => FromError<TResult>(task.Exception), 
				TaskStatus.Canceled => Canceled<TResult>(), 
				TaskStatus.RanToCompletion => FromMethod(successor), 
				_ => TaskRunners<object, TResult>.RunTask(task, successor), 
			};
		}

		public static Task Then(this Task[] tasks, Action successor)
		{
			if (tasks.Length == 0)
			{
				return FromMethod(successor);
			}
			TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
			Task.Factory.ContinueWhenAll(tasks, delegate(Task[] completedTasks)
			{
				Task task = completedTasks.FirstOrDefault((Task t) => t.IsFaulted);
				if (task != null)
				{
					tcs.SetUnwrappedException(task.Exception);
				}
				else
				{
					Task task2 = completedTasks.FirstOrDefault((Task t) => t.IsCanceled);
					if (task2 != null)
					{
						tcs.SetCanceled();
					}
					else
					{
						successor();
						tcs.SetResult(null);
					}
				}
			});
			return tcs.Task;
		}

		public static Task Then<T1>(this Task task, Action<T1> successor, T1 arg1)
		{
			switch (task.Status)
			{
			case TaskStatus.Canceled:
			case TaskStatus.Faulted:
				return task;
			case TaskStatus.RanToCompletion:
				return FromMethod(successor, arg1);
			default:
				return GenericDelegates<object, object, T1, object>.ThenWithArgs(task, successor, arg1);
			}
		}

		public static Task Then<T1, T2>(this Task task, Action<T1, T2> successor, T1 arg1, T2 arg2)
		{
			switch (task.Status)
			{
			case TaskStatus.Canceled:
			case TaskStatus.Faulted:
				return task;
			case TaskStatus.RanToCompletion:
				return FromMethod(successor, arg1, arg2);
			default:
				return GenericDelegates<object, object, T1, T2>.ThenWithArgs(task, successor, arg1, arg2);
			}
		}

		public static Task Then<T1>(this Task task, Func<T1, Task> successor, T1 arg1)
		{
			switch (task.Status)
			{
			case TaskStatus.Canceled:
			case TaskStatus.Faulted:
				return task;
			case TaskStatus.RanToCompletion:
				return FromMethod(successor, arg1);
			default:
				return GenericDelegates<object, Task, T1, object>.ThenWithArgs(task, successor, arg1).FastUnwrap();
			}
		}

		public static Task Then<T1, T2>(this Task task, Func<T1, T2, Task> successor, T1 arg1, T2 arg2)
		{
			switch (task.Status)
			{
			case TaskStatus.Canceled:
			case TaskStatus.Faulted:
				return task;
			case TaskStatus.RanToCompletion:
				return FromMethod(successor, arg1, arg2);
			default:
				return GenericDelegates<object, Task, T1, T2>.ThenWithArgs(task, successor, arg1, arg2).FastUnwrap();
			}
		}

		public static Task<TResult> Then<T, TResult>(this Task<T> task, Func<T, Task<TResult>> successor)
		{
			return task.Status switch
			{
				TaskStatus.Faulted => FromError<TResult>(task.Exception), 
				TaskStatus.Canceled => Canceled<TResult>(), 
				TaskStatus.RanToCompletion => FromMethod(successor, task.Result), 
				_ => TaskRunners<T, Task<TResult>>.RunTask(task, (Task<T> t) => successor(t.Result)).FastUnwrap(), 
			};
		}

		public static Task<TResult> Then<T, TResult>(this Task<T> task, Func<T, TResult> successor)
		{
			return task.Status switch
			{
				TaskStatus.Faulted => FromError<TResult>(task.Exception), 
				TaskStatus.Canceled => Canceled<TResult>(), 
				TaskStatus.RanToCompletion => FromMethod(successor, task.Result), 
				_ => TaskRunners<T, TResult>.RunTask(task, (Task<T> t) => successor(t.Result)), 
			};
		}

		public static Task<TResult> Then<T, T1, TResult>(this Task<T> task, Func<T, T1, TResult> successor, T1 arg1)
		{
			return task.Status switch
			{
				TaskStatus.Faulted => FromError<TResult>(task.Exception), 
				TaskStatus.Canceled => Canceled<TResult>(), 
				TaskStatus.RanToCompletion => FromMethod(successor, task.Result, arg1), 
				_ => GenericDelegates<T, TResult, T1, object>.ThenWithArgs(task, successor, arg1), 
			};
		}

		public static Task Then(this Task task, Func<Task> successor)
		{
			switch (task.Status)
			{
			case TaskStatus.Canceled:
			case TaskStatus.Faulted:
				return task;
			case TaskStatus.RanToCompletion:
				return FromMethod(successor);
			default:
				return TaskRunners<object, Task>.RunTask(task, successor).FastUnwrap();
			}
		}

		public static Task<TResult> Then<TResult>(this Task task, Func<Task<TResult>> successor)
		{
			return task.Status switch
			{
				TaskStatus.Faulted => FromError<TResult>(task.Exception), 
				TaskStatus.Canceled => Canceled<TResult>(), 
				TaskStatus.RanToCompletion => FromMethod(successor), 
				_ => TaskRunners<object, Task<TResult>>.RunTask(task, successor).FastUnwrap(), 
			};
		}

		public static Task Then<TResult>(this Task<TResult> task, Action<TResult> successor)
		{
			switch (task.Status)
			{
			case TaskStatus.Canceled:
			case TaskStatus.Faulted:
				return task;
			case TaskStatus.RanToCompletion:
				return FromMethod(successor, task.Result);
			default:
				return TaskRunners<TResult, object>.RunTask(task, successor);
			}
		}

		public static Task Then<TResult>(this Task<TResult> task, Func<TResult, Task> successor)
		{
			switch (task.Status)
			{
			case TaskStatus.Canceled:
			case TaskStatus.Faulted:
				return task;
			case TaskStatus.RanToCompletion:
				return FromMethod(successor, task.Result);
			default:
				return TaskRunners<TResult, Task>.RunTask(task, (Task<TResult> t) => successor(t.Result)).FastUnwrap();
			}
		}

		public static Task<TResult> Then<TResult, T1>(this Task<TResult> task, Func<Task<TResult>, T1, Task<TResult>> successor, T1 arg1)
		{
			switch (task.Status)
			{
			case TaskStatus.Canceled:
			case TaskStatus.Faulted:
				return task;
			case TaskStatus.RanToCompletion:
				return FromMethod(successor, task, arg1);
			default:
				return GenericDelegates<TResult, Task<TResult>, T1, object>.ThenWithArgs(task, successor, arg1).FastUnwrap();
			}
		}

		public static Task Finally(this Task task, Action<object> next, object state)
		{
			try
			{
				switch (task.Status)
				{
				case TaskStatus.Canceled:
				case TaskStatus.Faulted:
					next(state);
					return task;
				case TaskStatus.RanToCompletion:
					return FromMethod(next, state);
				default:
					return RunTaskSynchronously(task, next, state, onlyOnSuccess: false);
				}
			}
			catch (Exception e)
			{
				return FromError(e);
			}
		}

		public static Task RunSynchronously(this Task task, Action successor)
		{
			switch (task.Status)
			{
			case TaskStatus.Canceled:
			case TaskStatus.Faulted:
				return task;
			case TaskStatus.RanToCompletion:
				return FromMethod(successor);
			default:
				return RunTaskSynchronously(task, delegate(object state)
				{
					((Action)state)();
				}, successor);
			}
		}

		public static Task FastUnwrap(this Task<Task> task)
		{
			Task task2 = ((task.Status == TaskStatus.RanToCompletion) ? task.Result : null);
			return task2 ?? task.Unwrap();
		}

		public static Task<T> FastUnwrap<T>(this Task<Task<T>> task)
		{
			Task<T> task2 = ((task.Status == TaskStatus.RanToCompletion) ? task.Result : null);
			return task2 ?? task.Unwrap();
		}

		public static Task Delay(TimeSpan timeOut)
		{
			TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
			Timer timer = new Timer(taskCompletionSource.SetResult, null, timeOut, TimeSpan.FromMilliseconds(-1.0));
			return taskCompletionSource.Task.ContinueWith(delegate
			{
				timer.Dispose();
			}, TaskContinuationOptions.ExecuteSynchronously);
		}

		public static Task FromMethod(Action func)
		{
			try
			{
				func();
				return Empty;
			}
			catch (Exception e)
			{
				return FromError(e);
			}
		}

		public static Task FromMethod<T1>(Action<T1> func, T1 arg)
		{
			try
			{
				func(arg);
				return Empty;
			}
			catch (Exception e)
			{
				return FromError(e);
			}
		}

		public static Task FromMethod<T1, T2>(Action<T1, T2> func, T1 arg1, T2 arg2)
		{
			try
			{
				func(arg1, arg2);
				return Empty;
			}
			catch (Exception e)
			{
				return FromError(e);
			}
		}

		public static Task FromMethod(Func<Task> func)
		{
			try
			{
				return func();
			}
			catch (Exception e)
			{
				return FromError(e);
			}
		}

		public static Task<TResult> FromMethod<TResult>(Func<Task<TResult>> func)
		{
			try
			{
				return func();
			}
			catch (Exception e)
			{
				return FromError<TResult>(e);
			}
		}

		public static Task<TResult> FromMethod<TResult>(Func<TResult> func)
		{
			try
			{
				return FromResult(func());
			}
			catch (Exception e)
			{
				return FromError<TResult>(e);
			}
		}

		public static Task FromMethod<T1>(Func<T1, Task> func, T1 arg)
		{
			try
			{
				return func(arg);
			}
			catch (Exception e)
			{
				return FromError(e);
			}
		}

		public static Task FromMethod<T1, T2>(Func<T1, T2, Task> func, T1 arg1, T2 arg2)
		{
			try
			{
				return func(arg1, arg2);
			}
			catch (Exception e)
			{
				return FromError(e);
			}
		}

		public static Task<TResult> FromMethod<T1, TResult>(Func<T1, Task<TResult>> func, T1 arg)
		{
			try
			{
				return func(arg);
			}
			catch (Exception e)
			{
				return FromError<TResult>(e);
			}
		}

		public static Task<TResult> FromMethod<T1, TResult>(Func<T1, TResult> func, T1 arg)
		{
			try
			{
				return FromResult(func(arg));
			}
			catch (Exception e)
			{
				return FromError<TResult>(e);
			}
		}

		public static Task<TResult> FromMethod<T1, T2, TResult>(Func<T1, T2, Task<TResult>> func, T1 arg1, T2 arg2)
		{
			try
			{
				return func(arg1, arg2);
			}
			catch (Exception e)
			{
				return FromError<TResult>(e);
			}
		}

		public static Task<TResult> FromMethod<T1, T2, TResult>(Func<T1, T2, TResult> func, T1 arg1, T2 arg2)
		{
			try
			{
				return FromResult(func(arg1, arg2));
			}
			catch (Exception e)
			{
				return FromError<TResult>(e);
			}
		}

		public static Task<T> FromResult<T>(T value)
		{
			TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();
			taskCompletionSource.SetResult(value);
			return taskCompletionSource.Task;
		}

		internal static Task FromError(Exception e)
		{
			return FromError<object>(e);
		}

		internal static Task<T> FromError<T>(Exception e)
		{
			TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();
			taskCompletionSource.SetUnwrappedException(e);
			return taskCompletionSource.Task;
		}

		internal static void SetUnwrappedException<T>(this TaskCompletionSource<T> tcs, Exception e)
		{
			if (e is AggregateException ex)
			{
				tcs.SetException(ex.InnerExceptions);
			}
			else
			{
				tcs.SetException(e);
			}
		}

		internal static bool TrySetUnwrappedException<T>(this TaskCompletionSource<T> tcs, Exception e)
		{
			if (e is AggregateException ex)
			{
				return tcs.TrySetException(ex.InnerExceptions);
			}
			return tcs.TrySetException(e);
		}

		private static Task Canceled()
		{
			TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
			taskCompletionSource.SetCanceled();
			return taskCompletionSource.Task;
		}

		private static Task<T> Canceled<T>()
		{
			TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();
			taskCompletionSource.SetCanceled();
			return taskCompletionSource.Task;
		}

		private static Task RunTask(Task task, Action successor)
		{
			TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
			task.ContinueWith(delegate(Task t)
			{
				if (t.IsFaulted)
				{
					tcs.SetUnwrappedException(t.Exception);
				}
				else
				{
					if (!t.IsCanceled)
					{
						try
						{
							successor();
							tcs.SetResult(null);
							return;
						}
						catch (Exception e)
						{
							tcs.SetUnwrappedException(e);
							return;
						}
					}
					tcs.SetCanceled();
				}
			});
			return tcs.Task;
		}

		private static Task RunTaskSynchronously(Task task, Action<object> next, object state, bool onlyOnSuccess = true)
		{
			TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
			task.ContinueWith(delegate(Task t)
			{
				try
				{
					if (t.IsFaulted)
					{
						if (!onlyOnSuccess)
						{
							next(state);
						}
						tcs.SetUnwrappedException(t.Exception);
					}
					else if (t.IsCanceled)
					{
						if (!onlyOnSuccess)
						{
							next(state);
						}
						tcs.SetCanceled();
					}
					else
					{
						next(state);
						tcs.SetResult(null);
					}
				}
				catch (Exception e)
				{
					tcs.SetUnwrappedException(e);
				}
			}, TaskContinuationOptions.ExecuteSynchronously);
			return tcs.Task;
		}
	}
}
namespace Microsoft.AspNet.SignalR.Client
{
	public interface IConnection
	{
		TimeSpan ReconnectWindow { get; set; }

		KeepAliveData KeepAliveData { get; set; }

		string MessageId { get; set; }

		string GroupsToken { get; set; }

		IDictionary<string, object> Items { get; }

		string ConnectionId { get; }

		string ConnectionToken { get; }

		string Url { get; }

		string QueryString { get; }

		ConnectionState State { get; }

		IClientTransport Transport { get; }

		DateTime LastMessageAt { get; }

		DateTime LastActiveAt { get; }

		IDictionary<string, string> Headers { get; }

		ICredentials Credentials { get; set; }

		CookieContainer CookieContainer { get; set; }

		JsonSerializer JsonSerializer { get; }

		bool ChangeState(ConnectionState oldState, ConnectionState newState);

		void Stop();

		void Disconnect();

		Task Send(string data);

		void OnReceived(JToken data);

		void OnError(Exception ex);

		void OnReconnecting();

		void OnReconnected();

		void OnConnectionSlow();

		void PrepareRequest(IRequest request);

		void MarkLastMessage();

		void MarkActive();

		void Trace(TraceLevels level, string format, params object[] args);
	}
	public class Connection : IConnection
	{
		private class DebugTextWriter : TextWriter
		{
			public override Encoding Encoding => Encoding.UTF8;

			public DebugTextWriter()
				: base(CultureInfo.InvariantCulture)
			{
			}

			public override void WriteLine(string value)
			{
			}
		}

		internal static readonly TimeSpan DefaultAbortTimeout = TimeSpan.FromSeconds(30.0);

		private static Version _assemblyVersion;

		private IClientTransport _transport;

		private CancellationTokenSource _disconnectCts;

		private TimeSpan _disconnectTimeout;

		private IDisposable _disconnectTimeoutOperation;

		private ConnectionState _state;

		private KeepAliveData _keepAliveData;

		private TimeSpan _reconnectWindow;

		private Task _connectTask;

		private TextWriter _traceWriter;

		private string _connectionData;

		private TaskQueue _receiveQueue;

		private Task _lastQueuedReceiveTask;

		private TaskCompletionSource<object> _startTcs;

		private readonly object _stateLock = new object();

		private readonly object _startLock = new object();

		private readonly object _traceLock = new object();

		private DateTime _lastMessageAt;

		private DateTime _lastActiveAt;

		private HeartbeatMonitor _monitor;

		private JsonSerializer _jsonSerializer = new JsonSerializer();

		TimeSpan IConnection.ReconnectWindow
		{
			get
			{
				return _reconnectWindow;
			}
			set
			{
				_reconnectWindow = value;
			}
		}

		KeepAliveData IConnection.KeepAliveData
		{
			get
			{
				return _keepAliveData;
			}
			set
			{
				_keepAliveData = value;
			}
		}

		DateTime IConnection.LastMessageAt => _lastMessageAt;

		DateTime IConnection.LastActiveAt => _lastActiveAt;

		public TraceLevels TraceLevel { get; set; }

		public TextWriter TraceWriter
		{
			get
			{
				return _traceWriter;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				_traceWriter = value;
			}
		}

		public JsonSerializer JsonSerializer
		{
			get
			{
				return _jsonSerializer;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				_jsonSerializer = value;
			}
		}

		public CookieContainer CookieContainer { get; set; }

		public ICredentials Credentials { get; set; }

		public IDictionary<string, string> Headers { get; private set; }

		public IWebProxy Proxy { get; set; }

		public string Url { get; private set; }

		public string MessageId { get; set; }

		public string ConnectionId { get; set; }

		public string ConnectionToken { get; set; }

		public string GroupsToken { get; set; }

		public IDictionary<string, object> Items { get; private set; }

		public string QueryString { get; private set; }

		public IClientTransport Transport => _transport;

		public ConnectionState State
		{
			get
			{
				return _state;
			}
			private set
			{
				lock (_stateLock)
				{
					if (_state != value)
					{
						StateChange obj = new StateChange(_state, value);
						_state = value;
						if (this.StateChanged != null)
						{
							this.StateChanged(obj);
						}
					}
				}
			}
		}

		public event Action<string> Received;

		public event Action<Exception> Error;

		public event Action Closed;

		public event Action Reconnecting;

		public event Action Reconnected;

		public event Action<StateChange> StateChanged;

		public event Action ConnectionSlow;

		public Connection(string url)
			: this(url, (string)null)
		{
		}

		public Connection(string url, IDictionary<string, string> queryString)
			: this(url, CreateQueryString(queryString))
		{
		}

		public Connection(string url, string queryString)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			if (url.Contains("?"))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Error_UrlCantContainQueryStringDirectly), "url");
			}
			if (!url.EndsWith("/", StringComparison.Ordinal))
			{
				url += "/";
			}
			Url = url;
			QueryString = queryString;
			_disconnectTimeoutOperation = DisposableAction.Empty;
			_lastMessageAt = DateTime.UtcNow;
			_lastActiveAt = DateTime.UtcNow;
			_reconnectWindow = TimeSpan.Zero;
			Items = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
			State = ConnectionState.Disconnected;
			TraceLevel = TraceLevels.All;
			TraceWriter = new DebugTextWriter();
			Headers = new HeaderDictionary(this);
		}

		public Task Start()
		{
			return Start(new DefaultHttpClient());
		}

		public Task Start(IHttpClient httpClient)
		{
			return Start(new AutoTransport(httpClient));
		}

		public Task Start(IClientTransport transport)
		{
			lock (_startLock)
			{
				if (!ChangeState(ConnectionState.Disconnected, ConnectionState.Connecting))
				{
					return _connectTask ?? TaskAsyncHelper.Empty;
				}
				_disconnectCts = new CancellationTokenSource();
				_startTcs = new TaskCompletionSource<object>();
				_receiveQueue = new TaskQueue(_startTcs.Task);
				_lastQueuedReceiveTask = TaskAsyncHelper.Empty;
				_transport = transport;
				_connectTask = Negotiate(transport);
			}
			return _connectTask;
		}

		protected virtual string OnSending()
		{
			return null;
		}

		private Task Negotiate(IClientTransport transport)
		{
			_connectionData = OnSending();
			return transport.Negotiate(this, _connectionData).Then(delegate(NegotiationResponse negotiationResponse)
			{
				VerifyProtocolVersion(negotiationResponse.ProtocolVersion);
				ConnectionId = negotiationResponse.ConnectionId;
				ConnectionToken = negotiationResponse.ConnectionToken;
				_disconnectTimeout = TimeSpan.FromSeconds(negotiationResponse.DisconnectTimeout);
				TimeSpan beatInterval = TimeSpan.FromSeconds(5.0);
				if (negotiationResponse.KeepAliveTimeout.HasValue)
				{
					_keepAliveData = new KeepAliveData(TimeSpan.FromSeconds(negotiationResponse.KeepAliveTimeout.Value));
					_reconnectWindow = _disconnectTimeout + _keepAliveData.Timeout;
					beatInterval = _keepAliveData.CheckInterval;
				}
				else
				{
					_reconnectWindow = _disconnectTimeout;
				}
				_monitor = new HeartbeatMonitor(this, _stateLock, beatInterval);
				string data = OnSending();
				return StartTransport(data);
			}).ContinueWithNotComplete(delegate
			{
				Disconnect();
			});
		}

		private Task StartTransport(string data)
		{
			return _transport.Start(this, data, _disconnectCts.Token).RunSynchronously(delegate
			{
				ChangeState(ConnectionState.Connecting, ConnectionState.Connected);
				_startTcs.SetResult(null);
				_monitor.Start();
			}).Then(() => _lastQueuedReceiveTask);
		}

		private bool ChangeState(ConnectionState oldState, ConnectionState newState)
		{
			return ((IConnection)this).ChangeState(oldState, newState);
		}

		bool IConnection.ChangeState(ConnectionState oldState, ConnectionState newState)
		{
			lock (_stateLock)
			{
				if (_state == oldState)
				{
					Trace(TraceLevels.StateChanges, "ChangeState({0}, {1})", oldState, newState);
					State = newState;
					return true;
				}
			}
			return false;
		}

		private static void VerifyProtocolVersion(string versionString)
		{
			if (string.IsNullOrEmpty(versionString) || !TryParseVersion(versionString, out var version) || version.Major != 1 || version.Minor != 2)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.Error_IncompatibleProtocolVersion, new object[2]
				{
					"1.2",
					versionString ?? "null"
				}));
			}
		}

		public void Stop()
		{
			Stop(DefaultAbortTimeout);
		}

		public void Stop(TimeSpan timeout)
		{
			lock (_startLock)
			{
				if (_connectTask != null)
				{
					try
					{
						_connectTask.Wait(timeout);
					}
					catch (Exception ex)
					{
						Trace(TraceLevels.Events, "Error: {0}", ex.GetBaseException());
					}
				}
				if (_receiveQueue != null)
				{
					_receiveQueue.Drain().Catch();
				}
				lock (_stateLock)
				{
					if (State != ConnectionState.Disconnected)
					{
						string connectionId = ConnectionId;
						Trace(TraceLevels.Events, "Stop");
						_monitor.Dispose();
						_transport.Abort(this, timeout, _connectionData);
						Disconnect();
						_disconnectCts.Dispose();
						if (_transport != null)
						{
							Trace(TraceLevels.Events, "Transport.Dispose({0})", connectionId);
							_transport.Dispose();
							_transport = null;
						}
					}
				}
			}
		}

		void IConnection.Disconnect()
		{
			Disconnect();
		}

		private void Disconnect()
		{
			lock (_stateLock)
			{
				if (State != ConnectionState.Disconnected)
				{
					State = ConnectionState.Disconnected;
					Trace(TraceLevels.StateChanges, "Disconnect");
					_disconnectTimeoutOperation.Dispose();
					_disconnectCts.Cancel();
					if (_monitor != null)
					{
						_monitor.Dispose();
					}
					Trace(TraceLevels.Events, "Closed");
					ConnectionId = null;
					ConnectionToken = null;
					GroupsToken = null;
					MessageId = null;
					_connectionData = null;
					OnClosed();
				}
			}
		}

		protected virtual void OnClosed()
		{
			if (this.Closed != null)
			{
				this.Closed();
			}
		}

		public virtual Task Send(string data)
		{
			if (State == ConnectionState.Disconnected)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.Error_StartMustBeCalledBeforeDataCanBeSent));
			}
			if (State == ConnectionState.Connecting)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.Error_ConnectionHasNotBeenEstablished));
			}
			return _transport.Send(this, data, _connectionData);
		}

		public Task Send(object value)
		{
			return Send(this.JsonSerializeObject(value));
		}

		public void Trace(TraceLevels level, string format, params object[] args)
		{
			lock (_traceLock)
			{
				if (TraceLevel.HasFlag(level))
				{
					_traceWriter.WriteLine(DateTime.UtcNow.ToString("HH:mm:ss.fffffff", CultureInfo.InvariantCulture) + " - " + (ConnectionId ?? "null") + " - " + format, args);
				}
			}
		}

		void IConnection.OnReceived(JToken message)
		{
			_lastQueuedReceiveTask = _receiveQueue.Enqueue(() => Task.Factory.StartNew(delegate
			{
				try
				{
					OnMessageReceived(message);
				}
				catch (Exception error)
				{
					OnError(error);
				}
			}));
		}

		protected virtual void OnMessageReceived(JToken message)
		{
			if (this.Received != null)
			{
				this.Received(message.ToString());
			}
		}

		private void OnError(Exception error)
		{
			Trace(TraceLevels.Events, "OnError({0})", error);
			if (this.Error != null)
			{
				this.Error(error);
			}
		}

		void IConnection.OnError(Exception error)
		{
			OnError(error);
		}

		public virtual void OnReconnecting()
		{
			_disconnectTimeoutOperation = SetTimeout(_disconnectTimeout, Disconnect);
			if (this.Reconnecting != null)
			{
				this.Reconnecting();
			}
		}

		void IConnection.OnReconnected()
		{
			_disconnectTimeoutOperation.Dispose();
			if (this.Reconnected != null)
			{
				this.Reconnected();
			}
			((IConnection)this).MarkLastMessage();
		}

		void IConnection.OnConnectionSlow()
		{
			Trace(TraceLevels.Events, "OnConnectionSlow");
			if (this.ConnectionSlow != null)
			{
				this.ConnectionSlow();
			}
		}

		void IConnection.MarkLastMessage()
		{
			_lastMessageAt = DateTime.UtcNow;
		}

		void IConnection.MarkActive()
		{
			if (TransportHelper.VerifyLastActive(this))
			{
				_lastActiveAt = DateTime.UtcNow;
			}
		}

		void IConnection.PrepareRequest(IRequest request)
		{
			request.UserAgent = CreateUserAgentString("SignalR.Client");
			if (Credentials != null)
			{
				request.Credentials = Credentials;
			}
			if (CookieContainer != null)
			{
				request.CookieContainer = CookieContainer;
			}
			if (Proxy != null)
			{
				request.Proxy = Proxy;
			}
			request.SetRequestHeaders(Headers);
		}

		private static string CreateUserAgentString(string client)
		{
			if (_assemblyVersion == null)
			{
				_assemblyVersion = new AssemblyName(typeof(Connection).Assembly.FullName).Version;
			}
			return string.Format(CultureInfo.InvariantCulture, "{0}/{1} ({2})", new object[3]
			{
				client,
				_assemblyVersion,
				Environment.OSVersion
			});
		}

		private static bool TryParseVersion(string versionString, out Version version)
		{
			return Version.TryParse(versionString, out version);
		}

		private static string CreateQueryString(IDictionary<string, string> queryString)
		{
			return string.Join("&", queryString.Select((KeyValuePair<string, string> kvp) => kvp.Key + "=" + kvp.Value).ToArray());
		}

		private static IDisposable SetTimeout(TimeSpan delay, Action operation)
		{
			ThreadSafeInvoker cancellableInvoker = new ThreadSafeInvoker();
			TaskAsyncHelper.Delay(delay).Then(() => cancellableInvoker.Invoke(operation));
			return new DisposableAction(delegate
			{
				cancellableInvoker.Invoke();
			});
		}
	}
	public static class ConnectionExtensions
	{
		public static T GetValue<T>(this IConnection connection, string key)
		{
			if (connection == null)
			{
				throw new ArgumentNullException("connection");
			}
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}
			lock (connection.Items)
			{
				if (connection.Items.TryGetValue(key, out var value))
				{
					return (T)value;
				}
			}
			return default(T);
		}

		public static string JsonSerializeObject(this IConnection connection, object value)
		{
			if (connection == null)
			{
				throw new ArgumentNullException("connection");
			}
			StringBuilder sb = new StringBuilder(256);
			using StringWriter stringWriter = new StringWriter(sb, CultureInfo.InvariantCulture);
			JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter);
			jsonTextWriter.CloseOutput = false;
			using (JsonTextWriter jsonTextWriter2 = jsonTextWriter)
			{
				jsonTextWriter2.Formatting = connection.JsonSerializer.Formatting;
				connection.JsonSerializer.Serialize(jsonTextWriter2, value);
			}
			return stringWriter.ToString();
		}

		public static T JsonDeserializeObject<T>(this IConnection connection, string jsonValue)
		{
			if (connection == null)
			{
				throw new ArgumentNullException("connection");
			}
			using StringReader reader = new StringReader(jsonValue);
			JsonTextReader jsonTextReader = new JsonTextReader(reader);
			jsonTextReader.CloseInput = false;
			using JsonTextReader reader2 = jsonTextReader;
			return (T)connection.JsonSerializer.Deserialize(reader2, typeof(T));
		}

		public static bool EnsureReconnecting(this IConnection connection)
		{
			if (connection == null)
			{
				throw new ArgumentNullException("connection");
			}
			if (connection.ChangeState(ConnectionState.Connected, ConnectionState.Reconnecting))
			{
				connection.OnReconnecting();
			}
			return connection.State == ConnectionState.Reconnecting;
		}

		public static IObservable<string> AsObservable(this Connection connection)
		{
			return connection.AsObservable((string value) => value);
		}

		public static IObservable<T> AsObservable<T>(this Connection connection)
		{
			return connection.AsObservable((string value) => connection.JsonDeserializeObject<T>(value));
		}

		public static IObservable<T> AsObservable<T>(this Connection connection, Func<string, T> selector)
		{
			return new ObservableConnection<T>(connection, selector);
		}
	}
	public class HeaderDictionary : IDictionary<string, string>, ICollection<KeyValuePair<string, string>>, IEnumerable<KeyValuePair<string, string>>, IEnumerable
	{
		private IDictionary<string, string> _dictionary = new Dictionary<string, string>();

		private readonly IConnection _connection;

		public ICollection<string> Keys => _dictionary.Keys;

		public ICollection<string> Values => _dictionary.Values;

		public string this[string key]
		{
			get
			{
				return _dictionary[key];
			}
			set
			{
				_dictionary[key] = value;
			}
		}

		public int Count => _dictionary.Count;

		public bool IsReadOnly
		{
			get
			{
				if (_connection.State == ConnectionState.Disconnected)
				{
					return _dictionary.IsReadOnly;
				}
				return true;
			}
		}

		public HeaderDictionary(IConnection connection)
		{
			_connection = connection;
		}

		public void Add(string key, string value)
		{
			_dictionary.Add(key, value);
		}

		public bool ContainsKey(string key)
		{
			return _dictionary.ContainsKey(key);
		}

		public bool Remove(string key)
		{
			return _dictionary.Remove(key);
		}

		public bool TryGetValue(string key, out string value)
		{
			return _dictionary.TryGetValue(key, out value);
		}

		public void Add(KeyValuePair<string, string> item)
		{
			_dictionary.Add(item);
		}

		public void Clear()
		{
			_dictionary.Clear();
		}

		public bool Contains(KeyValuePair<string, string> item)
		{
			return _dictionary.Contains(item);
		}

		public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
		{
			_dictionary.CopyTo(array, arrayIndex);
		}

		public bool Remove(KeyValuePair<string, string> item)
		{
			return _dictionary.Remove(item);
		}

		public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
		{
			return _dictionary.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _dictionary.GetEnumerator();
		}
	}
	public class HeartbeatMonitor : IDisposable
	{
		private Timer _timer;

		private readonly object _connectionStateLock;

		private readonly IConnection _connection;

		private TimeSpan _beatInterval;

		private bool _monitorKeepAlive;

		public bool HasBeenWarned { get; private set; }

		public bool TimedOut { get; private set; }

		public HeartbeatMonitor(IConnection connection, object connectionStateLock, TimeSpan beatInterval)
		{
			_connection = connection;
			_connectionStateLock = connectionStateLock;
			_beatInterval = beatInterval;
		}

		public void Start()
		{
			_connection.MarkLastMessage();
			_connection.MarkActive();
			_monitorKeepAlive = _connection.KeepAliveData != null && _connection.Transport.SupportsKeepAlive;
			HasBeenWarned = false;
			TimedOut = false;
			_timer = new Timer(delegate
			{
				Beat();
			}, null, _beatInterval, _beatInterval);
		}

		private void Beat()
		{
			TimeSpan timeElapsed = DateTime.UtcNow - _connection.LastMessageAt;
			Beat(timeElapsed);
		}

		public void Beat(TimeSpan timeElapsed)
		{
			if (_monitorKeepAlive)
			{
				CheckKeepAlive(timeElapsed);
			}
			_connection.MarkActive();
		}

		private void CheckKeepAlive(TimeSpan timeElapsed)
		{
			lock (_connectionStateLock)
			{
				if (_connection.State != ConnectionState.Connected)
				{
					return;
				}
				if (timeElapsed >= _connection.KeepAliveData.Timeout)
				{
					if (!TimedOut)
					{
						_connection.Trace(TraceLevels.Events, "Connection Timed-out : Transport Lost Connection");
						TimedOut = true;
						_connection.Transport.LostConnection(_connection);
					}
				}
				else if (timeElapsed >= _connection.KeepAliveData.TimeoutWarning)
				{
					if (!HasBeenWarned)
					{
						_connection.Trace(TraceLevels.Events, "Connection Timeout Warning : Notifying user");
						HasBeenWarned = true;
						_connection.OnConnectionSlow();
					}
				}
				else
				{
					HasBeenWarned = false;
					TimedOut = false;
				}
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing && _timer != null)
			{
				_timer.Dispose();
				_timer = null;
			}
		}
	}
}
namespace Microsoft.AspNet.SignalR.Client.Http
{
	public static class IResponseExtensions
	{
		public static Task<string> ReadAsString(this IResponse response)
		{
			if (response == null)
			{
				throw new ArgumentNullException("response");
			}
			Stream stream = response.GetStream();
			AsyncStreamReader asyncStreamReader = new AsyncStreamReader(stream);
			StringBuilder result = new StringBuilder();
			TaskCompletionSource<string> resultTcs = new TaskCompletionSource<string>();
			asyncStreamReader.Data = delegate(ArraySegment<byte> buffer)
			{
				result.Append(Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count));
			};
			asyncStreamReader.Closed = delegate
			{
				response.Dispose();
				resultTcs.SetResult(result.ToString());
			};
			asyncStreamReader.Start();
			return resultTcs.Task;
		}
	}
}
namespace Microsoft.AspNet.SignalR.Client.Hubs
{
	public class HubInvocation
	{
		[JsonProperty("I")]
		public string CallbackId { get; set; }

		[JsonProperty("H")]
		public string Hub { get; set; }

		[JsonProperty("M")]
		public string Method { get; set; }

		[JsonProperty("A")]
		public JToken[] Args { get; set; }

		[JsonProperty("S", NullValueHandling = NullValueHandling.Ignore)]
		public Dictionary<string, JToken> State { get; set; }
	}
}
namespace Microsoft.AspNet.SignalR.Client
{
	public enum ConnectionState
	{
		Connecting,
		Connected,
		Reconnecting,
		Disconnected
	}
}
namespace Microsoft.AspNet.SignalR.Client.Hubs
{
	public interface IHubConnection : IConnection
	{
		string RegisterCallback(Action<HubResult> callback);

		void RemoveCallback(string callbackId);
	}
	public class HubConnection : Connection, IHubConnection, IConnection
	{
		private readonly Dictionary<string, HubProxy> _hubs = new Dictionary<string, HubProxy>(StringComparer.OrdinalIgnoreCase);

		internal readonly Dictionary<string, Action<HubResult>> _callbacks = new Dictionary<string, Action<HubResult>>();

		private int _callbackId;

		public HubConnection(string url)
			: this(url, useDefaultUrl: true)
		{
		}

		public HubConnection(string url, bool useDefaultUrl)
			: base(GetUrl(url, useDefaultUrl))
		{
		}

		public HubConnection(string url, string queryString)
			: this(url, queryString, useDefaultUrl: true)
		{
		}

		public HubConnection(string url, string queryString, bool useDefaultUrl)
			: base(GetUrl(url, useDefaultUrl), queryString)
		{
		}

		public HubConnection(string url, IDictionary<string, string> queryString)
			: this(url, queryString, useDefaultUrl: true)
		{
		}

		public HubConnection(string url, IDictionary<string, string> queryString, bool useDefaultUrl)
			: base(GetUrl(url, useDefaultUrl), queryString)
		{
		}

		public override void OnReconnecting()
		{
			ClearInvocationCallbacks(Resources.Message_Reconnecting);
			base.OnReconnecting();
		}

		protected override void OnMessageReceived(JToken message)
		{
			if (message["I"] != null)
			{
				HubResult hubResult = message.ToObject<HubResult>(base.JsonSerializer);
				Action<HubResult> value;
				lock (_callbacks)
				{
					if (_callbacks.TryGetValue(hubResult.Id, out value))
					{
						_callbacks.Remove(hubResult.Id);
					}
					else
					{
						Trace(TraceLevels.Messages, "Callback with id " + hubResult.Id + " not found!");
					}
				}
				value?.Invoke(hubResult);
				return;
			}
			HubInvocation hubInvocation = message.ToObject<HubInvocation>(base.JsonSerializer);
			if (_hubs.TryGetValue(hubInvocation.Hub, out var value2))
			{
				if (hubInvocation.State != null)
				{
					foreach (KeyValuePair<string, JToken> item in hubInvocation.State)
					{
						value2[item.Key] = item.Value;
					}
				}
				value2.InvokeEvent(hubInvocation.Method, hubInvocation.Args);
			}
			base.OnMessageReceived(message);
		}

		protected override string OnSending()
		{
			IEnumerable<HubRegistrationData> value = _hubs.Select((KeyValuePair<string, HubProxy> p) => new HubRegistrationData
			{
				Name = p.Key
			});
			return this.JsonSerializeObject(value);
		}

		protected override void OnClosed()
		{
			ClearInvocationCallbacks(Resources.Message_ConnectionClosed);
			base.OnClosed();
		}

		public IHubProxy CreateHubProxy(string hubName)
		{
			if (base.State != ConnectionState.Disconnected)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.Error_ProxiesCannotBeAddedConnectionStarted));
			}
			if (!_hubs.TryGetValue(hubName, out var value))
			{
				value = new HubProxy(this, hubName);
				_hubs[hubName] = value;
			}
			return value;
		}

		string IHubConnection.RegisterCallback(Action<HubResult> callback)
		{
			lock (_callbacks)
			{
				string text = _callbackId.ToString(CultureInfo.InvariantCulture);
				_callbacks[text] = callback;
				_callbackId++;
				return text;
			}
		}

		void IHubConnection.RemoveCallback(string callbackId)
		{
			lock (_callbacks)
			{
				_callbacks.Remove(callbackId);
			}
		}

		private static string GetUrl(string url, bool useDefaultUrl)
		{
			if (!url.EndsWith("/", StringComparison.Ordinal))
			{
				url += "/";
			}
			if (useDefaultUrl)
			{
				return url + "signalr";
			}
			return url;
		}

		private void ClearInvocationCallbacks(string error)
		{
			HubResult hubResult = new HubResult();
			hubResult.Error = error;
			lock (_callbacks)
			{
				foreach (Action<HubResult> value in _callbacks.Values)
				{
					value(hubResult);
				}
				_callbacks.Clear();
			}
		}
	}
	public interface IHubProxy
	{
		JToken this[string name] { get; set; }

		JsonSerializer JsonSerializer { get; }

		Task Invoke(string method, params object[] args);

		Task<T> Invoke<T>(string method, params object[] args);

		Subscription Subscribe(string eventName);
	}
	public class HubProxy : IHubProxy
	{
		private readonly string _hubName;

		private readonly IHubConnection _connection;

		private readonly Dictionary<string, JToken> _state = new Dictionary<string, JToken>(StringComparer.OrdinalIgnoreCase);

		private readonly Dictionary<string, Subscription> _subscriptions = new Dictionary<string, Subscription>(StringComparer.OrdinalIgnoreCase);

		public JToken this[string name]
		{
			get
			{
				lock (_state)
				{
					_state.TryGetValue(name, out var value);
					return value;
				}
			}
			set
			{
				lock (_state)
				{
					_state[name] = value;
				}
			}
		}

		public JsonSerializer JsonSerializer => _connection.JsonSerializer;

		public HubProxy(IHubConnection connection, string hubName)
		{
			_connection = connection;
			_hubName = hubName;
		}

		public Subscription Subscribe(string eventName)
		{
			if (eventName == null)
			{
				throw new ArgumentNullException("eventName");
			}
			if (!_subscriptions.TryGetValue(eventName, out var value))
			{
				value = new Subscription();
				_subscriptions.Add(eventName, value);
			}
			return value;
		}

		public Task Invoke(string method, params object[] args)
		{
			return Invoke<object>(method, args);
		}

		public Task<T> Invoke<T>(string method, params object[] args)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			if (args == null)
			{
				throw new ArgumentNullException("args");
			}
			JToken[] array = new JToken[args.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = JToken.FromObject(args[i], JsonSerializer);
			}
			TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
			string callbackId = _connection.RegisterCallback(delegate(HubResult result)
			{
				if (result != null)
				{
					if (result.Error == null)
					{
						try
						{
							if (result.State != null)
							{
								foreach (KeyValuePair<string, JToken> item in result.State)
								{
									this[item.Key] = item.Value;
								}
							}
							if (result.Result != null)
							{
								tcs.TrySetResult(result.Result.ToObject<T>(JsonSerializer));
							}
							else
							{
								tcs.TrySetResult(default(T));
							}
							return;
						}
						catch (Exception e)
						{
							tcs.TrySetUnwrappedException(e);
							return;
						}
					}
					tcs.TrySetUnwrappedException(new InvalidOperationException(result.Error));
				}
				else
				{
					tcs.TrySetCanceled();
				}
			});
			HubInvocation hubInvocation = new HubInvocation();
			hubInvocation.Hub = _hubName;
			hubInvocation.Method = method;
			hubInvocation.Args = array;
			hubInvocation.CallbackId = callbackId;
			HubInvocation hubInvocation2 = hubInvocation;
			if (_state.Count != 0)
			{
				hubInvocation2.State = _state;
			}
			string data = _connection.JsonSerializeObject(hubInvocation2);
			_connection.Send(data).ContinueWith(delegate(Task task)
			{
				if (task.IsCanceled)
				{
					_connection.RemoveCallback(callbackId);
					tcs.TrySetCanceled();
				}
				else if (task.IsFaulted)
				{
					_connection.RemoveCallback(callbackId);
					tcs.TrySetUnwrappedException(task.Exception);
				}
			}, TaskContinuationOptions.NotOnRanToCompletion);
			return tcs.Task;
		}

		public void InvokeEvent(string eventName, IList<JToken> args)
		{
			if (_subscriptions.TryGetValue(eventName, out var value))
			{
				value.OnReceived(args);
			}
		}
	}
	public class HubRegistrationData
	{
		public string Name { get; set; }
	}
	public class HubResult
	{
		[JsonProperty("I")]
		public string Id { get; set; }

		[JsonProperty("R")]
		public JToken Result { get; set; }

		[JsonProperty("E")]
		public string Error { get; set; }

		[JsonProperty("S")]
		public IDictionary<string, JToken> State { get; set; }
	}
	public class Hubservable : IObservable<IList<JToken>>
	{
		private readonly string _eventName;

		private readonly IHubProxy _proxy;

		public Hubservable(IHubProxy proxy, string eventName)
		{
			_proxy = proxy;
			_eventName = eventName;
		}

		public IDisposable Subscribe(IObserver<IList<JToken>> observer)
		{
			Subscription subscription = _proxy.Subscribe(_eventName);
			subscription.Received += observer.OnNext;
			return new DisposableAction(delegate
			{
				subscription.Received -= observer.OnNext;
			});
		}
	}
	public class Subscription
	{
		public event Action<IList<JToken>> Received;

		internal void OnReceived(IList<JToken> data)
		{
			if (this.Received != null)
			{
				this.Received(data);
			}
		}
	}
}
namespace Microsoft.AspNet.SignalR.Client
{
	public static class ErrorExtensions
	{
		public static SignalRError GetError(this Exception ex)
		{
			ex = ex.Unwrap();
			WebException ex2 = ex as WebException;
			SignalRError signalRError = new SignalRError(ex);
			if (ex2 != null && ex2.Response != null && ex2.Response is HttpWebResponse httpWebResponse)
			{
				signalRError.SetResponse(httpWebResponse);
				signalRError.StatusCode = httpWebResponse.StatusCode;
				Stream responseStream = httpWebResponse.GetResponseStream();
				if (responseStream.CanRead)
				{
					Stream stream = Clone(responseStream);
					using StreamReader streamReader = new StreamReader(stream);
					signalRError.ResponseBody = streamReader.ReadToEnd();
				}
			}
			return signalRError;
		}

		private static Stream Clone(Stream source)
		{
			MemoryStream memoryStream = new MemoryStream();
			source.CopyTo(memoryStream);
			if (source.CanSeek)
			{
				source.Seek(0L, SeekOrigin.Begin);
			}
			memoryStream.Seek(0L, SeekOrigin.Begin);
			return memoryStream;
		}
	}
}
namespace Microsoft.AspNet.SignalR.Client.Infrastructure
{
	public sealed class TransportAbortHandler : IDisposable
	{
		private const string _abortQueryString = "?transport={0}&connectionData={1}&connectionToken={2}{3}";

		private readonly string _transportName;

		private readonly IHttpClient _httpClient;

		private ManualResetEvent _abortResetEvent = new ManualResetEvent(initialState: false);

		private bool _startedAbort;

		private readonly object _abortLock = new object();

		private bool _disposed;

		private readonly object _disposeLock = new object();

		public TransportAbortHandler(IHttpClient httpClient, string transportName)
		{
			_httpClient = httpClient;
			_transportName = transportName;
		}

		public void Abort(IConnection connection, TimeSpan timeout, string connectionData)
		{
			if (connection == null)
			{
				throw new ArgumentNullException("connection");
			}
			string connectionToken = connection.ConnectionToken;
			if (connectionToken == null)
			{
				connection.Trace(TraceLevels.Messages, "Connection already disconnected, skipping abort.");
				return;
			}
			lock (_abortLock)
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().Name);
				}
				if (!_startedAbort)
				{
					_startedAbort = true;
					string text = connection.Url + "abort" + string.Format(CultureInfo.InvariantCulture, "?transport={0}&connectionData={1}&connectionToken={2}{3}", _transportName, connectionData, Uri.EscapeDataString(connectionToken), null);
					text += TransportHelper.AppendCustomQueryString(connection, text);
					_httpClient.Post(text, connection.PrepareRequest, isLongRunning: false).Catch(delegate(AggregateException ex, object state)
					{
						((TransportAbortHandler)state).CompleteAbort();
					}, this);
					if (!_abortResetEvent.WaitOne(timeout))
					{
						connection.Trace(TraceLevels.Events, "Abort never fired");
					}
				}
			}
		}

		public void CompleteAbort()
		{
			lock (_disposeLock)
			{
				if (!_disposed)
				{
					_startedAbort = true;
					_abortResetEvent.Set();
				}
			}
		}

		public bool TryCompleteAbort()
		{
			lock (_disposeLock)
			{
				if (_disposed)
				{
					return true;
				}
				if (_startedAbort)
				{
					_abortResetEvent.Set();
					return true;
				}
				return false;
			}
		}

		public void Dispose()
		{
			lock (_abortLock)
			{
				lock (_disposeLock)
				{
					if (!_disposed)
					{
						_abortResetEvent.Dispose();
						_disposed = true;
					}
				}
			}
		}
	}
}
namespace Microsoft.AspNet.SignalR.Client
{
	public class KeepAliveData
	{
		private const double _keepAliveWarnAt = 2.0 / 3.0;

		public TimeSpan Timeout { get; private set; }

		public TimeSpan TimeoutWarning { get; private set; }

		public TimeSpan CheckInterval { get; private set; }

		public KeepAliveData(TimeSpan timeout)
		{
			Timeout = timeout;
			TimeoutWarning = TimeSpan.FromTicks((long)((double)Timeout.Ticks * (2.0 / 3.0)));
			CheckInterval = TimeSpan.FromTicks((Timeout.Ticks - TimeoutWarning.Ticks) / 3);
		}

		public KeepAliveData(TimeSpan timeout, TimeSpan timeoutWarning, TimeSpan checkInterval)
		{
			Timeout = timeout;
			TimeoutWarning = timeoutWarning;
			CheckInterval = checkInterval;
		}
	}
	public class SignalRError : IDisposable
	{
		private HttpWebResponse _response;

		public HttpStatusCode StatusCode { get; set; }

		public string ResponseBody { get; set; }

		public Exception Exception { get; private set; }

		public SignalRError(Exception exception)
		{
			Exception = exception;
		}

		internal void SetResponse(HttpWebResponse response)
		{
			_response = response;
		}

		public override string ToString()
		{
			return Exception.ToString();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing && _response != null)
			{
				_response.Close();
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
		}
	}
}
namespace Microsoft.AspNet.SignalR.Client.Infrastructure
{
	internal static class ExceptionHelper
	{
		internal static bool IsRequestAborted(Exception exception)
		{
			exception = exception.Unwrap();
			if (exception is OperationCanceledException)
			{
				return true;
			}
			if (exception is ObjectDisposedException)
			{
				return true;
			}
			if (exception is WebException ex)
			{
				return ex.Status == WebExceptionStatus.RequestCanceled;
			}
			return false;
		}
	}
	internal class ThreadSafeInvoker
	{
		private int _invoked;

		public bool Invoke(Action action)
		{
			if (Interlocked.Exchange(ref _invoked, 1) == 0)
			{
				action();
				return true;
			}
			return false;
		}

		public bool Invoke<T>(Action<T> action, T arg)
		{
			if (Interlocked.Exchange(ref _invoked, 1) == 0)
			{
				action(arg);
				return true;
			}
			return false;
		}

		public bool Invoke<T1, T2>(Action<T1, T2> action, T1 arg1, T2 arg2)
		{
			if (Interlocked.Exchange(ref _invoked, 1) == 0)
			{
				action(arg1, arg2);
				return true;
			}
			return false;
		}

		public bool Invoke()
		{
			return Invoke(delegate
			{
			});
		}
	}
}
namespace Microsoft.AspNet.SignalR.Infrastructure
{
	internal static class UrlEncoder
	{
		public static string UrlEncode(string str)
		{
			if (str == null)
			{
				return null;
			}
			byte[] bytes = Encoding.UTF8.GetBytes(str);
			byte[] array = UrlEncode(bytes, 0, bytes.Length, alwaysCreateNewReturnValue: false);
			return Encoding.UTF8.GetString(array, 0, array.Length);
		}

		private static byte[] UrlEncode(byte[] bytes, int offset, int count, bool alwaysCreateNewReturnValue)
		{
			byte[] array = UrlEncode(bytes, offset, count);
			if (!alwaysCreateNewReturnValue || array == null || array != bytes)
			{
				return array;
			}
			return (byte[])array.Clone();
		}

		private static byte[] UrlEncode(byte[] bytes, int offset, int count)
		{
			if (!ValidateUrlEncodingParameters(bytes, offset, count))
			{
				return null;
			}
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < count; i++)
			{
				char c = (char)bytes[offset + i];
				if (c == ' ')
				{
					num++;
				}
				else if (!IsUrlSafeChar(c))
				{
					num2++;
				}
			}
			if (num == 0 && num2 == 0)
			{
				return bytes;
			}
			byte[] array = new byte[count + num2 * 2];
			int num3 = 0;
			for (int j = 0; j < count; j++)
			{
				byte b = bytes[offset + j];
				char c2 = (char)b;
				if (IsUrlSafeChar(c2))
				{
					array[num3++] = b;
					continue;
				}
				if (c2 == ' ')
				{
					array[num3++] = 43;
					continue;
				}
				array[num3++] = 37;
				array[num3++] = (byte)IntToHex((b >> 4) & 0xF);
				array[num3++] = (byte)IntToHex(b & 0xF);
			}
			return array;
		}

		private static char IntToHex(int n)
		{
			if (n <= 9)
			{
				return (char)(n + 48);
			}
			return (char)(n - 10 + 97);
		}

		private static bool IsUrlSafeChar(char ch)
		{
			if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || (ch >= '0' && ch <= '9'))
			{
				return true;
			}
			switch (ch)
			{
			case '!':
			case '(':
			case ')':
			case '*':
			case '-':
			case '.':
			case '_':
				return true;
			default:
				return false;
			}
		}

		private static bool ValidateUrlEncodingParameters(byte[] bytes, int offset, int count)
		{
			if (bytes == null && count == 0)
			{
				return false;
			}
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (offset < 0 || offset > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0 || offset + count > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			return true;
		}
	}
}
namespace Microsoft.AspNet.SignalR.Client
{
	[Flags]
	public enum TraceLevels
	{
		None = 0,
		Messages = 1,
		Events = 2,
		StateChanges = 4,
		All = 7
	}
	[DebuggerNonUserCode]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[CompilerGenerated]
	internal class Resources
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(resourceMan, null))
				{
					Assembly assembly = typeof(Resources).Assembly;
					ResourceManager resourceManager = new ResourceManager("Microsoft.AspNet.SignalR.Client.Resources", assembly);
					resourceMan = resourceManager;
				}
				return resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return resourceCulture;
			}
			set
			{
				resourceCulture = value;
			}
		}

		internal static string Error_CertsCanOnlyBeAddedWhenDisconnected => ResourceManager.GetString("Error_CertsCanOnlyBeAddedWhenDisconnected", resourceCulture);

		internal static string Error_ConnectionCancelled => ResourceManager.GetString("Error_ConnectionCancelled", resourceCulture);

		internal static string Error_ConnectionHasNotBeenEstablished => ResourceManager.GetString("Error_ConnectionHasNotBeenEstablished", resourceCulture);

		internal static string Error_IncompatibleProtocolVersion => ResourceManager.GetString("Error_IncompatibleProtocolVersion", resourceCulture);

		internal static string Error_ProxiesCannotBeAddedConnectionStarted => ResourceManager.GetString("Error_ProxiesCannotBeAddedConnectionStarted", resourceCulture);

		internal static string Error_ServerNegotiationFailed => ResourceManager.GetString("Error_ServerNegotiationFailed", resourceCulture);

		internal static string Error_StartMustBeCalledBeforeDataCanBeSent => ResourceManager.GetString("Error_StartMustBeCalledBeforeDataCanBeSent", resourceCulture);

		internal static string Error_UrlCantContainQueryStringDirectly => ResourceManager.GetString("Error_UrlCantContainQueryStringDirectly", resourceCulture);

		internal static string Message_ConnectionClosed => ResourceManager.GetString("Message_ConnectionClosed", resourceCulture);

		internal static string Message_Reconnecting => ResourceManager.GetString("Message_Reconnecting", resourceCulture);

		internal Resources()
		{
		}
	}
}
namespace Microsoft.AspNet.SignalR.Client.Transports
{
	public class AsyncStreamReader
	{
		private static class State
		{
			public const int Initial = 0;

			public const int Processing = 1;

			public const int Stopped = 2;
		}

		private readonly object _bufferLock = new object();

		private readonly Stream _stream;

		private byte[] _readBuffer;

		private int _reading;

		private Action _setOpened;

		protected object BufferLock => _bufferLock;

		public Action Opened { get; set; }

		public Action<Exception> Closed { get; set; }

		public Action<ArraySegment<byte>> Data { get; set; }

		private bool Processing => _reading == 1;

		public AsyncStreamReader(Stream stream)
		{
			_stream = stream;
		}

		public void Start()
		{
			if (Interlocked.CompareExchange(ref _reading, 1, 0) == 0)
			{
				_setOpened = delegate
				{
					OnOpened();
				};
				_readBuffer = new byte[4096];
				Process();
			}
		}

		private void Close()
		{
			Close(null);
		}

		private void Process()
		{
			Task<int> task;
			while (true)
			{
				lock (_bufferLock)
				{
					if (!Processing || _readBuffer == null)
					{
						return;
					}
					task = StreamExtensions.ReadAsync(_stream, _readBuffer);
				}
				if (!task.IsCompleted)
				{
					break;
				}
				try
				{
					task.Wait();
					int result = task.Result;
					if (TryProcessRead(result))
					{
						continue;
					}
					return;
				}
				catch (Exception exception)
				{
					Close(exception);
					return;
				}
			}
			ReadAsync(task);
		}

		private void ReadAsync(Task<int> readTask)
		{
			readTask.ContinueWith(delegate(Task<int> task)
			{
				if (task.IsFaulted)
				{
					Close(task.Exception);
				}
				else if (task.IsCanceled)
				{
					Close(new OperationCanceledException());
				}
				else if (TryProcessRead(task.Result))
				{
					Process();
				}
			}, TaskContinuationOptions.ExecuteSynchronously);
		}

		private bool TryProcessRead(int read)
		{
			Interlocked.Exchange(ref _setOpened, delegate
			{
			})();
			if (read > 0)
			{
				OnData(new ArraySegment<byte>(_readBuffer, 0, read));
				return true;
			}
			if (read == 0)
			{
				Close();
			}
			return false;
		}

		private void Close(Exception exception)
		{
			int num = Interlocked.Exchange(ref _reading, 2);
			if (num == 2)
			{
				return;
			}
			if (Closed != null)
			{
				if (exception != null)
				{
					exception = exception.Unwrap();
				}
				Closed(exception);
			}
			lock (_bufferLock)
			{
				_readBuffer = null;
			}
		}

		private void OnOpened()
		{
			if (Opened != null)
			{
				Opened();
			}
		}

		private void OnData(ArraySegment<byte> buffer)
		{
			if (Data != null)
			{
				Data(buffer);
			}
		}
	}
	internal class NegotiateInitializer
	{
		private readonly ThreadSafeInvoker _callbackInvoker;

		private readonly Action _initializeCallback;

		private readonly Action<Exception> _errorCallback;

		private readonly TimeSpan _assumeSuccessAfter;

		public event Action Initialized;

		public NegotiateInitializer(Action initializeCallback, Action<Exception> errorCallback, TimeSpan assumeSuccessAfter)
		{
			_initializeCallback = initializeCallback;
			_errorCallback = errorCallback;
			_assumeSuccessAfter = assumeSuccessAfter;
			_callbackInvoker = new ThreadSafeInvoker();
			Initialized += delegate
			{
			};
		}

		public void Initialize()
		{
			TaskAsyncHelper.Delay(_assumeSuccessAfter).Then(delegate
			{
				_callbackInvoker.Invoke(delegate
				{
					this.Initialized();
					_initializeCallback();
				});
			});
		}

		public void Complete()
		{
			_callbackInvoker.Invoke(delegate
			{
				this.Initialized();
				_initializeCallback();
			});
		}

		public void Complete(Exception exception)
		{
			_callbackInvoker.Invoke(delegate(Action<Exception> cb, Exception ex)
			{
				this.Initialized();
				cb(ex);
			}, _errorCallback, exception);
		}

		public void Abort(CancellationToken disconnectToken)
		{
			_callbackInvoker.Invoke(delegate(Action<Exception> cb, CancellationToken token)
			{
				this.Initialized();
				cb(new OperationCanceledException(Resources.Error_ConnectionCancelled, token));
			}, _errorCallback, disconnectToken);
		}
	}
	public class PollingRequestHandler
	{
		private IHttpClient _httpClient;

		private IRequest _currentRequest;

		private int _running;

		private object _stopLock;

		public Func<string> ResolveUrl { get; set; }

		public Func<Exception, Task> OnAfterPoll { get; set; }

		public event Action<IRequest> PrepareRequest;

		public event Action<string> OnMessage;

		public event Action<Exception> OnError;

		public event Action OnPolling;

		public event Action<IRequest> OnAbort;

		public PollingRequestHandler(IHttpClient httpClient)
		{
			_httpClient = httpClient;
			_running = 0;
			_stopLock = new object();
			ResolveUrl = () => "";
			this.PrepareRequest = delegate
			{
			};
			this.OnMessage = delegate
			{
			};
			this.OnError = delegate
			{
			};
			this.OnPolling = delegate
			{
			};
			OnAfterPoll = (Exception _) => TaskAsyncHelper.Empty;
			this.OnAbort = delegate
			{
			};
		}

		public void Start()
		{
			if (Interlocked.Exchange(ref _running, 1) == 0)
			{
				Poll();
			}
		}

		private void Poll()
		{
			lock (_stopLock)
			{
				if (_running == 0)
				{
					return;
				}
				string url = ResolveUrl();
				_httpClient.Post(url, delegate(IRequest request)
				{
					this.PrepareRequest(request);
					_currentRequest = request;
					this.OnPolling();
				}, isLongRunning: true).ContinueWith(delegate(Task<IResponse> task)
				{
					Task task2 = TaskAsyncHelper.Empty;
					Exception ex = null;
					if (task.IsFaulted)
					{
						ex = task.Exception.Unwrap();
						this.OnError(ex);
					}
					else
					{
						try
						{
							task2 = task.Result.ReadAsString().Then(delegate(string raw)
							{
								this.OnMessage(raw);
							});
						}
						catch (Exception ex2)
						{
							ex = ex2;
							this.OnError(ex);
						}
					}
					task2.Finally(delegate(object state)
					{
						OnAfterPoll((Exception)state).Then(delegate
						{
							Poll();
						});
					}, ex);
				});
			}
		}

		public void Stop()
		{
			lock (_stopLock)
			{
				if (Interlocked.Exchange(ref _running, 0) == 1)
				{
					Abort();
				}
			}
		}

		public void Abort()
		{
			this.OnAbort(_currentRequest);
			if (_currentRequest != null)
			{
				_currentRequest.Abort();
			}
		}
	}
}
namespace Microsoft.AspNet.SignalR.Client.Transports.ServerSentEvents
{
	public class ChunkBuffer
	{
		private int _offset;

		private readonly StringBuilder _buffer;

		private readonly StringBuilder _lineBuilder;

		public bool HasChunks => _offset < _buffer.Length;

		public ChunkBuffer()
		{
			_buffer = new StringBuilder();
			_lineBuilder = new StringBuilder();
		}

		public void Add(byte[] buffer, int length)
		{
			_buffer.Append(Encoding.UTF8.GetString(buffer, 0, length));
		}

		public void Add(ArraySegment<byte> buffer)
		{
			_buffer.Append(Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count));
		}

		public string ReadLine()
		{
			int num = _offset;
			while (num < _buffer.Length)
			{
				if (_buffer[num] == '\n')
				{
					_buffer.Remove(0, _offset + 1);
					string result = _lineBuilder.ToString().Trim();
					_lineBuilder.Clear();
					_offset = 0;
					return result;
				}
				_lineBuilder.Append(_buffer[num]);
				num++;
				_offset++;
			}
			return null;
		}
	}
}
namespace Microsoft.AspNet.SignalR.Client.Http
{
	public interface IHttpClient
	{
		Task<IResponse> Get(string url, Action<IRequest> prepareRequest, bool isLongRunning);

		Task<IResponse> Post(string url, Action<IRequest> prepareRequest, IDictionary<string, string> postData, bool isLongRunning);
	}
	public class DefaultHttpClient : IHttpClient
	{
		private readonly string _shortRunningGroup;

		private readonly string _longRunningGroup;

		public DefaultHttpClient()
		{
			string text = Guid.NewGuid().ToString();
			_shortRunningGroup = "SignalR-short-running-" + text;
			_longRunningGroup = "SignalR-long-running-" + text;
		}

		public Task<IResponse> Get(string url, Action<IRequest> prepareRequest, bool isLongRunning)
		{
			return HttpHelper.GetAsync(url, delegate(HttpWebRequest request)
			{
				request.ConnectionGroupName = (isLongRunning ? _longRunningGroup : _shortRunningGroup);
				HttpWebRequestWrapper obj = new HttpWebRequestWrapper(request);
				prepareRequest(obj);
			}).Then((Func<HttpWebResponse, IResponse>)((HttpWebResponse response) => new HttpWebResponseWrapper(response)));
		}

		public Task<IResponse> Post(string url, Action<IRequest> prepareRequest, IDictionary<string, string> postData, bool isLongRunning)
		{
			return HttpHelper.PostAsync(url, delegate(HttpWebRequest request)
			{
				request.ConnectionGroupName = (isLongRunning ? _longRunningGroup : _shortRunningGroup);
				HttpWebRequestWrapper obj = new HttpWebRequestWrapper(request);
				prepareRequest(obj);
			}, postData).Then((Func<HttpWebResponse, IResponse>)((HttpWebResponse response) => new HttpWebResponseWrapper(response)));
		}
	}
	internal static class HttpHelper
	{
		public static Task<HttpWebResponse> GetHttpResponseAsync(this HttpWebRequest request)
		{
			try
			{
				return Task.Factory.FromAsync(request.BeginGetResponse, (IAsyncResult ar) => (HttpWebResponse)request.EndGetResponse(ar), null);
			}
			catch (Exception e)
			{
				return TaskAsyncHelper.FromError<HttpWebResponse>(e);
			}
		}

		public static Task<Stream> GetHttpRequestStreamAsync(this HttpWebRequest request)
		{
			try
			{
				return Task.Factory.FromAsync((Func<AsyncCallback, object?, IAsyncResult>)request.BeginGetRequestStream, (Func<IAsyncResult, Stream>)request.EndGetRequestStream, (object?)null);
			}
			catch (Exception e)
			{
				return TaskAsyncHelper.FromError<Stream>(e);
			}
		}

		public static Task<HttpWebResponse> GetAsync(string url, Action<HttpWebRequest> requestPreparer)
		{
			HttpWebRequest httpWebRequest = CreateWebRequest(url);
			requestPreparer?.Invoke(httpWebRequest);
			return httpWebRequest.GetHttpResponseAsync();
		}

		public static Task<HttpWebResponse> PostAsync(string url, Action<HttpWebRequest> requestPreparer, IDictionary<string, string> postData)
		{
			return PostInternal(url, requestPreparer, postData);
		}

		private static Task<HttpWebResponse> PostInternal(string url, Action<HttpWebRequest> requestPreparer, IDictionary<string, string> postData)
		{
			HttpWebRequest request = CreateWebRequest(url);
			requestPreparer?.Invoke(request);
			byte[] buffer = ProcessPostData(postData);
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			request.ContentLength = ((buffer != null) ? buffer.LongLength : 0);
			if (buffer == null)
			{
				return request.GetHttpResponseAsync();
			}
			return request.GetHttpRequestStreamAsync().Then((Stream stream) => StreamExtensions.WriteAsync(stream, buffer).Then(delegate
			{
				stream.Dispose();
			})).Then(() => request.GetHttpResponseAsync());
		}

		private static byte[] ProcessPostData(IDictionary<string, string> postData)
		{
			if (postData == null || postData.Count == 0)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, string> postDatum in postData)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append("&");
				}
				if (!string.IsNullOrEmpty(postDatum.Value))
				{
					stringBuilder.AppendFormat("{0}={1}", postDatum.Key, UrlEncoder.UrlEncode(postDatum.Value));
				}
			}
			return Encoding.UTF8.GetBytes(stringBuilder.ToString());
		}

		private static HttpWebRequest CreateWebRequest(string url)
		{
			HttpWebRequest httpWebRequest = null;
			return (HttpWebRequest)WebRequest.Create(url);
		}
	}
	public interface IRequest
	{
		string UserAgent { get; set; }

		ICredentials Credentials { get; set; }

		CookieContainer CookieContainer { get; set; }

		IWebProxy Proxy { get; set; }

		string Accept { get; set; }

		void Abort();

		void SetRequestHeaders(IDictionary<string, string> headers);
	}
	public class HttpWebRequestWrapper : IRequest
	{
		private readonly HttpWebRequest _request;

		private IDictionary<string, Action<HttpWebRequest, string>> _restrictedHeadersSet = new Dictionary<string, Action<HttpWebRequest, string>>
		{
			{
				HttpRequestHeader.Accept.ToString(),
				delegate(HttpWebRequest request, string value)
				{
					request.Accept = value;
				}
			},
			{
				HttpRequestHeader.ContentType.ToString(),
				delegate(HttpWebRequest request, string value)
				{
					request.ContentType = value;
				}
			},
			{
				HttpRequestHeader.ContentLength.ToString(),
				delegate(HttpWebRequest request, string value)
				{
					request.ContentLength = int.Parse(value, CultureInfo.CurrentCulture);
				}
			},
			{
				HttpRequestHeader.UserAgent.ToString(),
				delegate(HttpWebRequest request, string value)
				{
					request.UserAgent = value;
				}
			},
			{
				HttpRequestHeader.Connection.ToString(),
				delegate(HttpWebRequest request, string value)
				{
					request.Connection = value;
				}
			},
			{
				HttpRequestHeader.Date.ToString(),
				delegate(HttpWebRequest request, string value)
				{
					request.Date = DateTime.Parse(value, CultureInfo.CurrentCulture);
				}
			},
			{
				HttpRequestHeader.Expect.ToString(),
				delegate(HttpWebRequest request, string value)
				{
					request.Expect = value;
				}
			},
			{
				HttpRequestHeader.Host.ToString(),
				delegate(HttpWebRequest request, string value)
				{
					request.Host = value;
				}
			},
			{
				HttpRequestHeader.IfModifiedSince.ToString(),
				delegate(HttpWebRequest request, string value)
				{
					request.IfModifiedSince = DateTime.Parse(value, CultureInfo.CurrentCulture);
				}
			},
			{
				HttpRequestHeader.Referer.ToString(),
				delegate(HttpWebRequest request, string value)
				{
					request.Referer = value;
				}
			},
			{
				HttpRequestHeader.TransferEncoding.ToString(),
				delegate(HttpWebRequest request, string value)
				{
					request.TransferEncoding = value;
				}
			}
		};

		public string UserAgent
		{
			get
			{
				return _request.UserAgent;
			}
			set
			{
				_request.UserAgent = value;
			}
		}

		public ICredentials Credentials
		{
			get
			{
				return _request.Credentials;
			}
			set
			{
				_request.Credentials = value;
			}
		}

		public CookieContainer CookieContainer
		{
			get
			{
				return _request.CookieContainer;
			}
			set
			{
				_request.CookieContainer = value;
			}
		}

		public string Accept
		{
			get
			{
				return _request.Accept;
			}
			set
			{
				_request.Accept = value;
			}
		}

		public IWebProxy Proxy
		{
			get
			{
				return _request.Proxy;
			}
			set
			{
				_request.Proxy = value;
			}
		}

		public HttpWebRequestWrapper(HttpWebRequest request)
		{
			_request = request;
		}

		public void Abort()
		{
			_request.Abort();
		}

		public void SetRequestHeaders(IDictionary<string, string> headers)
		{
			if (headers == null)
			{
				throw new ArgumentNullException("headers");
			}
			foreach (KeyValuePair<string, string> header in headers)
			{
				Action<HttpWebRequest, string> value;
				if (!_restrictedHeadersSet.Keys.Contains(header.Key))
				{
					_request.Headers.Add(header.Key, header.Value);
				}
				else if (_restrictedHeadersSet.TryGetValue(header.Key, out value))
				{
					value(_request, header.Value);
				}
			}
		}
	}
	public interface IResponse : IDisposable
	{
		Stream GetStream();
	}
	public class HttpWebResponseWrapper : IResponse, IDisposable
	{
		private readonly HttpWebResponse _response;

		public HttpWebResponseWrapper(HttpWebResponse response)
		{
			_response = response;
		}

		public Stream GetStream()
		{
			return _response.GetResponseStream();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				((IDisposable)_response).Dispose();
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
		}
	}
	public static class IHttpClientExtensions
	{
		public static Task<IResponse> Post(this IHttpClient client, string url, Action<IRequest> prepareRequest, bool isLongRunning)
		{
			if (client == null)
			{
				throw new ArgumentNullException("client");
			}
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			if (prepareRequest == null)
			{
				throw new ArgumentNullException("prepareRequest");
			}
			return client.Post(url, prepareRequest, null, isLongRunning);
		}
	}
}
namespace Microsoft.AspNet.SignalR.Client.Transports.ServerSentEvents
{
	public class EventSourceStreamReader : AsyncStreamReader
	{
		private readonly ChunkBuffer _buffer;

		private readonly IConnection _connection;

		public Action<SseEvent> Message { get; set; }

		public EventSourceStreamReader(IConnection connection, Stream stream)
			: base(stream)
		{
			_connection = connection;
			_buffer = new ChunkBuffer();
			base.Data = ProcessBuffer;
		}

		private void ProcessBuffer(ArraySegment<byte> readBuffer)
		{
			lock (base.BufferLock)
			{
				_buffer.Add(readBuffer);
				while (_buffer.HasChunks)
				{
					string text = _buffer.ReadLine();
					if (text != null)
					{
						if (SseEvent.TryParse(text, out var sseEvent))
						{
							_connection.Trace(TraceLevels.Messages, "SSE: OnMessage({0})", sseEvent);
							OnMessage(sseEvent);
						}
						continue;
					}
					break;
				}
			}
		}

		private void OnMessage(SseEvent sseEvent)
		{
			if (Message != null)
			{
				Message(sseEvent);
			}
		}
	}
}
namespace Microsoft.AspNet.SignalR.Infrastructure
{
	internal static class StreamExtensions
	{
		public static Task<int> ReadAsync(this Stream stream, byte[] buffer)
		{
			return FromAsync((AsyncCallback cb) => stream.BeginRead(buffer, 0, buffer.Length, cb, null), (IAsyncResult ar) => stream.EndRead(ar));
		}

		public static Task WriteAsync(this Stream stream, byte[] buffer)
		{
			return FromAsync((AsyncCallback cb) => stream.BeginWrite(buffer, 0, buffer.Length, cb, null), WrapEndWrite(stream));
		}

		private static Func<IAsyncResult, object> WrapEndWrite(Stream stream)
		{
			return delegate(IAsyncResult ar)
			{
				stream.EndWrite(ar);
				return (object)null;
			};
		}

		private static Task<T> FromAsync<T>(Func<AsyncCallback, IAsyncResult> begin, Func<IAsyncResult, T> end)
		{
			TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
			try
			{
				IAsyncResult asyncResult = begin(delegate(IAsyncResult ar)
				{
					if (!ar.CompletedSynchronously)
					{
						CompleteAsync(tcs, ar, end);
					}
				});
				if (asyncResult.CompletedSynchronously)
				{
					CompleteAsync(tcs, asyncResult, end);
				}
			}
			catch (Exception exception)
			{
				tcs.TrySetException(exception);
			}
			return tcs.Task;
		}

		private static void CompleteAsync<T>(TaskCompletionSource<T> tcs, IAsyncResult ar, Func<IAsyncResult, T> end)
		{
			try
			{
				tcs.TrySetResult(end(ar));
			}
			catch (OperationCanceledException)
			{
				tcs.TrySetCanceled();
			}
			catch (Exception exception)
			{
				tcs.TrySetException(exception);
			}
		}
	}
}
namespace Microsoft.AspNet.SignalR.Client
{
	[DebuggerDisplay("{ConnectionId} {Url} -> {ProtocolVersion}")]
	public class NegotiationResponse
	{
		public string ConnectionId { get; set; }

		public string ConnectionToken { get; set; }

		public string Url { get; set; }

		public string ProtocolVersion { get; set; }

		public double DisconnectTimeout { get; set; }

		public bool TryWebSockets { get; set; }

		public double? KeepAliveTimeout { get; set; }
	}
	public class ObservableConnection<T> : IObservable<T>
	{
		private readonly Connection _connection;

		private readonly Func<string, T> _convert;

		public ObservableConnection(Connection connection, Func<string, T> convert)
		{
			if (connection == null)
			{
				throw new ArgumentNullException("connection");
			}
			if (convert == null)
			{
				throw new ArgumentNullException("convert");
			}
			_convert = convert;
			_connection = connection;
		}

		public IDisposable Subscribe(IObserver<T> observer)
		{
			Action<string> received = delegate(string data)
			{
				observer.OnNext(_convert(data));
			};
			Action closed = delegate
			{
				observer.OnCompleted();
			};
			Action<Exception> error = delegate(Exception ex)
			{
				observer.OnError(ex);
			};
			_connection.Received += received;
			_connection.Closed += closed;
			_connection.Error += error;
			return new DisposableAction(delegate
			{
				_connection.Received -= received;
				_connection.Closed -= closed;
				_connection.Error -= error;
			});
		}
	}
}
namespace Microsoft.AspNet.SignalR.Client.Hubs
{
	public static class HubProxyExtensions
	{
		public static T GetValue<T>(this IHubProxy proxy, string name)
		{
			if (proxy == null)
			{
				throw new ArgumentNullException("proxy");
			}
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}
			return Convert<T>(proxy[name], proxy.JsonSerializer);
		}

		public static IDisposable On(this IHubProxy proxy, string eventName, Action onData)
		{
			if (proxy == null)
			{
				throw new ArgumentNullException("proxy");
			}
			if (string.IsNullOrEmpty(eventName))
			{
				throw new ArgumentNullException("eventName");
			}
			if (onData == null)
			{
				throw new ArgumentNullException("onData");
			}
			Subscription subscription = proxy.Subscribe(eventName);
			Action<IList<JToken>> handler = delegate
			{
				onData();
			};
			subscription.Received += handler;
			return new DisposableAction(delegate
			{
				subscription.Received -= handler;
			});
		}

		public static IDisposable On<T>(this IHubProxy proxy, string eventName, Action<T> onData)
		{
			if (proxy == null)
			{
				throw new ArgumentNullException("proxy");
			}
			if (string.IsNullOrEmpty(eventName))
			{
				throw new ArgumentNullException("eventName");
			}
			if (onData == null)
			{
				throw new ArgumentNullException("onData");
			}
			Subscription subscription = proxy.Subscribe(eventName);
			Action<IList<JToken>> handler = delegate(IList<JToken> args)
			{
				onData(Convert<T>(args[0], proxy.JsonSerializer));
			};
			subscription.Received += handler;
			return new DisposableAction(delegate
			{
				subscription.Received -= handler;
			});
		}

		public static IDisposable On<T1, T2>(this IHubProxy proxy, string eventName, Action<T1, T2> onData)
		{
			if (proxy == null)
			{
				throw new ArgumentNullException("proxy");
			}
			if (string.IsNullOrEmpty(eventName))
			{
				throw new ArgumentNullException("eventName");
			}
			if (onData == null)
			{
				throw new ArgumentNullException("onData");
			}
			Subscription subscription = proxy.Subscribe(eventName);
			Action<IList<JToken>> handler = delegate(IList<JToken> args)
			{
				onData(Convert<T1>(args[0], proxy.JsonSerializer), Convert<T2>(args[1], proxy.JsonSerializer));
			};
			subscription.Received += handler;
			return new DisposableAction(delegate
			{
				subscription.Received -= handler;
			});
		}

		public static IDisposable On<T1, T2, T3>(this IHubProxy proxy, string eventName, Action<T1, T2, T3> onData)
		{
			if (proxy == null)
			{
				throw new ArgumentNullException("proxy");
			}
			if (string.IsNullOrEmpty(eventName))
			{
				throw new ArgumentNullException("eventName");
			}
			if (onData == null)
			{
				throw new ArgumentNullException("onData");
			}
			Subscription subscription = proxy.Subscribe(eventName);
			Action<IList<JToken>> handler = delegate(IList<JToken> args)
			{
				onData(Convert<T1>(args[0], proxy.JsonSerializer), Convert<T2>(args[1], proxy.JsonSerializer), Convert<T3>(args[2], proxy.JsonSerializer));
			};
			subscription.Received += handler;
			return new DisposableAction(delegate
			{
				subscription.Received -= handler;
			});
		}

		public static IDisposable On<T1, T2, T3, T4>(this IHubProxy proxy, string eventName, Action<T1, T2, T3, T4> onData)
		{
			if (proxy == null)
			{
				throw new ArgumentNullException("proxy");
			}
			if (string.IsNullOrEmpty(eventName))
			{
				throw new ArgumentNullException("eventName");
			}
			if (onData == null)
			{
				throw new ArgumentNullException("onData");
			}
			Subscription subscription = proxy.Subscribe(eventName);
			Action<IList<JToken>> handler = delegate(IList<JToken> args)
			{
				onData(Convert<T1>(args[0], proxy.JsonSerializer), Convert<T2>(args[1], proxy.JsonSerializer), Convert<T3>(args[2], proxy.JsonSerializer), Convert<T4>(args[3], proxy.JsonSerializer));
			};
			subscription.Received += handler;
			return new DisposableAction(delegate
			{
				subscription.Received -= handler;
			});
		}

		public static IDisposable On(this IHubProxy proxy, string eventName, Action<dynamic> onData)
		{
			return proxy.On<object>(eventName, (Action<object>)onData);
		}

		public static IDisposable On<T1, T2, T3, T4, T5>(this IHubProxy proxy, string eventName, Action<T1, T2, T3, T4, T5> onData)
		{
			if (proxy == null)
			{
				throw new ArgumentNullException("proxy");
			}
			if (string.IsNullOrEmpty(eventName))
			{
				throw new ArgumentNullException("eventName");
			}
			if (onData == null)
			{
				throw new ArgumentNullException("onData");
			}
			Subscription subscription = proxy.Subscribe(eventName);
			Action<IList<JToken>> handler = delegate(IList<JToken> args)
			{
				onData(Convert<T1>(args[0], proxy.JsonSerializer), Convert<T2>(args[1], proxy.JsonSerializer), Convert<T3>(args[2], proxy.JsonSerializer), Convert<T4>(args[3], proxy.JsonSerializer), Convert<T5>(args[4], proxy.JsonSerializer));
			};
			subscription.Received += handler;
			return new DisposableAction(delegate
			{
				subscription.Received -= handler;
			});
		}

		public static IDisposable On<T1, T2, T3, T4, T5, T6>(this IHubProxy proxy, string eventName, Action<T1, T2, T3, T4, T5, T6> onData)
		{
			if (proxy == null)
			{
				throw new ArgumentNullException("proxy");
			}
			if (string.IsNullOrEmpty(eventName))
			{
				throw new ArgumentNullException("eventName");
			}
			if (onData == null)
			{
				throw new ArgumentNullException("onData");
			}
			Subscription subscription = proxy.Subscribe(eventName);
			Action<IList<JToken>> handler = delegate(IList<JToken> args)
			{
				onData(Convert<T1>(args[0], proxy.JsonSerializer), Convert<T2>(args[1], proxy.JsonSerializer), Convert<T3>(args[2], proxy.JsonSerializer), Convert<T4>(args[3], proxy.JsonSerializer), Convert<T5>(args[4], proxy.JsonSerializer), Convert<T6>(args[5], proxy.JsonSerializer));
			};
			subscription.Received += handler;
			return new DisposableAction(delegate
			{
				subscription.Received -= handler;
			});
		}

		public static IDisposable On<T1, T2, T3, T4, T5, T6, T7>(this IHubProxy proxy, string eventName, Action<T1, T2, T3, T4, T5, T6, T7> onData)
		{
			if (proxy == null)
			{
				throw new ArgumentNullException("proxy");
			}
			if (string.IsNullOrEmpty(eventName))
			{
				throw new ArgumentNullException("eventName");
			}
			if (onData == null)
			{
				throw new ArgumentNullException("onData");
			}
			Subscription subscription = proxy.Subscribe(eventName);
			Action<IList<JToken>> handler = delegate(IList<JToken> args)
			{
				onData(Convert<T1>(args[0], proxy.JsonSerializer), Convert<T2>(args[1], proxy.JsonSerializer), Convert<T3>(args[2], proxy.JsonSerializer), Convert<T4>(args[3], proxy.JsonSerializer), Convert<T5>(args[4], proxy.JsonSerializer), Convert<T6>(args[5], proxy.JsonSerializer), Convert<T7>(args[6], proxy.JsonSerializer));
			};
			subscription.Received += handler;
			return new DisposableAction(delegate
			{
				subscription.Received -= handler;
			});
		}

		public static IObservable<IList<JToken>> Observe(this IHubProxy proxy, string eventName)
		{
			if (proxy == null)
			{
				throw new ArgumentNullException("proxy");
			}
			if (string.IsNullOrEmpty(eventName))
			{
				throw new ArgumentNullException("eventName");
			}
			return new Hubservable(proxy, eventName);
		}

		private static T Convert<T>(JToken obj, JsonSerializer serializer)
		{
			if (obj == null)
			{
				return default(T);
			}
			return obj.ToObject<T>(serializer);
		}
	}
}
namespace Microsoft.AspNet.SignalR.Client
{
	public class StateChange
	{
		public ConnectionState OldState { get; private set; }

		public ConnectionState NewState { get; private set; }

		public StateChange(ConnectionState oldState, ConnectionState newState)
		{
			OldState = oldState;
			NewState = newState;
		}
	}
}
namespace Microsoft.AspNet.SignalR.Client.Transports
{
	public interface IClientTransport : IDisposable
	{
		string Name { get; }

		bool SupportsKeepAlive { get; }

		Task<NegotiationResponse> Negotiate(IConnection connection, string connectionData);

		Task Start(IConnection connection, string connectionData, CancellationToken disconnectToken);

		Task Send(IConnection connection, string data, string connectionData);

		void Abort(IConnection connection, TimeSpan timeout, string connectionData);

		void LostConnection(IConnection connection);
	}
	public class AutoTransport : IClientTransport, IDisposable
	{
		private IClientTransport _transport;

		private readonly IHttpClient _httpClient;

		private int _startIndex;

		private readonly IList<IClientTransport> _transports;

		public bool SupportsKeepAlive
		{
			get
			{
				if (_transport == null)
				{
					return false;
				}
				return _transport.SupportsKeepAlive;
			}
		}

		public string Name
		{
			get
			{
				if (_transport == null)
				{
					return null;
				}
				return _transport.Name;
			}
		}

		public AutoTransport(IHttpClient httpClient)
		{
			_httpClient = httpClient;
			_transports = new IClientTransport[2]
			{
				new ServerSentEventsTransport(httpClient),
				new LongPollingTransport(httpClient)
			};
		}

		public AutoTransport(IHttpClient httpClient, IList<IClientTransport> transports)
		{
			_httpClient = httpClient;
			_transports = transports;
		}

		public Task<NegotiationResponse> Negotiate(IConnection connection, string connectionData)
		{
			return _httpClient.GetNegotiationResponse(connection, connectionData);
		}

		public Task Start(IConnection connection, string connectionData, CancellationToken disconnectToken)
		{
			TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
			ResolveTransport(connection, connectionData, disconnectToken, taskCompletionSource, _startIndex);
			return taskCompletionSource.Task;
		}

		private void ResolveTransport(IConnection connection, string data, CancellationToken disconnectToken, TaskCompletionSource<object> tcs, int index)
		{
			IClientTransport transport = _transports[index];
			transport.Start(connection, data, disconnectToken).ContinueWith(delegate(Task task)
			{
				if (task.IsFaulted)
				{
					Exception baseException = task.Exception.GetBaseException();
					connection.Trace(TraceLevels.Events, "Auto: Failed to connect to using transport {0}. {1}", transport.Name, baseException);
					int num = index + 1;
					if (num < _transports.Count)
					{
						ResolveTransport(connection, data, disconnectToken, tcs, num);
					}
					else
					{
						tcs.SetException(task.Exception);
					}
				}
				else
				{
					_transport = transport;
					tcs.SetResult(null);
				}
			}, TaskContinuationOptions.ExecuteSynchronously);
		}

		public Task Send(IConnection connection, string data, string connectionData)
		{
			return _transport.Send(connection, data, connectionData);
		}

		public void Abort(IConnection connection, TimeSpan timeout, string connectionData)
		{
			if (_transport != null)
			{
				_transport.Abort(connection, timeout, connectionData);
			}
		}

		public void LostConnection(IConnection connection)
		{
			_transport.LostConnection(connection);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing && _transport != null)
			{
				_transport.Dispose();
			}
		}
	}
	public abstract class HttpBasedTransport : IClientTransport, IDisposable
	{
		private const string _sendQueryString = "?transport={0}&connectionData={1}&connectionToken={2}{3}";

		private readonly string _transport;

		private readonly IHttpClient _httpClient;

		private readonly TransportAbortHandler _abortHandler;

		public string Name => _transport;

		public abstract bool SupportsKeepAlive { get; }

		protected IHttpClient HttpClient => _httpClient;

		protected TransportAbortHandler AbortHandler => _abortHandler;

		protected HttpBasedTransport(IHttpClient httpClient, string transport)
		{
			_httpClient = httpClient;
			_transport = transport;
			_abortHandler = new TransportAbortHandler(httpClient, transport);
		}

		public Task<NegotiationResponse> Negotiate(IConnection connection, string connectionData)
		{
			return _httpClient.GetNegotiationResponse(connection, connectionData);
		}

		public Task Start(IConnection connection, string connectionData, CancellationToken disconnectToken)
		{
			TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
			OnStart(connection, connectionData, disconnectToken, delegate
			{
				tcs.TrySetResult(null);
			}, delegate(Exception exception)
			{
				tcs.TrySetException(exception);
			});
			return tcs.Task;
		}

		protected abstract void OnStart(IConnection connection, string connectionData, CancellationToken disconnectToken, Action initializeCallback, Action<Exception> errorCallback);

		public Task Send(IConnection connection, string data, string connectionData)
		{
			if (connection == null)
			{
				throw new ArgumentNullException("connection");
			}
			string text = connection.Url + "send";
			string text2 = (string.IsNullOrEmpty(connection.QueryString) ? string.Empty : ("&" + connection.QueryString));
			text += string.Format(CultureInfo.InvariantCulture, "?transport={0}&connectionData={1}&connectionToken={2}{3}", _transport, connectionData, Uri.EscapeDataString(connection.ConnectionToken), text2);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("data", data);
			Dictionary<string, string> postData = dictionary;
			return _httpClient.Post(text, connection.PrepareRequest, postData, isLongRunning: false).Then((IResponse response) => response.ReadAsString()).Then(delegate(string raw)
			{
				connection.Trace(TraceLevels.Messages, "OnMessage({0})", raw);
				if (!string.IsNullOrEmpty(raw))
				{
					connection.OnReceived(JObject.Parse(raw));
				}
			})
				.Catch(connection.OnError);
		}

		public void Abort(IConnection connection, TimeSpan timeout, string connectionData)
		{
			_abortHandler.Abort(connection, timeout, connectionData);
		}

		protected string GetReceiveQueryString(IConnection connection, string data)
		{
			return TransportHelper.GetReceiveQueryString(connection, data, _transport);
		}

		public abstract void LostConnection(IConnection connection);

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_abortHandler.Dispose();
			}
		}
	}
	public class LongPollingTransport : HttpBasedTransport
	{
		public TimeSpan ReconnectDelay { get; set; }

		public TimeSpan ErrorDelay { get; set; }

		public TimeSpan ConnectDelay { get; set; }

		public override bool SupportsKeepAlive => false;

		public LongPollingTransport()
			: this(new DefaultHttpClient())
		{
		}

		public LongPollingTransport(IHttpClient httpClient)
			: base(httpClient, "longPolling")
		{
			ReconnectDelay = TimeSpan.FromSeconds(5.0);
			ErrorDelay = TimeSpan.FromSeconds(2.0);
			ConnectDelay = TimeSpan.FromSeconds(2.0);
		}

		protected override void OnStart(IConnection connection, string connectionData, CancellationToken disconnectToken, Action initializeCallback, Action<Exception> errorCallback)
		{
			PollingRequestHandler requestHandler = new PollingRequestHandler(base.HttpClient);
			NegotiateInitializer negotiateInitializer = new NegotiateInitializer(initializeCallback, errorCallback, ConnectDelay);
			Action<string> initializeSuccess = delegate
			{
				negotiateInitializer.Complete();
			};
			Action<IRequest> initializeAbort = delegate
			{
				negotiateInitializer.Abort(disconnectToken);
			};
			requestHandler.OnMessage += initializeSuccess;
			requestHandler.OnError += negotiateInitializer.Complete;
			requestHandler.OnAbort += initializeAbort;
			negotiateInitializer.Initialized += delegate
			{
				requestHandler.OnMessage -= initializeSuccess;
				requestHandler.OnError -= negotiateInitializer.Complete;
				requestHandler.OnAbort -= initializeAbort;
			};
			PollingSetup(connection, connectionData, disconnectToken, requestHandler);
			requestHandler.Start();
			negotiateInitializer.Initialize();
		}

		private void PollingSetup(IConnection connection, string data, CancellationToken disconnectToken, PollingRequestHandler requestHandler)
		{
			ThreadSafeInvoker reconnectInvoker = new ThreadSafeInvoker();
			Disposer requestDisposer = new Disposer();
			requestHandler.ResolveUrl = delegate
			{
				string url = connection.Url;
				url = ((connection.MessageId == null) ? (url + "connect") : ((!IsReconnecting(connection)) ? (url + "poll") : (url + "reconnect")));
				return url + GetReceiveQueryString(connection, data);
			};
			requestHandler.PrepareRequest += delegate(IRequest req)
			{
				connection.PrepareRequest(req);
			};
			requestHandler.OnMessage += delegate(string message)
			{
				bool shouldReconnect = false;
				bool disconnected = false;
				connection.Trace(TraceLevels.Messages, "LP: OnMessage({0})", message);
				TransportHelper.ProcessResponse(connection, message, out shouldReconnect, out disconnected);
				if (IsReconnecting(connection))
				{
					TryReconnect(connection, reconnectInvoker);
				}
				if (shouldReconnect)
				{
					connection.EnsureReconnecting();
				}
				if (disconnected)
				{
					connection.Trace(TraceLevels.Messages, "Disconnect command received from server.");
					connection.Disconnect();
				}
			};
			requestHandler.OnError += delegate(Exception exception)
			{
				reconnectInvoker.Invoke();
				if (TransportHelper.VerifyLastActive(connection))
				{
					connection.EnsureReconnecting();
					if (!ExceptionHelper.IsRequestAborted(exception) && !(exception is IOException))
					{
						connection.OnError(exception);
					}
					else
					{
						requestHandler.Stop();
					}
				}
			};
			requestHandler.OnPolling += delegate
			{
				TryDelayedReconnect(connection, reconnectInvoker);
				requestDisposer.Set(disconnectToken.SafeRegister(delegate
				{
					reconnectInvoker.Invoke();
					requestHandler.Stop();
				}, null));
			};
			requestHandler.OnAfterPoll = delegate(Exception exception)
			{
				requestDisposer.Dispose();
				if (base.AbortHandler.TryCompleteAbort())
				{
					requestHandler.Stop();
				}
				else
				{
					requestDisposer = new Disposer();
					reconnectInvoker = new ThreadSafeInvoker();
					if (exception != null)
					{
						return TaskAsyncHelper.Delay(ErrorDelay);
					}
				}
				return TaskAsyncHelper.Empty;
			};
			requestHandler.OnAbort += delegate
			{
				base.AbortHandler.CompleteAbort();
			};
		}

		private void TryDelayedReconnect(IConnection connection, ThreadSafeInvoker reconnectInvoker)
		{
			if (IsReconnecting(connection))
			{
				TaskAsyncHelper.Delay(ReconnectDelay).Then(delegate
				{
					TryReconnect(connection, reconnectInvoker);
				});
			}
		}

		private static void TryReconnect(IConnection connection, ThreadSafeInvoker reconnectInvoker)
		{
			reconnectInvoker.Invoke(delegate(IConnection conn)
			{
				FireReconnected(conn);
			}, connection);
		}

		private static void FireReconnected(IConnection connection)
		{
			if (connection.ChangeState(ConnectionState.Reconnecting, ConnectionState.Connected))
			{
				connection.OnReconnected();
			}
		}

		private static bool IsReconnecting(IConnection connection)
		{
			return connection.State == ConnectionState.Reconnecting;
		}

		public override void LostConnection(IConnection connection)
		{
		}
	}
	public class ServerSentEventsTransport : HttpBasedTransport
	{
		private IRequest _request;

		public override bool SupportsKeepAlive => true;

		public TimeSpan ConnectionTimeout { get; set; }

		public TimeSpan ReconnectDelay { get; set; }

		public ServerSentEventsTransport()
			: this(new DefaultHttpClient())
		{
		}

		public ServerSentEventsTransport(IHttpClient httpClient)
			: base(httpClient, "serverSentEvents")
		{
			ReconnectDelay = TimeSpan.FromSeconds(2.0);
			ConnectionTimeout = TimeSpan.FromSeconds(5.0);
		}

		protected override void OnStart(IConnection connection, string connectionData, CancellationToken disconnectToken, Action initializeCallback, Action<Exception> errorCallback)
		{
			OpenConnection(connection, connectionData, disconnectToken, initializeCallback, errorCallback);
		}

		private void Reconnect(IConnection connection, string data, CancellationToken disconnectToken)
		{
			if (!TransportHelper.VerifyLastActive(connection))
			{
				return;
			}
			TaskAsyncHelper.Delay(ReconnectDelay).Then(delegate
			{
				if (TransportHelper.VerifyLastActive(connection) && !disconnectToken.IsCancellationRequested && connection.EnsureReconnecting())
				{
					OpenConnection(connection, data, disconnectToken, null, null);
				}
			});
		}

		private void OpenConnection(IConnection connection, string data, CancellationToken disconnectToken, Action initializeCallback, Action<Exception> errorCallback)
		{
			bool reconnecting = initializeCallback == null;
			ThreadSafeInvoker callbackInvoker = new ThreadSafeInvoker();
			Disposer requestDisposer = new Disposer();
			string text = connection.Url + (reconnecting ? "reconnect" : "connect") + GetReceiveQueryString(connection, data);
			connection.Trace(TraceLevels.Events, "SSE: GET {0}", text);
			base.HttpClient.Get(text, delegate(IRequest req)
			{
				_request = req;
				connection.PrepareRequest(_request);
				_request.Accept = "text/event-stream";
			}, isLongRunning: true).ContinueWith(delegate(Task<IResponse> task)
			{
				if (task.IsFaulted)
				{
					Exception ex2 = task.Exception.Unwrap();
					if (!ExceptionHelper.IsRequestAborted(ex2))
					{
						if (errorCallback != null)
						{
							callbackInvoker.Invoke(delegate(Action<Exception> cb, Exception ex)
							{
								cb(ex);
							}, errorCallback, ex2);
						}
						else if (reconnecting)
						{
							connection.OnError(ex2);
							Reconnect(connection, data, disconnectToken);
						}
					}
					requestDisposer.Dispose();
				}
				else
				{
					IResponse response = task.Result;
					Stream stream = response.GetStream();
					EventSourceStreamReader eventSourceStreamReader = new EventSourceStreamReader(connection, stream);
					bool stop = false;
					IDisposable esCancellationRegistration = disconnectToken.SafeRegister(delegate(object state)
					{
						stop = true;
						((IRequest)state).Abort();
					}, _request);
					eventSourceStreamReader.Opened = delegate
					{
						if (!reconnecting)
						{
							callbackInvoker.Invoke(initializeCallback);
						}
						else if (connection.ChangeState(ConnectionState.Reconnecting, ConnectionState.Connected))
						{
							connection.OnReconnected();
						}
					};
					eventSourceStreamReader.Message = delegate(SseEvent sseEvent)
					{
						if (sseEvent.EventType == EventType.Data && !sseEvent.Data.Equals("initialized", StringComparison.OrdinalIgnoreCase))
						{
							TransportHelper.ProcessResponse(connection, sseEvent.Data, out var _, out var disconnected);
							if (disconnected)
							{
								stop = true;
								connection.Trace(TraceLevels.Messages, "Disconnect command received from server.");
								connection.Disconnect();
							}
						}
					};
					eventSourceStreamReader.Closed = delegate(Exception exception)
					{
						if (exception != null && !ExceptionHelper.IsRequestAborted(exception))
						{
							connection.OnError(exception);
						}
						requestDisposer.Dispose();
						esCancellationRegistration.Dispose();
						response.Dispose();
						if (stop)
						{
							base.AbortHandler.CompleteAbort();
						}
						else if (!base.AbortHandler.TryCompleteAbort())
						{
							Reconnect(connection, data, disconnectToken);
						}
					};
					eventSourceStreamReader.Start();
				}
			});
			IDisposable disposable = disconnectToken.SafeRegister(delegate(object state)
			{
				if (state != null)
				{
					((IRequest)state).Abort();
				}
				if (errorCallback != null)
				{
					callbackInvoker.Invoke(delegate(Action<Exception> cb, CancellationToken token)
					{
						cb(new OperationCanceledException(Resources.Error_ConnectionCancelled, token));
					}, errorCallback, disconnectToken);
				}
			}, _request);
			requestDisposer.Set(disposable);
			if (errorCallback == null)
			{
				return;
			}
			TaskAsyncHelper.Delay(ConnectionTimeout).Then(delegate
			{
				callbackInvoker.Invoke(delegate(IConnection conn, Action<Exception> cb)
				{
					_request.Abort();
					cb(new TimeoutException());
				}, connection, errorCallback);
			});
		}

		public override void LostConnection(IConnection connection)
		{
			if (_request != null)
			{
				_request.Abort();
			}
		}
	}
}
namespace Microsoft.AspNet.SignalR.Client.Transports.ServerSentEvents
{
	public enum EventType
	{
		Id,
		Data
	}
	public class SseEvent
	{
		public EventType EventType { get; private set; }

		public string Data { get; private set; }

		public SseEvent(EventType type, string data)
		{
			EventType = type;
			Data = data;
		}

		public override string ToString()
		{
			return string.Concat(EventType, ": ", Data);
		}

		public static bool TryParse(string line, out SseEvent sseEvent)
		{
			sseEvent = null;
			if (line == null)
			{
				throw new ArgumentNullException("line");
			}
			if (line.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
			{
				string data = line.Substring("data:".Length).Trim();
				sseEvent = new SseEvent(EventType.Data, data);
				return true;
			}
			if (line.StartsWith("id:", StringComparison.OrdinalIgnoreCase))
			{
				string data2 = line.Substring("id:".Length).Trim();
				sseEvent = new SseEvent(EventType.Id, data2);
				return true;
			}
			return false;
		}
	}
}
namespace Microsoft.AspNet.SignalR.Client.Transports
{
	public static class TransportHelper
	{
		public static Task<NegotiationResponse> GetNegotiationResponse(this IHttpClient httpClient, IConnection connection, string connectionData)
		{
			if (httpClient == null)
			{
				throw new ArgumentNullException("httpClient");
			}
			if (connection == null)
			{
				throw new ArgumentNullException("connection");
			}
			string text = connection.Url + "negotiate";
			char c = '?';
			text += AppendCustomQueryString(connection, text);
			if (!string.IsNullOrEmpty(connectionData))
			{
				object obj = text;
				text = string.Concat(obj, c, "connectionData=", connectionData);
			}
			return httpClient.Get(text, connection.PrepareRequest, isLongRunning: false).Then((IResponse response) => response.ReadAsString()).Then(delegate(string raw)
			{
				if (string.IsNullOrEmpty(raw))
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.Error_ServerNegotiationFailed));
				}
				return JsonConvert.DeserializeObject<NegotiationResponse>(raw);
			});
		}

		public static string GetReceiveQueryString(IConnection connection, string connectionData, string transport)
		{
			if (connection == null)
			{
				throw new ArgumentNullException("connection");
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("?transport=" + transport).Append("&connectionToken=" + Uri.EscapeDataString(connection.ConnectionToken));
			if (connection.MessageId != null)
			{
				stringBuilder.Append("&messageId=" + Uri.EscapeDataString(connection.MessageId));
			}
			if (connection.GroupsToken != null)
			{
				stringBuilder.Append("&groupsToken=" + Uri.EscapeDataString(connection.GroupsToken));
			}
			if (connectionData != null)
			{
				stringBuilder.Append("&connectionData=" + connectionData);
			}
			string queryString = connection.QueryString;
			if (!string.IsNullOrEmpty(queryString))
			{
				stringBuilder.Append("&").Append(queryString);
			}
			return stringBuilder.ToString();
		}

		public static string AppendCustomQueryString(IConnection connection, string baseUrl)
		{
			if (connection == null)
			{
				throw new ArgumentNullException("connection");
			}
			if (baseUrl == null)
			{
				baseUrl = "";
			}
			string text = "";
			string queryString = connection.QueryString;
			string text2 = "";
			if (!string.IsNullOrEmpty(queryString))
			{
				char c = queryString[0];
				if (c != '?' && c != '&')
				{
					text = "?";
					if (baseUrl.Contains(text))
					{
						text = "&";
					}
				}
				text2 = text2 + text + queryString;
			}
			return text2;
		}

		public static void ProcessResponse(IConnection connection, string response, out bool shouldReconnect, out bool disconnected)
		{
			if (connection == null)
			{
				throw new ArgumentNullException("connection");
			}
			connection.MarkLastMessage();
			shouldReconnect = false;
			disconnected = false;
			if (string.IsNullOrEmpty(response))
			{
				return;
			}
			try
			{
				JToken jToken = JToken.Parse(response);
				if (!jToken.HasValues)
				{
					return;
				}
				if (jToken["I"] != null)
				{
					connection.OnReceived(jToken);
					return;
				}
				shouldReconnect = (int?)jToken["T"] == 1;
				disconnected = jToken.Value<int>("D") == 1;
				if (disconnected)
				{
					return;
				}
				UpdateGroups(connection, jToken["G"]);
				if (!(jToken["M"] is JArray jArray))
				{
					return;
				}
				foreach (JToken item in jArray)
				{
					try
					{
						connection.OnReceived(item);
					}
					catch (Exception ex)
					{
						connection.OnError(ex);
					}
				}
				connection.MessageId = jToken["C"].Value<string>();
			}
			catch (Exception ex2)
			{
				connection.OnError(ex2);
			}
		}

		public static bool VerifyLastActive(IConnection connection)
		{
			if (connection == null)
			{
				throw new ArgumentNullException("connection");
			}
			if (DateTime.UtcNow - connection.LastActiveAt >= connection.ReconnectWindow)
			{
				connection.Trace(TraceLevels.Events, "There has not been an active server connection for an extended period of time. Stopping connection.");
				connection.Stop();
				return false;
			}
			return true;
		}

		private static void UpdateGroups(IConnection connection, JToken groupsToken)
		{
			if (groupsToken != null)
			{
				connection.GroupsToken = groupsToken.Value<string>();
			}
		}

		private static string GetNoCacheUrlParam()
		{
			return "noCache=" + Guid.NewGuid();
		}
	}
}
