
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Enumerate;
using Admin.NET.Application.Interface;
using Admin.NET.Common.SnowflakeCommon;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using AngleSharp.Dom;
using Furion.DistributedIDGenerator;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Service;

namespace Admin.NET.Application.Strategy
{
    public class AdjustmentConfirmFrozenDefaultStrategy : IAdjustmentConfirmInterface
    {

        //注入数据库实例
        //public ISqlSugarClient _db { get; set; }

        //注入权限仓储
        public UserManager _userManager { get; set; }
        //注入Adjustment仓储

        public SqlSugarRepository<WMSAdjustment> _repAdjustment { get; set; }
        //注入AdjustmentDetail仓储
        public SqlSugarRepository<WMSAdjustmentDetail> _repAdjustmentDetail { get; set; }
        //注入仓库关系仓储

        public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
        //注入客户关系仓储
        public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }

        public AdjustmentConfirmFrozenDefaultStrategy()
        {
        }

        //public Task<Response<List<OrderStatusDto>>> Strategy(List<AddWMSAdjustmentInput> request)

        /// <summary>
        /// 默认方法不做任何处理
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<List<OrderStatusDto>>> Strategy(List<long> request)
        {

            Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };

            //开始校验数据
            List<OrderStatusDto> orderStatus = new List<OrderStatusDto>();
            //1判断PreOrder 是否已经存在已有的订单

            var AdjustmentCheck = _repAdjustment.AsQueryable().Where(a => request.Contains(a.Id));

            if (AdjustmentCheck == null && AdjustmentCheck.ToList().Count > 0)
            {

                AdjustmentCheck.ForEach(a =>
                {
                    if (a.AdjustmentStatus != (int)AdjustmentStatusEnum.新增)
                    {
                        response.Data.Add(new OrderStatusDto
                        {
                            ExternOrder = a.ExternNumber,
                            SystemOrder = a.AdjustmentNumber,
                            Type = a.AdjustmentType,
                            StatusCode = StatusCode.Warning,
                            //StatusMsg = StatusCode.warning.ToString(),
                            Msg = "订单状态异常"
                        });
                    }
                });
                if (response.Data.Count > 0)
                {
                    response.Code = StatusCode.Error;
                    response.Msg = "订单异常";
                    return response;
                }
            }
            List<WMSAdjustmentInformationDto> AdjustmentInfo = new List<WMSAdjustmentInformationDto>();


            foreach (var a in request)
            {
                var sugarParameter = new SugarParameter("@AdjustmentId", a, typeof(long), ParameterDirection.Input);
                DataTable AdjustmentInfoData = await _repAdjustment.Context.Ado.UseStoredProcedure().GetDataTableAsync("Proc_WMS_AdjustmentConfirmFrozen", sugarParameter);
                if (AdjustmentInfoData != null && AdjustmentInfoData.Rows.Count > 0)
                {
                    AdjustmentInfo.AddRange(AdjustmentInfoData.TableToList<WMSAdjustmentInformationDto>());
                }
            }
            if (AdjustmentInfo.Count > 0)
            {
                AdjustmentInfo.ForEach(a =>
                {
                    response.Data.Add(new OrderStatusDto
                    {
                        ExternOrder = a.OrderNumber,
                        SystemOrder = a.OrderNumber,
                        Type = a.InformationType,
                        StatusCode = StatusCode.Error,
                        //StatusMsg = StatusCode.warning.ToString(),
                        Msg = "订单移库失败:sku" + a.SKU + ",订单数量：" + a.Qty + ",库存数量：" + a.InventoryQty
                    });
                });

                response.Code = StatusCode.Error;
                response.Msg = "操作失败";
                return response;
            }

            var AdjustmentData = _repAdjustment.AsQueryable().Where(a => request.Contains(a.Id));
            //var ysxx = await _db.Ado.UseStoredProcedure().GetDataTableAsync("exec Proc_WMS_AutomatedOutbound ", sugarParameter);



            //if (AdjustmentCheck.AdjustmentStatus != (int)AdjustmentStatusEnum.新增)
            //{
            //    response.Data.Add(new OrderStatusDto
            //    {
            //        ExternOrder = AdjustmentCheck.ExternNumber,
            //        SystemOrder = AdjustmentCheck.AdjustmentNumber,
            //        Type = AdjustmentCheck.AdjustmentType,
            //        StatusCode = StatusCode.Warning,
            //        //StatusMsg = StatusCode.warning.ToString(),
            //        Msg = "订单状态异常"
            //    });
            //    response.Code = StatusCode.Error;
            //    response.Msg = "订单状态异常";
            //    return response;
            //}

            //var AdjustmentData = request.Adapt<List<WMSAdjustment>>();



            ////var AdjustmentData = mapper.Map<List<WMS_Adjustment>>(request);
            //int LineNumber = 1;
            //AdjustmentData.ForEach(item =>
            //    {
            //        var CustomerId = _repCustomerUser.AsQueryable().Where(b => b.CustomerName == item.CustomerName).First().CustomerId;
            //        var WarehouseId = _repWarehouseUser.AsQueryable().Where(b => b.WarehouseName == item.WarehouseName).First().WarehouseId;

            //        var AdjustmentNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString();
            //        //ShortIDGen.NextID(new GenerationOptions
            //        //{
            //        //    Length = 10// 设置长度，注意：不设置次长度是随机长度！！！！！！！
            //        //});// 生成一个包含数字，字母，不包含特殊符号的 8 位短id
            //        item.AdjustmentNumber = AdjustmentNumber;
            //        item.ExternNumber = item.ExternNumber;
            //        item.CustomerId = CustomerId;
            //        item.WarehouseId = WarehouseId;
            //        item.Creator = _userManager.Account;
            //        item.CreationTime = DateTime.Now;
            //        item.AdjustmentStatus = (int)AdjustmentStatusEnum.新增;
            //        item.Details.ForEach(a =>
            //            {
            //            a.AdjustmentNumber = AdjustmentNumber;
            //            item.ExternNumber = item.ExternNumber;
            //            a.CustomerId = CustomerId;
            //            a.CustomerName = item.CustomerName;
            //            a.WarehouseId = WarehouseId;
            //            a.WarehouseName = item.WarehouseName;
            //            a.AdjustmentNumber = item.AdjustmentNumber;
            //            a.Creator = _userManager.Account;
            //            a.CreationTime = DateTime.Now;
            //        });
            //        LineNumber++;
            //    });

            //////开始插入订单
            //await _db.InsertNav(AdjustmentData).Include(a => a.Details).ExecuteCommandAsync();


            AdjustmentData.ForEach(a =>
            {
                //if (a.AdjustmentStatus != (int)AdjustmentStatusEnum.新增)
                //{
                response.Data.Add(new OrderStatusDto
                {
                    ExternOrder = a.ExternNumber,
                    SystemOrder = a.AdjustmentNumber,
                    Type = a.AdjustmentType,
                    StatusCode = a.AdjustmentStatus == (int)AdjustmentStatusEnum.完成 ? StatusCode.Success : StatusCode.Error,
                    //StatusMsg = StatusCode.warning.ToString(),
                    Msg = a.AdjustmentStatus == (int)AdjustmentStatusEnum.完成 ? "订单变更成功" : "订单变更失败",

                });
                //}
            });


            response.Code = StatusCode.Success;
            response.Msg = "操作成功";
            return response;

        }

        private string AdjustmentConfirmFrozenSql = @" BEGIN try
          BEGIN TRANSACTION;
DECLARE @IId bigint		   
DECLARE @ICustomerId bigint	   
DECLARE @IWarehouseId bigint	   
DECLARE @IArea VARCHAR(50)		   
DECLARE @ILocation VARCHAR(50)		   
DECLARE @ISKU VARCHAR(100)		   
DECLARE @IUPC VARCHAR(100)		   
DECLARE @IGoodsType VARCHAR(50)		   
DECLARE @IInventoryStatus int		   
DECLARE @IUnitCode VARCHAR(100)		   
DECLARE @IOnwer VARCHAR(100)		   
DECLARE @IBoxCode VARCHAR(100)		   
DECLARE @ITrayCode VARCHAR(100)		   
DECLARE @IBatchCode VARCHAR(100)		   
DECLARE @IQty decimal(18,2)		   
DECLARE @IProductionDate VARCHAR(50)		   
DECLARE @IExpirationDate VARCHAR(50)		   

DECLARE @Id  bigint	   	   
DECLARE @AdjustmentNumber  VARCHAR(50)		   
DECLARE @ExternNumber  VARCHAR(50)		   
DECLARE @CustomerId  bigint	   
DECLARE @WarehouseId bigint	   
DECLARE @SKU  VARCHAR(100)		   
DECLARE @UPC  VARCHAR(100)		   
DECLARE @TrayCode  VARCHAR(100)		   
DECLARE @BatchCode  VARCHAR(100)		   
DECLARE @BoxCode  VARCHAR(100)		 	   
DECLARE @FromWarehouseName  VARCHAR(50)		   
DECLARE @ToWarehouseName  VARCHAR(50)		   
DECLARE @FromArea  VARCHAR(50)		   
DECLARE @ToArea  VARCHAR(50)		   
DECLARE @FromLocation  VARCHAR(50)		   
DECLARE @ToLocation  VARCHAR(50)		   
DECLARE @Qty    decimal(18,2)	 
DECLARE @FromOnwer  VARCHAR(50)		   
DECLARE @ToOnwer  VARCHAR(50)		   
DECLARE @FromGoodsType  VARCHAR(50)		   
DECLARE @ToGoodsType  VARCHAR(50)		   
DECLARE @FromUnitCode  VARCHAR(100)		   
DECLARE @ToUnitCode   VARCHAR(100)		    


DECLARE @NumberRemaining FLOAT

          --------------------------获取调整明细------------------------------------------------------
          DECLARE WMS_AdjustmentDetail CURSOR FOR --定义游标
             select Id     	   
,AdjustmentNumber   
,ExternNumber  	
,CustomerId  
,WarehouseId  	   
,SKU   		   
,UPC   		   
,TrayCode   		   
,BatchCode   		
,BoxCode   		 	
,FromWarehouseName  
,ToWarehouseName   
,FromArea   
,ToArea   	   
,FromLocation 	
,ToLocation   	
,Qty   
,FromOnwer
,ToOnwer
,FromGoodsType 	
,ToGoodsType   	
,FromUnitCode   		
,ToUnitCode    		 from  WMS_AdjustmentDetail where AdjustmentId=AdjustmentId

      OPEN cursor_adjustmentDetail --打开游标

      FETCH next FROM cursor_adjustmentDetail INTO @Id     	   
,@AdjustmentNumber   
,@ExternNumber  	
,@CustomerId  
,@WarehouseId  	   
,@SKU   		   
,@UPC   		   
,@TrayCode   		   
,@BatchCode   		
,@BoxCode   		 	
,@FromWarehouseName  
,@ToWarehouseName   
,@FromArea   
,@ToArea   	   
,@FromLocation 	
,@ToLocation   	
,@Qty    		
,@FromOnwer
,@ToOnwer
,@FromGoodsType 	
,@ToGoodsType   	
,@FromUnitCode   		
,@ToUnitCode    		--抓取下一行游标数据

      WHILE @@FETCH_STATUS = 0
        BEGIN
		  set  @NumberRemaining=@Qty
            -------------------------获取移动的库存数据------------------------------------------------
            DECLARE cursor_inventory CURSOR FOR --定义游标
              SELECT  Id 		   
             ,CustomerId 	   
             ,WarehouseId 	   
             ,Area 	   
             ,Location 	    
             ,SKU 		   
             ,UPC	   
             ,GoodsType    
             ,InventoryStatus  		   
             ,UnitCode    
             ,Onwer     
             ,BoxCode  	   
             ,TrayCode  		   
             ,BatchCode     
             ,Qty  	   
             ,ProductionDate    
             ,ExpirationDate from WMS_Inventory_Usable 
			 where  
             CustomerId 	    =   @CustomerId 	 
            and WarehouseId 	   	=   @WarehouseId 	
            and Area 	   			=   @FromArea 	   		
            and Location 	    	=   @FromLocation 	    
            and SKU 		   		=   @SKU 		   	
            and UPC	   			    =   @UPC	   		
            and GoodsType    		=   @FromGoodsType    	
            and InventoryStatus  	=	1   
            and UnitCode    		=   @FromUnitCode    	
            and Onwer     			=   @FromOnwer     		
            and BoxCode  	   		=   @BoxCode  	   	
            and TrayCode  		   	=   @TrayCode  		
            and BatchCode    		=   @BatchCode    	
			and Qty>0
  OPEN cursor_inventory --打开游标

  FETCH next FROM cursor_inventory INTO  @IId  		   
,@ICustomerId  	   
,@IWarehouseId  	   
,@IArea     
,@ILocation     
,@ISKU     
,@IUPC     
,@IGoodsType     
,@IInventoryStatus  		   
,@IUnitCode 	   
,@IOnwer 		   
,@IBoxCode  	   
,@ITrayCode   
,@IBatchCode 	   
,@IQty 	   
,@IProductionDate  	   
,@IExpirationDate  
  --抓取下一行游标数据

  WHILE @@FETCH_STATUS = 0
    BEGIN
	if(@NumberRemaining>0)
	begin
        -----------判断当前库存行数量是否满足要求--------------------------
        IF( @NumberRemaining>=@IQty)
          BEGIN

              INSERT INTO WMS_Inventory_Usable
                          ([ReceiptDetailId]
      ,[ReceiptReceivingId]
      ,[CustomerId]
      ,[CustomerName]
      ,[WarehouseId]
      ,[WarehouseName]
      ,[Area]
      ,[Location]
      ,[SKU]
      ,[UPC]
      ,[GoodsType]
      ,[InventoryStatus]
      ,[SuperId]
      ,[RelatedId]
      ,[GoodsName]
      ,[UnitCode]
      ,[Onwer]
      ,[BoxCode]
      ,[TrayCode]
      ,[BatchCode]
      ,[Qty]
      ,[ProductionDate]
      ,[ExpirationDate]
      ,[Remark]
      ,[InventoryTime]
      ,[Creator]
      ,[CreationTime]
      ,[Updator]
      ,[UpdateTime]
      ,[Str1]
      ,[Str2]
      ,[Str3]
      ,[Str4]
      ,[Str5]
      ,[DateTime1]
      ,[DateTime2]
      ,[Int1]
      ,[Int2])
             (select [ReceiptDetailId]
      ,[ReceiptReceivingId]
      ,[CustomerId]
      ,[CustomerName]
      ,[WarehouseId]
      ,[WarehouseName]
      ,@ToArea
      ,@ToLocation
      ,[SKU]
      ,[UPC]
      ,[GoodsType]
      ,20
      ,[SuperId]
      ,[RelatedId]
      ,[GoodsName]
      ,[UnitCode]
      ,[Onwer]
      ,[BoxCode]
      ,[TrayCode]
      ,[BatchCode]
      ,[Qty]
      ,[ProductionDate]
      ,[ExpirationDate]
      ,[Remark]
      ,[InventoryTime]
      ,[Creator]
      ,[CreationTime]
      ,[Updator]
      ,[UpdateTime]
      ,[Str1]
      ,[Str2]
      ,[Str3]
      ,[Str4]
      ,[Str5]
      ,[DateTime1]
      ,[DateTime2]
      ,[Int1]
      ,[Int2] from WMS_Inventory_Usable where Id=@IId)

              UPDATE WMS_Inventory_Usable
              SET    InventoryStatus=99,Qty=0
              WHERE  id = @IId
			      UPDATE WMS_AdjustmentDetail
              SET    ToQty=( ToQty+@IQty)
              WHERE  id = @Id
			  	  set @NumberRemaining=@NumberRemaining - @IQty;
            
          END
        ELSE
          BEGIN
                  INSERT INTO WMS_Inventory_Usable
                          ([ReceiptDetailId]
      ,[ReceiptReceivingId]
      ,[CustomerId]
      ,[CustomerName]
      ,[WarehouseId]
      ,[WarehouseName]
      ,[Area]
      ,[Location]
      ,[SKU]
      ,[UPC]
      ,[GoodsType]
      ,[InventoryStatus]
      ,[SuperId]
      ,[RelatedId]
      ,[GoodsName]
      ,[UnitCode]
      ,[Onwer]
      ,[BoxCode]
      ,[TrayCode]
      ,[BatchCode]
      ,[Qty]
      ,[ProductionDate]
      ,[ExpirationDate]
      ,[Remark]
      ,[InventoryTime]
      ,[Creator]
      ,[CreationTime]
      ,[Updator]
      ,[UpdateTime]
      ,[Str1]
      ,[Str2]
      ,[Str3]
      ,[Str4]
      ,[Str5]
      ,[DateTime1]
      ,[DateTime2]
      ,[Int1]
      ,[Int2])
             (select [ReceiptDetailId]
      ,[ReceiptReceivingId]
      ,[CustomerId]
      ,[CustomerName]
      ,[WarehouseId]
      ,[WarehouseName]
      ,@ToArea
      ,@ToLocation
      ,[SKU]
      ,[UPC]
      ,[GoodsType]
      ,20
      ,[SuperId]
      ,[RelatedId]
      ,[GoodsName]
      ,[UnitCode]
      ,[Onwer]
      ,[BoxCode]
      ,[TrayCode]
      ,[BatchCode]
      ,@NumberRemaining
      ,[ProductionDate]
      ,[ExpirationDate]
      ,[Remark]
      ,[InventoryTime]
      ,[Creator]
      ,[CreationTime]
      ,[Updator]
      ,[UpdateTime]
      ,[Str1]
      ,[Str2]
      ,[Str3]
      ,[Str4]
      ,[Str5]
      ,[DateTime1]
      ,[DateTime2]
      ,[Int1]
      ,[Int2] from WMS_Inventory_Usable where Id=@IId)
	       
	           UPDATE WMS_Inventory_Usable
              SET    Qty=(@NumberRemaining-@IQty)
              WHERE  id = @IId
			    UPDATE WMS_AdjustmentDetail
              SET    ToQty=( ToQty+@NumberRemaining)
              WHERE  id = @Id
			  	  set @NumberRemaining=0;
          END

		end
        FETCH cursor_inventory INTO @IId  		   
,@ICustomerId  	   
,@IWarehouseId  	   
,@IArea     
,@ILocation     
,@ISKU     
,@IUPC     
,@IGoodsType     
,@IInventoryStatus  		   
,@IUnitCode 	   
,@IOnwer 		   
,@IBoxCode  	   
,@ITrayCode   
,@IBatchCode 	   
,@IQty 	   
,@IProductionDate  	   
,@IExpirationDate  
  --抓取下一行游标数据
    END

  CLOSE cursor_inventory --关闭游标

  DEALLOCATE cursor_inventory --释放游标
  IF Cursor_status('local', 'cursor_inventory') <> -3
    BEGIN
        IF Cursor_status('local', 'cursor_inventory') <> -1
          BEGIN
              CLOSE cursor_inventory;

              DEALLOCATE cursor_inventory;
          END;
    END;

  FETCH cursor_adjustmentDetail INTO @Id     	   
,@AdjustmentNumber   
,@ExternNumber  	
,@CustomerId  
,@WarehouseId  	   
,@SKU   		   
,@UPC   		   
,@TrayCode   		   
,@BatchCode   		
,@BoxCode   		 	
,@FromWarehouseName  
,@ToWarehouseName   
,@FromArea   
,@ToArea   	   
,@FromLocation 	
,@ToLocation   	
,@Qty    		
,@FromOnwer
,@ToOnwer
,@FromGoodsType 	
,@ToGoodsType   	
,@FromUnitCode   		
,@ToUnitCode    		--抓取下一行游标数据
  END

  CLOSE cursor_adjustmentDetail --关闭游标

  DEALLOCATE cursor_adjustmentDetail --释放游标

 	 ---当ToQty = Qty 的时候意味着全部处理完成，改变订单状态
	 if((select COUNT(1) from WMS_AdjustmentDetail where AdjustmentId=@AdjustmentId
	 and ToQty<>Qty)>0)
	 begin
	     ---有数据没有处理完成，数据回退
	      ROLLBACK TRANSACTION;
	 end
	  
	 else  begin
	 ------明细全部处理完成，提交
	     update WMS_Adjustment set AdjustmentStatus=99 where id=@AdjustmentId 
		 
	   COMMIT;
	 end
	 

  IF Cursor_status('local', 'cursor_adjustmentDetail') <> -3
  BEGIN
  IF Cursor_status('local', 'cursor_adjustmentDetail') <> -1
    BEGIN
        CLOSE cursor_adjustmentDetail;

        DEALLOCATE cursor_adjustmentDetail;
    END;
  END;
  END try

      BEGIN catch
          IF Xact_state() <> 0
            BEGIN
                ROLLBACK TRANSACTION;
            END;
			 DECLARE @ERRORMessage NVARCHAR(max);

          SELECT @ERRORMessage = Error_message();

          RAISERROR( @ERRORMessage,16,1 );
          CLOSE cursor_inventory;

          DEALLOCATE cursor_inventory;

          CLOSE cursor_adjustmentDetail --关闭游标

          DEALLOCATE cursor_adjustmentDetail --释放游标

         
      END catch;


";

    }
}
