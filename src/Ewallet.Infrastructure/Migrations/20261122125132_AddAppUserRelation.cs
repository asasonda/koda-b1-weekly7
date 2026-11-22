using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ewallet.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddAppUserRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditLogs_Users_ActorUserId",
                schema: "dbo",
                table: "AuditLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_Users_UsersId",
                schema: "dbo",
                table: "Wallets");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "AspNetUsers",
                newSchema: "dbo");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                schema: "dbo",
                table: "Wallets",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Wallets_UsersId",
                schema: "dbo",
                table: "Wallets",
                newName: "IX_Wallets_AppUserId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_AspNetUsers_Status",
                schema: "dbo",
                table: "AspNetUsers",
                sql: "[Status] in ('ACTIVE', 'SUSPENDED')");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLogs_AspNetUsers_ActorUserId",
                schema: "dbo",
                table: "AuditLogs",
                column: "ActorUserId",
                principalSchema: "dbo",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_AspNetUsers_AppUserId",
                schema: "dbo",
                table: "Wallets",
                column: "AppUserId",
                principalSchema: "dbo",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditLogs_AspNetUsers_ActorUserId",
                schema: "dbo",
                table: "AuditLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_AspNetUsers_AppUserId",
                schema: "dbo",
                table: "Wallets");

            migrationBuilder.DropCheckConstraint(
                name: "CK_AspNetUsers_Status",
                schema: "dbo",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                schema: "dbo",
                newName: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                schema: "dbo",
                table: "Wallets",
                newName: "UsersId");

            migrationBuilder.RenameIndex(
                name: "IX_Wallets_AppUserId",
                schema: "dbo",
                table: "Wallets",
                newName: "IX_Wallets_UsersId");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.CheckConstraint("CK_Users_Status", "[Status] in ('ACTIVE', 'SUSPENDED')");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "dbo",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Phone",
                schema: "dbo",
                table: "Users",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                schema: "dbo",
                table: "Users",
                column: "UserName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLogs_Users_ActorUserId",
                schema: "dbo",
                table: "AuditLogs",
                column: "ActorUserId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_Users_UsersId",
                schema: "dbo",
                table: "Wallets",
                column: "UsersId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
