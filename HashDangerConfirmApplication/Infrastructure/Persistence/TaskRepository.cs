using SqlSugar;
using System.Data;
using TaskPlaApplication.Domain;
using TaskPlaApplication.Domain.Entities;


namespace TaskPlaApplication.Infrastructure.Persistence;
public sealed class TaskRepository : ITaskRepository
{
    private readonly ISqlSugarClient _db;
    public TaskRepository(ISqlSugarClient db) => _db = db;
    public async Task<List<WMSInstruction>> GetPendingAsync(int take, CancellationToken ct = default)
    {
        var IBTypesToFilter = new List<string>   // 使用字符串值进行比较
                        {
                            InstructionType.入库单回传HachDG.ToString(),
                            //InstructionType.出库单回传HachDG.ToString(),
                            InstructionType.入库单序列号回传HachDG.ToString(),
                            //InstructionType.出库单序列号回传HachDG.ToString(),
                            //InstructionType.出库单防伪码回传HachDG.ToString(),
                            //InstructionType.出库装箱回传HachDG.ToString()
                        };
        var OBTypesToFilter = new List<string>   // 使用字符串值进行比较
                        {
                            //InstructionType.入库单回传HachDG.ToString(),
                            InstructionType.出库单回传HachDG.ToString(),
                            //InstructionType.入库单序列号回传HachDG.ToString(),
                            InstructionType.出库单序列号回传HachDG.ToString(),
                            InstructionType.出库单防伪码回传HachDG.ToString(),
                            InstructionType.出库装箱回传HachDG.ToString()
                        };
        var now = DateTime.Now;

        string sqlString = $@"SELECT distinct TOP 100 
                              [TenantId],[Id],[CustomerId],[CustomerName],[WarehouseId],
                              [WarehouseName],[TableName],[InstructionType],[BusinessType],
                              [OperationId],[InstructionStatus],[OrderNumber],
                              [InstructionTaskNo],[InstructionPriority],[Message],
                              [Remark],[Creator],[CreationTime],[UpdationTime] 
                              FROM [WMS_Instruction]
                              WHERE (( [InstructionStatus] = @Pending ) )
                              AND ([InstructionType] IN ('入库单回传HachDG','入库单序列号回传HachDG'))
                              AND InstructionTaskNo IN (select OrderNo from hach_wms_receiving)
                              UNION ALL
                              SELECT distinct TOP 100 
                              [TenantId],[Id],[CustomerId],[CustomerName],[WarehouseId],
                              [WarehouseName],[TableName],[InstructionType],[BusinessType],
                              [OperationId],[InstructionStatus],[OrderNumber],
                              [InstructionTaskNo],[InstructionPriority],[Message],
                              [Remark],[Creator],[CreationTime],[UpdationTime] 
                              FROM [WMS_Instruction]
                              WHERE (( [InstructionStatus] =  @Pending)) 
                              AND ([InstructionType] IN ('出库装箱回传HachDG'))
                              AND InstructionTaskNo IN (select DeliveryNumber from hach_wms_outBound)
                              UNION ALL
                              SELECT distinct TOP 100 
                              [TenantId],[Id],[CustomerId],[CustomerName],[WarehouseId],
                              [WarehouseName],[TableName],[InstructionType],[BusinessType],
                              [OperationId],[InstructionStatus],[OrderNumber],
                              [InstructionTaskNo],[InstructionPriority],[Message],
                              [Remark],[Creator],[CreationTime],[UpdationTime] 
                              FROM [WMS_Instruction]
                              WHERE (( [InstructionStatus] =  @Pending or InstructionStatus=63)) 
                              AND ([InstructionType] IN ('出库单回传HachDG','出库单序列号回传HachDG','出库单防伪码回传HachDG'))
                              AND InstructionTaskNo IN (select OrderNumber from hach_wms_outBound)
                              order by InstructionPriority";

        ////测试某一单
        //string sqlString = $@"SELECT distinct TOP 100 
        //                         [TenantId],[Id],[CustomerId],[CustomerName],[WarehouseId],
        //                         [WarehouseName],[TableName],[InstructionType],[BusinessType],
        //                         [OperationId],[InstructionStatus],[OrderNumber],
        //                         [InstructionTaskNo],[InstructionPriority],[Message],
        //                         [Remark],[Creator],[CreationTime],[UpdationTime] 
        //                         FROM [WMS_Instruction]
        //                         WHERE id in (31181)order by InstructionPriority";

        // 参数
        var parameters = new
        {
            Pending = 1,  // 使用枚举转换为字符串
            now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),  // 格式化日期
            //IBTypesToFilter = "'入库单回传HachDG','入库单序列号回传HachDG'",  // 数组转换为逗号分隔的字符串
            //OBTypesToFilter = "'出库单回传HachDG','出库单序列号回传HachDG','出库单防伪码回传HachDG','出库装箱回传HachDG'" // 数组转换为逗号分隔的字符串
        };

        var result = _db.Ado.GetDataTable(sqlString, parameters);
        // 手动将 DataTable 转换为 List<WMSInstruction>
        var list = new List<WMSInstruction>();
        foreach (DataRow row in result.Rows)
        {
            var wmsInstruction = new WMSInstruction
            {
                TenantId = Convert.ToInt64(row["TenantId"].ToString()),
                Id = Convert.ToInt32(row["Id"]),
                CustomerId = Convert.ToInt64(row["CustomerId"].ToString()),
                CustomerName = row["CustomerName"].ToString(),
                WarehouseId = Convert.ToInt64(row["WarehouseId"].ToString()),
                WarehouseName = row["WarehouseName"].ToString(),
                TableName = row["TableName"].ToString(),
                BusinessType = row["BusinessType"].ToString(),
                OperationId = Convert.ToInt64(row["OperationId"].ToString()),
                InstructionStatus = Enum.TryParse<InstructionStatus>(row["InstructionStatus"].ToString(), out var status)
               ? status : InstructionStatus.Succeeded, // 默认值为 Pending
                InstructionTypeEnum = Enum.TryParse<InstructionType>(row["BusinessType"].ToString(), out var type)
               ? type : InstructionType.未知, // 默认值为 Pending
                OrderNumber = row["OrderNumber"].ToString(),
                InstructionTaskNo = row["InstructionTaskNo"].ToString(),
                InstructionPriority = Convert.ToInt32(row["InstructionPriority"]),
                Message = row["Message"].ToString(),
                Remark = row["Remark"].ToString(),
                Creator = row["Creator"].ToString(),
                CreationTime = Convert.ToDateTime(row["CreationTime"]),
                UpdationTime = row["UpdationTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["UpdationTime"])
            };
            list.Add(wmsInstruction);
        }
        return list;
    }
    public async Task UpdateAsync(WMSInstruction item, InstructionStatus statusCode,CancellationToken ct = default)
    {
        try
        {
            await _db.Updateable<WMSInstruction>()
              .SetColumns(a => a.InstructionStatus == statusCode)      // ✅ 改这里
                 .SetColumns(a => a.Message == item.Message)
                 .SetColumns(a => a.UpdationTime == DateTime.Now)
                 .Where(a => a.InstructionStatus == item.InstructionStatus)
                 .Where(a => a.InstructionPriority == item.InstructionPriority)
                 .Where(a => a.InstructionTypeCode == item.InstructionTypeCode)
                 .Where(a => a.InstructionTaskNo == item.InstructionTaskNo)
                 .Where(a => a.OrderNumber == item.OrderNumber)
                 //.Where(a => a.Id == item.Id)
                 .ExecuteCommandAsync();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
