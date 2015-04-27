namespace SampleRandomEmulation
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Windows;
	using System.Windows.Media;

	using Ecng.Common;
	using Ecng.Xaml;
	using Ecng.Collections;

	using StockSharp.Algo;
	using StockSharp.Algo.Candles;
	using StockSharp.Algo.Indicators;
	using StockSharp.Algo.Strategies.Reporting;
	using StockSharp.Algo.Testing;
	using StockSharp.BusinessEntities;
	using StockSharp.Logging;
	using StockSharp.Messages;
	using StockSharp.Xaml.Charting;
	using StockSharp.Localization;

	public partial class MainWindow
	{
		private SmaStrategy _strategy;

		private ICollection<EquityData> _curveItems;
		private HistoryEmulationConnector _connector;

		private readonly LogManager _logManager = new LogManager();

		private DateTime _startEmulationTime;

		public MainWindow()
		{
			InitializeComponent();

			_logManager.Listeners.Add(new FileLogListener("log.txt"));
		}

		private void StartBtnClick(object sender, RoutedEventArgs e)
		{
			// if process was already started, will stop it now
			if (_connector != null)
			{
				_strategy.Stop();
				_connector.Disconnect();
				_logManager.Sources.Clear();

				_connector = null;
				return;
			}

			// create test security
			var security = new Security
			{
				Id = "RIU9@FORTS",
				Code = "RIU9",
				Name = "RTS-9.09",
				Board = ExchangeBoard.Forts,
			};

			var startTime = new DateTime(2009, 6, 1);
			var stopTime = new DateTime(2009, 9, 1);

			var level1Info = new Level1ChangeMessage
			{
				SecurityId = security.ToSecurityId(),
				ServerTime = startTime,
			}
			.TryAdd(Level1Fields.PriceStep, 10m)
			.TryAdd(Level1Fields.StepPrice, 6m)
			.TryAdd(Level1Fields.MinPrice, 10m)
			.TryAdd(Level1Fields.MaxPrice, 1000000m)
			.TryAdd(Level1Fields.MarginBuy, 10000m)
			.TryAdd(Level1Fields.MarginSell, 10000m);

			// test portfolio
			var portfolio = new Portfolio
			{
				Name = "test account",
				BeginValue = 1000000,
			};

			var timeFrame = TimeSpan.FromMinutes(5);

			// create backtesting connector
			_connector = new HistoryEmulationConnector(
				new[] { security },
				new[] { portfolio })
			{ MarketTimeChangedInterval = timeFrame };

			_logManager.Sources.Add(_connector);

			_connector.NewSecurities += securities =>
			{
				if (securities.All(s => s != security))
					return;

				// fill level1 values
				_connector.SendOutMessage(level1Info);

				_connector.RegisterTrades(new RandomWalkTradeGenerator(_connector.GetSecurityId(security)));
				_connector.RegisterMarketDepth(new TrendMarketDepthGenerator(_connector.GetSecurityId(security)) { GenerateDepthOnEachTrade = false });

				// start historical data loading when connection established successfully and all data subscribed
				_connector.Start();
			};

			_connector.StartDate = startTime;
			_connector.StopDate = stopTime;

			var candleManager = new CandleManager(_connector);

			var series = new CandleSeries(typeof(TimeFrameCandle), security, timeFrame);

			// create strategy based on 80 5-min и 10 5-min
			_strategy = new SmaStrategy(series, new SimpleMovingAverage { Length = 80 }, new SimpleMovingAverage { Length = 10 })
			{
				Volume = 1,
				Security = security,
				Portfolio = portfolio,
				Connector = _connector,
			};

			// fill parameters panel
			ParameterGrid.Parameters.Clear();
			ParameterGrid.Parameters.AddRange(_strategy.StatisticManager.Parameters);

			_strategy.PnLChanged += () =>
			{
				var data = new EquityData
				{
					Time = _strategy.CurrentTime,
					Value = _strategy.PnL,
				};

				this.GuiAsync(() => _curveItems.Add(data));
			};

			_logManager.Sources.Add(_strategy);

			// ProgressBar refresh step
			var progressStep = ((stopTime - startTime).Ticks / 100).To<TimeSpan>();
			var nextTime = startTime + progressStep;

			TestingProcess.Maximum = 100;
			TestingProcess.Value = 0;

			// handle historical time for update ProgressBar
			_connector.MarketTimeChanged += diff =>
			{
				if (_connector.CurrentTime < nextTime && _connector.CurrentTime < stopTime)
					return;

				var steps = (_connector.CurrentTime - startTime).Ticks / progressStep.Ticks + 1;
				nextTime = startTime + (steps * progressStep.Ticks).To<TimeSpan>();
				this.GuiAsync(() => TestingProcess.Value = steps);
			};

			_connector.StateChanged += () =>
			{
				if (_connector.State == EmulationStates.Stopped)
				{
					this.GuiAsync(() =>
					{
						Report.IsEnabled = true;

						if (_connector.IsFinished)
						{
							TestingProcess.Value = TestingProcess.Maximum;
							MessageBox.Show(LocalizedStrings.Str3024.Put(DateTime.Now - _startEmulationTime));
						}
						else
							MessageBox.Show(LocalizedStrings.cancelled);
					});
				}
				else if (_connector.State == EmulationStates.Started)
				{
					// start strategy when emulation started
					_strategy.Start();
					candleManager.Start(series);
				}
			};

			if (_curveItems == null)
				_curveItems = Curve.CreateCurve(_strategy.Name, Colors.DarkGreen);
			else
				_curveItems.Clear();

			Report.IsEnabled = false;

			_startEmulationTime = DateTime.Now;

			// raise NewSecurities and NewPortfolio for full fill strategy properties
			_connector.Connect();
		}

		private void ReportClick(object sender, RoutedEventArgs e)
		{
			// generate report for backtested strategy
			// Warning! For the huge order or trade count,
			// generation will be extremely slow
			new ExcelStrategyReport(_strategy, "sma.xlsx").Generate();

			// order excel file
			Process.Start("sma.xlsx");
		}
	}
}