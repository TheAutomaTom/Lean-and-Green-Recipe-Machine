using LGRM.Model;
using LGRM.XamF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace LGRM.XamF.ViewModels
{
    public class RecipeVM : BaseVM
    {
        INavigationService _navigationService;
        public string FooterText => App.V.FooterText;

        private Recipe _recipe { get; set; }
        public Recipe Recipe
        {
            get => _recipe;
            set
            {
                _recipe = value;
                OnPropertyChanged("Recipe");
            }

        }

        private float _recipeServes { get; set; }
        public float RecipeServes
        {
            get => _recipeServes;
            set
            {
                _recipeServes = value;
                UpdateRecommendeds();
                OnPropertyChanged("RecipeServes");
            }
        }

        public ICommand VerifyClearRecipeDialogCommand { get; set; }
        public ICommand SaveOrUpdateRecipeDialogCommand { get; set; }

        #region ObservableCollection<Ingredient>s for view's bindings...
         ObservableCollection<Ingredient> _leans { get; set; }
         ObservableCollection<Ingredient> _greens { get; set; }
         ObservableCollection<Ingredient> _healthyfats { get; set; }
         ObservableCollection<Ingredient> _condiments { get; set; }
        public ObservableCollection<Ingredient> Leans
        {
            get => _leans;
            set
            {
                _leans = value;
                OnPropertyChanged("Leans");
            }
        } 
        public ObservableCollection<Ingredient> Greens
        {
            get => _greens;
            set
            {
                _greens = value;
                OnPropertyChanged("Greens");
            }

        }
        public ObservableCollection<Ingredient> HealthyFats
        {
            get => _healthyfats;
            set
            {
                _healthyfats = value;
                OnPropertyChanged("HealthyFats");
            }

        }
        public ObservableCollection<Ingredient> Condiments
        {
            get => _condiments;
            set
            {
                _condiments = value;
                OnPropertyChanged("Condiments");
            }

        }


        #region For adjusting collections' heights per items added...

        int emptyHeight = 60;
        int heightOf1UOM = 100;  // 102
        int heightOf2UOMs = 138; // 150

        public int CalcCollectionHeight(Kind kind = Kind.All)
        {
            var list = GetIngredients(kind);
            if (list.Count > 0)
            {
                int height = 0;
                foreach (var ing in list)
                {
                    var UOMsCount = (byte)ing.UOMs;
                    height += UOMsCount > 2 ? heightOf2UOMs : heightOf1UOM;
                }
                return height;
            }
            else return emptyHeight;
            
        }
        int _heightL { get; set; }
        int _heightG { get; set; }
        int _heightH { get; set; }
        int _heightC { get; set; }
        public int HeightL
        {
            get => _heightL;
            set
            {
                _heightL = CalcCollectionHeight(Kind.Lean);
                OnPropertyChanged("HeightL");
            }
        }        
        public int HeightG
        {
            get => _heightG;
            set
            {
                _heightG = CalcCollectionHeight(Kind.Green);
                OnPropertyChanged("HeightG");
            }
        }         
        public int HeightH
        {
            get => _heightH;
            set
            {
                _heightH = CalcCollectionHeight(Kind.HealthyFat);
                OnPropertyChanged("HeightH");
            }
        }         
        public int HeightC
        {
            get => _heightC;
            set
            {
                _heightC = CalcCollectionHeight(Kind.Condiment);
                OnPropertyChanged("HeightC");
            }
        }


        #endregion //~~ For adjusting View's collection heights per items added...

        #endregion

        #region For Recipe's summaries displayed in header...       \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        float _totalLs { get; set; }
        float _totalGs { get; set; }
        float _totalHs { get; set; }
        float _totalCs { get; set; }
        float _recommendedLs { get; set; }
        float _recommendedGs { get; set; }
        float _recommendedHs { get; set; }
        float _recommendedCs { get; set; }
        float _bgStateL { get; set; }
        float _bgStateG { get; set; }
        float _bgStateH { get; set; }
        float _bgStateC { get; set; }

        public float TotalLs
        {
            get => _totalLs;
            set
            {
                _totalLs = GetPortions(Kind.Lean);
                SetRecommendedHs();
                BGStateL = SetSummaryBackgroundColor(Kind.Lean);
                OnPropertyChanged("TotalLs");
            }

        }
        public float TotalGs
        {
            get => _totalGs;
            set
            {
                _totalGs = GetPortions(Kind.Green);
                BGStateG = SetSummaryBackgroundColor(Kind.Green);
                OnPropertyChanged("TotalGs");
            }

        }
        public float TotalHs
        {
            get => _totalHs;
            set
            {
                _totalHs = GetPortions(Kind.HealthyFat);
                BGStateH = SetSummaryBackgroundColor(Kind.HealthyFat);
                OnPropertyChanged("TotalHs");
            }

        }
        public float TotalCs
        {
            get => _totalCs;
            set
            {
                _totalCs = GetPortions(Kind.Condiment);
                BGStateC = SetSummaryBackgroundColor(Kind.Condiment);
                OnPropertyChanged("TotalCs");
            }

        }

        public float RecommendedLs
        {
            get => _recommendedLs;
            set
            {
                _recommendedLs = value;
                BGStateL = SetSummaryBackgroundColor(Kind.Lean);
                OnPropertyChanged("RecommendedLs");
            }

        }
        public float RecommendedGs
        {
            get => _recommendedGs;
            set
            {
                _recommendedGs = value;
                BGStateG = SetSummaryBackgroundColor(Kind.Green);
                OnPropertyChanged("RecommendedGs");
            }

        }
        public float RecommendedHs
        {
            get => _recommendedHs;
            set
            {
                _recommendedHs = value;
                BGStateH = SetSummaryBackgroundColor(Kind.HealthyFat);
                OnPropertyChanged("RecommendedHs");
            }

        }
        public float RecommendedCs
        {
            get => _recommendedCs;
            set
            {
                _recommendedCs = value;
                BGStateC = SetSummaryBackgroundColor(Kind.Condiment);
                OnPropertyChanged("RecommendedCs");
            }

        }

        public float BGStateL
        {
            get => _bgStateL;
            set
            {
                _bgStateL = value;
                OnPropertyChanged("BGStateL");
            }
        }
        public float BGStateG
        {
            get => _bgStateG;
            set
            {
                _bgStateG = value;
                OnPropertyChanged("BGStateG");
            }
        }
        public float BGStateH
        {
            get => _bgStateH;
            set
            {
                _bgStateH = value;
                OnPropertyChanged("BGStateH");
            }
        }
        public float BGStateC
        {
            get => _bgStateC;
            set
            {
                _bgStateC = value;
                OnPropertyChanged("BGStateC");
            }
        }

        #endregion
        
        #region CTOR...
        public RecipeVM(INavigationService navigationService)            
        {
            _navigationService = navigationService;
            
            Recipe = new Recipe();
            RecipeServes = Recipe.Serves;
            Leans       = new ObservableCollection<Ingredient>();
            Greens      = new ObservableCollection<Ingredient>();
            HealthyFats = new ObservableCollection<Ingredient>();
            Condiments  = new ObservableCollection<Ingredient>();
            FixForCollectionViewsEmptyViewToDisplay();

            Leans.CollectionChanged += CollectionContentsChanged;  //~~ To alert lists when their contents change (Xamarin doens't do this, naturally) https://stackoverflow.com/questions/1427471/observablecollection-not-noticing-when-item-in-it-changes-even-with-inotifyprop
            Greens.CollectionChanged += CollectionContentsChanged;
            HealthyFats.CollectionChanged += CollectionContentsChanged;
            Condiments.CollectionChanged += CollectionContentsChanged;


            MessagingCenter.Subscribe< GroceriesVM, object >(this, "UpdateIngredients", UpdateRecipeIngredients);   // ...from Lists of Groceries

            VerifyClearRecipeDialogCommand = new Command(OnVerifyClearRecipeDialog);
            SaveOrUpdateRecipeDialogCommand = new Command(OnSaveOrUpdateRecipeDialolg);
        }

        
        public override async Task Initialize(object parameter)
        {
            ClearRecipeAndVM();
            if (parameter == null)
            {                
                this.Recipe = new Recipe();
            }
            else
            {
                this.Recipe = App.MySQLite.GetRecipeById((int)parameter);                
                foreach (var ing in Recipe.Ingredients){ AddIngredient(ing, localOnly: true); }
                UpdatePortions(Kind.All);
            }
            RecipeServes = Recipe.Serves;
            UpdateRecommendeds();
        }
        #endregion ...CTOR

        #region Methods...


        void UpdateRecipeIngredients( GroceriesVM sender, object update )
        {
            var updateContents = (object[])update;
            var ingredientChanged = (Ingredient)updateContents[0];
            var toBeAdded = (bool)updateContents[1];

            if (toBeAdded)
            {
                AddIngredient(ingredientChanged);
            }
            else 
            {
                RemoveIngredient(ingredientChanged);
            }
            UpdatePortions(ingredientChanged.Kind);

        } // Called from GroceriesVM

        void AddIngredient(Ingredient ingredient, bool localOnly = true)
        {
            switch (ingredient.Kind)
            {
                case Kind.Lean:
                    bool contains = Leans.Any(ing => ing.CatalogNumber == ingredient.CatalogNumber);
                    if (!contains)
                    {
                        Leans.Add(ingredient);
                        HeightL += 1;
                    }
                    break;
                case Kind.Green:
                    contains = Leans.Any(ing => ing.CatalogNumber == ingredient.CatalogNumber);
                    if (!contains)
                    {
                        Greens.Add(ingredient);
                        HeightG += 1;
                    }
                    break;
                case Kind.HealthyFat:
                    contains = Leans.Any(ing => ing.CatalogNumber == ingredient.CatalogNumber);
                    if (!contains)
                    {
                        HealthyFats.Add(ingredient);
                        HeightH += 1;
                    }
                    break;
                case Kind.Condiment:
                    contains = Leans.Any(ing => ing.CatalogNumber == ingredient.CatalogNumber);
                    if (!contains)
                    {
                        Condiments.Add(ingredient);
                        HeightC += 1;
                        OnPropertyChanged("TotalCs");
                    }
                    break;
                default: throw new Exception(message: "RecipeVM.AddIngredient: No Kind found!");

            }
            if (!localOnly)
            {
                Recipe.Ingredients.Add(ingredient);
            }
        }
        void RemoveIngredient(Ingredient ingredient, bool localOnly = true)
        {
            switch (ingredient.Kind)
            {
                case Kind.Lean:
                    Leans.Remove(Leans.FirstOrDefault(ing => ing.CatalogNumber == ingredient.CatalogNumber));
                    HeightL -= 1;
                    break;
                case Kind.Green:
                    Greens.Remove(Greens.FirstOrDefault(ing => ing.CatalogNumber == ingredient.CatalogNumber));
                    HeightG -= 1;
                    break;
                case Kind.HealthyFat:
                    HealthyFats.Remove(HealthyFats.FirstOrDefault(ing => ing.CatalogNumber == ingredient.CatalogNumber));
                    HeightH -= 1;
                    break;
                case Kind.Condiment:
                    Condiments.Remove(Condiments.FirstOrDefault(ing => ing.CatalogNumber == ingredient.CatalogNumber));
                    HeightC -= 1;
                    break;
                default: throw new Exception(message: "RecipeVM.RemoveIngredient: No Kind found!");

            }
            if (!localOnly)
            {
                Recipe.Ingredients.Remove(ingredient);
            }
        }

        List<Ingredient> GetIngredients(Kind kind = Kind.All)
        {
            var Ings = new List<Ingredient>();
            switch (kind)
            {
                case Kind.Lean:
                    return Ings = new List<Ingredient>( Leans );
                case Kind.Green:
                    return Ings = new List<Ingredient>( Greens );
                case Kind.HealthyFat:
                    return Ings = new List<Ingredient>( HealthyFats );
                case Kind.Condiment:
                    return Ings = new List<Ingredient>( Condiments );
                default:
                    var lists = new ObservableCollection<Ingredient>[] { Leans, Greens, HealthyFats, Condiments };
                    foreach (var l in lists)
                    {
                        foreach (var ing in l)
                        {
                            Ings.Add(ing);
                        }
                    }
                    return Ings;
            }
        }

        float GetPortions(Kind kind = Kind.All) // ... counts bound in header
        {
            var list = new List<Ingredient>(GetIngredients(kind));
            float portion = 0;
            foreach (var ing in list)
            {
                portion += ing.QtyPortion;
            }
            return portion;
        }

        void UpdatePortions(Kind kind = Kind.All)
        {
            switch (kind)
            {
                case Kind.Lean:
                    if (Leans.Count > 0)
                    {
                        TotalLs = GetPortions(Kind.Lean);
                        RecommendedHs = SetRecommendedHs();
                    }
                    break;
                case Kind.Green:
                    if (Greens.Count > 0)
                    {
                        TotalGs = GetPortions(Kind.Green);
                    }
                    break;
                case Kind.HealthyFat:
                    if (HealthyFats.Count > 0)
                    {
                        TotalHs = GetPortions(Kind.HealthyFat);
                    }
                    break;
                case Kind.Condiment:
                    if (Condiments.Count > 0)
                    {
                        TotalCs = GetPortions(Kind.Condiment);
                    }
                    break;
                default:
                    UpdatePortions(Kind.Lean);
                    UpdatePortions(Kind.Green);
                    UpdatePortions(Kind.HealthyFat);
                    UpdatePortions(Kind.Condiment);
                    break;

            }
        }

        #region to update summary when ingredient lists change...
        void UpdateRecommendeds()
        {
            if (RecipeServes > 0)
            {
                RecommendedLs = 1 * RecipeServes;
                RecommendedGs = 3 * RecipeServes;
                RecommendedHs = SetRecommendedHs();
                RecommendedCs = 3 * RecipeServes;
            }
            else
            {
                RecommendedLs = 1;
                RecommendedGs = 3;
                RecommendedHs = SetRecommendedHs();
                RecommendedCs = 3;
            }
        }
        float SetRecommendedHs()
        {
            {
                float x = 0;
                if (Leans != null)
                {
                    foreach (var ing in Leans)
                    {
                        x = ing.Info1 switch
                        {
                            2 => x += (1 * ing.QtyPortion),
                            3 => x += (2 * ing.QtyPortion),
                            _ => x += (3 * ing.QtyPortion)
                        };
                    }
                    return x < 99 ? x : 99;
                }
                else return 0; // !Leans? ... there would be no Leans, anyhow.
            }
        }
        float SetSummaryBackgroundColor(Kind kind) //ConverterToEvaluateState sets actual colors
        {
            var x = kind switch
            {
                Kind.Lean =>        new float[] { TotalLs, RecommendedLs },
                Kind.Green =>       new float[] { TotalGs, RecommendedGs },
                Kind.HealthyFat =>  new float[] { TotalHs, RecommendedHs },
                Kind.Condiment =>   new float[] { TotalCs, RecommendedCs },
                _ => throw new NotImplementedException()
            };
            return x[0] - x[1];  //ConverterToEvaluateState uses this to determine background color
        }
        
        void CollectionContentsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Ingredient item in e.NewItems)
                {
                    item.PropertyChanged += IngredientPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Ingredient item in e.OldItems)
                {
                    item.PropertyChanged -= IngredientPropertyChanged;
                }
            }
        }   //Updates from Ingredient items on Recipe View

        void IngredientPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdatePortions();
        }   // called when the property of an object inside the collection changes

        #endregion ... to update summary when ingredient lists change

        void FixForCollectionViewsEmptyViewToDisplay() // Without this, the collection views will not display "Empty View" on startup ...
        {
            AddIngredient(new Ingredient() { Kind = Kind.Lean, Name1 = "TriggerEmptyView1", CatalogNumber = 666 }); //Something bugs out if I try to use the same object in all lists.
            AddIngredient(new Ingredient() { Kind = Kind.Green, Name1 = "TriggerEmptyView2", CatalogNumber = 777 });
            AddIngredient(new Ingredient() { Kind = Kind.HealthyFat, Name1 = "TriggerEmptyView3", CatalogNumber = 888 });
            AddIngredient(new Ingredient() { Kind = Kind.Condiment, Name1 = "TriggerEmptyView4", CatalogNumber = 999 });

            Leans.Clear();
            Greens.Clear();
            HealthyFats.Clear();
            Condiments.Clear();

            TotalLs = TotalGs = TotalHs = TotalCs = HeightL = HeightG = HeightH = HeightC = 0;
        }

        #region Clear Recipe ...
        void ClearRecipeAndVM() //Invoked from "Create New Recipe" or "Clear Recipe Command"
        {
            this.Recipe = new Recipe();
            RecipeServes = Recipe.Serves;

            Leans.Clear(); 
            Greens.Clear();
            HealthyFats.Clear();
            Condiments.Clear();

            FixForCollectionViewsEmptyViewToDisplay();
            UpdatePortions();
        }
        async void OnVerifyClearRecipeDialog()
        {
            var answer = await App.Current.MainPage.DisplayAlert("Clear All", "Start a new recipe?", "Yes", "No");
            if (answer == true) // "Yes"
            {
                ClearRecipeAndVM();
            }
        }
        #endregion ... clear recipe

        #region Save/ Update Recipe ...

        private async void OnSaveOrUpdateRecipeDialolg(object obj)
        {
            bool hasIngredients = GetIngredients(Kind.All).Count > 0;
            if (!hasIngredients)
            {
                // "RecipeIsEmptyDialog"
                var answer = await App.Current.MainPage.DisplayAlert("Your Recipe is Empty", "Add ingriedients to create a new recipe...", "See Tutorial", "Continue");
                if (answer == true) // "See Tutorial"
                {
                    await App.Current.MainPage.DisplayAlert("Tutorial", "Coming soon...", "Continue");
                }
            }
            
            if(Recipe.Name != Recipe.defaultName)
            {
                if(await App.Current.MainPage.DisplayAlert("Save Recipe", "Update or Save as New? ", "Update Recipe", "Save as New"))
                {
                    UpdateRecipe();
                }
                else // "Save As New"
                {
                    SaveAsNewRecipeDialog();
                }
            }
            else // "Save As New"
            {
                SaveAsNewRecipeDialog();
                
            }

        }

        private async void SaveAsNewRecipeDialog()
        {
            Recipe.Id = 0; // Id# 0 means the Db will save as new... it will assign a new number.
            string newName;
               
            newName = await App.Current.MainPage.DisplayPromptAsync(
            "Save Recipe", "Name your new recipe...", accept: "Save", cancel: "Cancel", placeholder: "New recipe name...", initialValue: null, maxLength: 25, keyboard: Keyboard.Plain);
            if (string.IsNullOrWhiteSpace(newName))
            {                
                await App.Current.MainPage.DisplayAlert("Alert", "Sorry, but recipes cannot be saved without a name", "Alrighty");
            }
            Recipe.Name = newName.Trim();
            SaveRecipe();
        }

        public async void SaveRecipe()
        {            
            Recipe.Serves = RecipeServes;
            Recipe.Ingredients.Clear();
            foreach (var ingredient in GetIngredients(Kind.All))
            {
                Recipe.Ingredients.Add(ingredient);
            }
            await App.MySQLite.SaveRecipeAsync(Recipe);
            MessagingCenter.Send<RecipeVM>(this, "UpdateSavedRecipesList");
        }

        public async void UpdateRecipe()
        {
            Recipe.Serves = RecipeServes;
            Recipe.Ingredients.Clear();
            foreach (var ingredient in GetIngredients(Kind.All))
            {
                Recipe.Ingredients.Add(ingredient);
            }
            await App.MySQLite.UpdateRecipeAsync(Recipe);
            MessagingCenter.Send<RecipeVM>(this, "UpdateSavedRecipesList");
        }

        #endregion ... save/ update recipe


        #region Navigation ...
        public ICommand NavigateToGroceriesCommand
        {
            get
            {
                return new Command<Kind>((k) => OnNavigateToGroceries(k));
            }
        }
        void OnNavigateToGroceries(Kind k)
        {           
            var catalogNumbers = new List<int>();
            foreach (var ingredient in GetIngredients(k))
            {
                catalogNumbers.Add(ingredient.CatalogNumber);
            }
            object[] parameteres = new object[] { k, catalogNumbers };
            
            var page = k switch
            {
                Kind.Lean => "GroceriesPageL",
                Kind.Green => "GroceriesPageG",
                Kind.HealthyFat => "GroceriesPageH",
                Kind.Condiment => "GroceriesPageC",
                _ => "GroceriesPage"
            };

            _navigationService.NavigateTo(page, parameteres);
        }

        
        #endregion ... navigation 

        #endregion ... methods











    }
}
