using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_App.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER PROCEDURE SP_CreateProduct
                            @Title varchar(100),
                            @Description varchar(255),
                            @Author varchar(60),
                            @ISBN int,
                            @ListedPrice int,
                            @Price int,
                            @Price50 int,
                            @Price100 int,   
                            @ImageUrl varchar(70) = null,
                            @CategoryId int,
                            @CoverTypeId int
                                      AS
                                    Insert into products (Title,Description,Author,ISBN,ListedPrice,Price,Price50,Price100,ImageUrl,CategoryId,CoverTypeId)

                                    values(@Title,@Description,@Author,@ISBN,@ListedPrice,@Price,@Price50,@Price100,@ImageUrl,@CategoryId,@CoverTypeId)");

            migrationBuilder.Sql(@"ALTER PROCEDURE SP_UpdateProduct
                            @Id int,       
                            @Title varchar(100),
                            @Description varchar(255),
                            @Author varchar(60),
                            @ISBN int,
                            @ListedPrice int,
                            @Price int,
                            @Price50 int,
                            @Price100 int,   
                            @ImageUrl varchar(70) = null,
                            @CategoryId int,
                            @CoverTypeId int
                                    AS
                                update products set     
                            Title= @Title,
                            Description =@Description,
                            Author=@Author ,
                            ISBN=@ISBN ,
                            ListedPrice =@ListedPrice,
                            Price =@Price,
                            Price50 =@Price50,
                            Price100 =@Price100,   
                            ImageUrl =ISNULL(@ImageUrl,ImageUrl),
                            CategoryId=@CategoryId,
                            CoverTypeId=@CoverTypeId where Id=@Id");



                                        
            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
