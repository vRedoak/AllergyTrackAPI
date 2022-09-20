using Microsoft.EntityFrameworkCore.Migrations;
using Infrastructure.Extensions;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddNotificationCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {            
           migrationBuilder.RunSqlScript( "20220906194037_AddNotificationCategories");         
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
