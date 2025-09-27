using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_App.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SearchOperationForCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_GetAllShoppingCartsByUser
                                            @ApplicationUserId varchar(100)
                                                AS
                                            select 
                                                sc.Id AS ShoppingCartId,
                                                sc.Price AS CartPrice,
                                                sc.ProductId,
                                                sc.ApplicationUserId,
        
                                                p.Id AS ProductId,
                                                p.Title,
                                                p.Author,
                                                p.ISBN,
                                                p.ListedPrice,
                                                p.Price,
                                                p.Price50,
                                                p.Price100,
                                                p.CategoryId,
                                                p.CoverTypeId,
                                                p.ImageUrl,
                                                p.Description
        
                                                        from ShoppingCarts sc
                                                        inner join Products p on sc.ProductId = p.Id
                                                        where sc.ApplicationUserId = @ApplicationUserId                           
                            
                                    ");
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_GetOneShoppingCartById
                                            @Id INT
                                        AS
                                            SELECT 
                                                sc.Id AS ShoppingCartId,
                                                sc.Price AS CartPrice,
                                                sc.ProductId,
                                                sc.ApplicationUserId,
        
                                                p.Id AS ProductId,
                                                p.Title,
                                                p.Author,
                                                p.ISBN,
                                                p.ListedPrice,
                                                p.Price,
                                                p.Price50,
                                                p.Price100,
                                                p.CategoryId,
                                                p.CoverTypeId,
                                                p.ImageUrl,
                                                p.Description
        
                                            from ShoppingCarts sc
                                            inner join  Products p ON sc.ProductId = p.Id
                                            where sc.Id = @Id
                                        
                                        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
