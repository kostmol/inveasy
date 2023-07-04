using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inveasy.Migrations
{
    /// <inheritdoc />
    public partial class AddModelChangesByTony : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleUser");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "UniqueName",
                table: "Role");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Role",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "RewardTier",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "RewardTier",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "Donation",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "View",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_View", x => x.Id);
                    table.ForeignKey(
                        name: "FK_View_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_View_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_View_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Role_UserId",
                table: "Role",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RewardTier_ProjectId",
                table: "RewardTier",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_RewardTier_RoleId",
                table: "RewardTier",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Donation_RoleId",
                table: "Donation",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_View_ProjectId",
                table: "View",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_View_RoleId",
                table: "View",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_View_UserId",
                table: "View",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Donation_Role_RoleId",
                table: "Donation",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RewardTier_Project_ProjectId",
                table: "RewardTier",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RewardTier_Role_RoleId",
                table: "RewardTier",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Role_User_UserId",
                table: "Role",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donation_Role_RoleId",
                table: "Donation");

            migrationBuilder.DropForeignKey(
                name: "FK_RewardTier_Project_ProjectId",
                table: "RewardTier");

            migrationBuilder.DropForeignKey(
                name: "FK_RewardTier_Role_RoleId",
                table: "RewardTier");

            migrationBuilder.DropForeignKey(
                name: "FK_Role_User_UserId",
                table: "Role");

            migrationBuilder.DropTable(
                name: "View");

            migrationBuilder.DropIndex(
                name: "IX_Role_UserId",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_RewardTier_ProjectId",
                table: "RewardTier");

            migrationBuilder.DropIndex(
                name: "IX_RewardTier_RoleId",
                table: "RewardTier");

            migrationBuilder.DropIndex(
                name: "IX_Donation_RoleId",
                table: "Donation");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "RewardTier");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "RewardTier");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Donation");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Role",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UniqueName",
                table: "Role",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "RoleUser",
                columns: table => new
                {
                    RolesId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.RolesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_RoleUser_Role_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUser_User_UsersId",
                        column: x => x.UsersId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_UsersId",
                table: "RoleUser",
                column: "UsersId");
        }
    }
}
