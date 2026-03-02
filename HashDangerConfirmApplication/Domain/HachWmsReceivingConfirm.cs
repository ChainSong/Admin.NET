namespace TaskPlaApplication.Domain
{
    public class HachWmsReceivingConfirm
    {
        public string receiptNum { get; set; }
        public string deliveryDate { get; set; }
        public string docNumber { get; set; }
        public string docType { get; set; }
        public List<HachWmsReceivingDetailConfirm> items { get; set; }
    }
    public class HachWmsReceivingDetailConfirm
    {
        public string? uom { get; set; }

        public long itemId { get; set; }

        public string? remark { get; set; }

        public string itemNum { get; set; }

        public string lineNum { get; set; }

        public float quantity { get; set; }

        public long transactionId { get; set; }

        public long sourceHeaderId { get; set; }

        public long sourceLineId { get; set; }

        public long shipmentHeaderId { get; set; }

        public long shipmentLineId { get; set; }

        public string organizationCode { get; set; }

        public string? subinventory { get; set; }

        public string? itemDescription { get; set; }

        public string? attribute1 { get; set; }

        public string? attribute2 { get; set; }

        public string? attribute3 { get; set; }

        public string? attribute4 { get; set; }

        public string? attribute5 { get; set; }

        public long organizationId { get; set; }
    }

    public class HachWmsReceivingSNConfirm {
        public string docNumber { get; set; }
        public string docType { get; set; }
        public string lineNum { get; set; }
        public string receiptNum { get; set; }
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
        public string deliveryDetailId { get; set; }
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

    public class ReceivingResponseData
    {
        public string message { get; set; }
        public bool success { get; set; }
        public string code { get; set; }
    }
}
