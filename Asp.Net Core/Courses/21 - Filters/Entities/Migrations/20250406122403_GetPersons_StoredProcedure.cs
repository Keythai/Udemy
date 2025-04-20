using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class GetPersons_StoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPersons = @"
                CREATE OR ALTER PROCEDURE [dbo].[GetAllPersons]
                AS
                BEGIN
                    SELECT PersonId, PersonName, Email, DateOfBirth, Gender, CountryId, Address, ReceiveNewsLetters, TIN FROM Persons;
                END
            ";
            migrationBuilder.Sql(sp_GetAllPersons);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPersons = @"
                DROP PROCEDURE [dbo].[GetAllPersons]
            ";
            migrationBuilder.Sql(sp_GetAllPersons);
            // Afterwards, type in Update-Database in the Package Manager Console to apply the changes, as it detects the newest migrations and run the update
        }
    }
}
