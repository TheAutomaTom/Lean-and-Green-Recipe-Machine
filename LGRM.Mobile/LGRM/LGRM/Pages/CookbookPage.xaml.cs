using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp.Views.Forms;
using LGRM.XamF.ViewModels.Framework;
using LGRM.XamF.ViewModels;
using LGRM.Model;

namespace LGRM.XamF.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CookbookPage : ContentPage
    {
        SkiaPainter skiaPainter;
        

        public CookbookPage()
        {            
            InitializeComponent();            
            this.BindingContext = ViewModelLocator.CookbookVM;
            skiaPainter = new SkiaPainter(); //Do not move this up!  It can break App.xaml StaticResoures.

            
        }

        void canvas_Open2Title(object sender, SKPaintSurfaceEventArgs args) => skiaPainter.OnCanvasPaint_Open2Title(sender, args);
        void canvas_Open2Sub(object sender, SKPaintSurfaceEventArgs args)   => skiaPainter.OnCanvasPaint_Title2Sub( sender, args);
        void canvas_Sub2Open(object sender, SKPaintSurfaceEventArgs args)   => skiaPainter.OnCanvasPaint_Sub2Open(  sender, args);

        private async void LoadButton_Clicked(object sender, System.EventArgs e)
        {
            indicator.IsRunning = true;
            indicator.IsVisible   = true;
            int recipeToLoadId = ((sender as Button).CommandParameter as Recipe).Id;
            await (BindingContext as CookbookVM).LoadSelectedRecipe(recipeToLoadId);
            indicator.IsVisible = false;
            indicator.IsRunning = false;

            //Old code: Command = "{Binding BindingContext.LoadSelectedRecipeCommand, Source={x:Reference Name=MyCookbookPage}}" CommandParameter = "{Binding .}"
        }
        private async void DeleteButton_Clicked(object sender, System.EventArgs e)
        {
            indicator.IsRunning = true;
            indicator.IsVisible = true;
            int recipeToDeleteId = ((sender as Button).CommandParameter as Recipe).Id;
            await (BindingContext as CookbookVM).VerifyDeleteSelectedRecipeDialog(recipeToDeleteId);
            indicator.IsVisible = false;
            indicator.IsRunning = false;

            //Old code: Command = "{Binding BindingContext.LoadSelectedRecipeCommand, Source={x:Reference Name=MyCookbookPage}}" CommandParameter = "{Binding .}"
        }
    }
}