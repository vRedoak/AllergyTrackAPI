using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddNotificationTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RunSqlScript("20220909172348_AddNotificationTypes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
