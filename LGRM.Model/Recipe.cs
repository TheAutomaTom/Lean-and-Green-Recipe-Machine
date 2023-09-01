using LGRM.Model.Framework;
using Newtonsoft.Json;
using SQLite;
using System.Collections.ObjectModel;

namespace LGRM.Model
{
    [Table("RecipesMeta")]
    public class Recipe : BindableBase
    {
        ///    MEMBERS    \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        
        #region Private Members ...
        int id { get; set; }
        string name { get; set; }
        float serves { get; set; }
        ObservableCollection<Ingredient> ingredients { get; set; }
        #endregion ... Private Members

        #region Public Members w/ Private Backers...
        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id
        {
            get => id;
            set
            {
                id = value;
                RaisePropertyChanged(nameof(Id));
            }
        }
        public string Name
        {
            get => name;
            set
            {
                name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }
        public float Serves
        {
            get => serves;
            set
            {
                serves = value;
                RaisePropertyChanged(nameof(Serves));
            }
        }
        
        [Ignore] [JsonIgnore]
        public ObservableCollection<Ingredient> Ingredients
        {
            get => ingredients;
            set
            {
                ingredients = value;
                RaisePropertyChanged(nameof(Ingredients));
            }
        }
        #endregion ... Public Members w/ Private Backers

        #region Other Public Members ...
        public const string defaultName = "My New Recipe";
        public int IngredientsCount { get; set; } // ... to display on LoadRecipePage
        
        #endregion Other Public Members ...


        ///    CTOR       \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        public Recipe()
        {
            Name = defaultName;
            Ingredients = new ObservableCollection<Ingredient>();
            Serves = 1;
        }


        public Recipe(string name, float servings)
        {
            Name = name;
            Serves = servings;
            Ingredients = new ObservableCollection<Ingredient>();

        }


        public Recipe(Recipe recipeToLoad)
        {
            Id = recipeToLoad.Id;
            Name = recipeToLoad.Name;
            Ingredients = new ObservableCollection<Ingredient>();
            Ingredients = recipeToLoad.Ingredients;
            Serves = recipeToLoad.Serves;
        }








        ///    METHODS    \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        // ... Nada!


    }


}
