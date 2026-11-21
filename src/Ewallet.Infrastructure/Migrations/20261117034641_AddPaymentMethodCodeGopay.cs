using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ewallet.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentMethodCodeGopay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_PaymentMethod_Code",
                schema: "dbo",
                table: "PaymentMethods");

            migrationBuilder.AddCheckConstraint(
                name: "CK_PaymentMethod_Code",
                schema: "dbo",
                table: "PaymentMethods",
                sql: "[Code] in ('BANK_TRANSFER', 'QRIS', 'GoPay')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_PaymentMethod_Code",
                schema: "dbo",
                table: "PaymentMethods");

            migrationBuilder.AddCheckConstraint(
                name: "CK_PaymentMethod_Code",
                schema: "dbo",
                table: "PaymentMethods",
                sql: "[Code] in ('BANK_TRANSFER', 'QRIS')");
        }
    }
}
