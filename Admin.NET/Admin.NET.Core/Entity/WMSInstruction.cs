namespace Admin.NET.Core.Entity;

/// <summary>
/// 指令
/// </summary>
[SugarTable("WMS_Instruction", "指令")]
[SystemTable]
public class WMSInstruction //: IEntityNotKey
{


    /// <summary>
    /// Id
    /// </summary>
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public virtual long Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public long? CustomerId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? CustomerName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public long? WarehouseId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? WarehouseName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? TableName { get; set; }



    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? InstructionType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? BusinessType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public long OperationId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public int InstructionStatus { get; set; }


    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public string InstructionTaskNo { get; set; }


    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public int InstructionPriority { get; set; }


    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public string Message { get; set; }


    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public string Remark { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Creator { get; set; }

    /// <summary>
    /// 
    /// </summary>
    //[SugarColumn(ColumnDescription = "", Length = 50)]
    //public long CreatorId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public DateTime? CreationTime { get; set; }

}
