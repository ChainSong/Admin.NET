using TaskPlaApplication.Domain;
namespace TaskPlaApplication.Domain
{
    public class HachWmsOutBoundConfirm
    {
        public string soNumber { get; set; }
        public string deliveryNum { get; set; }
    public List<HachWmsOutBoundDetailConfirm> items { get; set; }
    }
    public class HachWmsOutBoundDetailConfirm 
    {
        public long deliveryDetailId { get; set; }
        public string lineNum { get; set; }
        public long itemId { get; set; }
        public string itemNum { get; set; }
        public string itemDescription { get; set; }
        public string organizationCode { get; set; }
        public float shippedQuantity { get; set; }
        public string uom { get; set; }
        public string? attribute1 { get; set; }
        public string? attribute2 { get; set; }
        public string? attribute3 { get; set; }
        public string? attribute4 { get; set; }
        public string? attribute5 { get; set; }
        public string? attribute6 { get; set; }
        public string? attribute7 { get; set; }
        public string? attribute8 { get; set; }
        public string? attribute9 { get; set; }
        public string? attribute10 { get; set; }
        public string? subinventory { get; set; }
        public string? shipConfirmDate { get; set; }

        public long? organizationId { get; set; }
    }
    public class HachWmsOutBoundSNConfirm
    {
        public string docNumber { get; set; }
        public string docType { get; set; }
        public string lineNum { get; set; }
        public string delivery { get; set; }
        public string organizationCode { get; set; }
        public long organizationId { get; set; }
        public string subinventory { get; set; }
        public long? itemId { get; set; }
        public string itemNum { get; set; }
        public float quantity { get; set; }
        public string containerNo { get; set; }
        public string lsnNumber { get; set; }
        public string? expirationDate { get; set; }
        public string scanDate { get; set; }
        public long? transactionId { get; set; }
        public long? deliveryDetailId { get; set; }
        public string attribute1 { get; set; }
        public string? attribute2 { get; set; }
        public string attribute3 { get; set; }
        public string attribute4 { get; set; }
        public string attribute5 { get; set; }
        public string attribute6 { get; set; }
        public string attribute7 { get; set; }
        public string attribute8 { get; set; }
        public string attribute9 { get; set; }
        public string attribute10 { get; set; }

    }

    #region 包装
    public class HachWmsOutBoundPickingConfirm
    {
        //job号：发货运单号SOReference3
        public string deliveryNum { get; set; }
        //托总数量：未打托则传0
        public long palletQty { get; set; }
        //系统箱总数量
        public long cartonQty { get; set; }
        //箱或托总体积：单位:立方厘米
        public double totalVol { get; set; }
        //箱或托总总量：单位:公斤
        public double totalWeight { get; set; }
        //箱托对象
        public List<cartonPallet> cartonPallets { get; set; }
    }
    //箱托信息
    public class cartonPallet
    {
        //箱编号
        public string cartonNum { get; set; }
        //箱体积：单位:立方厘米
        public double? cartonVol { get; set; }
        //箱总量：单位:公斤
        public double? cartonWeight { get; set; }
        //当前箱号对应实际物理箱数
        public string cartonNumQuantity { get; set; }
        //托编号：未打托则传空
        public string palletNum { get; set; }
        //托体积：单位:立方厘米
        public long palletVol { get; set; }
        //托总量：单位:公斤
        public long palletWeight { get; set; }
        public List<items> items { get; set; }
    }
    public class items
    {
        //物料编码：SKU
        public string itemNum { get; set; }
        //发货数量：QtyOrdered
        public double quantity { get; set; }
        //发货明细ID：Dedi12
        public long deliveryDetailId { get; set; }
    }
    #endregion

    public class OutboundResponseData
    {
        public string message { get; set; }
        public bool success { get; set; }
        public string code { get; set; }
    }

    public class HachWmsOutBoundAFCConfirm
    {
        public string docNumber { get; set; }
        public string docType { get; set; }
        public string lineNum { get; set; }
        public string delivery { get; set; }
        public string organizationCode { get; set; }
        public long organizationId { get; set; }
        public string subinventory { get; set; }
        public long? itemId { get; set; }
        public string itemNum { get; set; }
        public float quantity { get; set; }
        public string containerNo { get; set; }
        public string lsnNumber { get; set; }
        public string? expirationDate { get; set; }
        public string scanDate { get; set; }
        public long? transactionId { get; set; }
        public long? deliveryDetailId { get; set; }
        public string attribute1 { get; set; }
        public string? attribute2 { get; set; }
        public string attribute3 { get; set; }
        public string attribute4 { get; set; }
        public string attribute5 { get; set; }
        public string attribute6 { get; set; }
        public string attribute7 { get; set; }
        public string attribute8 { get; set; }
        public string attribute9 { get; set; }
        public string attribute10 { get; set; }

    }

}
