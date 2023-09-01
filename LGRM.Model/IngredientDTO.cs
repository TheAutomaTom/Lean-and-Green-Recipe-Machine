using SQLite;

namespace LGRM.Model
{
    [Table("IngredientLists")]
    public class IngredientDTO
    {
        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }                 // if cast to Ingredient, becomes IngredientId (for display order)
        public int CatalogNumber { get; set; }      // Join on Grocery.CatalogNumber
        public int RecipeId { get; set; }           // Join on Recipes.Id
        public float QtyPortion { get; set; }       // Use to calc all UOMs


        public IngredientDTO() { } 

        public IngredientDTO(Ingredient ing)
        {
            CatalogNumber   = ing.CatalogNumber;
            RecipeId        = ing.RecipeId;
            QtyPortion      = ing.QtyPortion;

        }



    }
}
