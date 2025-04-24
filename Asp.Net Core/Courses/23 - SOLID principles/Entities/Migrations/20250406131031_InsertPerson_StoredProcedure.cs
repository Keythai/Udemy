using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class InsertPerson_StoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_InsertPerson = @"
                CREATE OR ALTER PROCEDURE [dbo].[InsertPerson]
                (@PersonId UNIQUEIDENTIFIER, 
                @PersonName NVARCHAR(100), 
                @Email NVARCHAR(100), 
                @DateOfBirth DATETIME, 
                @Gender NVARCHAR(10), 
                @CountryId UNIQUEIDENTIFIER, 
                @Address NVARCHAR(200), 
                @ReceiveNewsLetters BIT)
                AS
                BEGIN
                    INSERT INTO [dbo].[Persons](PersonId, PersonName, Email, DateOfBirth, Gender, CountryId, Address, ReceiveNewsLetters)
                    VALUES(@PersonId, @PersonName, @Email, @DateOfBirth, @Gender, @CountryId, @Address, @ReceiveNewsLetters)
                END
            ";
            migrationBuilder.Sql(sp_InsertPerson);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_InsertPerson = @"
                DROP PROCEDURE [dbo].[InsertPerson]
            ";
            migrationBuilder.Sql(sp_InsertPerson);
        }
    }
}
