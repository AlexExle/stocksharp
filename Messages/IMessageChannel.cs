namespace StockSharp.Messages
{
	using System;

	using Ecng.Common;

	/// <summary>
	/// ���������, ����������� ������������ ����� ���������.
	/// </summary>
	public interface IMessageChannel : IDisposable
	{
		/// <summary>
		/// ������ �� �����.
		/// </summary>
		bool IsOpened { get; }

		/// <summary>
		/// ������� �����.
		/// </summary>
		void Open();

		/// <summary>
		/// ������� �����.
		/// </summary>
		void Close();

		/// <summary>
		/// ��������� ���������.
		/// </summary>
		/// <param name="message">���������.</param>
		void SendInMessage(Message message);

		/// <summary>
		/// ������� ��������� ������ ���������.
		/// </summary>
		event Action<Message> NewOutMessage;
	}

	/// <summary>
	/// ������������ ����� ���������, ������� �������� ����� �� ����� ��� �������� ���������.
	/// </summary>
	public class PassThroughMessageChannel : IMessageChannel
	{
		/// <summary>
		/// ������� <see cref="PassThroughMessageChannel"/>.
		/// </summary>
		public PassThroughMessageChannel()
		{
		}

		void IDisposable.Dispose()
		{
		}

		bool IMessageChannel.IsOpened
		{
			get { return true; }
		}

		void IMessageChannel.Open()
		{
		}

		void IMessageChannel.Close()
		{
		}

		void IMessageChannel.SendInMessage(Message message)
		{
			_newMessage.SafeInvoke(message);
		}

		private Action<Message> _newMessage;

		event Action<Message> IMessageChannel.NewOutMessage
		{
			add { _newMessage += value; }
			remove { _newMessage -= value; }
		}
	}
}