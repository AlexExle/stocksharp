﻿namespace StockSharp.Algo.Export.Database
{
	using System;
	using System.Collections.Generic;

	using Ecng.Common;

	using StockSharp.BusinessEntities;
	using StockSharp.Messages;

	class ExecutionTable : Table<ExecutionMessage>
	{
		public ExecutionTable(Security security)
			: base("Execution", CreateColumns(security))
		{
		}

		private static IEnumerable<ColumnDescription> CreateColumns(Security security)
		{
			yield return new ColumnDescription("SecurityCode")
			{
				DbType = typeof(string),
				ValueRestriction = new StringRestriction(256)
			};
			yield return new ColumnDescription("BoardCode")
			{
				DbType = typeof(string),
				ValueRestriction = new StringRestriction(256)
			};
			yield return new ColumnDescription("ServerTime") { DbType = typeof(DateTimeOffset) };
			yield return new ColumnDescription("PortfolioName")
			{
				DbType = typeof(string),
				ValueRestriction = new StringRestriction(32)
			};
			yield return new ColumnDescription("TransactionId") { DbType = typeof(int) };
			yield return new ColumnDescription("OrderId")
			{
				DbType = typeof(string),
				ValueRestriction = new StringRestriction(32)
			};
			yield return new ColumnDescription("Price") { DbType = typeof(decimal), ValueRestriction = new DecimalRestriction { Scale = security.PriceStep == null ? 1 : security.PriceStep.Value.GetCachedDecimals() } };
			yield return new ColumnDescription("Volume") { DbType = typeof(decimal?), ValueRestriction = new DecimalRestriction { Scale = security.VolumeStep == null ? 1 : security.VolumeStep.Value.GetCachedDecimals() } };
			yield return new ColumnDescription("Balance") { DbType = typeof(decimal?), ValueRestriction = new DecimalRestriction { Scale = security.VolumeStep == null ? 1 : security.VolumeStep.Value.GetCachedDecimals() } };
			yield return new ColumnDescription("Side") { DbType = typeof(int) };
			yield return new ColumnDescription("OrderType") { DbType = typeof(int) };
			yield return new ColumnDescription("OrderState") { DbType = typeof(int?) };
			yield return new ColumnDescription("TradeId")
			{
				DbType = typeof(string),
				ValueRestriction = new StringRestriction(32)
			};
			yield return new ColumnDescription("TradePrice") { DbType = typeof(decimal?), ValueRestriction = new DecimalRestriction { Scale = security.PriceStep == null ? 1 : security.PriceStep.Value.GetCachedDecimals() } };
		}

		protected override IDictionary<string, object> ConvertToParameters(ExecutionMessage value)
		{
			var result = new Dictionary<string, object>
			{
				{ "SecurityCode", value.SecurityId.SecurityCode },
				{ "BoardCode", value.SecurityId.BoardCode },
				{ "ServerTime", value.ServerTime },
				{ "PortfolioName", value.PortfolioName },
				{ "TransactionId", value.TransactionId },
				{ "OrderId", value.OrderId == null ? value.OrderStringId : value.OrderId.To<string>() },
				{ "Price", value.Price },
				{ "Volume", value.Volume },
				{ "Balance", value.Balance },
				{ "Side", (int)value.Side },
				{ "OrderType", (int)value.OrderType },
				{ "OrderState", (int?)value.OrderState },
				{ "TradeId", value.TradeId == null ? value.TradeStringId : value.TradeId.To<string>() },
				{ "TradePrice", value.TradePrice },
			};
			return result;
		}
	}
}