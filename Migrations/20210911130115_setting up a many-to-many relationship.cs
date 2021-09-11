using Microsoft.EntityFrameworkCore.Migrations;

namespace mycookingrecepies.Migrations
{
    public partial class settingupamanytomanyrelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Ingridients_IngridientId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_IngridientId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "IngridientId",
                table: "Recipes");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Recipes",
                newName: "RecipeId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Ingridients",
                newName: "IngridientId");

            migrationBuilder.CreateTable(
                name: "IngridientRecipes",
                columns: table => new
                {
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    IngridientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngridientRecipes", x => new { x.RecipeId, x.IngridientId });
                    table.ForeignKey(
                        name: "FK_IngridientRecipes_Ingridients_IngridientId",
                        column: x => x.IngridientId,
                        principalTable: "Ingridients",
                        principalColumn: "IngridientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IngridientRecipes_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IngridientRecipes_IngridientId",
                table: "IngridientRecipes",
                column: "IngridientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IngridientRecipes");

            migrationBuilder.RenameColumn(
                name: "RecipeId",
                table: "Recipes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "IngridientId",
                table: "Ingridients",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "IngridientId",
                table: "Recipes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_IngridientId",
                table: "Recipes",
                column: "IngridientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Ingridients_IngridientId",
                table: "Recipes",
                column: "IngridientId",
                principalTable: "Ingridients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
