using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_App.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddorUpdateShopppingCartStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@" CREATE PROCEDURE SP_AddorUpdateShoppingCart
                                        @ApplicationUserId nvarchar(100),
                                        @ProductId int,
                                        @Count int
                                    AS
                                        IF exists (SELECT 1 FROM ShoppingCarts where ApplicationUserId = @ApplicationUserId AND ProductId = @ProductId)
                                        
                                            update ShoppingCarts
                                            set Count = Count + @Count
                                            where ApplicationUserId = @ApplicationUserId AND ProductId = @ProductId;
                                        
                                        ELSE
                                        
                                            insert into ShoppingCarts(ApplicationUserId, ProductId, Count)
                                            values(@ApplicationUserId, @ProductId, @Count);
                                                   ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
