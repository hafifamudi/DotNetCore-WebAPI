using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebService.Infrastructure.Migrations
{
    public partial class AlterDateTimeToDateTimeOffset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE ""Employees""
                ALTER COLUMN ""EntryDate"" SET DATA TYPE TIMESTAMP WITH TIME ZONE;

                ALTER TABLE ""Employees""
                ALTER COLUMN ""UpdatedDate"" SET DATA TYPE TIMESTAMP WITH TIME ZONE;

                ALTER TABLE ""Departments""
                ALTER COLUMN ""EntryDate"" SET DATA TYPE TIMESTAMP WITH TIME ZONE;

                ALTER TABLE ""Departments""
                ALTER COLUMN ""UpdatedDate"" SET DATA TYPE TIMESTAMP WITH TIME ZONE;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE ""Employees""
                ALTER COLUMN EntryDate SET DATA TYPE DateTime;

                ALTER TABLE ""Employees""
                ALTER COLUMN UpdatedDate SET DATA TYPE DateTime;

                ALTER TABLE ""Departments""
                ALTER COLUMN EntryDate SET DATA TYPE DateTime;

                ALTER TABLE ""Departments""
                ALTER COLUMN UpdatedDate SET DATA TYPE DateTime;
            ");
        }
    }
}
