namespace StockSharp.Algo.Testing
{
	using StockSharp.BusinessEntities;
	using StockSharp.Messages;

	/// <summary>
	/// ������� ����������� ��������.
	/// </summary>
	public abstract class BaseEmulationConnector : Connector
	{
		/// <summary>
		/// ���������������� <see cref="BaseEmulationConnector"/>.
		/// </summary>
		protected BaseEmulationConnector()
		{
			var adapter = new EmulationMessageAdapter(new MarketEmulator(), TransactionIdGenerator);
			Adapter.InnerAdapters.Add(adapter.ToChannel(this));
		}

		/// <summary>
		/// �������������� �� ��������������� ������ ����� ����� <see cref="IConnector.ReRegisterOrder(StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.Order)"/>
		/// � ���� ����� ����������.
		/// </summary>
		public override bool IsSupportAtomicReRegister
		{
			get { return MarketEmulator.Settings.IsSupportAtomicReRegister; }
		}

		/// <summary>
		/// �������� ������.
		/// </summary>
		public IMarketEmulator MarketEmulator
		{
			get { return ((EmulationMessageAdapter)TransactionAdapter).Emulator; }
			set { ((EmulationMessageAdapter)TransactionAdapter).Emulator = value; }
		}

		/// <summary>
		/// ��������� ������ ��������� ��������� <see cref="TimeMessage"/> � ���������� <see cref="Connector.MarketTimeChangedInterval"/>.
		/// </summary>
		protected override void StartMarketTimer()
		{
		}

		///// <summary>
		///// ���������� ���������, ���������� �������� ������.
		///// </summary>
		///// <param name="message">���������, ���������� �������� ������.</param>
		///// <param name="direction">����������� ���������.</param>
		//protected override void OnProcessMessage(Message message, MessageDirections direction)
		//{
		//	if (adapter == MarketDataAdapter && direction == MessageDirections.Out)
		//	{
		//		switch (message.Type)
		//		{
		//			case MessageTypes.Connect:
		//			case MessageTypes.Disconnect:
		//			case MessageTypes.MarketData:
		//			case MessageTypes.Error:
		//			case MessageTypes.SecurityLookupResult:
		//			case MessageTypes.PortfolioLookupResult:
		//				base.OnProcessMessage(message, direction);
		//				break;

		//			case MessageTypes.Execution:
		//			{
		//				var execMsg = (ExecutionMessage)message;

		//				if (execMsg.ExecutionType != ExecutionTypes.Trade)
		//					SendInMessage(message);
		//				else
		//					base.OnProcessMessage(message, direction);

		//				break;
		//			}

		//			default:
		//				SendInMessage(message);
		//				break;
		//		}
		//	}
		//	else
		//		base.OnProcessMessage(message, direction);
		//}
	}
}