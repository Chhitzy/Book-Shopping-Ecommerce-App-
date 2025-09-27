using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_App.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class GottaAddCompanyStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_CreateCompany
                                   @city varchar(40),
                                @streetaddress varchar(100),
                                @postalcode varchar(6),
                                @IsAuthorizedCompany bit,
                                @state varchar(30),
                                @phonenumber varchar(20),
                                @name varchar(35) 
                                AS
                                    insert into companies (name, streetaddress,city,state,postalcode,phonenumber,isauthorizedcompany)

                                    values(@name,@streetaddress,@city,@state,@postalcode,@phonenumber,@IsAuthorizedCompany)");
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_UpdateCompany
                               
                                @id int,
                                @city varchar(40),
                                @streetaddress varchar(100),
                                @postalcode varchar(6),
                                @IsAuthorizedCompany bit,
                                @state varchar(30),
                                @phonenumber varchar(20),
                                @name varchar(35) 
                                AS
                                    update  companies set 

                                        Name = @name,
                                        city = @city,
                                        streetaddress = @streetaddress,
                                        postalcode = @postalcode,
                                        state = @state,
                                        phonenumber = @phonenumber,
                                        isauthorizedcompany = @IsAuthorizedCompany
                                            where id= @id
                                    ");

            migrationBuilder.Sql(@"CREATE PROCEDURE SP_DeleteCompany
                                @id int 
                                AS
                                delete from companies where id=@id");
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_GetCompanies
                                AS 
                                Select * From companies");
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_GetCompany
                                @id int
                                AS
                                Select * From companies where id=@id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
