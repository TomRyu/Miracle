using Miracle.Helpers;
using Miracle.Models;
using Miracle.Services;
using MySqlConnector;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Miracle.ViewModels
{
    public class ProductionViewModel : ViewModelBase
    {
        public ICommand SearchCommand { get; private set; }

        private ObservableRangeCollection<FactoryModel> _factoryList = new ObservableRangeCollection<FactoryModel>();
        private ObservableRangeCollection<ProductionModel> _searchResult = new ObservableRangeCollection<ProductionModel>();

        private FactoryModel _selectedFactory = new FactoryModel();
        private PlotModel _plotChartModel = new PlotModel();

        private DateTime _dtFrom = Convert.ToDateTime("1900-01-01");
        private DateTime _dtTo = Convert.ToDateTime("1900-01-01");


        public ProductionViewModel()
        {

            SearchCommand = new Command(Search);

            FactoryList.Add(new FactoryModel { Code = 0, Name = "전체" });
            FactoryList.Add(new FactoryModel { Code = 1, Name = "1공장" });
            FactoryList.Add(new FactoryModel { Code = 2, Name = "2공장" });

            SelectedFactory = FactoryList.Where(m => m.Code == FactoryList[0].Code).FirstOrDefault();

            DtFrom = DateTime.Now.AddDays(-3);
            DtTo = DateTime.Now;
        }
        private void Search()
        {

            //그래프 출력
            var model = new PlotModel
            {
                //Title = "OxyPlot 그래프 테스트",
                //Subtitle = "서브타이블",
                PlotType = PlotType.XY,
                Background = OxyColors.White
            };

            var series1 = new LineSeries { MarkerType = MarkerType.Circle, Title = "1공장" };
            var series2 = new LineSeries { MarkerType = MarkerType.Circle, Title = "2공장" };

            List<ProductionModel> lstProduction = new List<ProductionModel>();
            List<MySqlParameter> paraList = new List<MySqlParameter>();

            paraList.Add(new MySqlParameter("dtFrom", DtFrom.Date.ToString("yyyy-MM-dd")));
            paraList.Add(new MySqlParameter("dtTo", DtTo.Date.ToString("yyyy-MM-dd")));
            paraList.Add(new MySqlParameter("facNo", SelectedFactory.Code));

            DataTable dt = Services.MariaDB.Instance.GetDT_SP("SP_WorkResult_Day_Mobile_Query", paraList);

            if (dt == null || dt.Rows.Count == 0) return;

            foreach (DataRow row in dt.Rows)
            {
                lstProduction.Add(new ProductionModel
                {
                    ProductionDate = row.IsNull("생산일") ? "" : Convert.ToDateTime(row["생산일"]).ToString("yy-MM-dd"),
                    FactoryNo = Convert.ToInt32(row["공장번호"].ToString()),
                    ProductionQuantity = Convert.ToInt32(row["생산량"].ToString())

                });

                if (row["공장번호"].ToString() == "1")
                {
                    series1.Points.Add(new DataPoint(TimeSpanAxis.ToDouble(Convert.ToDateTime(row["생산일"])), Convert.ToInt32(row["생산량"])));
                }
                else if (row["공장번호"].ToString() == "2")
                {
                    series2.Points.Add(new DataPoint(TimeSpanAxis.ToDouble(Convert.ToDateTime(row["생산일"])), Convert.ToInt32(row["생산량"])));
                }
            }

            SearchResult.Clear();
            SearchResult.AddRange(lstProduction, System.Collections.Specialized.NotifyCollectionChangedAction.Reset);

            model.Series.Add(series1);
            model.Series.Add(series2);


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

        public ObservableRangeCollection<ProductionModel> SearchResult
        {
            get => _searchResult;
            set => SetProperty(ref this._searchResult, value);
        }

        public ObservableRangeCollection<FactoryModel> FactoryList
        {
            get => _factoryList;
            set => SetProperty(ref this._factoryList, value);
        }

        public FactoryModel SelectedFactory
        {
            get => _selectedFactory;
            set => SetProperty(ref this._selectedFactory, value);
        }

        public PlotModel PlotChartModel
        {
            get => _plotChartModel;
            set => SetProperty(ref this._plotChartModel, value);
        }

        public DateTime DtFrom
        {
            get => _dtFrom;
            set => SetProperty(ref this._dtFrom, value);
        }

        public DateTime DtTo
        {
            get => _dtTo;
            set => SetProperty(ref this._dtTo, value);
        }
    }
}
