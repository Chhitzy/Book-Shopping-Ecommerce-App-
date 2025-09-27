using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_App.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class GetAllStoredProcedureForOrderHeader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"Create Procedure GetAllOrderHeader
                                    @id nvarchar(450)
                                        AS
                                    Select * From OrderHeader
                                    
                                   
                                    
                                  where @id = ApplicationUserId

            
            
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
