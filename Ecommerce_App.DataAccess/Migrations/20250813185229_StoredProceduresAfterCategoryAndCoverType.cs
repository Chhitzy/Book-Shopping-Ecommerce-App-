using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_App.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class StoredProceduresAfterCategoryAndCoverType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Product Stored Procedures
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_CreateProduct
                                @name varchar(35) 
                                AS
                                    insert products values(@name)");
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_UpdateProduct
                                @name varchar(35),
                                @id int
                                AS
                                update products set name = @name where id =@id");
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_DeleteProduct
                                @id int 
                                AS
                                delete from products where id=@id");
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_GetProducts
                                AS 
                                Select * From products");
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_GetProduct
                                @id int
                                AS
                                Select * From products where id=@id");

            //company

           //New stored procedures can't be applied through new migrations

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
