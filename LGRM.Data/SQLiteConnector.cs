using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using Newtonsoft.Json;
using LGRM.Model;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Diagnostics;
// Pull note (double check this, in v2.5:
// adb pull /data/user/0/com.TomgCo.LGRM.android/files/.local/share/MyDatabase.db3 C:\Users\ScumS\Desktop

namespace LGRM.Data
{
    public partial class SQLiteConnector
    {
        static bool initialized = false;
        static object myLock = new object();
        public List<Grocery> GroceryRepo { get; set; }
        List<int> recipeIds; // for tracking Ids assigned on new saves


        static readonly Lazy<SQLiteAsyncConnection> LazyDb = new Lazy<SQLiteAsyncConnection>(() =>
        {
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var fullPath = Path.Combine(basePath, "MyDatabase.db3");
            return new SQLiteAsyncConnection( fullPath,
                SQLite.SQLiteOpenFlags.ReadWrite    |       // open the database in read/write mode
                SQLite.SQLiteOpenFlags.Create       |       // create the database if it doesn't exist            
                //SQLite.SQLiteOpenFlags.SharedCache );       // enable multi-threaded database access
                SQLiteOpenFlags.FullMutex, true);
        });   

        static SQLiteAsyncConnection Db => LazyDb.Value;        
        public SQLiteConnector() => InitializeAsync().SafeFireAndForget(false);
        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Db.TableMappings.Any(m => m.MappedType.Name == typeof(Grocery).Name)) // This is an "Initial Run"
                {
                    Console.WriteLine($"~~~ ''{typeof(Grocery).Name}'' Table found? {Db.TableMappings.Any(m => m.MappedType.Name == typeof(Grocery).Name)}");
                    Console.WriteLine("~~~ ");
                    try
                    {                            
                        await CreateTableOfGroceriesAsync();
                    }
                    catch (Exception e)
                    {
                        ReportException(e);
                    }
                }
                initialized = true;
            }

        }






        #region ///    GROCERIES...  \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public async Task<CreateTablesResult> CreateTableOfGroceriesAsync()
        {
            if (Db.TableMappings.Any(m => m.MappedType.Name == typeof(Grocery).Name)) // This is a "Refresh Run"
            {
                await DropTableOfGroceriesAsync();
            }

            var result = await Db.CreateTablesAsync(CreateFlags.None, typeof(Grocery)).ConfigureAwait(false);                
            return result;            
        }

        public async Task<int> PopulateTableOfGroceriesAsync(string catalog)
        {
            if (!Db.TableMappings.Any(m => m.MappedType.Name == typeof(Grocery).Name)) // ...Double check for table
            {
                await CreateTableOfGroceriesAsync();                
            }

            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(catalog));
                string json;
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    json = await reader.ReadToEndAsync();
                }
                GroceryRepo = JsonConvert.DeserializeObject<List<Grocery>>(json);

                int count = 0;

                lock (myLock)
                {
                    foreach (var g in GroceryRepo)
                    {
                        InsertGrocery(g);               // ~~~stopwatch... 00:00:01.7452646, 01.8212847, 01.9907719
                                                        //await InsertGroceryAsync(g);  // ~~~stopwatch... 00:00:03.2519847
                        count++;
                    }
                }
                #region Other foreach implementations to test speeds...
                //foreach (var g in GroceryRepo)
                //{                
                //    await InsertGroceryAsync(g);  // ~~~stopwatch... 00:00:03.2519847
                //    count++;
                //}

                //Parallel.ForEach(GroceryRepo, g => { InsertGrocery(g); count++; }); // ~~~stopwatch... 00:00:02.6221148 

                //Parallel.ForEach(GroceryRepo, async g => { await InsertGroceryAsync(g); }); 
                //count = GetAllGroceries().Count;      // ~~~ stopwatch... 02.2069957
                //count = 342;                          // ~~~ skip GetAllGroceries().Count : 02.7951590
                #endregion Other foreach implementations to test speeds...

                if (count < GroceryRepo.Count) { throw new Exception(message: $"GroceryRepo.Count = {GroceryRepo.Count},\n Db Catalog Count  = {count} !!!"); }
                return count;
            }
            catch (Exception ex)
            {
                ReportException(ex);
                return 0;
            }
        }

        public async Task<int> DropTableOfGroceriesAsync()
        {
            return await Db.DropTableAsync<Grocery>();
        }


        //public async Task<int> InsertGroceryAsync(Grocery grocery) { return await Db.InsertAsync(grocery);}
        public void InsertGrocery(Grocery grocery)
        {
            Db.InsertAsync(grocery);
        }
        public async Task<int> InsertGroceryAsync(Grocery grocery)
        {
            return await Db.InsertAsync(grocery);
        }

        //public async Task<List<Grocery>> GetAllGroceriesAsync() => await Db.Table<Grocery>().ToListAsync();
        public List<Grocery> GetAllGroceries()
        {
            lock (myLock)
            {
                GroceryRepo ??= Db.Table<Grocery>().ToListAsync().Result;
                return GroceryRepo;
            }
        }

        //public async Task<Grocery> GetGroceryByIdAsync(int id) { return await Db.Table<Grocery>().Where(g => g.CatalogNumber == catalogNumber).FirstOrDefaultAsync(); }
        public Grocery GetGroceryByCatalogNumber(int catalogNumber)
        {
            return Db.Table<Grocery>().Where(g => g.CatalogNumber == catalogNumber).FirstOrDefaultAsync().Result;
        }

        #endregion \\\ ...GROCERIES    ////////////////////////////////////////////////////////////////
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        #region    /// RECIPE METAS... \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        //public async Task<CreateTableResult> CreateTableRecipeMetaAsync()
        public async Task<CreateTableResult> CreateTableRecipesMetaAsync()
        {
            //lock (myLock)
            //{
            if (!Db.TableMappings.Any(m => m.MappedType.Name == typeof(Recipe).Name))
            {
                return await Db.CreateTableAsync<Recipe>();
            }
            else
            {
                return 0;
            }
            //}
        }

        

        public List<Recipe> GetAllRecipeMetas()
        {
            List<Recipe> recipeMetaList = new List<Recipe>();
            //if (count > 0)
            //{
                try
                {
                    foreach (var pm in Db.Table<Recipe>().ToListAsync().Result)
                    {
                        recipeMetaList.Add(pm);
                    }
                    //return recipeMetaList;
                }
                catch (Exception ex)
                {
                    ReportException(ex);
                    
                }
            recipeIds ??= new List<int>();
            foreach (var item in recipeMetaList)
            {
                recipeIds.Add(item.Id);
            }
            return recipeMetaList;
            //}

        }

        public async Task<int> SaveRecipeMetaAsync(Recipe recipe)
        {
            recipeIds ??= new List<int>();
            if (recipeIds.Count > 0)
            {
                recipe.Id = recipeIds.Max() + 1;
            }
            else
            {
                recipe.Id = 1;
            }
            recipe.IngredientsCount = recipe.Ingredients.Count;
            return await Db.InsertAsync(recipe);
        }

        public async Task<int> UpdateRecipeMetaAsync(Recipe recipe)
        {
            return await Db.InsertOrReplaceAsync(recipe);     // Page dialog checks if this is a rename or new entry
        }

        //public Task<int> DeleteRecipeMetaAsync(int recipeId) { return await Db.Table<Recipe>().Where(p => p.Id == recipeId).DeleteAsync(); }
        public int DeleteRecipeMeta(int recipeId)
        {
            lock (myLock)
            {
                return Db.Table<Recipe>().Where(p => p.Id == recipeId).DeleteAsync().Result;
            }
        }




        #endregion  \\\ ...RECIPE METAS   ////////////////////////////////////////////////////////////////
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        #region     ///  INGREDIENTS...   \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\


        //public async Task<CreateTableResult> CreateTableIngredientsAsync()
        public Task<CreateTableResult> CreateTableOfIngredientsAsync()
        {
            lock (myLock)
            {
                return Db.CreateTableAsync<IngredientDTO>();
            }
        }

        public async Task<int> SaveIngredientsAsync(Recipe recipe)
        {
            if (!Db.TableMappings.Any(m => m.MappedType.Name == typeof(IngredientDTO).Name)) // This is an "Initial Run"
            {
                try
                {
                    await CreateTableOfIngredientsAsync();
                }
                catch (Exception e)
                {
                    ReportException(e);
                    throw (e);
                }
            }
            DeleteIngredients(recipe.Id); // Clear old entries
            var listOfIngs = new List<IngredientDTO>();
            foreach (var ing in recipe.Ingredients)
            {
                ing.RecipeId = recipe.Id;
                listOfIngs.Add(new IngredientDTO(ing));
            }
            return await Db.InsertAllAsync(listOfIngs);
        }

        public int DeleteIngredients(int recipeId)
        {
            lock (myLock)
            {
                return Db.Table<IngredientDTO>().Where(g => g.RecipeId == recipeId).DeleteAsync().Result;
            }
        }


        #endregion  \\\ ...INGREDIENTS    ////////////////////////////////////////////////////////////////
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        #region     ///   JOIN RECIPE META & INGs...  \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public Recipe GetRecipeById(int id)
        {
            var recipe = Db.Table<Recipe>().Where(p => p.Id == id).FirstOrDefaultAsync().Result;

            var relatedIngDTOs = Db.Table<IngredientDTO>().Where(g => g.RecipeId == id).ToListAsync().Result;
            foreach (var ingDTO in relatedIngDTOs)
            {
                var baseGrocery = GetGroceryByCatalogNumber(ingDTO.CatalogNumber);
                recipe.Ingredients.Add(new Ingredient(ingDTO, baseGrocery));
            }
            return recipe;
        }

        public async Task<int> SaveRecipeAsync(Recipe recipe)
        {
            var x = await SaveRecipeMetaAsync(recipe);
            x += await SaveIngredientsAsync(recipe);
            return x;
        }

        public async Task<int> UpdateRecipeAsync(Recipe recipe)
        {
            var x = await UpdateRecipeMetaAsync(recipe);
            x += await SaveIngredientsAsync(recipe);
            return x;
        }



        public int DeleteRecipe(int recipeId)
        {
            var x = DeleteRecipeMeta(recipeId);
            x += DeleteIngredients(recipeId);
            return x;
        }



        #endregion \\\   JOIN RECIPE META & INGs...  /////////////////////////////////////////////////
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        #region    ///    UTILITY...   \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\




        public bool TableExists(string tableName)
        {
            //Console.WriteLine("~~~ Begin TableExists ");
            string query = string.Format("SELECT name FROM sqlite_master WHERE type='table' AND name='{0}';", tableName);
            var item = Db.QueryAsync<object>(query).Result;
            return (item.Count > 0);
        }



        public void ReportException(Exception e)
        {
            Console.WriteLine("~~~ Begin ReportException ");
            Debug.WriteLine("~~~ Source ......." + e.Source);
            Debug.WriteLine("~~~ Message ......" + e.Message);
            Debug.WriteLine("~~~ InnerEx ......" + e.InnerException);
            Debug.WriteLine("~~~ Data ........." + e.Data);
            Debug.WriteLine("~~~ StackTrace ..." + e.StackTrace);
            Debug.WriteLine("~~~~~~~~~~~~~~~~~~ ");
        }

        public void ReportGroceryFromJson(Grocery g)
        {
            Console.WriteLine("~~~ Begin ReportGroceryFromJson ");
            Debug.WriteLine("~~~ From Json g.Id) ......... " + g.Id);
            
            Debug.WriteLine("~~~~~~~~~~~~~~ g.Name1 ...... " + g.Name1);
            Debug.WriteLine("~~~~~~~~~~~~~~ g.Name2 ...... " + g.Name2);
            Debug.WriteLine("~~~~~~~~~~~~~~ g.Desc1 ...... " + g.Desc1);
            Debug.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
        }


        public async Task<List<TableName>> GetAllTablesAsync() //... see SQLiteExtender
        {
            Console.WriteLine("~~~ Begin GetAllTablesAsync ");
            string queryString = $"SELECT name FROM sqlite_master WHERE type = 'table'";
            return await Db.QueryAsync<TableName>(queryString).ConfigureAwait(false);

        }


    }

    //This helps count tables in the database.. see public async Task<List<TableName>> GetAllTablesAsync()    //https://forums.xamarin.com/discussion/122320/how-can-i-get-a-collection-of-all-tables-in-a-sqlite-database
    public class TableName
    {
        public TableName() { }
        public string Name { get; set; }
    }

    public static class TaskExtensions //https://docs.microsoft.com/en-us/xamarin/xamarin-forms/data-cloud/data/databases
    {
        // NOTE: Async void is intentional here. This provides a way
        // to call an async method from the constructor while
        // communicating intent to fire and forget, and allow
        // handling of exceptions
        public static async void SafeFireAndForget(this Task task,
            bool returnToCallingContext,
            Action<Exception> onException = null)
        {
            try
            {
                await task.ConfigureAwait(returnToCallingContext);
            }

            // if the provided action is not null, catch and
            // pass the thrown exception
            catch (Exception ex) when (onException != null)
            {
                onException(ex);
            }
        }
    }
    #endregion \\\ ...UTILITY      ////////////////////////////////////////////////////////////////
    

}
