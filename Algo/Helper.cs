namespace StockSharp.Algo
{
	using System;

	using Ecng.Collections;
	using Ecng.Common;

	using StockSharp.BusinessEntities;
	using StockSharp.Messages;
	using StockSharp.Localization;

	static class Helper
	{
		public static void CheckOption(this Security option)
		{
			if (option == null)
				throw new ArgumentNullException("option");

			if (option.Type != SecurityTypes.Option)
				throw new ArgumentException(LocalizedStrings.Str900Params.Put(option.Type), "option");

			if (option.OptionType == null)
				throw new ArgumentException(LocalizedStrings.Str703Params.Put(option), "option");

			if (option.ExpiryDate == null)
				throw new ArgumentException(LocalizedStrings.Str901Params.Put(option), "option");

			if (option.UnderlyingSecurityId == null)
				throw new ArgumentException(LocalizedStrings.Str902Params.Put(option), "option");
		}

		public static ExchangeBoard CheckExchangeBoard(this Security security)
		{
			if (security == null)
				throw new ArgumentNullException("security");

			if (security.Board == null)
				throw new ArgumentException(LocalizedStrings.Str903Params.Put(security), "security");

			return security.Board;
		}

		public static Security CheckPriceStep(this Security security)
		{
			if (security == null)
				throw new ArgumentNullException("security");

			if (security.PriceStep == null)
				throw new ArgumentException(LocalizedStrings.Str905Params.Put(security.Id));

			return security;
		}

		public static int ChangeSubscribers<T>(this CachedSynchronizedDictionary<T, int> subscribers, T subscriber, int delta)
		{
			if (subscribers == null)
				throw new ArgumentNullException("subscribers");

			lock (subscribers.SyncRoot)
			{
				var value = subscribers.TryGetValue2(subscriber) ?? 0;

				value += delta;

				if (value > 0)
					subscribers[subscriber] = value;
				else
					subscribers.Remove(subscriber);

				return value;
			}
		}

		public static long GetTradeId(this ExecutionMessage message)
		{
			if (message == null)
				throw new ArgumentNullException("message");

			var tradeId = message.TradeId;

			if (tradeId == null)
				throw new ArgumentOutOfRangeException("message", null, LocalizedStrings.Str1020);

			return tradeId.Value;
		}

		public static decimal GetTradePrice(this ExecutionMessage message)
		{
			if (message == null)
				throw new ArgumentNullException("message");

			var price = message.TradePrice;

			if (price == null)
				throw new ArgumentOutOfRangeException("message", null, LocalizedStrings.Str1021Params.Put(message.TradeId));

			return price.Value;
		}

		public static decimal GetVolume(this ExecutionMessage message)
		{
			if (message == null)
				throw new ArgumentNullException("message");

			var volume = message.Volume;

			if (volume != null)
				return volume.Value;

			var errorMsg = message.ExecutionType == ExecutionTypes.Tick || message.ExecutionType == ExecutionTypes.Trade
				? LocalizedStrings.Str1022Params.Put((object)message.TradeId ?? message.TradeStringId)
				: LocalizedStrings.Str927Params.Put((object)message.OrderId ?? message.OrderStringId);

			throw new ArgumentOutOfRangeException("message", null, errorMsg);
		}

		public static decimal GetBalance(this ExecutionMessage message)
		{
			if (message == null)
				throw new ArgumentNullException("message");

			var balance = message.Balance;

			if (balance != null)
				return balance.Value;

			throw new ArgumentOutOfRangeException("message");
		}

		public static long GetOrderId(this ExecutionMessage message)
		{
			if (message == null)
				throw new ArgumentNullException("message");

			var orderId = message.OrderId;

			if (orderId != null)
				return orderId.Value;

			throw new ArgumentOutOfRangeException("message", null, LocalizedStrings.Str925);
		}
	}
}