using SqlSugar;
using TaskPlaApplication.Domain;

namespace TaskPlaApplication.Domain.Entities;

/// <summary>
/// 指令
/// </summary>
[SugarTable("WMS_Instruction", "队列表")]
public class WMSInstruction 
{
    [SugarColumn(ColumnDescription = "租户Id", IsOnlyIgnoreUpdate = true)]
    public  long? TenantId { get; set; }
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public  long Id { get; set; }
    [SugarColumn(ColumnDescription = "")]
    public long? CustomerId { get; set; }
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? CustomerName { get; set; }
    [SugarColumn(ColumnDescription = "")]
    public long? WarehouseId { get; set; }
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? WarehouseName { get; set; }
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? TableName { get; set; }
    // ① 映射到数据库真正的字符串列（列名仍叫 InstructionType）
    [SugarColumn(ColumnDescription = "", Length = 50, ColumnName = "InstructionType")]
    public string? InstructionTypeCode { get; set; }
    // ② 代码里用的枚举属性，不参与ORM映射
    [SugarColumn(IsIgnore = true)]
    public InstructionType InstructionTypeEnum
    {
        get => InstructionTypeHelper.TryParse(InstructionTypeCode, out var t) ? t : InstructionType.未知;
        set => InstructionTypeCode = value.ToString(); // 如需存数字可改成 ((int)value).ToString()
    }
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? BusinessType { get; set; }
    [SugarColumn(ColumnDescription = "")]
    public long OperationId { get; set; }
    [SugarColumn(ColumnDescription = "")]
    public InstructionStatus InstructionStatus { get; set; }= InstructionStatus.Pending;
    [SugarColumn(ColumnDescription = "")]
    public string OrderNumber { get; set; }
    [SugarColumn(ColumnDescription = "")]
    public string InstructionTaskNo { get; set; }
    [SugarColumn(ColumnDescription = "")]
    public int InstructionPriority { get; set; }
    [SugarColumn(ColumnDescription = "")]
    public string? Message { get; set; }
    [SugarColumn(ColumnDescription = "")]
    public string? Remark { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Creator { get; set; }
    [SugarColumn(ColumnDescription = "")]
    public DateTime? CreationTime { get; set; }
    [SugarColumn(ColumnDescription = "")]
    public DateTime? UpdationTime { get; set; } = DateTime.Now;
}
