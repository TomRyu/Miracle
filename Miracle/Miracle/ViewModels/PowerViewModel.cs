using Miracle.Helpers;
using Miracle.Models;
using Miracle.Services;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Miracle.ViewModels
{
	public class PowerViewModel : ViewModelBase
	{
		private ObservableRangeCollection<PowerModel> _searchResult = new ObservableRangeCollection<PowerModel>();
		public ICommand SearchCommand { get; private set; }
		private DateTime _dtFrom = Convert.ToDateTime("1900-01-01");
		private PlotModel _plotChartModel = new PlotModel();

		public PowerViewModel()
		{
			SearchCommand = new Command(async () => await Search());

			DtFrom = DateTime.Now;

		}

		private async Task Search()
		{
			//그래프 출력
			var model = new PlotModel
			{
				//Title = "OxyPlot 그래프 테스트",
				//Subtitle = "서브타이블", 
				PlotType = PlotType.XY,
				Background = OxyColors.White
			};

			var series1 = new LineSeries { MarkerType = MarkerType.Circle};

			List<PowerModel> lstPower = new List<PowerModel>();
			
			var result = await BaseHttpService.Instance.SendRequestAsync2(DtFrom.ToString("yyyyMMdd"));
			Dictionary<string, double>  pas =  ParsingPowerData(result.ToString());

			foreach (var item in pas)
			{
				lstPower.Add(new PowerModel
				{
					PwrTime = item.Key,
					PwrValue = item.Value

				});

				
				var obj = new DateTime(Convert.ToInt32(DtFrom.ToString("yyyy")), 
					Convert.ToInt32(DtFrom.ToString("MM")), 
					Convert.ToInt32(DtFrom.ToString("dd")), 
					Convert.ToInt32(item.Key.Substring(0,2)),
					Convert.ToInt32(item.Key.Substring(2, 2)),
					00);
				

				 //series1.Points.Add(new DataPoint(TimeSpanAxis.ToDouble(item.Key), item.Value));
				series1.Points.Add(new DataPoint(TimeSpanAxis.ToDouble(obj), item.Value));
			}
			model.Series.Add(series1);

			SearchResult.Clear();
			SearchResult.AddRange(lstPower, System.Collections.Specialized.NotifyCollectionChangedAction.Reset);


			// x축은 시간이 보이도록 설정합니다.
			model.Axes.Add(new DateTimeAxis
			{
				//Title = "일",
				Position = AxisPosition.Bottom
				//StringFormat = "dd" //화면에 보여질 단위를 정한다.
			});

			// Y 축은 값입니다.
			model.Axes.Add(new LinearAxis
			{
				// Title = "값",
				Position = AxisPosition.Left
			});

			PlotChartModel = model;
		}



		public Dictionary<string, double> ParsingPowerData(string sData)
		{
			Dictionary<string, double> dictPower = new Dictionary<string, double>();
			string[] words = sData.Split(',');
			string[] innerWords = null;
			double dTotalPower = 0.0;
			try
			{
				foreach (var word in words)
				{
					if (!word.Contains("pwr_qty"))
						continue;

					innerWords = word.Split(':');
					innerWords[0] = innerWords[0].Substring(8, 4);
					//innerWords[1] = innerWords[1].Replace('\"', ' ');
					innerWords[1] = innerWords[1].Trim('}');
					innerWords[1] = innerWords[1].Trim(']');
					innerWords[1] = innerWords[1].Trim('}');
					innerWords[1] = innerWords[1].Trim('\"');
					double dVal = innerWords[1].Contains("N/A") ? 0 : double.Parse(innerWords[1]);

					dTotalPower += dVal;
					dictPower.Add(innerWords[0], dVal);
					System.Console.WriteLine(string.Format("{0}=사용량 {1} [kWh]", innerWords[0], dVal));
				}
				System.Console.WriteLine(string.Format("총 사용량 {0}  [kWh]", dTotalPower));
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return dictPower;
		}

		public DateTime DtFrom
		{
			get => _dtFrom;
			set => SetProperty(ref this._dtFrom, value);
		}

		public ObservableRangeCollection<PowerModel> SearchResult
		{
			get => _searchResult;
			set => SetProperty(ref this._searchResult, value);
		}

		public PlotModel PlotChartModel
		{
			get => _plotChartModel;
			set => SetProperty(ref this._plotChartModel, value);
		}
	}
}
