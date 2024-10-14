using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoviesManagementSystem.EF.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieReview_AspNetUsers_UserId",
                table: "MovieReview");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieReview_Movies_MovieId",
                table: "MovieReview");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieReview",
                table: "MovieReview");

            migrationBuilder.RenameTable(
                name: "MovieReview",
                newName: "MovieReviews");

            migrationBuilder.RenameIndex(
                name: "IX_MovieReview_UserId",
                table: "MovieReviews",
                newName: "IX_MovieReviews_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieReview_MovieId",
                table: "MovieReviews",
                newName: "IX_MovieReviews_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieReviews",
                table: "MovieReviews",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieReviews_AspNetUsers_UserId",
                table: "MovieReviews",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieReviews_Movies_MovieId",
                table: "MovieReviews",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieReviews_AspNetUsers_UserId",
                table: "MovieReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieReviews_Movies_MovieId",
                table: "MovieReviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieReviews",
                table: "MovieReviews");

            migrationBuilder.RenameTable(
                name: "MovieReviews",
                newName: "MovieReview");

            migrationBuilder.RenameIndex(
                name: "IX_MovieReviews_UserId",
                table: "MovieReview",
                newName: "IX_MovieReview_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieReviews_MovieId",
                table: "MovieReview",
                newName: "IX_MovieReview_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieReview",
                table: "MovieReview",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieReview_AspNetUsers_UserId",
                table: "MovieReview",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieReview_Movies_MovieId",
                table: "MovieReview",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
