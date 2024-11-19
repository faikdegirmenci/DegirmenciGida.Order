using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DegirmenciGida.Order.Infrastructure.Migrations
{
    public partial class InitialCreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsertDateTime",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "InsertDateTime",
                table: "OrderDetail");

            migrationBuilder.RenameColumn(
                name: "UpdateDateTime",
                table: "Orders",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "IsRecordValid",
                table: "Orders",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "UpdateDateTime",
                table: "OrderDetail",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "IsRecordValid",
                table: "OrderDetail",
                newName: "IsDeleted");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "OrderDetail",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "OrderDetail",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "OrderDetail");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Orders",
                newName: "IsRecordValid");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Orders",
                newName: "UpdateDateTime");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "OrderDetail",
                newName: "IsRecordValid");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "OrderDetail",
                newName: "UpdateDateTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "InsertDateTime",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "InsertDateTime",
                table: "OrderDetail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
