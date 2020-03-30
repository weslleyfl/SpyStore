﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SpyStore.Dal.EfStructures.MigrationHelpers
{
    public static class FunctionsHelper
    {
        public static void CreateOrderTotalFunction(MigrationBuilder migrationBuilder)
        {
            const string sql = @" CREATE FUNCTION Store.GetOrderTotal ( @OrderId INT )
                            RETURNS MONEY WITH SCHEMABINDING
                            BEGIN
                            DECLARE @Result MONEY;
                            SELECT @Result = SUM([Quantity]*[UnitCost]) FROM Store.OrderDetails
                            WHERE OrderId = @OrderId;
                            RETURN coalesce(@Result,0)
                            END";

            migrationBuilder.Sql(sql);

        }

        public static void DropOrderTotalFunction(MigrationBuilder builder)
        {
            builder.Sql("drop function [Store].[GetOrderTotal]");
        }
    }
}
