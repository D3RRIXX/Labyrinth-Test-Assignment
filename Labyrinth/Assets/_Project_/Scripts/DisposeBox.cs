using System;

namespace Labyrinth
{
	public class DisposeBox<T>
	{
		private readonly Action<T> _disposeAction;
		private T _value;

		public DisposeBox(Action<T> disposeAction)
		{
			_disposeAction = disposeAction;
		}

		public T Value
		{
			get => _value;
			set
			{
				if (_value?.Equals(value) != null)
					return;

				_disposeAction(_value);
				_value = value;
			}
		}
	}
}