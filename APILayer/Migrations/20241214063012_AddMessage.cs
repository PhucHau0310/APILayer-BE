using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APILayer.Migrations
{
    /// <inheritdoc />
    public partial class AddMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_APIDocumentations_APIs_ApiId",
                table: "APIDocumentations");

            migrationBuilder.DropForeignKey(
                name: "FK_APIs_Users_OwnerId",
                table: "APIs");

            migrationBuilder.DropForeignKey(
                name: "FK_APIVersions_APIs_ApiId",
                table: "APIVersions");

            migrationBuilder.DropForeignKey(
                name: "FK_FAQs_Users_UserId",
                table: "FAQs");

            migrationBuilder.DropForeignKey(
                name: "FK_FeaturedAPIs_APIs_ApiId",
                table: "FeaturedAPIs");

            migrationBuilder.DropForeignKey(
                name: "FK_FeaturedAPIs_Users_UserId",
                table: "FeaturedAPIs");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_UserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_APIs_ApiId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Users_UserId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_APIs_ApiId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_UserId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSubscriptions_APIs_ApiId",
                table: "UserSubscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSubscriptions_Users_UserId",
                table: "UserSubscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSubscriptions",
                table: "UserSubscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payments",
                table: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NewsletterSubscriptions",
                table: "NewsletterSubscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeaturedAPIs",
                table: "FeaturedAPIs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FAQs",
                table: "FAQs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_APIVersions",
                table: "APIVersions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_APIs",
                table: "APIs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_APIDocumentations",
                table: "APIDocumentations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "UserSubscriptions",
                newName: "UserSubscription");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "Review");

            migrationBuilder.RenameTable(
                name: "Payments",
                newName: "Payment");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "Notification");

            migrationBuilder.RenameTable(
                name: "NewsletterSubscriptions",
                newName: "NewsletterSubscription");

            migrationBuilder.RenameTable(
                name: "FeaturedAPIs",
                newName: "FeaturedAPI");

            migrationBuilder.RenameTable(
                name: "FAQs",
                newName: "FAQ");

            migrationBuilder.RenameTable(
                name: "APIVersions",
                newName: "APIVersion");

            migrationBuilder.RenameTable(
                name: "APIs",
                newName: "API");

            migrationBuilder.RenameTable(
                name: "APIDocumentations",
                newName: "APIDocumentation");

            migrationBuilder.RenameColumn(
                name: "SubscriptionId",
                table: "UserSubscription",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_UserSubscriptions_UserId",
                table: "UserSubscription",
                newName: "IX_UserSubscription_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserSubscriptions_ApiId",
                table: "UserSubscription",
                newName: "IX_UserSubscription_ApiId");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "User",
                newName: "EmailConfirmationToken");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                table: "User",
                newName: "IX_User_Email");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_UserId",
                table: "Review",
                newName: "IX_Review_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_ApiId",
                table: "Review",
                newName: "IX_Review_ApiId");

            migrationBuilder.RenameColumn(
                name: "PaymentId",
                table: "Payment",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_UserId",
                table: "Payment",
                newName: "IX_Payment_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_ApiId",
                table: "Payment",
                newName: "IX_Payment_ApiId");

            migrationBuilder.RenameColumn(
                name: "NotificationId",
                table: "Notification",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_UserId",
                table: "Notification",
                newName: "IX_Notification_UserId");

            migrationBuilder.RenameColumn(
                name: "SubscriptionId",
                table: "NewsletterSubscription",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_NewsletterSubscriptions_Email",
                table: "NewsletterSubscription",
                newName: "IX_NewsletterSubscription_Email");

            migrationBuilder.RenameColumn(
                name: "FeaturedApiId",
                table: "FeaturedAPI",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_FeaturedAPIs_UserId",
                table: "FeaturedAPI",
                newName: "IX_FeaturedAPI_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FeaturedAPIs_ApiId",
                table: "FeaturedAPI",
                newName: "IX_FeaturedAPI_ApiId");

            migrationBuilder.RenameColumn(
                name: "FaqId",
                table: "FAQ",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_FAQs_UserId",
                table: "FAQ",
                newName: "IX_FAQ_UserId");

            migrationBuilder.RenameColumn(
                name: "VersionId",
                table: "APIVersion",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_APIVersions_ApiId",
                table: "APIVersion",
                newName: "IX_APIVersion_ApiId");

            migrationBuilder.RenameColumn(
                name: "ApiId",
                table: "API",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_APIs_OwnerId",
                table: "API",
                newName: "IX_API_OwnerId");

            migrationBuilder.RenameColumn(
                name: "DocumentationId",
                table: "APIDocumentation",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_APIDocumentations_ApiId",
                table: "APIDocumentation",
                newName: "IX_APIDocumentation_ApiId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserSubscription",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "User",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HashedPassword",
                table: "User",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailConfirmed",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Review",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Payment",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Notification",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "FeaturedAPI",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "FAQ",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                table: "API",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "API",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSubscription",
                table: "UserSubscription",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Review",
                table: "Review",
                column: "ReviewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payment",
                table: "Payment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                table: "Notification",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NewsletterSubscription",
                table: "NewsletterSubscription",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeaturedAPI",
                table: "FeaturedAPI",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FAQ",
                table: "FAQ",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_APIVersion",
                table: "APIVersion",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_API",
                table: "API",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_APIDocumentation",
                table: "APIDocumentation",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ChatMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    RecipientId = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessage_User_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChatMessage_User_SenderId",
                        column: x => x.SenderId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_RecipientId",
                table: "ChatMessage",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_SenderId",
                table: "ChatMessage",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_API_User_OwnerId",
                table: "API",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_APIDocumentation_API_ApiId",
                table: "APIDocumentation",
                column: "ApiId",
                principalTable: "API",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_APIVersion_API_ApiId",
                table: "APIVersion",
                column: "ApiId",
                principalTable: "API",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FAQ_User_UserId",
                table: "FAQ",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FeaturedAPI_API_ApiId",
                table: "FeaturedAPI",
                column: "ApiId",
                principalTable: "API",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FeaturedAPI_User_UserId",
                table: "FeaturedAPI",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_User_UserId",
                table: "Notification",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_API_ApiId",
                table: "Payment",
                column: "ApiId",
                principalTable: "API",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_User_UserId",
                table: "Payment",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_API_ApiId",
                table: "Review",
                column: "ApiId",
                principalTable: "API",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_User_UserId",
                table: "Review",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSubscription_API_ApiId",
                table: "UserSubscription",
                column: "ApiId",
                principalTable: "API",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSubscription_User_UserId",
                table: "UserSubscription",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_API_User_OwnerId",
                table: "API");

            migrationBuilder.DropForeignKey(
                name: "FK_APIDocumentation_API_ApiId",
                table: "APIDocumentation");

            migrationBuilder.DropForeignKey(
                name: "FK_APIVersion_API_ApiId",
                table: "APIVersion");

            migrationBuilder.DropForeignKey(
                name: "FK_FAQ_User_UserId",
                table: "FAQ");

            migrationBuilder.DropForeignKey(
                name: "FK_FeaturedAPI_API_ApiId",
                table: "FeaturedAPI");

            migrationBuilder.DropForeignKey(
                name: "FK_FeaturedAPI_User_UserId",
                table: "FeaturedAPI");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_User_UserId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_API_ApiId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_User_UserId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_API_ApiId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_User_UserId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSubscription_API_ApiId",
                table: "UserSubscription");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSubscription_User_UserId",
                table: "UserSubscription");

            migrationBuilder.DropTable(
                name: "ChatMessage");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSubscription",
                table: "UserSubscription");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Review",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payment",
                table: "Payment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                table: "Notification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NewsletterSubscription",
                table: "NewsletterSubscription");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeaturedAPI",
                table: "FeaturedAPI");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FAQ",
                table: "FAQ");

            migrationBuilder.DropPrimaryKey(
                name: "PK_APIVersion",
                table: "APIVersion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_APIDocumentation",
                table: "APIDocumentation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_API",
                table: "API");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsEmailConfirmed",
                table: "User");

            migrationBuilder.RenameTable(
                name: "UserSubscription",
                newName: "UserSubscriptions");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "Review",
                newName: "Reviews");

            migrationBuilder.RenameTable(
                name: "Payment",
                newName: "Payments");

            migrationBuilder.RenameTable(
                name: "Notification",
                newName: "Notifications");

            migrationBuilder.RenameTable(
                name: "NewsletterSubscription",
                newName: "NewsletterSubscriptions");

            migrationBuilder.RenameTable(
                name: "FeaturedAPI",
                newName: "FeaturedAPIs");

            migrationBuilder.RenameTable(
                name: "FAQ",
                newName: "FAQs");

            migrationBuilder.RenameTable(
                name: "APIVersion",
                newName: "APIVersions");

            migrationBuilder.RenameTable(
                name: "APIDocumentation",
                newName: "APIDocumentations");

            migrationBuilder.RenameTable(
                name: "API",
                newName: "APIs");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "UserSubscriptions",
                newName: "SubscriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_UserSubscription_UserId",
                table: "UserSubscriptions",
                newName: "IX_UserSubscriptions_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserSubscription_ApiId",
                table: "UserSubscriptions",
                newName: "IX_UserSubscriptions_ApiId");

            migrationBuilder.RenameColumn(
                name: "EmailConfirmationToken",
                table: "Users",
                newName: "FullName");

            migrationBuilder.RenameIndex(
                name: "IX_User_Email",
                table: "Users",
                newName: "IX_Users_Email");

            migrationBuilder.RenameIndex(
                name: "IX_Review_UserId",
                table: "Reviews",
                newName: "IX_Reviews_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_ApiId",
                table: "Reviews",
                newName: "IX_Reviews_ApiId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Payments",
                newName: "PaymentId");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_UserId",
                table: "Payments",
                newName: "IX_Payments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_ApiId",
                table: "Payments",
                newName: "IX_Payments_ApiId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Notifications",
                newName: "NotificationId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_UserId",
                table: "Notifications",
                newName: "IX_Notifications_UserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "NewsletterSubscriptions",
                newName: "SubscriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_NewsletterSubscription_Email",
                table: "NewsletterSubscriptions",
                newName: "IX_NewsletterSubscriptions_Email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "FeaturedAPIs",
                newName: "FeaturedApiId");

            migrationBuilder.RenameIndex(
                name: "IX_FeaturedAPI_UserId",
                table: "FeaturedAPIs",
                newName: "IX_FeaturedAPIs_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FeaturedAPI_ApiId",
                table: "FeaturedAPIs",
                newName: "IX_FeaturedAPIs_ApiId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "FAQs",
                newName: "FaqId");

            migrationBuilder.RenameIndex(
                name: "IX_FAQ_UserId",
                table: "FAQs",
                newName: "IX_FAQs_UserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "APIVersions",
                newName: "VersionId");

            migrationBuilder.RenameIndex(
                name: "IX_APIVersion_ApiId",
                table: "APIVersions",
                newName: "IX_APIVersions_ApiId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "APIDocumentations",
                newName: "DocumentationId");

            migrationBuilder.RenameIndex(
                name: "IX_APIDocumentation_ApiId",
                table: "APIDocumentations",
                newName: "IX_APIDocumentations_ApiId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "APIs",
                newName: "ApiId");

            migrationBuilder.RenameIndex(
                name: "IX_API_OwnerId",
                table: "APIs",
                newName: "IX_APIs_OwnerId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserSubscriptions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "HashedPassword",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Notifications",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "FeaturedAPIs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "FAQs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "APIs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "APIs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSubscriptions",
                table: "UserSubscriptions",
                column: "SubscriptionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "ReviewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payments",
                table: "Payments",
                column: "PaymentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "NotificationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NewsletterSubscriptions",
                table: "NewsletterSubscriptions",
                column: "SubscriptionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeaturedAPIs",
                table: "FeaturedAPIs",
                column: "FeaturedApiId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FAQs",
                table: "FAQs",
                column: "FaqId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_APIVersions",
                table: "APIVersions",
                column: "VersionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_APIDocumentations",
                table: "APIDocumentations",
                column: "DocumentationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_APIs",
                table: "APIs",
                column: "ApiId");

            migrationBuilder.AddForeignKey(
                name: "FK_APIDocumentations_APIs_ApiId",
                table: "APIDocumentations",
                column: "ApiId",
                principalTable: "APIs",
                principalColumn: "ApiId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_APIs_Users_OwnerId",
                table: "APIs",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_APIVersions_APIs_ApiId",
                table: "APIVersions",
                column: "ApiId",
                principalTable: "APIs",
                principalColumn: "ApiId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FAQs_Users_UserId",
                table: "FAQs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FeaturedAPIs_APIs_ApiId",
                table: "FeaturedAPIs",
                column: "ApiId",
                principalTable: "APIs",
                principalColumn: "ApiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FeaturedAPIs_Users_UserId",
                table: "FeaturedAPIs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_APIs_ApiId",
                table: "Payments",
                column: "ApiId",
                principalTable: "APIs",
                principalColumn: "ApiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Users_UserId",
                table: "Payments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_APIs_ApiId",
                table: "Reviews",
                column: "ApiId",
                principalTable: "APIs",
                principalColumn: "ApiId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_UserId",
                table: "Reviews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSubscriptions_APIs_ApiId",
                table: "UserSubscriptions",
                column: "ApiId",
                principalTable: "APIs",
                principalColumn: "ApiId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSubscriptions_Users_UserId",
                table: "UserSubscriptions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
