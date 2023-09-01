using System.Collections.Generic;
using LGRM.Data;
using LGRM.Model;
using System.Threading.Tasks;
using SQLite;
using System;
using System.Collections.ObjectModel;
using Xamarin.Essentials;

namespace LGRM.XamF.Services
{
    public class SQLiteDataService
    {
        private static SQLiteConnector db;
        public static SQLiteConnector Db
        {
            get
            {
                if (db == null)
                {

                    db = new SQLiteConnector();
                }
                return db;
            }
        }

        #region ///    GROCERIES...  \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public List<Grocery> GroceryList;

        public async Task<CreateTablesResult> CreateTableOfGroceriesAsync()
        {
            return await Db.CreateTableOfGroceriesAsync();
        }

        public async Task<int> PopulateTableOfGroceriesAsync(string catalog)
        {
            return await Db.PopulateTableOfGroceriesAsync(catalog);
        }

        public async Task<int> DropTableOfGroceriesAsync()
        {
            return await Db.DropTableOfGroceriesAsync();
        }

        //public async Task<List<Grocery>> GetAllGroceriesAsync() => await Db.GetAllGroceriesAsync();
        public List<Grocery> GetAllGroceries()
        {
            return GroceryList ??= new List<Grocery>( Db.GetAllGroceries() );
        }

        #endregion \\\ ...GROCERIES    ////////////////////////////////////////////////////////////////
        // |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        #region    ///    RECIPES...   \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        int savedRecipesCount;
        public SQLiteDataService()
        {
            if (Preferences.ContainsKey("savedRecipesCount"))
            {
                savedRecipesCount = Db.GetAllRecipeMetas().Count;
                Preferences.Set("savedRecipesCount", savedRecipesCount);
            }
            else
            {
                Preferences.Get("savedRecipesCount", 0);
            }
        }
        

        public List<Recipe> GetAllRecipeMetas() 
        {
            if ( savedRecipesCount > 0 )
            {
                var toReturn = Db.GetAllRecipeMetas();
                Preferences.Set("savedRecipesCount", toReturn.Count);
                return toReturn;
            }
            else // no recipes are saved, so create the empty table...
            {
                var x = Db.CreateTableRecipesMetaAsync();
                return new List<Recipe>();
            }
        }




        public Recipe GetRecipeById(int id)
        {
            return Db.GetRecipeById(id);
        }

        public async Task<int> SaveRecipeAsync(Recipe recipe)
        {
            savedRecipesCount++;
            return await Db.SaveRecipeAsync(recipe);
        }



        public async Task<int> UpdateRecipeAsync(Recipe recipe)
        {
            return await Db.UpdateRecipeAsync(recipe);
        }

        public int DeleteRecipe(int id)
        {
            return Db.DeleteRecipe(id);
        }

        #endregion  \\\ ...RECIPES    ////////////////////////////////////////////////////////////////

    }
}

