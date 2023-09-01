using LGRM.Model;
using LGRM.XamF.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace LGRM.XamF.ViewModels
{
    public class CookbookVM : BaseVM
    {
        INavigationService _navigationService;
        public string FooterText => App.V.FooterText;


        private List<Recipe> _recipesDisplayed;
        public List<Recipe> RecipesDisplayed
        {
            get => _recipesDisplayed;
            set
            {
                _recipesDisplayed = value;
                OnPropertyChanged("RecipesDisplayed");
            }
        }


        public int EmptyHeight = 60;
        public int HeightOfStandardRecipe = 100; //Standard height        
        private int _heightOfCollectionView { get; set; }
        public int HeightOfCollectionView
        {
            get
            {
                return RecipesDisplayed.Count > 0
                    ? (_heightOfCollectionView * HeightOfStandardRecipe)
                    : EmptyHeight;
            }
            set
            {
                _heightOfCollectionView = value;
                OnPropertyChanged("HeightOfCollectionView");
            }
        }



        #region CTOR...
        public CookbookVM(INavigationService navigationService)        
        {
            _navigationService = navigationService;

            RecipesDisplayed = new List<Recipe>();
            var recipeMetas = App.MySQLite.GetAllRecipeMetas();
            foreach (var item in recipeMetas)
            {
                RecipesDisplayed.Add(item);
            }

            CreateNewRecipeCommand = new Command(OnCreateNewRecipeCommand);

            MessagingCenter.Subscribe<RecipeVM>(this, "UpdateSavedRecipesList", OnUpdateSavedRecipesList);   // ...from Lists of Groceries            
            
        }
        #endregion ...CTOR


        public ICommand CreateNewRecipeCommand { get; }
        private void OnCreateNewRecipeCommand(object obj)
        {
            _navigationService.NavigateTo("RecipePage");
        }

        private void OnUpdateSavedRecipesList() //Called by Delete Buttons
        {
            RecipesDisplayed = App.MySQLite.GetAllRecipeMetas();
        }
        private void OnUpdateSavedRecipesList(RecipeVM obj) //Called by RecipeVM Messages 
        {
            RecipesDisplayed = App.MySQLite.GetAllRecipeMetas(); 
        }

        public async Task LoadSelectedRecipe(int recipeToLoadId)
        {            
            await _navigationService.NavigateTo("RecipePage", recipeToLoadId);         
        }

        public async Task VerifyDeleteSelectedRecipeDialog(int recipeToLoadId)
        {
            var answer = await App.Current.MainPage.DisplayAlert("Delete", "Remove this recipe?", "Yes", "No");
            if (answer == true) // "Yes"
            {
                await Task.Run(() => App.MySQLite.DeleteRecipe(recipeToLoadId));
                OnUpdateSavedRecipesList();
            }
        }
        







    }
}
