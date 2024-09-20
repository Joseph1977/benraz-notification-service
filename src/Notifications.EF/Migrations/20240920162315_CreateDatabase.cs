using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Notifications.EF.Migrations
{
    /// <inheritdoc />
    public partial class CreateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SettingsEntries",
                columns: table => new
                {
                    SettingsEntryId = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingsEntries", x => x.SettingsEntryId);
                });

            migrationBuilder.InsertData(
                table: "SettingsEntries",
                columns: new[] { "SettingsEntryId", "CreateTimeUtc", "Description", "UpdateTimeUtc", "Value" },
                values: new object[,]
                {
                    { "Emails:SendGridApiKey", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6634), "SendGrid API key for email", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6634), null },
                    { "Emails:SendGridAsmId", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6639), "SendGrid ASM identifier for email", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6639), null },
                    { "Emails:SupportEmail", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6636), "Support email", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6636), null },
                    { "Emails:SupportPhone", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6637), "Support phone number", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6637), null },
                    { "General:AuthorizationAccessToken", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6631), "Access token to Authorization service", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6631), null },
                    { "General:AuthorizationBaseUrl", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6620), "Base URL of Authorization service", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6620), null },
                    { "MessagesFilter:Emails:IsEnabled", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6645), "Email filter enable/disable parameter", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6645), "False" },
                    { "MessagesFilter:Emails:Type", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6655), "0 (eWhiteList) - Allow only from white list; 1 (ePatern) - Allo only if fit a regix pattern; 2 (eTraverse) -Travers ALL to specific email; 3 (eAllowAll) - Do not filter, allow all.; 4 (eDiableAll) - This mean No email will be passed, kind of disable feature; 5 (eWhiteDomainList) - Allow emails only with Domain in the DomainsWhitelist from EmailFilter list, not valid for phones", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6655), null },
                    { "MessagesFilter:Emails:Value", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6656), "Email filter value (can be list with comma)", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6656), null },
                    { "MessagesFilter:Phones:IsEnabled", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6664), "Phone filter enable/disable parameter", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6664), "False" },
                    { "MessagesFilter:Phones:Type", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6695), "0 (eWhiteList) - Allow only from white list; 1 (ePatern) - Allo only if fit a regix pattern; 2 (eTraverse) -Travers ALL to specific email; 3 (eAllowAll) - Do not filter, allow all.; 4 (eDiableAll) - This mean No email will be passed, kind of disable feature; 5 (eWhiteDomainList) - Allow emails only with Domain in the DomainsWhitelist from EmailFilter list, not valid for phones", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6695), null },
                    { "MessagesFilter:Phones:Value", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6698), "Phone filter value (can be list with comma)", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6698), null },
                    { "Phones:AccountSID", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6640), "SendGrid API key for sms", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6640), null },
                    { "Phones:AuthToken", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6642), "Account authentication", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6642), null },
                    { "Phones:OutTwillionumber", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6643), "Support phone number", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6643), null },
                    { "TokenValidation:Audience", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6633), "Comma separated audiences allowed", new DateTime(2024, 9, 20, 16, 23, 14, 534, DateTimeKind.Utc).AddTicks(6633), null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SettingsEntries");
        }
    }
}
