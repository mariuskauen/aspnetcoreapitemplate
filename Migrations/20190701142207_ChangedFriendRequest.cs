using Microsoft.EntityFrameworkCore.Migrations;

namespace soapApi.Migrations
{
    public partial class ChangedFriendRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Accepted",
                table: "FriendRequest",
                newName: "IsActive");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "FriendRequest",
                newName: "Accepted");
        }
    }
}
