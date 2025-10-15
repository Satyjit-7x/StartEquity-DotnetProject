using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StartEquity.Migrations
{
    public partial class sellshares : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Rounds_CurrentRoundId1",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_CurrentRoundId1",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "CurrentRoundId1",
                table: "Companies");

            migrationBuilder.AddColumn<string>(
                name: "ShareOfferId",
                table: "Transfers",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CurrentRoundId",
                table: "Companies",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ShareOffers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CompanyId = table.Column<string>(nullable: true),
                    SellerId = table.Column<string>(nullable: true),
                    InvestmentId = table.Column<string>(nullable: true),
                    SharePercentage = table.Column<decimal>(type: "decimal(12,6)", nullable: false),
                    AskingPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentMarketPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    SoldAt = table.Column<DateTime>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    BuyerId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShareOffers_Investors_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "Investors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShareOffers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShareOffers_Investments_InvestmentId",
                        column: x => x.InvestmentId,
                        principalTable: "Investments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShareOffers_Investors_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Investors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_ShareOfferId",
                table: "Transfers",
                column: "ShareOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CurrentRoundId",
                table: "Companies",
                column: "CurrentRoundId");

            migrationBuilder.CreateIndex(
                name: "IX_ShareOffers_BuyerId",
                table: "ShareOffers",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_ShareOffers_CompanyId",
                table: "ShareOffers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ShareOffers_InvestmentId",
                table: "ShareOffers",
                column: "InvestmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ShareOffers_SellerId",
                table: "ShareOffers",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Rounds_CurrentRoundId",
                table: "Companies",
                column: "CurrentRoundId",
                principalTable: "Rounds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_ShareOffers_ShareOfferId",
                table: "Transfers",
                column: "ShareOfferId",
                principalTable: "ShareOffers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Rounds_CurrentRoundId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_ShareOffers_ShareOfferId",
                table: "Transfers");

            migrationBuilder.DropTable(
                name: "ShareOffers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_ShareOfferId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Companies_CurrentRoundId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "ShareOfferId",
                table: "Transfers");

            migrationBuilder.AlterColumn<string>(
                name: "CurrentRoundId",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentRoundId1",
                table: "Companies",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CurrentRoundId1",
                table: "Companies",
                column: "CurrentRoundId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Rounds_CurrentRoundId1",
                table: "Companies",
                column: "CurrentRoundId1",
                principalTable: "Rounds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
