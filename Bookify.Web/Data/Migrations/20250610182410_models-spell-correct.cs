using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class modelsspellcorrect : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Subscripers_SubscriperId",
                table: "Rentals");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Subscripers_SubscriperId",
                table: "Subscriptions");

            migrationBuilder.RenameColumn(
                name: "SubscriperId",
                table: "Subscriptions",
                newName: "SubscriberId");

            migrationBuilder.RenameColumn(
                name: "CreateOn",
                table: "Subscriptions",
                newName: "CreatedOn");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_SubscriperId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_SubscriberId");

            migrationBuilder.RenameColumn(
                name: "LasrName",
                table: "Subscripers",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "ImageThumbailUrl",
                table: "Subscripers",
                newName: "ImageThumbnailUrl");

            migrationBuilder.RenameColumn(
                name: "CreateOn",
                table: "Subscripers",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "SubscriperId",
                table: "Rentals",
                newName: "SubscriberId");

            migrationBuilder.RenameColumn(
                name: "CreateOn",
                table: "Rentals",
                newName: "CreatedOn");

            migrationBuilder.RenameIndex(
                name: "IX_Rentals_SubscriperId",
                table: "Rentals",
                newName: "IX_Rentals_SubscriberId");

            migrationBuilder.RenameColumn(
                name: "CreateOn",
                table: "Governorates",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "CreateOn",
                table: "Categories",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "ImageThumbailUrl",
                table: "Books",
                newName: "ImageThumbnailUrl");

            migrationBuilder.RenameColumn(
                name: "CreateOn",
                table: "Books",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "CreateOn",
                table: "BookCopies",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "CreateOn",
                table: "Authors",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "CreateOn",
                table: "AspNetUsers",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "CreateOn",
                table: "Areas",
                newName: "CreatedOn");

            migrationBuilder.AlterColumn<string>(
                name: "Publisher",
                table: "Books",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ImagePublicId",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserName",
                table: "AspNetUsers",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Subscripers_SubscriberId",
                table: "Rentals",
                column: "SubscriberId",
                principalTable: "Subscripers",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Subscripers_SubscriberId",
                table: "Rentals");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Subscripers_SubscriberId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ImagePublicId",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "SubscriberId",
                table: "Subscriptions",
                newName: "SubscriperId");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Subscriptions",
                newName: "CreateOn");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_SubscriberId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_SubscriperId");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Subscripers",
                newName: "LasrName");

            migrationBuilder.RenameColumn(
                name: "ImageThumbnailUrl",
                table: "Subscripers",
                newName: "ImageThumbailUrl");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Subscripers",
                newName: "CreateOn");

            migrationBuilder.RenameColumn(
                name: "SubscriberId",
                table: "Rentals",
                newName: "SubscriperId");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Rentals",
                newName: "CreateOn");

            migrationBuilder.RenameIndex(
                name: "IX_Rentals_SubscriberId",
                table: "Rentals",
                newName: "IX_Rentals_SubscriperId");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Governorates",
                newName: "CreateOn");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Categories",
                newName: "CreateOn");

            migrationBuilder.RenameColumn(
                name: "ImageThumbnailUrl",
                table: "Books",
                newName: "ImageThumbailUrl");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Books",
                newName: "CreateOn");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "BookCopies",
                newName: "CreateOn");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Authors",
                newName: "CreateOn");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "AspNetUsers",
                newName: "CreateOn");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Areas",
                newName: "CreateOn");

            migrationBuilder.AlterColumn<string>(
                name: "Publisher",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Subscripers_SubscriperId",
                table: "Rentals",
                column: "SubscriperId",
                principalTable: "Subscripers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Subscripers_SubscriperId",
                table: "Subscriptions",
                column: "SubscriperId",
                principalTable: "Subscripers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
