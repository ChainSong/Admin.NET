using Admin.NET.MAUI2C.AppApis;

namespace Admin.NET.MAUI2C;
public partial class NotificationsPageViewModel(
        INotificationService notificationService, IAppSettingsService appSettingsService,
        IAppNavigator appNavigator) : NavigationAwareBaseViewModel(appNavigator)
{
    private readonly INotificationService notificationService = notificationService;

    private const int PAGE_SIZE = 20;
    private int currentPage;
    private bool hasMoreItems;

    //[ObservableProperty]
    //ObservableCollection<Notifications> notifications;



    public ObservableCollection<Notifications> _Notifications;
    public ObservableCollection<Notifications> Notifications
    {
        get => _Notifications; set
        {
            if (_Notifications != value)
            {
                _Notifications = value;
                //属性值发生变化时，执行OnPropertyChanged方法
                //如果不传参，则传入本属性。可以通过OnPropertyChanged("属性名")方式，传入指定属性
                OnPropertyChanged();
            }
        }
    }

    [ObservableProperty]
    bool isBusy;

    [ObservableProperty]
    bool isRefreshing;

    public override async Task OnAppearingAsync()
    {
        await base.OnAppearingAsync();

        LoadDataAsync(true).FireAndForget();
        //await LoadDataAsync(true);
    }

    private async Task LoadDataAsync(bool forced)
    {
        Notifications = new ObservableCollection<Notifications>();
        //if (!IsBusy) return;
        IsBusy = true;

        currentPage = forced ? 0 : currentPage + 1;

        var items = await new AsyncNetworkHttpClient(appSettingsService, appNavigator).GetAsync<SysMessage>(SystemApi._messageCount);
        //if (result != null && result.result != null)
        //{
        //    messageCount = result.result.Count;
        //}
        //else
        //{
        //    messageCount = 0;
        //}

        //var items = await notificationService.GetNotificationsAsync(currentPage, PAGE_SIZE);

        IsBusy = false;

        hasMoreItems = items.result.Count() >= PAGE_SIZE;

        if (items.result == null)
        {
            Notifications = new ObservableCollection<Notifications>();
            return;
        }

        if (forced)
        {
            Notifications.Clear();
        }

        foreach (var item in items.result)
        {
            Notifications.Add(item);
        }
    }

    //[RelayCommand]
    //private void LoadMore() => LoadDataAsync(false).FireAndForget();

    //[RelayCommand]
    //private void Refresh() => LoadDataAsync(true)
    //    .ContinueWith(x => IsRefreshing = false)
    //    .FireAndForget();
}

