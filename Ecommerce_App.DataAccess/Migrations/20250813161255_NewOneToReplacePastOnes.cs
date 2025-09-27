using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_App.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class NewOneToReplacePastOnes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Category Stored Procedures

            migrationBuilder.Sql(@"CREATE PROCEDURE SP_CreateCategory
                                @name varchar(35) 
                                AS
                                    insert category values(@name)");
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_UpdateCategory
                                @name varchar(35),
                                @id int
                                AS
                                update category set name = @name where id =@id");
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_DeleteCategory
                                @id int 
                                AS
                                delete from category where id=@id");
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_GetCategories
                                AS 
                                Select * From category");
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_GetCategory
                                @id int
                                AS
                                Select * From category where id=@id");

            //covertypes Stored Procedures

            migrationBuilder.Sql(@"CREATE PROCEDURE SP_Createcovertypes
                                @name varchar(35) 
                                AS
                                    insert covertypes values(@name)");
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_Updatecovertypes
                                @name varchar(35),
                                @id int
                                AS
                                update covertypes set name = @name where id =@id");
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_Deletecovertypes
                                @id int 
                                AS
                                delete from covertypes where id=@id");
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_Getcovertypes
                                AS 
                                Select * From covertypes");
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_Getcovertype
                                @id int
                                AS
                                Select * From covertypes where id=@id");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
