using Microsoft.EntityFrameworkCore.Migrations;
using System.Reflection;

namespace Infrastructure.Extensions
{
    public static class MigrationExtensions
    {
        public static void RunSqlScript(this MigrationBuilder migrationBuilder, string script)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames().FirstOrDefault(x => x.EndsWith($"{script}.sql"));
            using var stream = assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream);
            var sqlResult = reader.ReadToEnd();
            migrationBuilder.Sql(sqlResult);
        }
    }
}
