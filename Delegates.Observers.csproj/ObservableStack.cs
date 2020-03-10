using System;
using System.Collections.Generic;
using System.Text;

namespace Delegates.Observers
{
	public class StackOperationsLogger
	{
		private readonly Observer observer = new Observer();

		public void SubscribeOn<T>(ObservableStack<T> stack)
		{
			stack.Notify += (s, e) => observer.HandleEvent(e);
		}

		public string GetLog() => observer.Log.ToString();
	}

	public interface IObserver
	{
		void HandleEvent(object eventData);
	}

	public class Observer : IObserver
	{
		public StringBuilder Log = new StringBuilder();
		public void HandleEvent(object eventData) => Log.Append(eventData);
	}

	public class ObservableStack<T>
	{
		//public delegate void HandlerEvent(object sender, StackEventData<T> data);
		//public event HandlerEvent Notify;

		//public event Action<object> Notify;

		public event EventHandler<object> Notify;

		private List<T> data = new List<T>();

		public void Push(T obj)
		{
			data.Add(obj);
			Notify?.Invoke(this, new StackEventData<T> { IsPushed = true, Value = obj });
		}

		public T Pop()
		{
			if (data.Count == 0)
				throw new InvalidOperationException();
			var result = data[data.Count - 1];
			Notify?.Invoke(this, new StackEventData<T> { IsPushed = false, Value = result });
			data.RemoveAt(data.Count - 1);
			return result;
		}
	}
}