using LiveChartsCore.SkiaSharpView;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Admin.NET.MAUI;

public partial class ProfilePageViewModel(IAppNavigator appNavigator, IAppSettingsService appSettings) :
    NavigationAwareBaseViewModel(appNavigator)
{

    [ObservableProperty]
    string phoneNumber;



    [RelayCommand]
    private Task ViewSettings() => AppNavigator.NavigateAsync(AppRoutes.SettingsAndHelp);


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
        //LoadContacts();
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
    public override async Task OnAppearingAsync()
    {
        await base.OnAppearingAsync();
        //LoadContacts();
    }

    //public ObservableCollection<IndividualCenter> _Contacts;
    //public ObservableCollection<IndividualCenter> Contacts
    //{
    //    get => _Contacts; set
    //    {
    //        if (_Contacts != value)
    //        {
    //            _Contacts = value;
    //            //属性值发生变化时，执行OnPropertyChanged方法
    //            //如果不传参，则传入本属性。可以通过OnPropertyChanged("属性名")方式，传入指定属性
    //            OnPropertyChanged();
    //        }
    //    }
    //}
    //public void LoadContacts()
    //{
    //    Contacts=new ObservableCollection<IndividualCenter>();
    //    // 模拟添加一些联系人数据
    //    Contacts.Add(new IndividualCenter { Name = "退出", PhoneNumber = "123 - 456 - 7890" });
    //    Contacts.Add(new IndividualCenter { Name = "Jane Smith", PhoneNumber = "987 - 654 - 3210" });
    //}


    [RelayCommand]
    private Task SignOut()
    {
        appSettings.Clear();
        return AppNavigator.NavigateAsync(AppRoutes.SignIn);
    }

}

