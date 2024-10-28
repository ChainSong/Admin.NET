using Admin.NET.Common;
using Admin.NET.Entity;
using Admin.NET.Entity.ChartModel;
using Bogus.DataSets;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static System.Net.WebRequestMethods;
namespace Admin.NET.MAUI2C;

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
        //var result = await new AsyncNetworkHttpClient(appSettingsService, appNavigator).GetAsync<SysMessage>(SystemApi._messageCount);
        //if (result != null && result.result != null)
        //{
        //    messageCount = result.result.Count;
        //}
        //else
        //{
        //    messageCount = 0;
        //}
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
       
    }
    //[RelayCommand]
    //async void ViewNotifications() {
    //    messageCount = 99;
    //}

    [RelayCommand]
    async void ChangeInTab()
    {

         

    }
    [RelayCommand]
    void CreateStory()
    {

    }
}

