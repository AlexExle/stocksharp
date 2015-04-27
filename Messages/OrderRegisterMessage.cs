namespace StockSharp.Messages
{
	using System;
	using System.Runtime.Serialization;

	using Ecng.Common;
	using Ecng.Serialization;

	using StockSharp.Localization;

	/// <summary>
	/// Сообщение, содержащее информацию для регистрации заявки.
	/// </summary>
	[System.Runtime.Serialization.DataContract]
	[Serializable]
	public class OrderRegisterMessage : OrderMessage
	{
		/// <summary>
		/// Идентификатор транзакции.
		/// </summary>
		[DataMember]
		[DisplayNameLoc(LocalizedStrings.TransactionKey)]
		[DescriptionLoc(LocalizedStrings.TransactionIdKey, true)]
		[MainCategory]
		public long TransactionId { get; set; }

		/// <summary>
		/// Цена заявки.
		/// </summary>
		[DataMember]
		[DisplayNameLoc(LocalizedStrings.PriceKey)]
		[DescriptionLoc(LocalizedStrings.OrderPriceKey)]
		[MainCategory]
		public decimal Price { get; set; }

		/// <summary>
		/// Количество контрактов в заявке.
		/// </summary>
		[DataMember]
		[DisplayNameLoc(LocalizedStrings.VolumeKey)]
		[DescriptionLoc(LocalizedStrings.OrderVolumeKey)]
		[MainCategory]
		public decimal Volume { get; set; }

		/// <summary>
		/// Видимое количество контрактов в заявке.
		/// </summary>
		[DataMember]
		[DisplayNameLoc(LocalizedStrings.VisibleVolumeKey)]
		[DescriptionLoc(LocalizedStrings.Str127Key)]
		[MainCategory]
		[Nullable]
		public decimal? VisibleVolume { get; set; }

		/// <summary>
		/// Направление заявки (покупка или продажа).
		/// </summary>
		[DataMember]
		[DisplayNameLoc(LocalizedStrings.Str128Key)]
		[DescriptionLoc(LocalizedStrings.Str129Key)]
		[MainCategory]
		public Sides Side { get; set; }

		/// <summary>
		/// Комментарий к выставляемой заявке.
		/// </summary>
		[DataMember]
		[DisplayNameLoc(LocalizedStrings.Str135Key)]
		[DescriptionLoc(LocalizedStrings.Str136Key)]
		[MainCategory]
		public string Comment { get; set; }

		/// <summary>
		/// Время экспирации заявки. По-умолчанию равно <see langword="null"/>, что означает действие заявки до отмены (GTC).
		/// </summary>
		/// <remarks>
		/// Если значение равно <see langword="null"/> или <see cref="DateTimeOffset.MaxValue"/>, то заявка выставляется до отмены (GTC).
		/// Иначе, указывается конкретный срок.
		/// </remarks>
		[DataMember]
		[DisplayNameLoc(LocalizedStrings.Str141Key)]
		[DescriptionLoc(LocalizedStrings.Str142Key)]
		[MainCategory]
		public DateTimeOffset? TillDate { get; set; }

		/// <summary>
		/// Условие заявки (например, параметры стоп- или алго- заявков).
		/// </summary>
		[DisplayNameLoc(LocalizedStrings.Str154Key)]
		[DescriptionLoc(LocalizedStrings.Str155Key)]
		[CategoryLoc(LocalizedStrings.Str156Key)]
		public OrderCondition Condition { get; set; }

		/// <summary>
		/// Время жизни лимитной заявки.
		/// </summary>
		[DisplayNameLoc(LocalizedStrings.Str231Key)]
		[DescriptionLoc(LocalizedStrings.Str232Key)]
		[MainCategory]
		[Nullable]
		public TimeInForce? TimeInForce { get; set; }

		/// <summary>
		/// Информация для РЕПО\РЕПО-М заявок.
		/// </summary>
		[DisplayNameLoc(LocalizedStrings.Str233Key)]
		[DescriptionLoc(LocalizedStrings.Str234Key)]
		[MainCategory]
		public RepoOrderInfo RepoInfo { get; set; }

		/// <summary>
		/// Информация для РПС заявок.
		/// </summary>
		[DisplayNameLoc(LocalizedStrings.Str235Key)]
		[DescriptionLoc(LocalizedStrings.Str236Key)]
		[MainCategory]
		public RpsOrderInfo RpsInfo { get; set; }

		/// <summary>
		/// Создать <see cref="OrderRegisterMessage"/>.
		/// </summary>
		public OrderRegisterMessage()
			: base(MessageTypes.OrderRegister)
		{
		}

		/// <summary>
		/// Инициализировать <see cref="OrderRegisterMessage"/>.
		/// </summary>
		/// <param name="type">Тип сообщения.</param>
		protected OrderRegisterMessage(MessageTypes type)
			: base(type)
		{
		}

		/// <summary>
		/// Создать копию объекта <see cref="OrderRegisterMessage"/>.
		/// </summary>
		/// <returns>Копия.</returns>
		public override Message Clone()
		{
			var clone = new OrderRegisterMessage(Type)
			{
				Comment = Comment,
				Condition = Condition,
				TillDate = TillDate,
				OrderType = OrderType,
				PortfolioName = PortfolioName,
				Price = Price,
				RepoInfo = RepoInfo.CloneNullable(),
				RpsInfo = RpsInfo.CloneNullable(),
				SecurityId = SecurityId,
				//SecurityType = SecurityType,
				Side = Side,
				TimeInForce = TimeInForce,
				TransactionId = TransactionId,
				VisibleVolume = VisibleVolume,
				Volume = Volume,
				Currency = Currency,
				UserOrderId = UserOrderId,
				ClientCode = ClientCode,
				BrokerCode = BrokerCode,
			};

			CopyTo(clone);

			return clone;
		}

		/// <summary>
		/// Получить строковое представление.
		/// </summary>
		/// <returns>Строковое представление.</returns>
		public override string ToString()
		{
			return base.ToString() + ",TransId={0},Price={1},Side={2},OrdType={3},Vol={4},Sec={5}".Put(TransactionId, Price, Side, OrderType, Volume, SecurityId);
		}
	}
}