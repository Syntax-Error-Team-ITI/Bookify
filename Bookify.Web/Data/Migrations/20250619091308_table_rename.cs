using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class table_rename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Subscripers_SubscriberId",
                table: "Rentals");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscripers_Areas_AreaId",
                table: "Subscripers");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscripers_AspNetUsers_CreatedById",
                table: "Subscripers");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscripers_AspNetUsers_LastUpdatedById",
                table: "Subscripers");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscripers_Governorates_GovernorateId",
                table: "Subscripers");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Subscripers_SubscriberId",
                table: "Subscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscripers",
                table: "Subscripers");

            migrationBuilder.RenameTable(
                name: "Subscripers",
                newName: "Subscribers");

            migrationBuilder.RenameIndex(
                name: "IX_Subscripers_NationalId",
                table: "Subscribers",
                newName: "IX_Subscribers_NationalId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscripers_MobileNumber",
                table: "Subscribers",
                newName: "IX_Subscribers_MobileNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Subscripers_LastUpdatedById",
                table: "Subscribers",
                newName: "IX_Subscribers_LastUpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Subscripers_GovernorateId",
                table: "Subscribers",
                newName: "IX_Subscribers_GovernorateId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscripers_Email",
                table: "Subscribers",
                newName: "IX_Subscribers_Email");

            migrationBuilder.RenameIndex(
                name: "IX_Subscripers_CreatedById",
                table: "Subscribers",
                newName: "IX_Subscribers_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Subscripers_AreaId",
                table: "Subscribers",
                newName: "IX_Subscribers_AreaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscribers",
                table: "Subscribers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Subscribers_SubscriberId",
                table: "Rentals",
                column: "SubscriberId",
                principalTable: "Subscribers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribers_Areas_AreaId",
                table: "Subscribers",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribers_AspNetUsers_CreatedById",
                table: "Subscribers",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribers_AspNetUsers_LastUpdatedById",
                table: "Subscribers",
                column: "LastUpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribers_Governorates_GovernorateId",
                table: "Subscribers",
                column: "GovernorateId",
                principalTable: "Governorates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Subscribers_SubscriberId",
                table: "Subscriptions",
                column: "SubscriberId",
                principalTable: "Subscribers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Subscribers_SubscriberId",
                table: "Rentals");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscribers_Areas_AreaId",
                table: "Subscribers");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscribers_AspNetUsers_CreatedById",
                table: "Subscribers");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscribers_AspNetUsers_LastUpdatedById",
                table: "Subscribers");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscribers_Governorates_GovernorateId",
                table: "Subscribers");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Subscribers_SubscriberId",
                table: "Subscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscribers",
                table: "Subscribers");

            migrationBuilder.RenameTable(
                name: "Subscribers",
                newName: "Subscripers");

            migrationBuilder.RenameIndex(
                name: "IX_Subscribers_NationalId",
                table: "Subscripers",
                newName: "IX_Subscripers_NationalId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscribers_MobileNumber",
                table: "Subscripers",
                newName: "IX_Subscripers_MobileNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Subscribers_LastUpdatedById",
                table: "Subscripers",
                newName: "IX_Subscripers_LastUpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Subscribers_GovernorateId",
                table: "Subscripers",
                newName: "IX_Subscripers_GovernorateId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscribers_Email",
                table: "Subscripers",
                newName: "IX_Subscripers_Email");

            migrationBuilder.RenameIndex(
                name: "IX_Subscribers_CreatedById",
                table: "Subscripers",
                newName: "IX_Subscripers_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Subscribers_AreaId",
                table: "Subscripers",
                newName: "IX_Subscripers_AreaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscripers",
                table: "Subscripers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Subscripers_SubscriberId",
                table: "Rentals",
                column: "SubscriberId",
                principalTable: "Subscripers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscripers_Areas_AreaId",
                table: "Subscripers",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscripers_AspNetUsers_CreatedById",
                table: "Subscripers",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscripers_AspNetUsers_LastUpdatedById",
                table: "Subscripers",
                column: "LastUpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscripers_Governorates_GovernorateId",
                table: "Subscripers",
                column: "GovernorateId",
                principalTable: "Governorates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Subscripers_SubscriberId",
                table: "Subscriptions",
                column: "SubscriberId",
                principalTable: "Subscripers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
