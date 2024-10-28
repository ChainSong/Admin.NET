using Admin.NET.Common;
using Admin.NET.Entity;
using Admin.NET.Entity.ChartModel;
using Admin.NET.MAUI.AppApis;
using Bogus.DataSets;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Maui;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static SkiaSharp.HarfBuzz.SKShaper;
using static System.Net.WebRequestMethods;
namespace Admin.NET.MAUI;

public partial class HomePageViewModel(
        IProfileService profileService,
        INewsFeedService newsFeedService,
        IAppNavigator appNavigator, IAppSettingsService appSettingsService)
            : NavigationAwareBaseViewModel(appNavigator)
 , INotifyPropertyChanged
, INotifyCollectionChanged
{
    const int PAGE_SIZE = 10;
    private readonly IProfileService profileService = profileService;
    private readonly INewsFeedService newsFeedService = newsFeedService;

    private readonly IAppSettingsService appSettingsService = appSettingsService;

    private int currentPage;
    private bool hasMoreItems;

    [ObservableProperty]
    string coupleCoverUrl;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DaysSinceMet))]
    PartnerModel partner;

    public int DaysSinceMet { get => ((DateTime.Today - Partner?.FirstMet.Date)?.Days ?? 0) + 1; }

    [ObservableProperty]
    ObservableCollection<NewsFeedModel> newsFeeds;

    [ObservableProperty]
    bool isBusy;

    [ObservableProperty]
    bool isRefreshing;

    //[ObservableProperty]
    int _messageCount;

    public int messageCount
    {
        get => _messageCount; set
        {
            if (_messageCount != value)
            {
                _messageCount = value;
                //属性值发生变化时，执行OnPropertyChanged方法
                //如果不传参，则传入本属性。可以通过OnPropertyChanged("属性名")方式，传入指定属性
                OnPropertyChanged();
            }
        }
    }


    [ObservableProperty]
    HomeItemType itemType = HomeItemType.CoupleMilestoneExpanded;

    [ObservableProperty]
    HomeTab homeTab = HomeTab.OurStories;


    private bool _inbound;

    private bool _outbound;

    public bool inbound
    {
        get => _inbound; set
        {
            if (_inbound != value)
            {
                _inbound = value;
                //属性值发生变化时，执行OnPropertyChanged方法
                //如果不传参，则传入本属性。可以通过OnPropertyChanged("属性名")方式，传入指定属性
                OnPropertyChanged();
            }
        }
    }


    public bool outbound
    {
        get => _outbound; set
        {
            if (_outbound != value)
            {
                _outbound = value;
                //属性值发生变化时，执行OnPropertyChanged方法
                //如果不传参，则传入本属性。可以通过OnPropertyChanged("属性名")方式，传入指定属性
                OnPropertyChanged();
            }
        }
    }


    private List<ISeries> _StatusSeries;
    public List<ISeries> StatusSeries
    {
        get => _StatusSeries; set
        {
            if (_StatusSeries != value)
            {
                _StatusSeries = value;
                //属性值发生变化时，执行OnPropertyChanged方法
                //如果不传参，则传入本属性。可以通过OnPropertyChanged("属性名")方式，传入指定属性
                OnPropertyChanged();
            }
        }
    }


    public ObservableCollection<ISeries> _OrderSeries;
    public ObservableCollection<ISeries> OrderSeries
    {
        get => _OrderSeries; set
        {
            if (_OrderSeries != value)
            {
                _OrderSeries = value;
                //属性值发生变化时，执行OnPropertyChanged方法
                //如果不传参，则传入本属性。可以通过OnPropertyChanged("属性名")方式，传入指定属性
                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<Axis> _OrderYSeries;
    public ObservableCollection<Axis> OrderYSeries
    {
        get => _OrderYSeries; set
        {
            if (_OrderYSeries != value)
            {
                _OrderYSeries = value;
                //属性值发生变化时，执行OnPropertyChanged方法
                //如果不传参，则传入本属性。可以通过OnPropertyChanged("属性名")方式，传入指定属性
                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<Axis> _OrderXSeries;
    public ObservableCollection<Axis> OrderXSeries
    {
        get => _OrderXSeries; set
        {
            if (_OrderXSeries != value)
            {
                _OrderXSeries = value;
                //属性值发生变化时，执行OnPropertyChanged方法
                //如果不传参，则传入本属性。可以通过OnPropertyChanged("属性名")方式，传入指定属性
                OnPropertyChanged();
            }
        }
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(NoRelationship))]
    bool hasRelationship;

    public bool NoRelationship => !HasRelationship;

    public event PropertyChangedEventHandler PropertyChanged;
    public event NotifyCollectionChangedEventHandler CollectionChanged;


    public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        if (CollectionChanged != null)
        {
            CollectionChanged(this, e);
        }
    }

    protected override void OnInit(IDictionary<string, object> query)
    {
        base.OnInit(query);

        //CoupleCoverUrl = "https://www.notariesofeurope.eu/wp-content/uploads/2021/07/couple3_header_cnue_notaireeurope.jpg";

        //Partner = new PartnerModel
        //{
        //    Id = Guid.NewGuid(),
        //    PairingId = "896958",
        //    NickName = "Gà con",

        //    FirstName = "Elon",
        //    LastName = "Musk",

        //    FirstMet = DateTime.Today.AddDays(-255),
        //};
    }
    //获取未读消息数量
    public async Task GetMessageCount()
    {
        var result = await new AsyncNetworkHttpClient(appSettingsService, appNavigator).GetAsync<SysMessage>(SystemApi._messageCount);
        if (result != null && result.result != null)
        {
            messageCount = result.result.Count;
        }
        else
        {
            messageCount = 0;
        }
    }

    public override async Task OnAppearingAsync()
    {
        await base.OnAppearingAsync();
        HasRelationship = await profileService.CheckRelationshipAsync();
        await GetMessageCount();
        ChangeInTab();
        LoadDataAsync(true)
            .FireAndForget();

    }

    private async Task LoadDataAsync(bool forced)
    {

        if (IsBusy) return;
        IsBusy = true;

        currentPage = forced ? 0 : currentPage + 100;

        var items = await newsFeedService.GetNewsFeedsAsync(currentPage, PAGE_SIZE);

        IsBusy = false;

        hasMoreItems = items.Count() >= PAGE_SIZE;

        if (NewsFeeds == null || forced)
        {
            NewsFeeds = new ObservableCollection<NewsFeedModel>(items);
            return;
        }

        foreach (var item in items)
        {
            NewsFeeds.Add(item);
        }
    }

    [RelayCommand]
    private void LoadMore() => LoadDataAsync(false).FireAndForget();

    [RelayCommand]
    private void Refresh() => LoadDataAsync(true)
        .ContinueWith(x => IsRefreshing = false)
        .FireAndForget();

    [RelayCommand]
    private Task PairAsync() => AppNavigator.NavigateAsync(AppRoutes.Pair);

    [RelayCommand]
    private Task ViewNotificationsAsync() => AppNavigator.NavigateAsync(AppRoutes.Notifications);

    [RelayCommand]
    void ChangeMode()
    {


    }

    [RelayCommand]
    async void ChangeOutTab()
    {
        try
        {
            OrderSeries = new ObservableCollection<ISeries>();
            OrderXSeries = new ObservableCollection<Axis>();
            OrderYSeries = new ObservableCollection<Axis>();


            var resultNum = await new AsyncNetworkHttpClient(appSettingsService, appNavigator).GetAsync<BaseResponse<List<StatusChartModel>>>(ChartApi._PreOrderNumChart);
            var CreationTimes = resultNum.Result.DistinctBy(x => x.CreationTime).Select(a => a.CreationTime).ToList();
            int NumColor = 0;

            OrderXSeries.Add(new Axis
            {
                Labels = CreationTimes,
                LabelsRotation = -25,
                SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
                SeparatorsAtCenter = false,
                TicksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
                TicksAtCenter = true,
                // By default the axis tries to optimize the number of 
                // labels to fit the available space, 
                // when you need to force the axis to show all the labels then you must: 
                ForceStepToMin = true,
                MinStep = 1
            });
            foreach (var item in resultNum.Result.DistinctBy(x => x.Status).OrderBy(x => x.Status))
            {
                if (NumColor >= LiveChartsColor.s_colors.Length)
                {
                    NumColor = 0;
                }
                var resultValues = new List<double>();

                foreach (var b in CreationTimes)
                {
                    if (resultNum.Result.Where(a => a.Status == item.Status && a.CreationTime == b).Any())
                    {
                        resultValues.Add(resultNum.Result.Where(a => a.Status == item.Status && a.CreationTime == b).Sum(x => x.Count + 0.00));
                    }
                    else
                    {
                        resultValues.Add(0);
                    }
                }
                OrderSeries.Add(new StackedColumnSeries<double>
                {
                    IsHoverable = false, // disables the series from the tooltips 
                    Values = resultValues,
                    Stroke = null,
                    StackGroup = 0,
                    Fill = new SolidColorPaint(LiveChartsColor.s_colors[NumColor]),
                    Name = Enum.GetName(typeof(PreOrderStatusEnum), item.Status),
                    IgnoresBarPosition = true,
                    DataLabelsFormatter = (point) => { return Enum.GetName(typeof(PreOrderStatusEnum), item.Status); },
                    DataLabelsPaint = new SolidColorPaint(SKColors.Black)
                    {
                        SKTypeface = SKFontManager.Default.MatchCharacter('汉')
                    },
                });
                NumColor++;
            }
            if (resultNum.Result != null && resultNum.Result.Count > 0)
            {
                OrderYSeries.Add(
                       new Axis
                       {
                           MinLimit = 0,
                           MaxLimit = resultNum.Result.GroupBy(x => x.CreationTime).Select(a => a.Sum(x => x.Count + 0.00)).Max()
                       }
                );
            }
            inbound = false;
            outbound = true;
            var result = await new AsyncNetworkHttpClient(appSettingsService, appNavigator).GetAsync<BaseResponse<List<StatusChartModel>>>(ChartApi._PreOrderStatusChart);
            StatusSeries = new List<ISeries>();

            int i = 0;
            foreach (var item in result.Result)
            {
                if (i >= LiveChartsColor.s_colors.Length)
                {
                    i = 0;
                }
                StatusSeries.Add(new PieSeries<double>
                {
                    Values = new double[] { item.Count },
                    Name = "Outbound  Status",
                    //点上的文本
                    DataLabelsFormatter = (point) => { return Enum.GetName(typeof(PreOrderStatusEnum), item.Status); },
                    Fill = new LiveChartsCore.SkiaSharpView.Painting.RadialGradientPaint(LiveChartsColor.s_colors[i], LiveChartsColor.s_colors[i]),
                    DataLabelsSize = 12,
                    DataLabelsPaint = new SolidColorPaint(SKColors.Black)
                    {
                        SKTypeface = SKFontManager.Default.MatchCharacter('汉')
                    },
                });
                i++;
            }

        }
        catch (Exception ex)
        {

            throw;
        }
    }
    //[RelayCommand]
    //async void ViewNotifications() {
    //    messageCount = 99;
    //}

    [RelayCommand]
    async void ChangeInTab()
    {

        OrderSeries = new ObservableCollection<ISeries>();
        OrderXSeries = new ObservableCollection<Axis>();
        OrderYSeries = new ObservableCollection<Axis>();
        var resultNum = await new AsyncNetworkHttpClient(appSettingsService, appNavigator).GetAsync<BaseResponse<List<StatusChartModel>>>(ChartApi._ASNNumChart);
        List<string> CreationTimes = new List<string>();
        if (resultNum.Result != null)
        {
            CreationTimes = resultNum.Result.DistinctBy(x => x.CreationTime).Select(a => a.CreationTime).ToList();

        }
        int NumColor = 0;
        //foreach (var item in resultNum.Result.DistinctBy(x => x.CreationTime))
        //{
        OrderXSeries.Add(new Axis
        {
            Labels = CreationTimes,
            LabelsRotation = -30,
            SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
            SeparatorsAtCenter = false,
            TicksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
            TicksAtCenter = true,
            // By default the axis tries to optimize the number of 
            // labels to fit the available space, 
            // when you need to force the axis to show all the labels then you must: 
            ForceStepToMin = true,
            MinStep = 1
        });
        //}
        foreach (var item in resultNum.Result.DistinctBy(x => x.Status).OrderBy(x => x.Status))
        {
            if (NumColor >= LiveChartsColor.s_colors.Length)
            {
                NumColor = 0;
            }
            var resultValues = new List<double>();
            foreach (var b in CreationTimes)
            {
                if (resultNum.Result.Where(a => a.Status == item.Status && a.CreationTime == b).Any())
                {
                    resultValues.Add(resultNum.Result.Where(a => a.Status == item.Status && a.CreationTime == b).Sum(x => x.Count + 0.00));
                }
                else
                {
                    resultValues.Add(0);
                }
            }
            OrderSeries.Add(new StackedColumnSeries<double>
            {
                IsHoverable = false, // disables the series from the tooltips 
                Values = resultValues,
                Stroke = null,
                StackGroup = 0,
                Fill = new SolidColorPaint(LiveChartsColor.s_colors[NumColor]),
                Name = Enum.GetName(typeof(ASNStatusEnum), item.Status),
                IgnoresBarPosition = true,
                DataLabelsFormatter = (point) => { return Enum.GetName(typeof(ASNStatusEnum), item.Status); },
                DataLabelsPaint = new SolidColorPaint(SKColors.Black)
                {
                    SKTypeface = SKFontManager.Default.MatchCharacter('汉')
                },
            });
            NumColor++;
        }
        if (resultNum.Result.Count > 0)
        {
            OrderYSeries.Add(
                   new Axis { MinLimit = 0, MaxLimit = resultNum.Result.GroupBy(x => x.CreationTime).Select(a => a.Sum(x => x.Count + 0.00)).Max() }
            );
        }
        //OrderSeries.Add(new ColumnSeries<double>
        //{
        //    IsHoverable = false, // disables the series from the tooltips 
        //    Values = new double[] { 10, 10, 10, 10, 10, 10, 10 },
        //    Stroke = null,
        //    Fill = new SolidColorPaint(new SKColor(30, 30, 30, 30)),
        //    IgnoresBarPosition = true
        //});
        //OrderSeries.Add(new ColumnSeries<double>
        //{
        //    Values = new double[] { 3, 10, 5, 3, 7, 3, 8 },
        //    Stroke = null,
        //    Fill = new SolidColorPaint(SKColors.CornflowerBlue),
        //    IgnoresBarPosition = true
        //});




        inbound = true;
        outbound = false;
        //Random random = new Random();
        //// 生成一个从0到100的随机整数
        //int randomNumber = random.Next(0, 101);
        //var result = await new AsyncNetworkHttpClient(appSettingsService, appNavigator).GetAsync<SysMessage>(SystemApi._messageCount);

        var result = await new AsyncNetworkHttpClient(appSettingsService, appNavigator).GetAsync<BaseResponse<List<StatusChartModel>>>(ChartApi._ASNStatusChart);
        StatusSeries = new List<ISeries>();
        int i = 0;
        foreach (var item in result.Result)
        {
            if (i >= LiveChartsColor.s_colors.Length)
            {
                i = 0;
            }
            StatusSeries.Add(new PieSeries<double>
            {
                Values = new double[] { item.Count },
                Name = "Inbound  Status",
                //点上的文本
                DataLabelsFormatter = (point) => { return Enum.GetName(typeof(ASNStatusEnum), item.Status); },
                //DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top,
                DataLabelsSize = 12,
                Fill = new LiveChartsCore.SkiaSharpView.Painting.RadialGradientPaint(LiveChartsColor.s_colors[i], LiveChartsColor.s_colors[i]),
                DataLabelsPaint = new SolidColorPaint(SKColors.Black)
                {
                    SKTypeface = SKFontManager.Default.MatchCharacter('汉')
                },
                //IsHoverable = false
            });
            i++;
        }
        //XAxes = ([new Axis { Labels = new string[] { "A" , "B" , "C"  } }]);
        //Series = new ISeries[] { new ColumnSeries<double> { Values = new double[] { 1, 1, 1 }, Stroke = null, Fill = new SolidColorPaint(SKColors.CornflowerBlue), IgnoresBarPosition = true } };
        //}
        //ASNStatusSeries.Add(new PieSeries<double> { Values = new double[] { 4 } });
        //ASNStatusSeries.Add(new PieSeries<double> { Values = new double[] { 1 } });

        //XAxes = ([new Axis { Labels = new string[] { "D" , "E" , "F"  } }]);
        //Series = new ISeries[] { new ColumnSeries<double> { Values = new double[] { 5, 5, 5 }, Stroke = null, Fill = new SolidColorPaint(SKColors.CornflowerBlue), IgnoresBarPosition = true } };


    }
    [RelayCommand]
    void CreateStory()
    {

    }
}

