using Calligraphy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Calligraphy.Infrastructure.Migrations;

[DbContext(typeof(ApplicationDbContext))]
[Migration("20261017120000_AddPaymentMethod")]
public partial class AddPaymentMethod : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "PaymentMethod",
            table: "Payments",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "UPI");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "PaymentMethod", table: "Payments");
    }
}
