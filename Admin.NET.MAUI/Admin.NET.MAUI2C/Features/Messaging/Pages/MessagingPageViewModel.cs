using Admin.NET.Entity;

using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Web;

namespace Admin.NET.MAUI2C;

public partial class MessagingPageViewModel(IAppNavigator appNavigator, IAppSettingsService appSettingsService)
: NavigationAwareBaseViewModel(appNavigator), INotifyPropertyChanged
{
    public IAppNavigator _appNavigator = appNavigator;
    public IAppSettingsService _appSettingsService = appSettingsService;
    private List<string> strings = new List<string>();
    public event PropertyChangedEventHandler PropertyChanged;
    private readonly IAppSettingsService appSettingsService = appSettingsService;
    //[ObservableProperty]
    //ObservableCollection<Message> notifications;
    //private ObservableCollection<MessageModel> messages;

    //public ObservableCollection<MessageModel> Messages { get => messages; set { messages = value; OnPropertyChanged(nameof(Messages)); } }

    private string _Msg { get; set; }
    public string Msg { get => _Msg; set { _Msg = value; OnPropertyChanged(nameof(Msg)); } }

    [ObservableProperty]
    string phoneNumber;

        [ObservableProperty]
     string chatList; // 这里的名称应该与XAML中定义的x:Name一致

    //[RelayCommand]
    //private void GetOTP()
    //{
    //    //Messages = new ObservableCollection<MessageModel>
    //    //{  
    //    //    // 初始消息  
    //    //    new MessageModel {   Mag = "Hello!", Sender = "Alice", BackgroundColor = Colors.LightGray }
    //    //};
    //}

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


    protected override void OnInit(IDictionary<string, object> query)
    {
      
        //Messages = new ObservableCollection<MessageModel>
        //{  
        //    // 初始消息  
        //    new MessageModel {   Msg = "Hello!", Sender = "Alice", BackgroundColor = Colors.White }
        //};
    }
    //[RelayCommand]
    //private async void Pair()
    //{
     
    //    //    // 发送消息逻辑  
    //    Messages.Add(new MessageModel
    //    {
    //        Msg = Msg,
    //        Sender = "我",
    //        BackgroundColor = Colors.White,
    //        IsString = true
    //    });
    //    strings = new List<string>();
    //    //strings.Add("Customer是客户表:CustomerCode的含义是客户代码。请帮我查询客户表中的客户代码");
    //    //strings.Add("Customer是客户表:CustomerName的含义是客户名称。请帮我查询客户表中的客户名称");
    //    //strings.Add("Customer是客户表:Description的含义是描述。请帮我查询客户表中的描述");
    //    //strings.Add("Customer是客户表:CustomerType的含义是客户类型。请帮我查询客户表中的客户类型");
    //    //strings.Add("Customer是客户表:CustomerStatus的含义是客户状态。请帮我查询客户表中的客户状态");
    //    //strings.Add("Customer是客户表:CreditLine的含义是信用额度。请帮我查询客户表中的信用额度");
    //    //strings.Add("Customer是客户表:Province的含义是省份。请帮我查询客户表中的省份");
    //    //strings.Add("Customer是客户表:City的含义是城市。请帮我查询客户表中的城市");
    //    //strings.Add("Customer是客户表:Address的含义是地址。请帮我查询客户表中的地址");
    //    //strings.Add("Customer是客户表:Remark的含义是备注。请帮我查询客户表中的备注");
    //    //strings.Add("Customer是客户表:Email的含义是邮箱。请帮我查询客户表中的邮箱");
    //    //strings.Add("Customer是客户表:Phone的含义是电话。请帮我查询客户表中的电话");
    //    //strings.Add("Customer是客户表:LawPerson的含义是法人代表。请帮我查询客户表中的法人代表");
    //    //strings.Add("Customer是客户表:PostCode的含义是邮政编码。请帮我查询客户表中的邮政编码");
    //    //strings.Add("Customer是客户表:Bank的含义是开户银行。请帮我查询客户表中的开户银行");
    //    //strings.Add("Customer是客户表:Account的含义是帐号。请帮我查询客户表中的帐号");
    //    //strings.Add("Customer是客户表:TaxId的含义是税号。请帮我查询客户表中的税号");
    //    //strings.Add("Customer是客户表:InvoiceTitle的含义是发票抬头。请帮我查询客户表中的发票抬头");
    //    //strings.Add("Customer是客户表:Fax的含义是传真。请帮我查询客户表中的传真");
    //    //strings.Add("Customer是客户表:WebSite的含义是网址。请帮我查询客户表中的网址");
    //    //strings.Add("Customer是客户表:CreateTime的含义是创建时间。请帮我查询客户表中的创建时间");
    //    //strings.Add("CustomerDetail是客户明细表:Contact的含义是联系人。请帮我查询客户明细表中的联系人");
    //    //strings.Add("CustomerDetail是客户明细表:Tel的含义是联系方式。请帮我查询客户明细表中的联系方式");
    //    //strings.Add("WMS_Adjustment是调整表:ExternNumber的含义是外部单号。请帮我查询调整表中的外部单号");
    //    //strings.Add("WMS_Adjustment是调整表:CustomerName的含义是客户名称。请帮我查询调整表中的客户名称");
    //    //strings.Add("WMS_Adjustment是调整表:WarehouseName的含义是仓库名称。请帮我查询调整表中的仓库名称");
    //    //strings.Add("WMS_Adjustment是调整表:AdjustmentStatus的含义是调整状态。请帮我查询调整表中的调整状态");
    //    //strings.Add("WMS_Adjustment是调整表:AdjustmentType的含义是调整类型。请帮我查询调整表中的调整类型");
    //    //strings.Add("WMS_Adjustment是调整表:AdjustmentReason的含义是调整原因。请帮我查询调整表中的调整原因");
    //    //strings.Add("WMS_Adjustment是调整表:AdjustmentTime的含义是调整时间。请帮我查询调整表中的调整时间");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:FromOnwer的含义是从所属。请帮我查询调整明细表中的从所属");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:ToOnwer的含义是至所属。请帮我查询调整明细表中的至所属");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:SKU的含义是SKU。请帮我查询调整明细表中的SKU");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:UPC的含义是UPC。请帮我查询调整明细表中的UPC");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:TrayCode的含义是唯一编号。请帮我查询调整明细表中的唯一编号");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:BatchCode的含义是批次号。请帮我查询调整明细表中的批次号");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:BoxCode的含义是箱号。请帮我查询调整明细表中的箱号");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:GoodsName的含义是品名。请帮我查询调整明细表中的品名");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:FromWarehouseName的含义是从仓库。请帮我查询调整明细表中的从仓库");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:ToWarehouseName的含义是至仓库。请帮我查询调整明细表中的至仓库");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:FromArea的含义是从库区。请帮我查询调整明细表中的从库区");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:ToArea的含义是至库区。请帮我查询调整明细表中的至库区");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:FromLocation的含义是从库位。请帮我查询调整明细表中的从库位");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:ToLocation的含义是至库位。请帮我查询调整明细表中的至库位");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:Qty的含义是数量。请帮我查询调整明细表中的数量");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:FromGoodsType的含义是从品级。请帮我查询调整明细表中的从品级");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:ToGoodsType的含义是至品级。请帮我查询调整明细表中的至品级");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:FromUnitCode的含义是从单位。请帮我查询调整明细表中的从单位");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:ToUnitCode的含义是至单位。请帮我查询调整明细表中的至单位");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:AdjustmentReason的含义是调整原因。请帮我查询调整明细表中的调整原因");
    //    //strings.Add("WMS_Area是库区表:WarehouseName的含义是仓库名称。请帮我查询库区表中的仓库名称");
    //    //strings.Add("WMS_Area是库区表:AreaName的含义是库区。请帮我查询库区表中的库区");
    //    //strings.Add("WMS_Area是库区表:AreaStatus的含义是状态。请帮我查询库区表中的状态");
    //    //strings.Add("WMS_Area是库区表:AreaType的含义是类型。请帮我查询库区表中的类型");
    //    //strings.Add("WMS_ASN是预入库:ExternReceiptNumber的含义是外部单号。请帮我查询预入库中的外部单号");
    //    //strings.Add("WMS_ASN是预入库:CustomerName的含义是客户名称。请帮我查询预入库中的客户名称");
    //    //strings.Add("WMS_ASN是预入库:WarehouseName的含义是仓库名称。请帮我查询预入库中的仓库名称");
    //    //strings.Add("WMS_ASN是预入库:ExpectDate的含义是订单时间。请帮我查询预入库中的订单时间");
    //    //strings.Add("WMS_ASN是预入库:ReceiptType的含义是订单类型。请帮我查询预入库中的订单类型");
    //    //strings.Add("WMS_ASN是预入库:Contact的含义是联系人。请帮我查询预入库中的联系人");
    //    //strings.Add("WMS_ASN是预入库:ContactInfo的含义是联系人信息。请帮我查询预入库中的联系人信息");
    //    //strings.Add("WMS_ASNDetail是预入库明细:SKU的含义是SKU。请帮我查询预入库明细中的SKU");
    //    //strings.Add("WMS_ASNDetail是预入库明细:GoodsType的含义是产品等级。请帮我查询预入库明细中的产品等级");
    //    //strings.Add("WMS_ASNDetail是预入库明细:GoodsName的含义是产品名称。请帮我查询预入库明细中的产品名称");
    //    //strings.Add("WMS_ASNDetail是预入库明细:BoxCode的含义是箱号。请帮我查询预入库明细中的箱号");
    //    //strings.Add("WMS_ASNDetail是预入库明细:TrayCode的含义是托盘号。请帮我查询预入库明细中的托盘号");
    //    //strings.Add("WMS_ASNDetail是预入库明细:BatchCode的含义是批次号。请帮我查询预入库明细中的批次号");
    //    //strings.Add("WMS_ASNDetail是预入库明细:PoCode的含义是Po。请帮我查询预入库明细中的Po");
    //    //strings.Add("WMS_ASNDetail是预入库明细:ExpectedQty的含义是期望数量。请帮我查询预入库明细中的期望数量");
    //    //strings.Add("WMS_ASNDetail是预入库明细:ProductionDate的含义是生产日期。请帮我查询预入库明细中的生产日期");
    //    //strings.Add("WMS_ASNDetail是预入库明细:ExpirationDate的含义是过期日期。请帮我查询预入库明细中的过期日期");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:CustomerName的含义是客户名称。请帮我查询可用库存表中的客户名称");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:WarehouseName的含义是仓库名称。请帮我查询可用库存表中的仓库名称");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:Area的含义是库区。请帮我查询可用库存表中的库区");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:Location的含义是库位。请帮我查询可用库存表中的库位");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:SKU的含义是SKU。请帮我查询可用库存表中的SKU");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:UPC的含义是UPC。请帮我查询可用库存表中的UPC");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:GoodsType的含义是产品类型。请帮我查询可用库存表中的产品类型");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:InventoryStatus的含义是库存状态。请帮我查询可用库存表中的库存状态");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:GoodsName的含义是产品名称。请帮我查询可用库存表中的产品名称");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:UnitCode的含义是单位。请帮我查询可用库存表中的单位");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:Onwer的含义是所属。请帮我查询可用库存表中的所属");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:BoxCode的含义是箱号。请帮我查询可用库存表中的箱号");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:TrayCode的含义是托号。请帮我查询可用库存表中的托号");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:BatchCode的含义是批次号。请帮我查询可用库存表中的批次号");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:Qty的含义是数量。请帮我查询可用库存表中的数量");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:ProductionDate的含义是生产日期。请帮我查询可用库存表中的生产日期");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:ExpirationDate的含义是过期日期。请帮我查询可用库存表中的过期日期");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:Remark的含义是备注。请帮我查询可用库存表中的备注");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:InventoryTime的含义是入库时间。请帮我查询可用库存表中的入库时间");
    //    //strings.Add("WMS_Location是库位表:WarehouseName的含义是仓库。请帮我查询库位表中的仓库");
    //    //strings.Add("WMS_Location是库位表:AreaName的含义是库区。请帮我查询库位表中的库区");
    //    //strings.Add("WMS_Location是库位表:Location的含义是库位。请帮我查询库位表中的库位");
    //    //strings.Add("WMS_Location是库位表:LocationStatus的含义是库位状态。请帮我查询库位表中的库位状态");
    //    //strings.Add("WMS_Location是库位表:LocationType的含义是库位类型。请帮我查询库位表中的库位类型");
    //    //strings.Add("WMS_Order是出库表:PreOrderNumber的含义是预出库单号。请帮我查询出库表中的预出库单号");
    //    //strings.Add("WMS_Order是出库表:ExternOrderNumber的含义是外部单号。请帮我查询出库表中的外部单号");
    //    //strings.Add("WMS_Order是出库表:CustomerName的含义是客户。请帮我查询出库表中的客户");
    //    //strings.Add("WMS_Order是出库表:WarehouseName的含义是仓库。请帮我查询出库表中的仓库");
    //    //strings.Add("WMS_Order是出库表:OrderType的含义是订单类型。请帮我查询出库表中的订单类型");
    //    //strings.Add("WMS_Order是出库表:OrderStatus的含义是订单状态。请帮我查询出库表中的订单状态");
    //    //strings.Add("WMS_Order是出库表:OrderTime的含义是订单时间。请帮我查询出库表中的订单时间");
    //    //strings.Add("WMS_Order是出库表:Creator的含义是创建人。请帮我查询出库表中的创建人");
    //    //strings.Add("WMS_Order是出库表:CreationTime的含义是创建时间。请帮我查询出库表中的创建时间");
    //    //strings.Add("WMS_Order是出库表:Remark的含义是备注。请帮我查询出库表中的备注");
    //    //strings.Add("WMS_OrderAddress是出货地址:Name的含义是联系人。请帮我查询出货地址中的联系人");
    //    //strings.Add("WMS_OrderAddress是出货地址:Phone的含义是联系电话。请帮我查询出货地址中的联系电话");
    //    //strings.Add("WMS_OrderAddress是出货地址:ZipCode的含义是邮政编码。请帮我查询出货地址中的邮政编码");
    //    //strings.Add("WMS_OrderAddress是出货地址:Province的含义是省份。请帮我查询出货地址中的省份");
    //    //strings.Add("WMS_OrderAddress是出货地址:City的含义是城市。请帮我查询出货地址中的城市");
    //    //strings.Add("WMS_OrderAddress是出货地址:County的含义是地区。请帮我查询出货地址中的地区");
    //    //strings.Add("WMS_OrderAddress是出货地址:Address的含义是地址。请帮我查询出货地址中的地址");
    //    //strings.Add("WMS_OrderAddress是出货地址:ExpressCompany的含义是快递公司。请帮我查询出货地址中的快递公司");
    //    //strings.Add("WMS_OrderAddress是出货地址:ExpressNumber的含义是快递单号。请帮我查询出货地址中的快递单号");
    //    //strings.Add("WMS_OrderAllocation是分配表:Area的含义是库区。请帮我查询分配表中的库区");
    //    //strings.Add("WMS_OrderAllocation是分配表:Location的含义是库位。请帮我查询分配表中的库位");
    //    //strings.Add("WMS_OrderAllocation是分配表:SKU的含义是SKU。请帮我查询分配表中的SKU");
    //    //strings.Add("WMS_OrderAllocation是分配表:UPC的含义是UPC。请帮我查询分配表中的UPC");
    //    //strings.Add("WMS_OrderAllocation是分配表:GoodsType的含义是产品类型。请帮我查询分配表中的产品类型");
    //    //strings.Add("WMS_OrderAllocation是分配表:GoodsName的含义是产品名称。请帮我查询分配表中的产品名称");
    //    //strings.Add("WMS_OrderAllocation是分配表:UnitCode的含义是单位。请帮我查询分配表中的单位");
    //    //strings.Add("WMS_OrderAllocation是分配表:Onwer的含义是所属。请帮我查询分配表中的所属");
    //    //strings.Add("WMS_OrderAllocation是分配表:BoxCode的含义是箱号。请帮我查询分配表中的箱号");
    //    //strings.Add("WMS_OrderAllocation是分配表:TrayCode的含义是托号。请帮我查询分配表中的托号");
    //    //strings.Add("WMS_OrderAllocation是分配表:BatchCode的含义是批次号。请帮我查询分配表中的批次号");
    //    //strings.Add("WMS_OrderAllocation是分配表:Qty的含义是数量。请帮我查询分配表中的数量");
    //    //strings.Add("WMS_OrderAllocation是分配表:ProductionDate的含义是生产日期。请帮我查询分配表中的生产日期");
    //    //strings.Add("WMS_OrderAllocation是分配表:ExpirationDate的含义是过期日期。请帮我查询分配表中的过期日期");
    //    //strings.Add("WMS_OrderAllocation是分配表:Remark的含义是备注。请帮我查询分配表中的备注");
    //    //strings.Add("WMS_OrderAllocation是分配表:Creator的含义是创建人。请帮我查询分配表中的创建人");
    //    //strings.Add("WMS_OrderAllocation是分配表:CreationTime的含义是创建时间。请帮我查询分配表中的创建时间");
    //    //strings.Add("WMS_OrderDetail是出库明细表:LineNumber的含义是行号。请帮我查询出库明细表中的行号");
    //    //strings.Add("WMS_OrderDetail是出库明细表:SKU的含义是SKU。请帮我查询出库明细表中的SKU");
    //    //strings.Add("WMS_OrderDetail是出库明细表:UPC的含义是UPC。请帮我查询出库明细表中的UPC");
    //    //strings.Add("WMS_OrderDetail是出库明细表:GoodsName的含义是产品名称。请帮我查询出库明细表中的产品名称");
    //    //strings.Add("WMS_OrderDetail是出库明细表:GoodsType的含义是产品类型。请帮我查询出库明细表中的产品类型");
    //    //strings.Add("WMS_OrderDetail是出库明细表:OrderQty的含义是出库数量。请帮我查询出库明细表中的出库数量");
    //    //strings.Add("WMS_OrderDetail是出库明细表:AllocatedQty的含义是分配数量。请帮我查询出库明细表中的分配数量");
    //    //strings.Add("WMS_OrderDetail是出库明细表:BoxCode的含义是箱号。请帮我查询出库明细表中的箱号");
    //    //strings.Add("WMS_OrderDetail是出库明细表:TrayCode的含义是托号。请帮我查询出库明细表中的托号");
    //    //strings.Add("WMS_OrderDetail是出库明细表:BatchCode的含义是批次号。请帮我查询出库明细表中的批次号");
    //    //strings.Add("WMS_OrderDetail是出库明细表:UnitCode的含义是单位。请帮我查询出库明细表中的单位");
    //    //strings.Add("WMS_OrderDetail是出库明细表:Onwer的含义是所属。请帮我查询出库明细表中的所属");
    //    //strings.Add("WMS_PickTask是拣货任务表:CustomerName的含义是客户名称。请帮我查询拣货任务表中的客户名称");
    //    //strings.Add("WMS_PickTask是拣货任务表:WarehouseName的含义是仓库名称。请帮我查询拣货任务表中的仓库名称");
    //    //strings.Add("WMS_PickTask是拣货任务表:PickTaskNumber的含义是拣货任务号。请帮我查询拣货任务表中的拣货任务号");
    //    //strings.Add("WMS_PickTask是拣货任务表:PickStatus的含义是拣货状态。请帮我查询拣货任务表中的拣货状态");
    //    //strings.Add("WMS_PickTask是拣货任务表:PickType的含义是拣货类型。请帮我查询拣货任务表中的拣货类型");
    //    //strings.Add("WMS_PickTask是拣货任务表:StartTime的含义是开始时间。请帮我查询拣货任务表中的开始时间");
    //    //strings.Add("WMS_PickTask是拣货任务表:EndTime的含义是结束时间。请帮我查询拣货任务表中的结束时间");
    //    //strings.Add("WMS_PickTask是拣货任务表:PrintNum的含义是打印次数。请帮我查询拣货任务表中的打印次数");
    //    //strings.Add("WMS_PickTask是拣货任务表:PrintTime的含义是打印时间。请帮我查询拣货任务表中的打印时间");
    //    //strings.Add("WMS_PickTask是拣货任务表:PrintPersonnel的含义是打印人。请帮我查询拣货任务表中的打印人");
    //    //strings.Add("WMS_PickTask是拣货任务表:PickPlanPersonnel的含义是计划拣货人。请帮我查询拣货任务表中的计划拣货人");
    //    //strings.Add("WMS_PickTask是拣货任务表:DetailQty的含义是明细数量。请帮我查询拣货任务表中的明细数量");
    //    //strings.Add("WMS_PickTask是拣货任务表:DetailKindsQty的含义是明细种类。请帮我查询拣货任务表中的明细种类");
    //    //strings.Add("WMS_PickTask是拣货任务表:PickContainer的含义是拣货容器。请帮我查询拣货任务表中的拣货容器");
    //    //strings.Add("WMS_PickTask是拣货任务表:Creator的含义是创建人。请帮我查询拣货任务表中的创建人");
    //    //strings.Add("WMS_PickTask是拣货任务表:CreationTime的含义是创建时间。请帮我查询拣货任务表中的创建时间");
    //    //strings.Add("WMS_PickTask是拣货任务表:Updator的含义是修改人。请帮我查询拣货任务表中的修改人");
    //    //strings.Add("WMS_PickTask是拣货任务表:UpdateTime的含义是修改时间。请帮我查询拣货任务表中的修改时间");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:PickStatus的含义是拣货状态。请帮我查询拣货任务明细表中的拣货状态");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:PikcTime的含义是拣货时间。请帮我查询拣货任务明细表中的拣货时间");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:PickBoxNumber的含义是拣货箱号。请帮我查询拣货任务明细表中的拣货箱号");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:PickQty的含义是拣货数量。请帮我查询拣货任务明细表中的拣货数量");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:Area的含义是库区。请帮我查询拣货任务明细表中的库区");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:Location的含义是库位。请帮我查询拣货任务明细表中的库位");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:SKU的含义是SKU。请帮我查询拣货任务明细表中的SKU");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:UPC的含义是UPC。请帮我查询拣货任务明细表中的UPC");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:GoodsType的含义是品级。请帮我查询拣货任务明细表中的品级");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:GoodsName的含义是品名。请帮我查询拣货任务明细表中的品名");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:UnitCode的含义是单位。请帮我查询拣货任务明细表中的单位");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:Onwer的含义是所属。请帮我查询拣货任务明细表中的所属");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:BoxCode的含义是箱号。请帮我查询拣货任务明细表中的箱号");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:TrayCode的含义是托号。请帮我查询拣货任务明细表中的托号");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:BatchCode的含义是批次号。请帮我查询拣货任务明细表中的批次号");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:Qty的含义是数量。请帮我查询拣货任务明细表中的数量");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:ProductionDate的含义是生产日期。请帮我查询拣货任务明细表中的生产日期");
    //    //strings.Add("WMS_PreOrder是预出库表:ExternOrderNumber的含义是外部单号。请帮我查询预出库表中的外部单号");
    //    //strings.Add("WMS_PreOrder是预出库表:CustomerName的含义是客户名称。请帮我查询预出库表中的客户名称");
    //    //strings.Add("WMS_PreOrder是预出库表:WarehouseName的含义是仓库名称。请帮我查询预出库表中的仓库名称");
    //    //strings.Add("WMS_PreOrder是预出库表:OrderType的含义是订单类型。请帮我查询预出库表中的订单类型");
    //    //strings.Add("WMS_PreOrder是预出库表:OrderTime的含义是订单时间。请帮我查询预出库表中的订单时间");
    //    //strings.Add("WMS_PreOrderDetail是预出库明细表:SKU的含义是SKU。请帮我查询预出库明细表中的SKU");
    //    //strings.Add("WMS_PreOrderDetail是预出库明细表:UPC的含义是UPC。请帮我查询预出库明细表中的UPC");
    //    //strings.Add("WMS_PreOrderDetail是预出库明细表:GoodsName的含义是产品名称。请帮我查询预出库明细表中的产品名称");
    //    //strings.Add("WMS_PreOrderDetail是预出库明细表:GoodsType的含义是产品等级。请帮我查询预出库明细表中的产品等级");
    //    //strings.Add("WMS_PreOrderDetail是预出库明细表:OrderQty的含义是出库数量。请帮我查询预出库明细表中的出库数量");
    //    //strings.Add("WMS_PreOrderDetail是预出库明细表:BoxCode的含义是箱号。请帮我查询预出库明细表中的箱号");
    //    //strings.Add("WMS_PreOrderDetail是预出库明细表:TrayCode的含义是托号。请帮我查询预出库明细表中的托号");
    //    //strings.Add("WMS_PreOrderDetail是预出库明细表:BatchCode的含义是批次号。请帮我查询预出库明细表中的批次号");
    //    //strings.Add("WMS_PreOrderDetail是预出库明细表:UnitCode的含义是单位。请帮我查询预出库明细表中的单位");
    //    //strings.Add("WMS_PreOrderDetail是预出库明细表:Onwer的含义是所属。请帮我查询预出库明细表中的所属");
    //    //strings.Add("WMS_PreOrderDetail是预出库明细表:ProductionDate的含义是生产日期。请帮我查询预出库明细表中的生产日期");
    //    //strings.Add("WMS_PreOrderDetail是预出库明细表:ExpirationDate的含义是过期日期。请帮我查询预出库明细表中的过期日期");
    //    //strings.Add("WMS_Product是产品表:CustomerName的含义是客户名称。请帮我查询产品表中的客户名称");
    //    //strings.Add("WMS_Product是产品表:SKU的含义是SKU。请帮我查询产品表中的SKU");
    //    //strings.Add("WMS_Product是产品表:GoodsName的含义是产品名称。请帮我查询产品表中的产品名称");
    //    //strings.Add("WMS_Product是产品表:GoodsType的含义是产品类型。请帮我查询产品表中的产品类型");
    //    //strings.Add("WMS_Product是产品表:SKUClassification的含义是产品分类。请帮我查询产品表中的产品分类");
    //    //strings.Add("WMS_Product是产品表:SKULevel的含义是产品等级。请帮我查询产品表中的产品等级");
    //    //strings.Add("WMS_Product是产品表:SKUGroup的含义是产品分组。请帮我查询产品表中的产品分组");
    //    //strings.Add("WMS_Product是产品表:ManufacturerSKU的含义是制造商SKU。请帮我查询产品表中的制造商SKU");
    //    //strings.Add("WMS_Product是产品表:RetailSKU的含义是零售商SKU。请帮我查询产品表中的零售商SKU");
    //    //strings.Add("WMS_Product是产品表:ReplaceSKU的含义是可替换SKU。请帮我查询产品表中的可替换SKU");
    //    //strings.Add("WMS_Product是产品表:BoxGroup的含义是箱组。请帮我查询产品表中的箱组");
    //    //strings.Add("WMS_Product是产品表:Country的含义是国家。请帮我查询产品表中的国家");
    //    //strings.Add("WMS_Product是产品表:Manufacturer的含义是制造商。请帮我查询产品表中的制造商");
    //    //strings.Add("WMS_Product是产品表:DangerCode的含义是危险等级。请帮我查询产品表中的危险等级");
    //    //strings.Add("WMS_Product是产品表:Volume的含义是体积。请帮我查询产品表中的体积");
    //    //strings.Add("WMS_Product是产品表:StandardVolume的含义是标准体积。请帮我查询产品表中的标准体积");
    //    //strings.Add("WMS_Product是产品表:Weight的含义是重量。请帮我查询产品表中的重量");
    //    //strings.Add("WMS_Product是产品表:StandardWeight的含义是标准重量。请帮我查询产品表中的标准重量");
    //    //strings.Add("WMS_Product是产品表:NetWeight的含义是净重。请帮我查询产品表中的净重");
    //    //strings.Add("WMS_Product是产品表:StandardNetWeight的含义是标准净重。请帮我查询产品表中的标准净重");
    //    //strings.Add("WMS_Product是产品表:Price的含义是价格。请帮我查询产品表中的价格");
    //    //strings.Add("WMS_Product是产品表:ActualPrice的含义是实际价格。请帮我查询产品表中的实际价格");
    //    //strings.Add("WMS_Product是产品表:Cost的含义是成本。请帮我查询产品表中的成本");
    //    //strings.Add("WMS_Product是产品表:Color的含义是颜色。请帮我查询产品表中的颜色");
    //    //strings.Add("WMS_Product是产品表:Length的含义是长。请帮我查询产品表中的长");
    //    //strings.Add("WMS_Product是产品表:Wide的含义是宽。请帮我查询产品表中的宽");
    //    //strings.Add("WMS_Product是产品表:High的含义是高。请帮我查询产品表中的高");
    //    //strings.Add("WMS_Product是产品表:ExpirationDate的含义是过期日期。请帮我查询产品表中的过期日期");
    //    //strings.Add("WMS_Product是产品表:Remark的含义是备注。请帮我查询产品表中的备注");
    //    //strings.Add("WMS_Receipt是入库表:ASNNumber的含义是预入库单号。请帮我查询入库表中的预入库单号");
    //    //strings.Add("WMS_Receipt是入库表:ReceiptNumber的含义是入库单号。请帮我查询入库表中的入库单号");
    //    //strings.Add("WMS_Receipt是入库表:ExternReceiptNumber的含义是外部单号。请帮我查询入库表中的外部单号");
    //    //strings.Add("WMS_Receipt是入库表:CustomerName的含义是客户名称。请帮我查询入库表中的客户名称");
    //    //strings.Add("WMS_Receipt是入库表:WarehouseName的含义是仓库名称。请帮我查询入库表中的仓库名称");
    //    //strings.Add("WMS_Receipt是入库表:ReceiptTime的含义是入库单时间。请帮我查询入库表中的入库单时间");
    //    //strings.Add("WMS_Receipt是入库表:ReceiptType的含义是订单类型。请帮我查询入库表中的订单类型");
    //    //strings.Add("WMS_Receipt是入库表:Contact的含义是联系人。请帮我查询入库表中的联系人");
    //    //strings.Add("WMS_Receipt是入库表:ContactInfo的含义是联系方式。请帮我查询入库表中的联系方式");
    //    //strings.Add("WMS_Receipt是入库表:CompleteTime的含义是完成时间。请帮我查询入库表中的完成时间");
    //    //strings.Add("WMS_Receipt是入库表:Remark的含义是备注。请帮我查询入库表中的备注");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:LineNumber的含义是行号。请帮我查询入库明细表中的行号");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:SKU的含义是SKU。请帮我查询入库明细表中的SKU");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:UPC的含义是UPC。请帮我查询入库明细表中的UPC");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:GoodsType的含义是产品等级。请帮我查询入库明细表中的产品等级");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:GoodsName的含义是产品名称。请帮我查询入库明细表中的产品名称");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:BoxCode的含义是箱号。请帮我查询入库明细表中的箱号");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:TrayCode的含义是托号。请帮我查询入库明细表中的托号");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:BatchCode的含义是批次号。请帮我查询入库明细表中的批次号");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:ExpectedQty的含义是预收数量。请帮我查询入库明细表中的预收数量");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:ReceivedQty的含义是实收数量。请帮我查询入库明细表中的实收数量");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:ReceiptQty的含义是入库数量。请帮我查询入库明细表中的入库数量");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:UnitCode的含义是单位。请帮我查询入库明细表中的单位");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:Onwer的含义是所属。请帮我查询入库明细表中的所属");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:ProductionDate的含义是生产日期。请帮我查询入库明细表中的生产日期");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:ExpirationDate的含义是过期日期。请帮我查询入库明细表中的过期日期");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:Remark的含义是备注。请帮我查询入库明细表中的备注");
    //    //strings.Add("WMS_ReceiptReceiptDetail是上架虚拟主表:ASNNumber的含义是预入库单号。请帮我查询上架虚拟主表中的预入库单号");
    //    //strings.Add("WMS_ReceiptReceiptDetail是上架虚拟主表:ReceiptNumber的含义是入库单号。请帮我查询上架虚拟主表中的入库单号");
    //    //strings.Add("WMS_ReceiptReceiptDetail是上架虚拟主表:ExternReceiptNumber的含义是外部单号。请帮我查询上架虚拟主表中的外部单号");
    //    //strings.Add("WMS_ReceiptReceiptDetail是上架虚拟主表:CustomerName的含义是客户名称。请帮我查询上架虚拟主表中的客户名称");
    //    //strings.Add("WMS_ReceiptReceiptDetail是上架虚拟主表:WarehouseName的含义是仓库名称。请帮我查询上架虚拟主表中的仓库名称");
    //    //strings.Add("WMS_ReceiptReceiptDetail是上架虚拟主表:ReceiptTime的含义是入库单时间。请帮我查询上架虚拟主表中的入库单时间");
    //    //strings.Add("WMS_ReceiptReceiptDetail是上架虚拟主表:ReceiptType的含义是订单类型。请帮我查询上架虚拟主表中的订单类型");
    //    //strings.Add("WMS_ReceiptReceiptDetail是上架虚拟主表:Contact的含义是联系人。请帮我查询上架虚拟主表中的联系人");
    //    //strings.Add("WMS_ReceiptReceiptDetail是上架虚拟主表:ContactInfo的含义是联系方式。请帮我查询上架虚拟主表中的联系方式");
    //    //strings.Add("WMS_ReceiptReceiptDetail是上架虚拟主表:CompleteTime的含义是完成时间。请帮我查询上架虚拟主表中的完成时间");
    //    //strings.Add("WMS_ReceiptReceiptDetail是上架虚拟主表:Remark的含义是备注。请帮我查询上架虚拟主表中的备注");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:LineNumber的含义是行号。请帮我查询上架表中的行号");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:SKU的含义是SKU。请帮我查询上架表中的SKU");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:UPC的含义是UPC。请帮我查询上架表中的UPC");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:GoodsType的含义是产品等级。请帮我查询上架表中的产品等级");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:GoodsName的含义是产品名称。请帮我查询上架表中的产品名称");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:BoxCode的含义是箱号。请帮我查询上架表中的箱号");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:TrayCode的含义是托号。请帮我查询上架表中的托号");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:BatchCode的含义是批次号。请帮我查询上架表中的批次号");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:ReceivedQty的含义是上架数量。请帮我查询上架表中的上架数量");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:UnitCode的含义是单位。请帮我查询上架表中的单位");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:Onwer的含义是所属。请帮我查询上架表中的所属");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:Area的含义是库区。请帮我查询上架表中的库区");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:Location的含义是库位。请帮我查询上架表中的库位");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:ProductionDate的含义是生产日期。请帮我查询上架表中的生产日期");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:ExpirationDate的含义是过期日期。请帮我查询上架表中的过期日期");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:Remark的含义是备注。请帮我查询上架表中的备注");
    //    //strings.Add("WMS_Warehouse是仓库表:WarehouseName的含义是仓库名称。请帮我查询仓库表中的仓库名称");
    //    //strings.Add("WMS_Warehouse是仓库表:WarehouseStatus的含义是仓库状态。请帮我查询仓库表中的仓库状态");
    //    //strings.Add("WMS_Warehouse是仓库表:WarehouseType的含义是仓库类型。请帮我查询仓库表中的仓库类型");
    //    //strings.Add("WMS_Warehouse是仓库表:Description的含义是描述。请帮我查询仓库表中的描述");
    //    //strings.Add("WMS_Warehouse是仓库表:Company的含义是公司。请帮我查询仓库表中的公司");
    //    //strings.Add("WMS_Warehouse是仓库表:Address的含义是地址。请帮我查询仓库表中的地址");
    //    //strings.Add("WMS_Warehouse是仓库表:Province的含义是省份。请帮我查询仓库表中的省份");
    //    //strings.Add("WMS_Warehouse是仓库表:City的含义是城市。请帮我查询仓库表中的城市");
    //    //strings.Add("WMS_Warehouse是仓库表:Contractor的含义是联系人。请帮我查询仓库表中的联系人");
    //    //strings.Add("WMS_Warehouse是仓库表:ContractorAddress的含义是联系人地址。请帮我查询仓库表中的联系人地址");
    //    //strings.Add("WMS_Warehouse是仓库表:Mobile的含义是电话。请帮我查询仓库表中的电话");
    //    //strings.Add("WMS_Warehouse是仓库表:Phone的含义是手机。请帮我查询仓库表中的手机");
    //    //strings.Add("WMS_Warehouse是仓库表:Fax的含义是传真。请帮我查询仓库表中的传真");
    //    //strings.Add("WMS_Warehouse是仓库表:Email的含义是邮箱。请帮我查询仓库表中的邮箱");
    //    //strings.Add("WMS_Warehouse是仓库表:Remark的含义是备注。请帮我查询仓库表中的备注");
    //    //strings.Add("WMS_Receipt_ReceiptDetail是上架虚拟主表:ASNNumber的含义是预入库单号。请帮我查询上架虚拟主表中的预入库单号");
    //    //strings.Add("WMS_Receipt_ReceiptDetail是上架虚拟主表:ReceiptNumber的含义是入库单号。请帮我查询上架虚拟主表中的入库单号");
    //    //strings.Add("WMS_Receipt_ReceiptDetail是上架虚拟主表:ExternReceiptNumber的含义是外部单号。请帮我查询上架虚拟主表中的外部单号");
    //    //strings.Add("WMS_Receipt_ReceiptDetail是上架虚拟主表:CustomerName的含义是客户名称。请帮我查询上架虚拟主表中的客户名称");
    //    //strings.Add("WMS_Receipt_ReceiptDetail是上架虚拟主表:WarehouseName的含义是仓库名称。请帮我查询上架虚拟主表中的仓库名称");
    //    //strings.Add("WMS_Receipt_ReceiptDetail是上架虚拟主表:ReceiptTime的含义是入库单时间。请帮我查询上架虚拟主表中的入库单时间");
    //    //strings.Add("WMS_Receipt_ReceiptDetail是上架虚拟主表:ReceiptType的含义是订单类型。请帮我查询上架虚拟主表中的订单类型");
    //    //strings.Add("WMS_Receipt_ReceiptDetail是上架虚拟主表:Contact的含义是联系人。请帮我查询上架虚拟主表中的联系人");
    //    //strings.Add("WMS_Receipt_ReceiptDetail是上架虚拟主表:ContactInfo的含义是联系方式。请帮我查询上架虚拟主表中的联系方式");
    //    //strings.Add("WMS_Receipt_ReceiptDetail是上架虚拟主表:CompleteTime的含义是完成时间。请帮我查询上架虚拟主表中的完成时间");
    //    //strings.Add("WMS_Receipt_ReceiptDetail是上架虚拟主表:Remark的含义是备注。请帮我查询上架虚拟主表中的备注");
    //    //strings.Add("WMS_Package是包装主表:PickTaskNumber的含义是拣货任务号。请帮我查询包装主表中的拣货任务号");
    //    //strings.Add("WMS_Package是包装主表:PreOrderNumber的含义是预出库单号。请帮我查询包装主表中的预出库单号");
    //    //strings.Add("WMS_Package是包装主表:PackageNumber的含义是包装单号。请帮我查询包装主表中的包装单号");
    //    //strings.Add("WMS_Package是包装主表:CustomerName的含义是客户名称。请帮我查询包装主表中的客户名称");
    //    //strings.Add("WMS_Package是包装主表:WarehouseName的含义是仓库名称。请帮我查询包装主表中的仓库名称");
    //    //strings.Add("WMS_Package是包装主表:PackageType的含义是包装类型。请帮我查询包装主表中的包装类型");
    //    //strings.Add("WMS_Package是包装主表:NetWeight的含义是净重。请帮我查询包装主表中的净重");
    //    //strings.Add("WMS_Package是包装主表:ExpressCompany的含义是快递公司。请帮我查询包装主表中的快递公司");
    //    //strings.Add("WMS_Package是包装主表:ExpressNumber的含义是快递单号。请帮我查询包装主表中的快递单号");
    //    //strings.Add("WMS_Package是包装主表:PackageTime的含义是包装时间。请帮我查询包装主表中的包装时间");
    //    //strings.Add("WMS_Package是包装主表:DetailCount的含义是明细数量。请帮我查询包装主表中的明细数量");
    //    //strings.Add("WMS_Package是包装主表:PrintNum的含义是打印次数。请帮我查询包装主表中的打印次数");
    //    //strings.Add("WMS_Package是包装主表:PrintPersonnel的含义是打印人。请帮我查询包装主表中的打印人");
    //    //strings.Add("WMS_Package是包装主表:PrintTime的含义是打印时间。请帮我查询包装主表中的打印时间");
    //    //strings.Add("WMS_PackageDetail是包装明细表:PickTaskNumber的含义是拣货任务号。请帮我查询包装明细表中的拣货任务号");
    //    //strings.Add("WMS_PackageDetail是包装明细表:PreOrderNumber的含义是预出库单号。请帮我查询包装明细表中的预出库单号");
    //    //strings.Add("WMS_PackageDetail是包装明细表:OrderNumber的含义是出库单号。请帮我查询包装明细表中的出库单号");
    //    //strings.Add("WMS_PackageDetail是包装明细表:ExternOrderNumber的含义是外部单号。请帮我查询包装明细表中的外部单号");
    //    //strings.Add("WMS_PackageDetail是包装明细表:PackageNumber的含义是包装单号。请帮我查询包装明细表中的包装单号");
    //    //strings.Add("WMS_PackageDetail是包装明细表:CustomerName的含义是客户名称。请帮我查询包装明细表中的客户名称");
    //    //strings.Add("WMS_PackageDetail是包装明细表:WarehouseName的含义是仓库名称。请帮我查询包装明细表中的仓库名称");
    //    //strings.Add("WMS_PackageDetail是包装明细表:SKU的含义是SKU。请帮我查询包装明细表中的SKU");
    //    //strings.Add("WMS_PackageDetail是包装明细表:UPC的含义是UPC。请帮我查询包装明细表中的UPC");
    //    //strings.Add("WMS_PackageDetail是包装明细表:GoodsName的含义是产品名称。请帮我查询包装明细表中的产品名称");
    //    //strings.Add("WMS_PackageDetail是包装明细表:GoodsType的含义是产品类型。请帮我查询包装明细表中的产品类型");
    //    //strings.Add("WMS_PackageDetail是包装明细表:UnitCode的含义是单位。请帮我查询包装明细表中的单位");
    //    //strings.Add("WMS_PackageDetail是包装明细表:Onwer的含义是所属。请帮我查询包装明细表中的所属");
    //    //strings.Add("WMS_PackageDetail是包装明细表:BoxCode的含义是箱号。请帮我查询包装明细表中的箱号");
    //    //strings.Add("WMS_PackageDetail是包装明细表:TrayCode的含义是托号。请帮我查询包装明细表中的托号");
    //    //strings.Add("WMS_PackageDetail是包装明细表:BatchCode的含义是批次号。请帮我查询包装明细表中的批次号");
    //    //strings.Add("WMS_PackageDetail是包装明细表:Qty的含义是数量。请帮我查询包装明细表中的数量");
    //    //strings.Add("Customer是客户表:CustomerCode的含义是客户代码。请帮我查询客户表中的客户代码");
    //    //strings.Add("Customer是客户表:CustomerName的含义是客户名称。请帮我查询客户表中的客户名称");
    //    //strings.Add("Customer是客户表:Description的含义是描述。请帮我查询客户表中的描述");
    //    //strings.Add("Customer是客户表:CustomerType的含义是客户类型。请帮我查询客户表中的客户类型");
    //    //strings.Add("Customer是客户表:CustomerStatus的含义是客户状态。请帮我查询客户表中的客户状态");
    //    //strings.Add("Customer是客户表:CreditLine的含义是信用额度。请帮我查询客户表中的信用额度");
    //    //strings.Add("Customer是客户表:Province的含义是省份。请帮我查询客户表中的省份");
    //    //strings.Add("Customer是客户表:City的含义是城市。请帮我查询客户表中的城市");
    //    //strings.Add("Customer是客户表:Address的含义是地址。请帮我查询客户表中的地址");
    //    //strings.Add("Customer是客户表:Remark的含义是备注。请帮我查询客户表中的备注");
    //    //strings.Add("Customer是客户表:Email的含义是邮箱。请帮我查询客户表中的邮箱");
    //    //strings.Add("Customer是客户表:Phone的含义是电话。请帮我查询客户表中的电话");
    //    //strings.Add("Customer是客户表:LawPerson的含义是法人代表。请帮我查询客户表中的法人代表");
    //    //strings.Add("Customer是客户表:PostCode的含义是邮政编码。请帮我查询客户表中的邮政编码");
    //    //strings.Add("Customer是客户表:Bank的含义是开户银行。请帮我查询客户表中的开户银行");
    //    //strings.Add("Customer是客户表:Account的含义是帐号。请帮我查询客户表中的帐号");
    //    //strings.Add("Customer是客户表:TaxId的含义是税号。请帮我查询客户表中的税号");
    //    //strings.Add("Customer是客户表:InvoiceTitle的含义是发票抬头。请帮我查询客户表中的发票抬头");
    //    //strings.Add("Customer是客户表:Fax的含义是传真。请帮我查询客户表中的传真");
    //    //strings.Add("Customer是客户表:WebSite的含义是网址。请帮我查询客户表中的网址");
    //    //strings.Add("Customer是客户表:CreateTime的含义是创建时间。请帮我查询客户表中的创建时间");
    //    //strings.Add("CustomerDetail是客户明细表:Contact的含义是联系人。请帮我查询客户明细表中的联系人");
    //    //strings.Add("CustomerDetail是客户明细表:Tel的含义是联系方式。请帮我查询客户明细表中的联系方式");
    //    //strings.Add("WMS_Adjustment是调整表:ExternNumber的含义是外部单号。请帮我查询调整表中的外部单号");
    //    //strings.Add("WMS_Adjustment是调整表:CustomerName的含义是客户名称。请帮我查询调整表中的客户名称");
    //    //strings.Add("WMS_Adjustment是调整表:WarehouseName的含义是仓库名称。请帮我查询调整表中的仓库名称");
    //    //strings.Add("WMS_Adjustment是调整表:AdjustmentStatus的含义是调整状态。请帮我查询调整表中的调整状态");
    //    //strings.Add("WMS_Adjustment是调整表:AdjustmentType的含义是调整类型。请帮我查询调整表中的调整类型");
    //    //strings.Add("WMS_Adjustment是调整表:AdjustmentReason的含义是调整原因。请帮我查询调整表中的调整原因");
    //    //strings.Add("WMS_Adjustment是调整表:AdjustmentTime的含义是调整时间。请帮我查询调整表中的调整时间");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:FromOnwer的含义是从所属。请帮我查询调整明细表中的从所属");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:ToOnwer的含义是至所属。请帮我查询调整明细表中的至所属");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:SKU的含义是SKU。请帮我查询调整明细表中的SKU");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:UPC的含义是UPC。请帮我查询调整明细表中的UPC");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:TrayCode的含义是唯一编号。请帮我查询调整明细表中的唯一编号");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:BatchCode的含义是批次号。请帮我查询调整明细表中的批次号");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:BoxCode的含义是箱号。请帮我查询调整明细表中的箱号");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:GoodsName的含义是品名。请帮我查询调整明细表中的品名");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:FromWarehouseName的含义是从仓库。请帮我查询调整明细表中的从仓库");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:ToWarehouseName的含义是至仓库。请帮我查询调整明细表中的至仓库");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:FromArea的含义是从库区。请帮我查询调整明细表中的从库区");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:ToArea的含义是至库区。请帮我查询调整明细表中的至库区");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:FromLocation的含义是从库位。请帮我查询调整明细表中的从库位");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:ToLocation的含义是至库位。请帮我查询调整明细表中的至库位");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:Qty的含义是数量。请帮我查询调整明细表中的数量");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:FromGoodsType的含义是从品级。请帮我查询调整明细表中的从品级");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:ToGoodsType的含义是至品级。请帮我查询调整明细表中的至品级");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:FromUnitCode的含义是从单位。请帮我查询调整明细表中的从单位");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:ToUnitCode的含义是至单位。请帮我查询调整明细表中的至单位");
    //    //strings.Add("WMS_AdjustmentDetail是调整明细表:AdjustmentReason的含义是调整原因。请帮我查询调整明细表中的调整原因");
    //    //strings.Add("WMS_Area是库区表:WarehouseName的含义是仓库名称。请帮我查询库区表中的仓库名称");
    //    //strings.Add("WMS_Area是库区表:AreaName的含义是库区。请帮我查询库区表中的库区");
    //    //strings.Add("WMS_Area是库区表:AreaStatus的含义是状态。请帮我查询库区表中的状态");
    //    //strings.Add("WMS_Area是库区表:AreaType的含义是类型。请帮我查询库区表中的类型");
    //    //strings.Add("WMS_ASN是预入库:ExternReceiptNumber的含义是外部单号。请帮我查询预入库中的外部单号");
    //    //strings.Add("WMS_ASN是预入库:CustomerName的含义是客户名称。请帮我查询预入库中的客户名称");
    //    //strings.Add("WMS_ASN是预入库:WarehouseName的含义是仓库名称。请帮我查询预入库中的仓库名称");
    //    //strings.Add("WMS_ASN是预入库:ExpectDate的含义是订单时间。请帮我查询预入库中的订单时间");
    //    //strings.Add("WMS_ASN是预入库:ReceiptType的含义是订单类型。请帮我查询预入库中的订单类型");
    //    //strings.Add("WMS_ASN是预入库:Contact的含义是联系人。请帮我查询预入库中的联系人");
    //    //strings.Add("WMS_ASN是预入库:ContactInfo的含义是联系人信息。请帮我查询预入库中的联系人信息");
    //    //strings.Add("WMS_ASNDetail是预入库明细:SKU的含义是SKU。请帮我查询预入库明细中的SKU");
    //    //strings.Add("WMS_ASNDetail是预入库明细:UPC的含义是UPC。请帮我查询预入库明细中的UPC");
    //    //strings.Add("WMS_ASNDetail是预入库明细:GoodsType的含义是产品等级。请帮我查询预入库明细中的产品等级");
    //    //strings.Add("WMS_ASNDetail是预入库明细:GoodsName的含义是产品名称。请帮我查询预入库明细中的产品名称");
    //    //strings.Add("WMS_ASNDetail是预入库明细:BoxCode的含义是箱号。请帮我查询预入库明细中的箱号");
    //    //strings.Add("WMS_ASNDetail是预入库明细:TrayCode的含义是托盘号。请帮我查询预入库明细中的托盘号");
    //    //strings.Add("WMS_ASNDetail是预入库明细:BatchCode的含义是批次号。请帮我查询预入库明细中的批次号");
    //    //strings.Add("WMS_ASNDetail是预入库明细:ExpectedQty的含义是期望数量。请帮我查询预入库明细中的期望数量");
    //    //strings.Add("WMS_ASNDetail是预入库明细:ProductionDate的含义是生产日期。请帮我查询预入库明细中的生产日期");
    //    //strings.Add("WMS_ASNDetail是预入库明细:ExpirationDate的含义是过期日期。请帮我查询预入库明细中的过期日期");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:CustomerName的含义是客户名称。请帮我查询可用库存表中的客户名称");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:WarehouseName的含义是仓库名称。请帮我查询可用库存表中的仓库名称");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:Area的含义是库区。请帮我查询可用库存表中的库区");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:Location的含义是库位。请帮我查询可用库存表中的库位");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:SKU的含义是SKU。请帮我查询可用库存表中的SKU");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:UPC的含义是UPC。请帮我查询可用库存表中的UPC");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:GoodsType的含义是产品类型。请帮我查询可用库存表中的产品类型");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:InventoryStatus的含义是库存状态。请帮我查询可用库存表中的库存状态");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:GoodsName的含义是产品名称。请帮我查询可用库存表中的产品名称");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:UnitCode的含义是单位。请帮我查询可用库存表中的单位");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:Onwer的含义是所属。请帮我查询可用库存表中的所属");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:BoxCode的含义是箱号。请帮我查询可用库存表中的箱号");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:TrayCode的含义是托号。请帮我查询可用库存表中的托号");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:BatchCode的含义是批次号。请帮我查询可用库存表中的批次号");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:Qty的含义是数量。请帮我查询可用库存表中的数量");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:ProductionDate的含义是生产日期。请帮我查询可用库存表中的生产日期");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:ExpirationDate的含义是过期日期。请帮我查询可用库存表中的过期日期");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:Remark的含义是备注。请帮我查询可用库存表中的备注");
    //    //strings.Add("WMS_Inventory_Usable是可用库存表:InventoryTime的含义是入库时间。请帮我查询可用库存表中的入库时间");
    //    //strings.Add("WMS_Location是库位表:WarehouseName的含义是仓库。请帮我查询库位表中的仓库");
    //    //strings.Add("WMS_Location是库位表:AreaName的含义是库区。请帮我查询库位表中的库区");
    //    //strings.Add("WMS_Location是库位表:Location的含义是库位。请帮我查询库位表中的库位");
    //    //strings.Add("WMS_Location是库位表:LocationStatus的含义是库位状态。请帮我查询库位表中的库位状态");
    //    //strings.Add("WMS_Location是库位表:LocationType的含义是库位类型。请帮我查询库位表中的库位类型");
    //    //strings.Add("WMS_Order是出库表:PreOrderNumber的含义是预出库单号。请帮我查询出库表中的预出库单号");
    //    //strings.Add("WMS_Order是出库表:ExternOrderNumber的含义是外部单号。请帮我查询出库表中的外部单号");
    //    //strings.Add("WMS_Order是出库表:CustomerName的含义是客户。请帮我查询出库表中的客户");
    //    //strings.Add("WMS_Order是出库表:WarehouseName的含义是仓库。请帮我查询出库表中的仓库");
    //    //strings.Add("WMS_Order是出库表:OrderType的含义是订单类型。请帮我查询出库表中的订单类型");
    //    //strings.Add("WMS_Order是出库表:OrderStatus的含义是订单状态。请帮我查询出库表中的订单状态");
    //    //strings.Add("WMS_Order是出库表:OrderTime的含义是订单时间。请帮我查询出库表中的订单时间");
    //    //strings.Add("WMS_Order是出库表:Creator的含义是创建人。请帮我查询出库表中的创建人");
    //    //strings.Add("WMS_Order是出库表:CreationTime的含义是创建时间。请帮我查询出库表中的创建时间");
    //    //strings.Add("WMS_Order是出库表:Remark的含义是备注。请帮我查询出库表中的备注");
    //    //strings.Add("WMS_OrderAddress是出货地址:Name的含义是名称。请帮我查询出货地址中的名称");
    //    //strings.Add("WMS_OrderAddress是出货地址:Phone的含义是联系电话。请帮我查询出货地址中的联系电话");
    //    //strings.Add("WMS_OrderAddress是出货地址:ZipCode的含义是邮政编码。请帮我查询出货地址中的邮政编码");
    //    //strings.Add("WMS_OrderAddress是出货地址:Province的含义是省份。请帮我查询出货地址中的省份");
    //    //strings.Add("WMS_OrderAddress是出货地址:City的含义是城市。请帮我查询出货地址中的城市");
    //    //strings.Add("WMS_OrderAddress是出货地址:Country的含义是地区。请帮我查询出货地址中的地区");
    //    //strings.Add("WMS_OrderAddress是出货地址:Address的含义是地址。请帮我查询出货地址中的地址");
    //    //strings.Add("WMS_OrderAddress是出货地址:ExpressCompany的含义是快递公司。请帮我查询出货地址中的快递公司");
    //    //strings.Add("WMS_OrderAddress是出货地址:ExpressNumber的含义是快递单号。请帮我查询出货地址中的快递单号");
    //    //strings.Add("WMS_OrderAllocation是分配表:Area的含义是库区。请帮我查询分配表中的库区");
    //    //strings.Add("WMS_OrderAllocation是分配表:Location的含义是库位。请帮我查询分配表中的库位");
    //    //strings.Add("WMS_OrderAllocation是分配表:SKU的含义是SKU。请帮我查询分配表中的SKU");
    //    //strings.Add("WMS_OrderAllocation是分配表:UPC的含义是UPC。请帮我查询分配表中的UPC");
    //    //strings.Add("WMS_OrderAllocation是分配表:GoodsType的含义是产品类型。请帮我查询分配表中的产品类型");
    //    //strings.Add("WMS_OrderAllocation是分配表:GoodsName的含义是产品名称。请帮我查询分配表中的产品名称");
    //    //strings.Add("WMS_OrderAllocation是分配表:UnitCode的含义是单位。请帮我查询分配表中的单位");
    //    //strings.Add("WMS_OrderAllocation是分配表:Onwer的含义是所属。请帮我查询分配表中的所属");
    //    //strings.Add("WMS_OrderAllocation是分配表:BoxCode的含义是箱号。请帮我查询分配表中的箱号");
    //    //strings.Add("WMS_OrderAllocation是分配表:TrayCode的含义是托号。请帮我查询分配表中的托号");
    //    //strings.Add("WMS_OrderAllocation是分配表:BatchCode的含义是批次号。请帮我查询分配表中的批次号");
    //    //strings.Add("WMS_OrderAllocation是分配表:Qty的含义是数量。请帮我查询分配表中的数量");
    //    //strings.Add("WMS_OrderAllocation是分配表:ProductionDate的含义是生产日期。请帮我查询分配表中的生产日期");
    //    //strings.Add("WMS_OrderAllocation是分配表:ExpirationDate的含义是过期日期。请帮我查询分配表中的过期日期");
    //    //strings.Add("WMS_OrderAllocation是分配表:Remark的含义是备注。请帮我查询分配表中的备注");
    //    //strings.Add("WMS_OrderAllocation是分配表:Creator的含义是创建人。请帮我查询分配表中的创建人");
    //    //strings.Add("WMS_OrderAllocation是分配表:CreationTime的含义是创建时间。请帮我查询分配表中的创建时间");
    //    //strings.Add("WMS_OrderDetail是出库明细表:LineNumber的含义是行号。请帮我查询出库明细表中的行号");
    //    //strings.Add("WMS_OrderDetail是出库明细表:SKU的含义是SKU。请帮我查询出库明细表中的SKU");
    //    //strings.Add("WMS_OrderDetail是出库明细表:UPC的含义是UPC。请帮我查询出库明细表中的UPC");
    //    //strings.Add("WMS_OrderDetail是出库明细表:GoodsName的含义是产品名称。请帮我查询出库明细表中的产品名称");
    //    //strings.Add("WMS_OrderDetail是出库明细表:GoodsType的含义是产品类型。请帮我查询出库明细表中的产品类型");
    //    //strings.Add("WMS_OrderDetail是出库明细表:OrderQty的含义是出库数量。请帮我查询出库明细表中的出库数量");
    //    //strings.Add("WMS_OrderDetail是出库明细表:AllocatedQty的含义是分配数量。请帮我查询出库明细表中的分配数量");
    //    //strings.Add("WMS_OrderDetail是出库明细表:BoxCode的含义是箱号。请帮我查询出库明细表中的箱号");
    //    //strings.Add("WMS_OrderDetail是出库明细表:TrayCode的含义是托号。请帮我查询出库明细表中的托号");
    //    //strings.Add("WMS_OrderDetail是出库明细表:BatchCode的含义是批次号。请帮我查询出库明细表中的批次号");
    //    //strings.Add("WMS_OrderDetail是出库明细表:UnitCode的含义是单位。请帮我查询出库明细表中的单位");
    //    //strings.Add("WMS_OrderDetail是出库明细表:Onwer的含义是所属。请帮我查询出库明细表中的所属");
    //    //strings.Add("WMS_PickTask是拣货任务表:CustomerName的含义是客户名称。请帮我查询拣货任务表中的客户名称");
    //    //strings.Add("WMS_PickTask是拣货任务表:WarehouseName的含义是仓库名称。请帮我查询拣货任务表中的仓库名称");
    //    //strings.Add("WMS_PickTask是拣货任务表:PickTaskNumber的含义是拣货任务号。请帮我查询拣货任务表中的拣货任务号");
    //    //strings.Add("WMS_PickTask是拣货任务表:PickStatus的含义是拣货状态。请帮我查询拣货任务表中的拣货状态");
    //    //strings.Add("WMS_PickTask是拣货任务表:PickType的含义是拣货类型。请帮我查询拣货任务表中的拣货类型");
    //    //strings.Add("WMS_PickTask是拣货任务表:StartTime的含义是开始时间。请帮我查询拣货任务表中的开始时间");
    //    //strings.Add("WMS_PickTask是拣货任务表:EndTime的含义是结束时间。请帮我查询拣货任务表中的结束时间");
    //    //strings.Add("WMS_PickTask是拣货任务表:PrintNum的含义是打印次数。请帮我查询拣货任务表中的打印次数");
    //    //strings.Add("WMS_PickTask是拣货任务表:PrintTime的含义是打印时间。请帮我查询拣货任务表中的打印时间");
    //    //strings.Add("WMS_PickTask是拣货任务表:PrintPersonnel的含义是打印人。请帮我查询拣货任务表中的打印人");
    //    //strings.Add("WMS_PickTask是拣货任务表:PickPlanPersonnel的含义是计划拣货人。请帮我查询拣货任务表中的计划拣货人");
    //    //strings.Add("WMS_PickTask是拣货任务表:DetailQty的含义是明细数量。请帮我查询拣货任务表中的明细数量");
    //    //strings.Add("WMS_PickTask是拣货任务表:DetailKindsQty的含义是明细种类。请帮我查询拣货任务表中的明细种类");
    //    //strings.Add("WMS_PickTask是拣货任务表:PickContainer的含义是拣货容器。请帮我查询拣货任务表中的拣货容器");
    //    //strings.Add("WMS_PickTask是拣货任务表:Creator的含义是创建人。请帮我查询拣货任务表中的创建人");
    //    //strings.Add("WMS_PickTask是拣货任务表:CreationTime的含义是创建时间。请帮我查询拣货任务表中的创建时间");
    //    //strings.Add("WMS_PickTask是拣货任务表:Updator的含义是修改人。请帮我查询拣货任务表中的修改人");
    //    //strings.Add("WMS_PickTask是拣货任务表:UpdateTime的含义是修改时间。请帮我查询拣货任务表中的修改时间");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:PickStatus的含义是拣货状态。请帮我查询拣货任务明细表中的拣货状态");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:PikcTime的含义是拣货时间。请帮我查询拣货任务明细表中的拣货时间");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:PickBoxNumber的含义是拣货箱号。请帮我查询拣货任务明细表中的拣货箱号");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:PickQty的含义是拣货数量。请帮我查询拣货任务明细表中的拣货数量");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:Area的含义是库区。请帮我查询拣货任务明细表中的库区");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:Location的含义是库位。请帮我查询拣货任务明细表中的库位");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:SKU的含义是SKU。请帮我查询拣货任务明细表中的SKU");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:UPC的含义是UPC。请帮我查询拣货任务明细表中的UPC");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:GoodsType的含义是品级。请帮我查询拣货任务明细表中的品级");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:GoodsName的含义是品名。请帮我查询拣货任务明细表中的品名");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:UnitCode的含义是单位。请帮我查询拣货任务明细表中的单位");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:Onwer的含义是所属。请帮我查询拣货任务明细表中的所属");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:BoxCode的含义是箱号。请帮我查询拣货任务明细表中的箱号");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:TrayCode的含义是托号。请帮我查询拣货任务明细表中的托号");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:BatchCode的含义是批次号。请帮我查询拣货任务明细表中的批次号");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:Qty的含义是数量。请帮我查询拣货任务明细表中的数量");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:ProductionDate的含义是生产日期。请帮我查询拣货任务明细表中的生产日期");
    //    //strings.Add("WMS_PreOrder是预出库表:ExternOrderNumber的含义是外部单号。请帮我查询预出库表中的外部单号");
    //    //strings.Add("WMS_PreOrder是预出库表:CustomerName的含义是客户名称。请帮我查询预出库表中的客户名称");
    //    //strings.Add("WMS_PreOrder是预出库表:WarehouseName的含义是仓库名称。请帮我查询预出库表中的仓库名称");
    //    //strings.Add("WMS_PreOrder是预出库表:OrderType的含义是订单类型。请帮我查询预出库表中的订单类型");
    //    //strings.Add("WMS_PreOrder是预出库表:OrderTime的含义是订单时间。请帮我查询预出库表中的订单时间");
    //    //strings.Add("WMS_PreOrderDetail是预出库明细表:SKU的含义是SKU。请帮我查询预出库明细表中的SKU");
    //    //strings.Add("WMS_PreOrderDetail是预出库明细表:UPC的含义是UPC。请帮我查询预出库明细表中的UPC");
    //    //strings.Add("WMS_PreOrderDetail是预出库明细表:GoodsName的含义是产品名称。请帮我查询预出库明细表中的产品名称");
    //    //strings.Add("WMS_PreOrderDetail是预出库明细表:GoodsType的含义是产品等级。请帮我查询预出库明细表中的产品等级");
    //    //strings.Add("WMS_PreOrderDetail是预出库明细表:OrderQty的含义是出库数量。请帮我查询预出库明细表中的出库数量");
    //    //strings.Add("WMS_PreOrderDetail是预出库明细表:BoxCode的含义是箱号。请帮我查询预出库明细表中的箱号");
    //    //strings.Add("WMS_PreOrderDetail是预出库明细表:TrayCode的含义是托号。请帮我查询预出库明细表中的托号");
    //    //strings.Add("WMS_PreOrderDetail是预出库明细表:BatchCode的含义是批次号。请帮我查询预出库明细表中的批次号");
    //    //strings.Add("WMS_PreOrderDetail是预出库明细表:UnitCode的含义是单位。请帮我查询预出库明细表中的单位");
    //    //strings.Add("WMS_PreOrderDetail是预出库明细表:Onwer的含义是所属。请帮我查询预出库明细表中的所属");
    //    //strings.Add("WMS_PreOrderDetail是预出库明细表:ProductionDate的含义是生产日期。请帮我查询预出库明细表中的生产日期");
    //    //strings.Add("WMS_PreOrderDetail是预出库明细表:ExpirationDate的含义是过期日期。请帮我查询预出库明细表中的过期日期");
    //    //strings.Add("WMS_Product是产品表:CustomerName的含义是客户名称。请帮我查询产品表中的客户名称");
    //    //strings.Add("WMS_Product是产品表:SKU的含义是SKU。请帮我查询产品表中的SKU");
    //    //strings.Add("WMS_Product是产品表:ProductStatus的含义是产品状态。请帮我查询产品表中的产品状态");
    //    //strings.Add("WMS_Product是产品表:GoodsName的含义是产品名称。请帮我查询产品表中的产品名称");
    //    //strings.Add("WMS_Product是产品表:GoodsType的含义是产品类型。请帮我查询产品表中的产品类型");
    //    //strings.Add("WMS_Product是产品表:SKUClassification的含义是产品分类。请帮我查询产品表中的产品分类");
    //    //strings.Add("WMS_Product是产品表:SKULevel的含义是产品等级。请帮我查询产品表中的产品等级");
    //    //strings.Add("WMS_Product是产品表:SKUGroup的含义是产品分组。请帮我查询产品表中的产品分组");
    //    //strings.Add("WMS_Product是产品表:ManufacturerSKU的含义是制造商SKU。请帮我查询产品表中的制造商SKU");
    //    //strings.Add("WMS_Product是产品表:RetailSKU的含义是零售商SKU。请帮我查询产品表中的零售商SKU");
    //    //strings.Add("WMS_Product是产品表:ReplaceSKU的含义是可替换SKU。请帮我查询产品表中的可替换SKU");
    //    //strings.Add("WMS_Product是产品表:BoxGroup的含义是箱组。请帮我查询产品表中的箱组");
    //    //strings.Add("WMS_Product是产品表:Country的含义是国家。请帮我查询产品表中的国家");
    //    //strings.Add("WMS_Product是产品表:Manufacturer的含义是制造商SKU。请帮我查询产品表中的制造商SKU");
    //    //strings.Add("WMS_Product是产品表:DangerCode的含义是危险等级。请帮我查询产品表中的危险等级");
    //    //strings.Add("WMS_Product是产品表:Volume的含义是体积。请帮我查询产品表中的体积");
    //    //strings.Add("WMS_Product是产品表:StandardVolume的含义是标准体积。请帮我查询产品表中的标准体积");
    //    //strings.Add("WMS_Product是产品表:Weight的含义是重量。请帮我查询产品表中的重量");
    //    //strings.Add("WMS_Product是产品表:StandardWeight的含义是标准重量。请帮我查询产品表中的标准重量");
    //    //strings.Add("WMS_Product是产品表:NetWeight的含义是净重。请帮我查询产品表中的净重");
    //    //strings.Add("WMS_Product是产品表:StandardNetWeight的含义是标准净重。请帮我查询产品表中的标准净重");
    //    //strings.Add("WMS_Product是产品表:Price的含义是价格。请帮我查询产品表中的价格");
    //    //strings.Add("WMS_Product是产品表:ActualPrice的含义是实际价格。请帮我查询产品表中的实际价格");
    //    //strings.Add("WMS_Product是产品表:Cost的含义是成本。请帮我查询产品表中的成本");
    //    //strings.Add("WMS_Product是产品表:Color的含义是颜色。请帮我查询产品表中的颜色");
    //    //strings.Add("WMS_Product是产品表:Length的含义是长。请帮我查询产品表中的长");
    //    //strings.Add("WMS_Product是产品表:Wide的含义是宽。请帮我查询产品表中的宽");
    //    //strings.Add("WMS_Product是产品表:High的含义是高。请帮我查询产品表中的高");
    //    //strings.Add("WMS_Product是产品表:ExpirationDate的含义是过期日期。请帮我查询产品表中的过期日期");
    //    //strings.Add("WMS_Product是产品表:Remark的含义是备注。请帮我查询产品表中的备注");
    //    //strings.Add("WMS_Receipt是入库表:ASNNumber的含义是预入库单号。请帮我查询入库表中的预入库单号");
    //    //strings.Add("WMS_Receipt是入库表:ReceiptNumber的含义是入库单号。请帮我查询入库表中的入库单号");
    //    //strings.Add("WMS_Receipt是入库表:ExternReceiptNumber的含义是外部单号。请帮我查询入库表中的外部单号");
    //    //strings.Add("WMS_Receipt是入库表:CustomerName的含义是客户名称。请帮我查询入库表中的客户名称");
    //    //strings.Add("WMS_Receipt是入库表:WarehouseName的含义是仓库名称。请帮我查询入库表中的仓库名称");
    //    //strings.Add("WMS_Receipt是入库表:ReceiptTime的含义是入库单时间。请帮我查询入库表中的入库单时间");
    //    //strings.Add("WMS_Receipt是入库表:ReceiptType的含义是订单类型。请帮我查询入库表中的订单类型");
    //    //strings.Add("WMS_Receipt是入库表:Contact的含义是联系人。请帮我查询入库表中的联系人");
    //    //strings.Add("WMS_Receipt是入库表:ContactInfo的含义是联系方式。请帮我查询入库表中的联系方式");
    //    //strings.Add("WMS_Receipt是入库表:CompleteTime的含义是完成时间。请帮我查询入库表中的完成时间");
    //    //strings.Add("WMS_Receipt是入库表:Remark的含义是备注。请帮我查询入库表中的备注");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:LineNumber的含义是行号。请帮我查询入库明细表中的行号");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:SKU的含义是SKU。请帮我查询入库明细表中的SKU");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:UPC的含义是UPC。请帮我查询入库明细表中的UPC");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:GoodsType的含义是产品等级。请帮我查询入库明细表中的产品等级");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:GoodsName的含义是产品名称。请帮我查询入库明细表中的产品名称");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:BoxCode的含义是箱号。请帮我查询入库明细表中的箱号");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:TrayCode的含义是托号。请帮我查询入库明细表中的托号");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:BatchCode的含义是批次号。请帮我查询入库明细表中的批次号");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:ExpectedQty的含义是预收数量。请帮我查询入库明细表中的预收数量");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:ReceivedQty的含义是实收数量。请帮我查询入库明细表中的实收数量");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:ReceiptQty的含义是入库数量。请帮我查询入库明细表中的入库数量");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:UnitCode的含义是单位。请帮我查询入库明细表中的单位");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:Onwer的含义是所属。请帮我查询入库明细表中的所属");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:ProductionDate的含义是生产日期。请帮我查询入库明细表中的生产日期");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:ExpirationDate的含义是过期日期。请帮我查询入库明细表中的过期日期");
    //    //strings.Add("WMS_ReceiptDetail是入库明细表:Remark的含义是备注。请帮我查询入库明细表中的备注");
    //    //strings.Add("WMS_ReceiptReceiptDetail是上架虚拟主表:ASNNumber的含义是预入库单号。请帮我查询上架虚拟主表中的预入库单号");
    //    //strings.Add("WMS_ReceiptReceiptDetail是上架虚拟主表:ReceiptNumber的含义是入库单号。请帮我查询上架虚拟主表中的入库单号");
    //    //strings.Add("WMS_ReceiptReceiptDetail是上架虚拟主表:ExternReceiptNumber的含义是外部单号。请帮我查询上架虚拟主表中的外部单号");
    //    //strings.Add("WMS_ReceiptReceiptDetail是上架虚拟主表:CustomerName的含义是客户名称。请帮我查询上架虚拟主表中的客户名称");
    //    //strings.Add("WMS_ReceiptReceiptDetail是上架虚拟主表:WarehouseName的含义是仓库名称。请帮我查询上架虚拟主表中的仓库名称");
    //    //strings.Add("WMS_ReceiptReceiptDetail是上架虚拟主表:ReceiptTime的含义是入库单时间。请帮我查询上架虚拟主表中的入库单时间");
    //    //strings.Add("WMS_ReceiptReceiptDetail是上架虚拟主表:ReceiptType的含义是订单类型。请帮我查询上架虚拟主表中的订单类型");
    //    //strings.Add("WMS_ReceiptReceiptDetail是上架虚拟主表:Contact的含义是联系人。请帮我查询上架虚拟主表中的联系人");
    //    //strings.Add("WMS_ReceiptReceiptDetail是上架虚拟主表:ContactInfo的含义是联系方式。请帮我查询上架虚拟主表中的联系方式");
    //    //strings.Add("WMS_ReceiptReceiptDetail是上架虚拟主表:CompleteTime的含义是完成时间。请帮我查询上架虚拟主表中的完成时间");
    //    //strings.Add("WMS_ReceiptReceiptDetail是上架虚拟主表:Remark的含义是备注。请帮我查询上架虚拟主表中的备注");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:LineNumber的含义是行号。请帮我查询上架表中的行号");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:SKU的含义是SKU。请帮我查询上架表中的SKU");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:UPC的含义是UPC。请帮我查询上架表中的UPC");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:GoodsType的含义是产品等级。请帮我查询上架表中的产品等级");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:GoodsName的含义是产品名称。请帮我查询上架表中的产品名称");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:BoxCode的含义是箱号。请帮我查询上架表中的箱号");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:TrayCode的含义是托号。请帮我查询上架表中的托号");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:BatchCode的含义是批次号。请帮我查询上架表中的批次号");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:ReceivedQty的含义是上架数量。请帮我查询上架表中的上架数量");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:UnitCode的含义是单位。请帮我查询上架表中的单位");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:Onwer的含义是所属。请帮我查询上架表中的所属");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:Area的含义是库区。请帮我查询上架表中的库区");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:Location的含义是库位。请帮我查询上架表中的库位");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:ProductionDate的含义是生产日期。请帮我查询上架表中的生产日期");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:ExpirationDate的含义是过期日期。请帮我查询上架表中的过期日期");
    //    //strings.Add("WMS_ReceiptReceiving是上架表:Remark的含义是备注。请帮我查询上架表中的备注");
    //    //strings.Add("WMS_Warehouse是仓库表:WarehouseName的含义是仓库名称。请帮我查询仓库表中的仓库名称");
    //    //strings.Add("WMS_Warehouse是仓库表:WarehouseStatus的含义是仓库状态。请帮我查询仓库表中的仓库状态");
    //    //strings.Add("WMS_Warehouse是仓库表:WarehouseType的含义是仓库类型。请帮我查询仓库表中的仓库类型");
    //    //strings.Add("WMS_Warehouse是仓库表:Description的含义是描述。请帮我查询仓库表中的描述");
    //    //strings.Add("WMS_Warehouse是仓库表:Company的含义是国家。请帮我查询仓库表中的国家");
    //    //strings.Add("WMS_Warehouse是仓库表:Address的含义是地址。请帮我查询仓库表中的地址");
    //    //strings.Add("WMS_Warehouse是仓库表:Province的含义是省份。请帮我查询仓库表中的省份");
    //    //strings.Add("WMS_Warehouse是仓库表:City的含义是城市。请帮我查询仓库表中的城市");
    //    //strings.Add("WMS_Warehouse是仓库表:Contractor的含义是联系人。请帮我查询仓库表中的联系人");
    //    //strings.Add("WMS_Warehouse是仓库表:ContractorAddress的含义是联系人地址。请帮我查询仓库表中的联系人地址");
    //    //strings.Add("WMS_Warehouse是仓库表:Mobile的含义是电话。请帮我查询仓库表中的电话");
    //    //strings.Add("WMS_Warehouse是仓库表:Phone的含义是手机。请帮我查询仓库表中的手机");
    //    //strings.Add("WMS_Warehouse是仓库表:Fax的含义是传真。请帮我查询仓库表中的传真");
    //    //strings.Add("WMS_Warehouse是仓库表:Email的含义是邮箱。请帮我查询仓库表中的邮箱");
    //    //strings.Add("WMS_Warehouse是仓库表:Remark的含义是备注。请帮我查询仓库表中的备注");
    //    //strings.Add("WMS_Receipt_ReceiptDetail是上架虚拟主表:ASNNumber的含义是预入库单号。请帮我查询上架虚拟主表中的预入库单号");
    //    //strings.Add("WMS_Receipt_ReceiptDetail是上架虚拟主表:ReceiptNumber的含义是入库单号。请帮我查询上架虚拟主表中的入库单号");
    //    //strings.Add("WMS_Receipt_ReceiptDetail是上架虚拟主表:ExternReceiptNumber的含义是外部单号。请帮我查询上架虚拟主表中的外部单号");
    //    //strings.Add("WMS_Receipt_ReceiptDetail是上架虚拟主表:CustomerName的含义是客户名称。请帮我查询上架虚拟主表中的客户名称");
    //    //strings.Add("WMS_Receipt_ReceiptDetail是上架虚拟主表:WarehouseName的含义是仓库名称。请帮我查询上架虚拟主表中的仓库名称");
    //    //strings.Add("WMS_Receipt_ReceiptDetail是上架虚拟主表:ReceiptTime的含义是入库单时间。请帮我查询上架虚拟主表中的入库单时间");
    //    //strings.Add("WMS_Receipt_ReceiptDetail是上架虚拟主表:ReceiptType的含义是订单类型。请帮我查询上架虚拟主表中的订单类型");
    //    //strings.Add("WMS_Receipt_ReceiptDetail是上架虚拟主表:Contact的含义是联系人。请帮我查询上架虚拟主表中的联系人");
    //    //strings.Add("WMS_Receipt_ReceiptDetail是上架虚拟主表:ContactInfo的含义是联系方式。请帮我查询上架虚拟主表中的联系方式");
    //    //strings.Add("WMS_Receipt_ReceiptDetail是上架虚拟主表:CompleteTime的含义是完成时间。请帮我查询上架虚拟主表中的完成时间");
    //    //strings.Add("WMS_Receipt_ReceiptDetail是上架虚拟主表:Remark的含义是备注。请帮我查询上架虚拟主表中的备注");
    //    //strings.Add("WMS_Package是包装主表:PickTaskNumber的含义是拣货任务号。请帮我查询包装主表中的拣货任务号");
    //    //strings.Add("WMS_Package是包装主表:PreOrderNumber的含义是预出库单号。请帮我查询包装主表中的预出库单号");
    //    //strings.Add("WMS_Package是包装主表:PackageNumber的含义是包装单号。请帮我查询包装主表中的包装单号");
    //    //strings.Add("WMS_Package是包装主表:CustomerName的含义是客户名称。请帮我查询包装主表中的客户名称");
    //    //strings.Add("WMS_Package是包装主表:WarehouseName的含义是仓库名称。请帮我查询包装主表中的仓库名称");
    //    //strings.Add("WMS_Package是包装主表:PackageType的含义是包装类型。请帮我查询包装主表中的包装类型");
    //    //strings.Add("WMS_Package是包装主表:NetWeight的含义是净重。请帮我查询包装主表中的净重");
    //    //strings.Add("WMS_Package是包装主表:ExpressCompany的含义是快递单号。请帮我查询包装主表中的快递单号");
    //    //strings.Add("WMS_Package是包装主表:ExpressNumber的含义是快递公司。请帮我查询包装主表中的快递公司");
    //    //strings.Add("WMS_Package是包装主表:PackageTime的含义是包装时间。请帮我查询包装主表中的包装时间");
    //    //strings.Add("WMS_Package是包装主表:DetailCount的含义是明细数量。请帮我查询包装主表中的明细数量");
    //    //strings.Add("WMS_Package是包装主表:PrintNum的含义是打印次数。请帮我查询包装主表中的打印次数");
    //    //strings.Add("WMS_Package是包装主表:PrintPersonnel的含义是打印人。请帮我查询包装主表中的打印人");
    //    //strings.Add("WMS_Package是包装主表:PrintTime的含义是打印时间。请帮我查询包装主表中的打印时间");
    //    //strings.Add("WMS_PackageDetail是包装明细表:PickTaskNumber的含义是拣货任务号。请帮我查询包装明细表中的拣货任务号");
    //    //strings.Add("WMS_PackageDetail是包装明细表:PreOrderNumber的含义是预出库单号。请帮我查询包装明细表中的预出库单号");
    //    //strings.Add("WMS_PackageDetail是包装明细表:OrderNumber的含义是出库单号。请帮我查询包装明细表中的出库单号");
    //    //strings.Add("WMS_PackageDetail是包装明细表:ExternOrderNumber的含义是外部单号。请帮我查询包装明细表中的外部单号");
    //    //strings.Add("WMS_PackageDetail是包装明细表:PackageNumber的含义是包装单号。请帮我查询包装明细表中的包装单号");
    //    //strings.Add("WMS_PackageDetail是包装明细表:CustomerName的含义是客户名称。请帮我查询包装明细表中的客户名称");
    //    //strings.Add("WMS_PackageDetail是包装明细表:WarehouseName的含义是仓库名称。请帮我查询包装明细表中的仓库名称");
    //    //strings.Add("WMS_PackageDetail是包装明细表:SKU的含义是SKU。请帮我查询包装明细表中的SKU");
    //    //strings.Add("WMS_PackageDetail是包装明细表:UPC的含义是UPC。请帮我查询包装明细表中的UPC");
    //    //strings.Add("WMS_PackageDetail是包装明细表:GoodsName的含义是产品名称。请帮我查询包装明细表中的产品名称");
    //    //strings.Add("WMS_PackageDetail是包装明细表:GoodsType的含义是产品类型。请帮我查询包装明细表中的产品类型");
    //    //strings.Add("WMS_PackageDetail是包装明细表:UnitCode的含义是单位。请帮我查询包装明细表中的单位");
    //    //strings.Add("WMS_PackageDetail是包装明细表:Onwer的含义是所属。请帮我查询包装明细表中的所属");
    //    //strings.Add("WMS_PackageDetail是包装明细表:BoxCode的含义是箱号。请帮我查询包装明细表中的箱号");
    //    //strings.Add("WMS_PackageDetail是包装明细表:TrayCode的含义是托号。请帮我查询包装明细表中的托号");
    //    //strings.Add("WMS_PackageDetail是包装明细表:BatchCode的含义是批次号。请帮我查询包装明细表中的批次号");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:CustomerName的含义是客户名称。请帮我查询库存报表中的客户名称");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:WarehouseName的含义是仓库名称。请帮我查询库存报表中的仓库名称");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:Area的含义是库区。请帮我查询库存报表中的库区");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:Location的含义是库位。请帮我查询库存报表中的库位");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:SKU的含义是SKU。请帮我查询库存报表中的SKU");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:UPC的含义是UPC。请帮我查询库存报表中的UPC");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:GoodsType的含义是产品类型。请帮我查询库存报表中的产品类型");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:InventoryStatus的含义是库存状态。请帮我查询库存报表中的库存状态");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:GoodsName的含义是产品名称。请帮我查询库存报表中的产品名称");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:UnitCode的含义是单位。请帮我查询库存报表中的单位");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:Onwer的含义是所属。请帮我查询库存报表中的所属");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:BoxCode的含义是箱号。请帮我查询库存报表中的箱号");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:TrayCode的含义是托号。请帮我查询库存报表中的托号");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:BatchCode的含义是批次号。请帮我查询库存报表中的批次号");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:Qty的含义是数量。请帮我查询库存报表中的数量");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:ProductionDate的含义是生产日期。请帮我查询库存报表中的生产日期");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:ExpirationDate的含义是过期日期。请帮我查询库存报表中的过期日期");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:Remark的含义是备注。请帮我查询库存报表中的备注");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:InventoryTime的含义是入库时间。请帮我查询库存报表中的入库时间");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:CustomerName的含义是客户名称。请帮我查询库存报表中的客户名称");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:WarehouseName的含义是仓库名称。请帮我查询库存报表中的仓库名称");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:Area的含义是库区。请帮我查询库存报表中的库区");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:Location的含义是库位。请帮我查询库存报表中的库位");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:SKU的含义是SKU。请帮我查询库存报表中的SKU");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:UPC的含义是UPC。请帮我查询库存报表中的UPC");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:GoodsType的含义是产品类型。请帮我查询库存报表中的产品类型");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:InventoryStatus的含义是库存状态。请帮我查询库存报表中的库存状态");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:GoodsName的含义是产品名称。请帮我查询库存报表中的产品名称");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:UnitCode的含义是单位。请帮我查询库存报表中的单位");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:Onwer的含义是所属。请帮我查询库存报表中的所属");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:BoxCode的含义是箱号。请帮我查询库存报表中的箱号");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:TrayCode的含义是托号。请帮我查询库存报表中的托号");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:BatchCode的含义是批次号。请帮我查询库存报表中的批次号");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:Qty的含义是数量。请帮我查询库存报表中的数量");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:ProductionDate的含义是生产日期。请帮我查询库存报表中的生产日期");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:ExpirationDate的含义是过期日期。请帮我查询库存报表中的过期日期");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:Remark的含义是备注。请帮我查询库存报表中的备注");
    //    //strings.Add("WMS_Inventory_Usable_Report是库存报表:InventoryTime的含义是入库时间。请帮我查询库存报表中的入库时间");
    //    //strings.Add("MMS_Supplier是供应商表:SupplierCode的含义是供应商代码。请帮我查询供应商表中的供应商代码");
    //    //strings.Add("MMS_Supplier是供应商表:SupplierName的含义是供应商名称。请帮我查询供应商表中的供应商名称");
    //    //strings.Add("MMS_Supplier是供应商表:Description的含义是描述。请帮我查询供应商表中的描述");
    //    //strings.Add("MMS_Supplier是供应商表:SupplierType的含义是供应商类型。请帮我查询供应商表中的供应商类型");
    //    //strings.Add("MMS_Supplier是供应商表:SupplierStatus的含义是供应商状态。请帮我查询供应商表中的供应商状态");
    //    //strings.Add("MMS_Supplier是供应商表:CreditLine的含义是信用额度。请帮我查询供应商表中的信用额度");
    //    //strings.Add("MMS_Supplier是供应商表:Province的含义是省份。请帮我查询供应商表中的省份");
    //    //strings.Add("MMS_Supplier是供应商表:City的含义是城市。请帮我查询供应商表中的城市");
    //    //strings.Add("MMS_Supplier是供应商表:Address的含义是地址。请帮我查询供应商表中的地址");
    //    //strings.Add("MMS_Supplier是供应商表:Remark的含义是备注。请帮我查询供应商表中的备注");
    //    //strings.Add("MMS_Supplier是供应商表:Email的含义是邮箱。请帮我查询供应商表中的邮箱");
    //    //strings.Add("MMS_Supplier是供应商表:Phone的含义是电话。请帮我查询供应商表中的电话");
    //    //strings.Add("MMS_Supplier是供应商表:LawPerson的含义是法人代表。请帮我查询供应商表中的法人代表");
    //    //strings.Add("MMS_Supplier是供应商表:PostCode的含义是邮政编码。请帮我查询供应商表中的邮政编码");
    //    //strings.Add("MMS_Supplier是供应商表:Bank的含义是开户银行。请帮我查询供应商表中的开户银行");
    //    //strings.Add("MMS_Supplier是供应商表:Account的含义是帐号。请帮我查询供应商表中的帐号");
    //    //strings.Add("MMS_Supplier是供应商表:TaxId的含义是税号。请帮我查询供应商表中的税号");
    //    //strings.Add("MMS_Supplier是供应商表:InvoiceTitle的含义是发票抬头。请帮我查询供应商表中的发票抬头");
    //    //strings.Add("MMS_Supplier是供应商表:Fax的含义是传真。请帮我查询供应商表中的传真");
    //    //strings.Add("MMS_Supplier是供应商表:WebSite的含义是网址。请帮我查询供应商表中的网址");
    //    //strings.Add("MMS_Receipt是物料入库表:PurchaseOrderNumber的含义是采购单号。请帮我查询物料入库表中的采购单号");
    //    //strings.Add("MMS_Receipt是物料入库表:ExternReceiptNumber的含义是外部单号。请帮我查询物料入库表中的外部单号");
    //    //strings.Add("MMS_Receipt是物料入库表:SupplierName的含义是供应商名称。请帮我查询物料入库表中的供应商名称");
    //    //strings.Add("MMS_Receipt是物料入库表:WarehouseName的含义是仓库名称。请帮我查询物料入库表中的仓库名称");
    //    //strings.Add("MMS_Receipt是物料入库表:ReceiptTime的含义是入库单时间。请帮我查询物料入库表中的入库单时间");
    //    //strings.Add("MMS_Receipt是物料入库表:ReceiptType的含义是订单类型。请帮我查询物料入库表中的订单类型");
    //    //strings.Add("MMS_Receipt是物料入库表:Contact的含义是联系人。请帮我查询物料入库表中的联系人");
    //    //strings.Add("MMS_Receipt是物料入库表:ContactInfo的含义是联系方式。请帮我查询物料入库表中的联系方式");
    //    //strings.Add("MMS_Receipt是物料入库表:Remark的含义是备注。请帮我查询物料入库表中的备注");
    //    //strings.Add("MMS_ReceiptDetail是物料入库明细表:SKU的含义是SKU。请帮我查询物料入库明细表中的SKU");
    //    //strings.Add("MMS_ReceiptDetail是物料入库明细表:UPC的含义是UPC。请帮我查询物料入库明细表中的UPC");
    //    //strings.Add("MMS_ReceiptDetail是物料入库明细表:GoodsType的含义是产品等级。请帮我查询物料入库明细表中的产品等级");
    //    //strings.Add("MMS_ReceiptDetail是物料入库明细表:GoodsName的含义是产品名称。请帮我查询物料入库明细表中的产品名称");
    //    //strings.Add("MMS_ReceiptDetail是物料入库明细表:BoxCode的含义是箱号。请帮我查询物料入库明细表中的箱号");
    //    //strings.Add("MMS_ReceiptDetail是物料入库明细表:TrayCode的含义是托号。请帮我查询物料入库明细表中的托号");
    //    //strings.Add("MMS_ReceiptDetail是物料入库明细表:BatchCode的含义是批次号。请帮我查询物料入库明细表中的批次号");
    //    //strings.Add("MMS_ReceiptDetail是物料入库明细表:ExpectedQty的含义是预收数量。请帮我查询物料入库明细表中的预收数量");
    //    //strings.Add("MMS_ReceiptDetail是物料入库明细表:ReceivedQty的含义是实收数量。请帮我查询物料入库明细表中的实收数量");
    //    //strings.Add("MMS_ReceiptDetail是物料入库明细表:ReceiptQty的含义是入库数量。请帮我查询物料入库明细表中的入库数量");
    //    //strings.Add("MMS_ReceiptDetail是物料入库明细表:UnitCode的含义是单位。请帮我查询物料入库明细表中的单位");
    //    //strings.Add("MMS_ReceiptDetail是物料入库明细表:Onwer的含义是所属。请帮我查询物料入库明细表中的所属");
    //    //strings.Add("MMS_ReceiptDetail是物料入库明细表:ProductionDate的含义是生产日期。请帮我查询物料入库明细表中的生产日期");
    //    //strings.Add("MMS_ReceiptDetail是物料入库明细表:ExpirationDate的含义是过期日期。请帮我查询物料入库明细表中的过期日期");
    //    //strings.Add("MMS_ReceiptDetail是物料入库明细表:Remark的含义是备注。请帮我查询物料入库明细表中的备注");
    //    //strings.Add("MMS_Material是物料产品表:SupplierName的含义是供应商名称。请帮我查询物料产品表中的供应商名称");
    //    //strings.Add("MMS_Material是物料产品表:SKU的含义是SKU。请帮我查询物料产品表中的SKU");
    //    //strings.Add("MMS_Material是物料产品表:GoodsName的含义是产品名称。请帮我查询物料产品表中的产品名称");
    //    //strings.Add("MMS_Material是物料产品表:GoodsType的含义是产品类型。请帮我查询物料产品表中的产品类型");
    //    //strings.Add("MMS_Material是物料产品表:SKUClassification的含义是产品分类。请帮我查询物料产品表中的产品分类");
    //    //strings.Add("MMS_Material是物料产品表:SKULevel的含义是产品等级。请帮我查询物料产品表中的产品等级");
    //    //strings.Add("MMS_Material是物料产品表:SKUGroup的含义是产品分组。请帮我查询物料产品表中的产品分组");
    //    //strings.Add("MMS_Material是物料产品表:ManufacturerSKU的含义是制造商SKU。请帮我查询物料产品表中的制造商SKU");
    //    //strings.Add("MMS_Material是物料产品表:RetailSKU的含义是零售商SKU。请帮我查询物料产品表中的零售商SKU");
    //    //strings.Add("MMS_Material是物料产品表:ReplaceSKU的含义是可替换SKU。请帮我查询物料产品表中的可替换SKU");
    //    //strings.Add("MMS_Material是物料产品表:BoxGroup的含义是箱组。请帮我查询物料产品表中的箱组");
    //    //strings.Add("MMS_Material是物料产品表:Country的含义是国家。请帮我查询物料产品表中的国家");
    //    //strings.Add("MMS_Material是物料产品表:Manufacturer的含义是制造商。请帮我查询物料产品表中的制造商");
    //    //strings.Add("MMS_Material是物料产品表:DangerCode的含义是危险等级。请帮我查询物料产品表中的危险等级");
    //    //strings.Add("MMS_Material是物料产品表:Volume的含义是体积。请帮我查询物料产品表中的体积");
    //    //strings.Add("MMS_Material是物料产品表:StandardVolume的含义是标准体积。请帮我查询物料产品表中的标准体积");
    //    //strings.Add("MMS_Material是物料产品表:Weight的含义是重量。请帮我查询物料产品表中的重量");
    //    //strings.Add("MMS_Material是物料产品表:StandardWeight的含义是标准重量。请帮我查询物料产品表中的标准重量");
    //    //strings.Add("MMS_Material是物料产品表:NetWeight的含义是净重。请帮我查询物料产品表中的净重");
    //    //strings.Add("MMS_Material是物料产品表:StandardNetWeight的含义是标准净重。请帮我查询物料产品表中的标准净重");
    //    //strings.Add("MMS_Material是物料产品表:Price的含义是价格。请帮我查询物料产品表中的价格");
    //    //strings.Add("MMS_Material是物料产品表:ActualPrice的含义是实际价格。请帮我查询物料产品表中的实际价格");
    //    //strings.Add("MMS_Material是物料产品表:Cost的含义是成本。请帮我查询物料产品表中的成本");
    //    //strings.Add("MMS_Material是物料产品表:Color的含义是颜色。请帮我查询物料产品表中的颜色");
    //    //strings.Add("MMS_Material是物料产品表:Length的含义是长。请帮我查询物料产品表中的长");
    //    //strings.Add("MMS_Material是物料产品表:Wide的含义是宽。请帮我查询物料产品表中的宽");
    //    //strings.Add("MMS_Material是物料产品表:High的含义是高。请帮我查询物料产品表中的高");
    //    //strings.Add("MMS_Material是物料产品表:ExpirationDate的含义是过期日期。请帮我查询物料产品表中的过期日期");
    //    //strings.Add("MMS_Material是物料产品表:Remark的含义是备注。请帮我查询物料产品表中的备注");
    //    //strings.Add("MMS_ReceiptReceiving是物料上架主表:PurchaseOrderNumber的含义是采购单号。请帮我查询物料上架主表中的采购单号");
    //    //strings.Add("MMS_ReceiptReceiving是物料上架主表:ExternReceiptNumber的含义是外部单号。请帮我查询物料上架主表中的外部单号");
    //    //strings.Add("MMS_ReceiptReceiving是物料上架主表:SupplierName的含义是供应商名称。请帮我查询物料上架主表中的供应商名称");
    //    //strings.Add("MMS_ReceiptReceiving是物料上架主表:ReceiptReceivingStartTime的含义是上架开始时间。请帮我查询物料上架主表中的上架开始时间");
    //    //strings.Add("MMS_ReceiptReceiving是物料上架主表:ReceiptReceivingEndTime的含义是上架结束时间。请帮我查询物料上架主表中的上架结束时间");
    //    //strings.Add("MMS_ReceiptReceiving是物料上架主表:ReceiptReceivingStatus的含义是上架状态。请帮我查询物料上架主表中的上架状态");
    //    //strings.Add("MMS_ReceiptReceiving是物料上架主表:ReceiptReceivingType的含义是上架类型。请帮我查询物料上架主表中的上架类型");
    //    //strings.Add("MMS_ReceiptReceiving是物料上架主表:CompleteTime的含义是完成时间。请帮我查询物料上架主表中的完成时间");
    //    //strings.Add("MMS_ReceiptReceivingDetail是物料上架明细表:SKU的含义是SKU。请帮我查询物料上架明细表中的SKU");
    //    //strings.Add("MMS_ReceiptReceivingDetail是物料上架明细表:UPC的含义是UPC。请帮我查询物料上架明细表中的UPC");
    //    //strings.Add("MMS_ReceiptReceivingDetail是物料上架明细表:GoodsType的含义是产品等级。请帮我查询物料上架明细表中的产品等级");
    //    //strings.Add("MMS_ReceiptReceivingDetail是物料上架明细表:GoodsName的含义是产品名称。请帮我查询物料上架明细表中的产品名称");
    //    //strings.Add("MMS_ReceiptReceivingDetail是物料上架明细表:BoxCode的含义是箱号。请帮我查询物料上架明细表中的箱号");
    //    //strings.Add("MMS_ReceiptReceivingDetail是物料上架明细表:TrayCode的含义是托号。请帮我查询物料上架明细表中的托号");
    //    //strings.Add("MMS_ReceiptReceivingDetail是物料上架明细表:BatchCode的含义是批次号。请帮我查询物料上架明细表中的批次号");
    //    //strings.Add("MMS_ReceiptReceivingDetail是物料上架明细表:LotCode的含义是Lot。请帮我查询物料上架明细表中的Lot");
    //    //strings.Add("MMS_ReceiptReceivingDetail是物料上架明细表:PoCode的含义是Po。请帮我查询物料上架明细表中的Po");
    //    //strings.Add("MMS_ReceiptReceivingDetail是物料上架明细表:Weight的含义是重量。请帮我查询物料上架明细表中的重量");
    //    //strings.Add("MMS_ReceiptReceivingDetail是物料上架明细表:Volume的含义是体积。请帮我查询物料上架明细表中的体积");
    //    //strings.Add("MMS_ReceiptReceivingDetail是物料上架明细表:ExpectedQty的含义是预计数量。请帮我查询物料上架明细表中的预计数量");
    //    //strings.Add("MMS_ReceiptReceivingDetail是物料上架明细表:ReceivedQty的含义是收货数量。请帮我查询物料上架明细表中的收货数量");
    //    //strings.Add("MMS_ReceiptReceivingDetail是物料上架明细表:ReceiptQty的含义是入库数量。请帮我查询物料上架明细表中的入库数量");
    //    //strings.Add("MMS_ReceiptReceivingDetail是物料上架明细表:UnitCode的含义是单位。请帮我查询物料上架明细表中的单位");
    //    //strings.Add("MMS_ReceiptReceivingDetail是物料上架明细表:Onwer的含义是所属。请帮我查询物料上架明细表中的所属");
    //    //strings.Add("MMS_ReceiptReceivingDetail是物料上架明细表:Area的含义是库区。请帮我查询物料上架明细表中的库区");
    //    //strings.Add("MMS_ReceiptReceivingDetail是物料上架明细表:Location的含义是库位。请帮我查询物料上架明细表中的库位");
    //    //strings.Add("MMS_ReceiptReceivingDetail是物料上架明细表:GoodsStatus的含义是产品状态。请帮我查询物料上架明细表中的产品状态");
    //    //strings.Add("MMS_ReceiptReceivingDetail是物料上架明细表:ProductionDate的含义是生产日期。请帮我查询物料上架明细表中的生产日期");
    //    //strings.Add("MMS_ReceiptReceivingDetail是物料上架明细表:ExpirationDate的含义是过期日期。请帮我查询物料上架明细表中的过期日期");
    //    //strings.Add("MMS_Inventory_Usable是物料可用库存表:SupplierName的含义是供应商名称。请帮我查询物料可用库存表中的供应商名称");
    //    //strings.Add("MMS_Inventory_Usable是物料可用库存表:WarehouseName的含义是仓库名称。请帮我查询物料可用库存表中的仓库名称");
    //    //strings.Add("MMS_Inventory_Usable是物料可用库存表:Area的含义是库区。请帮我查询物料可用库存表中的库区");
    //    //strings.Add("MMS_Inventory_Usable是物料可用库存表:Location的含义是库位。请帮我查询物料可用库存表中的库位");
    //    //strings.Add("MMS_Inventory_Usable是物料可用库存表:SKU的含义是SKU。请帮我查询物料可用库存表中的SKU");
    //    //strings.Add("MMS_Inventory_Usable是物料可用库存表:UPC的含义是UPC。请帮我查询物料可用库存表中的UPC");
    //    //strings.Add("MMS_Inventory_Usable是物料可用库存表:GoodsType的含义是产品类型。请帮我查询物料可用库存表中的产品类型");
    //    //strings.Add("MMS_Inventory_Usable是物料可用库存表:InventoryStatus的含义是库存状态。请帮我查询物料可用库存表中的库存状态");
    //    //strings.Add("MMS_Inventory_Usable是物料可用库存表:GoodsName的含义是产品名称。请帮我查询物料可用库存表中的产品名称");
    //    //strings.Add("MMS_Inventory_Usable是物料可用库存表:UnitCode的含义是单位。请帮我查询物料可用库存表中的单位");
    //    //strings.Add("MMS_Inventory_Usable是物料可用库存表:Onwer的含义是所属。请帮我查询物料可用库存表中的所属");
    //    //strings.Add("MMS_Inventory_Usable是物料可用库存表:BoxCode的含义是箱号。请帮我查询物料可用库存表中的箱号");
    //    //strings.Add("MMS_Inventory_Usable是物料可用库存表:TrayCode的含义是托号。请帮我查询物料可用库存表中的托号");
    //    //strings.Add("MMS_Inventory_Usable是物料可用库存表:BatchCode的含义是批次号。请帮我查询物料可用库存表中的批次号");
    //    //strings.Add("MMS_Inventory_Usable是物料可用库存表:Qty的含义是数量。请帮我查询物料可用库存表中的数量");
    //    //strings.Add("MMS_Inventory_Usable是物料可用库存表:ProductionDate的含义是生产日期。请帮我查询物料可用库存表中的生产日期");
    //    //strings.Add("MMS_Inventory_Usable是物料可用库存表:ExpirationDate的含义是过期日期。请帮我查询物料可用库存表中的过期日期");
    //    //strings.Add("MMS_Inventory_Usable是物料可用库存表:Remark的含义是备注。请帮我查询物料可用库存表中的备注");
    //    //strings.Add("MMS_Inventory_Usable是物料可用库存表:InventoryTime的含义是入库时间。请帮我查询物料可用库存表中的入库时间");
    //    //strings.Add("WMS_OrderAddress是出货地址:CompanyName的含义是联系人公司名称。请帮我查询出货地址中的联系人公司名称");
    //    //strings.Add("WMS_OrderAddress是出货地址:AddressTag的含义是标签。请帮我查询出货地址中的标签");
    //    //strings.Add("WMS_Warehouse是仓库表:County的含义是地区。请帮我查询仓库表中的地区");
    //    //strings.Add("WMS_Warehouse是仓库表:Country的含义是国家。请帮我查询仓库表中的国家");
    //    //strings.Add("WMS_OrderAddress是出货地址:Country的含义是国家。请帮我查询出货地址中的国家");
    //    //strings.Add("WMS_WarehouseDetail是仓库明细表:CustomerName的含义是客户名称。请帮我查询仓库明细表中的客户名称");
    //    //strings.Add("WMS_WarehouseDetail是仓库明细表:Contact的含义是联系人。请帮我查询仓库明细表中的联系人");
    //    //strings.Add("WMS_WarehouseDetail是仓库明细表:Tel的含义是联系电话。请帮我查询仓库明细表中的联系电话");
    //    //strings.Add("WMS_WarehouseDetail是仓库明细表:Phone的含义是手机。请帮我查询仓库明细表中的手机");
    //    //strings.Add("WMS_WarehouseDetail是仓库明细表:Email的含义是邮箱。请帮我查询仓库明细表中的邮箱");
    //    //strings.Add("WMS_PickTask是拣货任务表:ExternOrderNumber的含义是外部单号。请帮我查询拣货任务表中的外部单号");
    //    //strings.Add("WMS_PickTask是拣货任务表:OrderNumber的含义是出库单号。请帮我查询拣货任务表中的出库单号");
    //    //strings.Add("WMS_ASNForReceiptDetail是预入库转入库明细:SKU的含义是SKU。请帮我查询预入库转入库明细中的SKU");
    //    //strings.Add("WMS_ASNForReceiptDetail是预入库转入库明细:GoodsType的含义是产品等级。请帮我查询预入库转入库明细中的产品等级");
    //    //strings.Add("WMS_ASNForReceiptDetail是预入库转入库明细:GoodsName的含义是产品名称。请帮我查询预入库转入库明细中的产品名称");
    //    //strings.Add("WMS_ASNForReceiptDetail是预入库转入库明细:BoxCode的含义是箱号。请帮我查询预入库转入库明细中的箱号");
    //    //strings.Add("WMS_ASNForReceiptDetail是预入库转入库明细:TrayCode的含义是托盘号。请帮我查询预入库转入库明细中的托盘号");
    //    //strings.Add("WMS_ASNForReceiptDetail是预入库转入库明细:BatchCode的含义是批次号。请帮我查询预入库转入库明细中的批次号");
    //    //strings.Add("WMS_ASNForReceiptDetail是预入库转入库明细:LotCode的含义是Lot。请帮我查询预入库转入库明细中的Lot");
    //    //strings.Add("WMS_ASNForReceiptDetail是预入库转入库明细:Weight的含义是重量。请帮我查询预入库转入库明细中的重量");
    //    //strings.Add("WMS_ASNForReceiptDetail是预入库转入库明细:Volume的含义是体积。请帮我查询预入库转入库明细中的体积");
    //    //strings.Add("WMS_ASNForReceiptDetail是预入库转入库明细:ExpectedQty的含义是期望数量。请帮我查询预入库转入库明细中的期望数量");
    //    //strings.Add("WMS_ASNForReceiptDetail是预入库转入库明细:ReceivedQty的含义是实收数量。请帮我查询预入库转入库明细中的实收数量");
    //    //strings.Add("WMS_ASNForReceiptDetail是预入库转入库明细:ReceiptQty的含义是入库数量。请帮我查询预入库转入库明细中的入库数量");
    //    //strings.Add("WMS_ASNForReceiptDetail是预入库转入库明细:ForReceiptQty的含义是转入库数量。请帮我查询预入库转入库明细中的转入库数量");
    //    //strings.Add("WMS_ASNForReceiptDetail是预入库转入库明细:UnitCode的含义是单位。请帮我查询预入库转入库明细中的单位");
    //    //strings.Add("CustomerConfig是客户配置表:CustomerLogo的含义是客户Logo。请帮我查询客户配置表中的客户Logo");
    //    //strings.Add("CustomerConfig是客户配置表:PrintShippingTemplate的含义是发货单模板。请帮我查询客户配置表中的发货单模板");
    //    //strings.Add("WMS_PreOrderExtend是预入库扩展:ShippingAttachmentsUrl的含义是发货单文件。请帮我查询预入库扩展中的发货单文件");
    //    //strings.Add("WMS_ExpressConfig是快递配置表:CustomerName的含义是客户名称。请帮我查询快递配置表中的客户名称");
    //    //strings.Add("WMS_ExpressConfig是快递配置表:WarehouseName的含义是仓库名称。请帮我查询快递配置表中的仓库名称");
    //    //strings.Add("WMS_ExpressConfig是快递配置表:ExpressCode的含义是快递Code。请帮我查询快递配置表中的快递Code");
    //    //strings.Add("WMS_ExpressConfig是快递配置表:ExpressCompany的含义是快递公司。请帮我查询快递配置表中的快递公司");
    //    //strings.Add("WMS_ExpressConfig是快递配置表:Url的含义是请求地址。请帮我查询快递配置表中的请求地址");
    //    //strings.Add("WMS_ExpressConfig是快递配置表:UrlToken的含义是请求Token地址。请帮我查询快递配置表中的请求Token地址");
    //    //strings.Add("WMS_ExpressConfig是快递配置表:AppKey的含义是密钥。请帮我查询快递配置表中的密钥");
    //    //strings.Add("WMS_ExpressConfig是快递配置表:CompanyCode的含义是CompanyCode。请帮我查询快递配置表中的CompanyCode");
    //    //strings.Add("WMS_ExpressConfig是快递配置表:Sign的含义是签名。请帮我查询快递配置表中的签名");
    //    //strings.Add("WMS_ExpressConfig是快递配置表:CustomerCode的含义是客户代码。请帮我查询快递配置表中的客户代码");
    //    //strings.Add("WMS_ExpressConfig是快递配置表:PartnerId的含义是伙伴id。请帮我查询快递配置表中的伙伴id");
    //    //strings.Add("WMS_ExpressConfig是快递配置表:Checkword的含义是伙伴密钥。请帮我查询快递配置表中的伙伴密钥");
    //    //strings.Add("WMS_ExpressConfig是快递配置表:ClientId的含义是ClientId。请帮我查询快递配置表中的ClientId");
    //    //strings.Add("WMS_ExpressConfig是快递配置表:Version的含义是Version。请帮我查询快递配置表中的Version");
    //    //strings.Add("WMS_ExpressConfig是快递配置表:TemplateCode的含义是模板Code。请帮我查询快递配置表中的模板Code");
    //    //strings.Add("WMS_ExpressConfig是快递配置表:Password的含义是Password。请帮我查询快递配置表中的Password");
    //    //strings.Add("WMS_ExpressConfig是快递配置表:MonthAccount的含义是月结账号。请帮我查询快递配置表中的月结账号");
    //    //strings.Add("WMS_ExpressConfig是快递配置表:Env的含义是环境。请帮我查询快递配置表中的环境");
    //    //strings.Add("WMS_ASN是预入库:Po的含义是Po。请帮我查询预入库中的Po");
    //    //strings.Add("WMS_ASN是预入库:So的含义是So。请帮我查询预入库中的So");
    //    //strings.Add("WMS_Order是出库表:Po的含义是Po。请帮我查询出库表中的Po");
    //    //strings.Add("WMS_Order是出库表:So的含义是So。请帮我查询出库表中的So");
    //    //strings.Add("WMS_PreOrder是预出库表:Po的含义是Po。请帮我查询预出库表中的Po");
    //    //strings.Add("WMS_PreOrder是预出库表:So的含义是So。请帮我查询预出库表中的So");
    //    //strings.Add("WMS_Receipt是入库表:Po的含义是Po。请帮我查询入库表中的Po");
    //    //strings.Add("WMS_Receipt是入库表:So的含义是So。请帮我查询入库表中的So");
    //    //strings.Add("WMS_PickTaskDetail是拣货任务明细表:PackageQty的含义是拣货数量。请帮我查询拣货任务明细表中的拣货数量");
    //    //strings.Add("WMS_StockCheck是盘点主表:ExternNumber的含义是外部单号。请帮我查询盘点主表中的外部单号");
    //    //strings.Add("WMS_StockCheck是盘点主表:CustomerName的含义是客户名称。请帮我查询盘点主表中的客户名称");
    //    //strings.Add("WMS_StockCheck是盘点主表:WarehouseName的含义是仓库名称。请帮我查询盘点主表中的仓库名称");
    //    //strings.Add("WMS_StockCheck是盘点主表:StockCheckDate的含义是盘点时间。请帮我查询盘点主表中的盘点时间");
    //    //strings.Add("WMS_StockCheck是盘点主表:StockCheckType的含义是盘点类型。请帮我查询盘点主表中的盘点类型");
    //    //strings.Add("WMS_StockCheck是盘点主表:ToCheckUser的含义是指定盘点人。请帮我查询盘点主表中的指定盘点人");
    //    //strings.Add("WMS_StockCheck是盘点主表:ToCheckAccount的含义是指定盘点账户。请帮我查询盘点主表中的指定盘点账户");
    //    //strings.Add("WMS_StockCheck是盘点主表:Remark的含义是备注。请帮我查询盘点主表中的备注");
    //    //strings.Add("WMS_StockCheckDetail是盘点主明细表:Area的含义是库区。请帮我查询盘点主明细表中的库区");
    //    //strings.Add("WMS_StockCheckDetail是盘点主明细表:Location的含义是库位。请帮我查询盘点主明细表中的库位");
    //    //strings.Add("WMS_StockCheckDetail是盘点主明细表:SKU的含义是SKU。请帮我查询盘点主明细表中的SKU");
    //    //strings.Add("WMS_StockCheckDetail是盘点主明细表:UPC的含义是UPC。请帮我查询盘点主明细表中的UPC");
    //    //strings.Add("WMS_StockCheckDetail是盘点主明细表:GoodsType的含义是产品类型。请帮我查询盘点主明细表中的产品类型");
    //    //strings.Add("WMS_StockCheckDetail是盘点主明细表:GoodsName的含义是产品名称。请帮我查询盘点主明细表中的产品名称");
    //    //strings.Add("WMS_StockCheckDetail是盘点主明细表:UnitCode的含义是单位。请帮我查询盘点主明细表中的单位");
    //    //strings.Add("WMS_StockCheckDetail是盘点主明细表:Onwer的含义是所属。请帮我查询盘点主明细表中的所属");
    //    //strings.Add("WMS_StockCheckDetail是盘点主明细表:BoxCode的含义是箱号。请帮我查询盘点主明细表中的箱号");
    //    //strings.Add("WMS_StockCheckDetail是盘点主明细表:TrayCode的含义是托号。请帮我查询盘点主明细表中的托号");
    //    //strings.Add("WMS_StockCheckDetail是盘点主明细表:BatchCode的含义是批次号。请帮我查询盘点主明细表中的批次号");
    //    //strings.Add("WMS_StockCheckDetail是盘点主明细表:LotCode的含义是Lot号。请帮我查询盘点主明细表中的Lot号");
    //    //strings.Add("WMS_StockCheckDetail是盘点主明细表:PoCode的含义是Po。请帮我查询盘点主明细表中的Po");
    //    //strings.Add("WMS_StockCheckDetail是盘点主明细表:ProductionDate的含义是生成日期。请帮我查询盘点主明细表中的生成日期");
    //    //strings.Add("WMS_StockCheckDetail是盘点主明细表:ExpirationDate的含义是过期日期。请帮我查询盘点主明细表中的过期日期");
    //    //strings.Add("WMS_StockCheckDetail是盘点主明细表:InventoryQty的含义是库存数量。请帮我查询盘点主明细表中的库存数量");
    //    //strings.Add("WMS_StockCheckDetail是盘点主明细表:CheckQty的含义是盘点数量。请帮我查询盘点主明细表中的盘点数量");
    //    //strings.Add("WMS_StockCheckDetail是盘点主明细表:Remark的含义是备注。请帮我查询盘点主明细表中的备注");
    //    //strings.Add("WMS_StockCheckDetail是盘点主明细表:StockCheckStatus的含义是盘点状态。请帮我查询盘点主明细表中的盘点状态");
    //    //strings.Add("WMS_StockCheckDetail是盘点主明细表:CheckUser的含义是盘点人。请帮我查询盘点主明细表中的盘点人");
    //    //strings.Add("WMS_StockCheckDetail是盘点主明细表:CheckAccount的含义是盘点账号。请帮我查询盘点主明细表中的盘点账号");
    //    //strings.Add("WMS_StockCheckDetail是盘点主明细表:StockCheckTime的含义是盘点时间。请帮我查询盘点主明细表中的盘点时间");


    //    string htmlContent = "<chart-view content=\"{&quot;type&quot;: &quot;Data display method&quot;, &quot;sql&quot;: &quot;SELECT * FROM wms.student&quot;, &quot;data&quot;: [{&quot;Id&quot;: 1, &quot;name&quot;: &quot;张三&quot;, &quot;age&quot;: 2, &quot;fenShu&quot;: 50, &quot;class&quot;: &quot;数学&quot;},{&quot;Id&quot;: 1, &quot;name&quot;: &quot;张三&quot;, &quot;age&quot;: 2, &quot;fenShu&quot;: 50, &quot;class&quot;: &quot;数学&quot;}]}\" />";

    //    // 使用正则表达式匹配content属性的值  
    //    string pattern = @"content=""([^""]*)""";
    //    Match match = Regex.Match(htmlContent, pattern);

    //    string jsonContent = match.Groups[1].Value;
    //    // 将HTML实体转换为普通字符串（C#中没有直接的HtmlDecode方法像Web.HttpUtility那样，但我们可以手动替换）  
    //    jsonContent = HttpUtility.HtmlDecode(jsonContent); // 注意：这通常需要在Web项目中添加System.Web引用，或在.NET Core/5/6+中使用其他方法，如WebUtility.HtmlDecode  
    //    string _html = ""; 
    //    string _htmlhead = ""; 
    //    string _htmlbody = ""; 
    //    var jsonObj = JsonConvert.DeserializeObject<chatJson>(jsonContent);
    //     var jsonObjStr ="";
    //    foreach (var item in jsonObj.data)
    //    {
    //        _htmlhead = "<tr>";
    //        _htmlbody+="<tr>";
    //        jsonObjStr +=item.ToString();
    //        //string patterns = @"{}";
    //        //Match matchs = Regex.Match(item, patterns);
    //        string input = "";
    //        //// 去除最外层的非标准大括号  
    //        input = item.ToString().TrimStart('{').TrimEnd('}');

    //        // 将JSON字符串转换为Dictionary
    //        Dictionary<string, object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>("{"+input+"}");

    //        // 打印Dictionary的键值对
    //        foreach (var kvp in dict)
    //        {
    //            _htmlhead+="<td>"+kvp.Key+"</td>";
    //            _htmlbody+="<td>"+kvp.Value+"</td>";
    //        }
    //        _htmlhead += "</tr>";
    //        _htmlbody += "</tr>";
    //    }
    //    _html="<table>" + _htmlhead + "" + _htmlbody + "</table>";
        
    //    Messages.Add(new MessageModel
    //    {
    //        Msg = _html,
    //        Sender = "Alice",
    //        BackgroundColor = Colors.White,
    //        IsDataTable = true
    //    });
        
    //    Msg = ""; 
    //}
    //private void SendButton()
    //{
    //    // 发送消息逻辑  
    //    var message = new MessageModel
    //    {
    //        Mag = "Hello, I am Bob.",
    //        Sender = "You",
    //        BackgroundColor = Colors.LightBlue
    //    };

    //    Messages.Add(message);
    //    //MessageEntry.Text = ""; // 清空输入框  
    //}

    public async virtual void Initialize()
    {
        

        //IsBusy = true;
        //Messages = new ObservableCollection<MessageModel>
        //{  
        //    // 初始消息  
        //    new MessageModel {   Msg = "Hello!", Sender = "Alice", BackgroundColor = Colors.White,    IsString = true },
        //    //new MessageModel {   Mag = "Hello!", Sender = "Alice", BackgroundColor = Colors.LightGray },
        //    //new MessageModel {   Mag = "Hello!", Sender = "Alice", BackgroundColor = Colors.LightGray },
        //    //new MessageModel {   Mag = "Hello!", Sender = "Alice", BackgroundColor = Colors.LightGray }
        //};
        //IsBusy = false;
    }
}

//public class Messagesss
//{
//    public string select_param { get; set; }
//    public string chat_mode { get; set; }
//    public string model_name { get; set; }
//    public string user_input { get; set; }
//    public string conv_uid { get; set; }
//}

//public class chatJson
//{
//    public string type { get; set; }
//    public string sql { get; set; }

//    public List<dynamic> data { get; set; }
//    //public decimal data { get; set; } 
//}
//public class student
//{
//    public string id { get; set; }

//    public string name { get; set; }

//    public string age { get; set; }
//    public string fenShu { get; set; }

//    //public decimal data { get; set; } 
//}
