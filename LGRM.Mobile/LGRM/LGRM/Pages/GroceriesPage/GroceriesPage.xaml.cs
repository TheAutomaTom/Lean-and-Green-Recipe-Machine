using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LGRM.Model;
using LGRM.XamF.ViewModels.Framework;
using SkiaSharp.Views.Forms;

namespace LGRM.XamF.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroceriesPage : ContentPage
    {
        protected Kind kind;
        protected SkiaPainter skiaPainter;

        protected Color colorA1;
        //protected Color colorA2;
        //protected Color colorB1;


        public GroceriesPage()
        {            
            InitializeComponent();
            BindingContext = ViewModelLocator.GroceriesVM;
            

            Application.Current.Resources.TryGetValue("String2HexColor", out var resourceValue);
            var fromHex = (IValueConverter)resourceValue;
            Application.Current.Resources.TryGetValue("StringExists2Visibility", out resourceValue);
            var ToBeVisible = (IValueConverter)resourceValue;

            
            
        }



        internal void SetPerKind(Kind kind)
        {
            #region Kind Switch...
            switch (kind)
            {
                case Kind.Lean:
                    Application.Current.Resources.TryGetValue("LeansA1", out var resourceValue);
                    colorA1 = (Color)resourceValue;
                    //Application.Current.Resources.TryGetValue("LeansA2", out resourceValue);
                    //colorA2 = (Color)resourceValue;
                    //Application.Current.Resources.TryGetValue("NeutralBG1", out resourceValue);
                    //colorB1 = (Color)resourceValue;                    
                    break;

                case Kind.Green:
                    Application.Current.Resources.TryGetValue("GreensA1", out resourceValue);
                    colorA1 = (Color)resourceValue;
                    //Application.Current.Resources.TryGetValue("GreensA2", out resourceValue);
                    //colorA2 = (Color)resourceValue;
                    //Application.Current.Resources.TryGetValue("LeansA1", out resourceValue);
                    //colorB1 = (Color)resourceValue;
                    break;

                case Kind.HealthyFat:
                    Application.Current.Resources.TryGetValue("HealthyFatsA1", out resourceValue);
                    colorA1 = (Color)resourceValue;
                    //Application.Current.Resources.TryGetValue("HealthyFatsA2", out resourceValue);
                    //colorA2 = (Color)resourceValue;
                    //Application.Current.Resources.TryGetValue("GreensA1", out resourceValue);
                    //colorB1 = (Color)resourceValue;
                    break;

                case Kind.Condiment:
                    Application.Current.Resources.TryGetValue("CondimentsA1", out resourceValue);
                    colorA1 = (Color)resourceValue;
                    //Application.Current.Resources.TryGetValue("CondimentsA2", out resourceValue);
                    //colorA2 = (Color)resourceValue;
                    //Application.Current.Resources.TryGetValue("HealthyFatsA1", out resourceValue);
                    //colorB1 = (Color)resourceValue;
                    break;                    
            }


            pickerStack.BackgroundColor = colorA1;
            picker.BackgroundColor = colorA1;
            cvStack.BackgroundColor = colorA1;
        }
            #endregion Kind Switch... ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

            void ClearSearch_Clicked(object sender, System.EventArgs e) => searchEntry.Text = "";

            void canvas_Open2Title(object sender, SKPaintSurfaceEventArgs args) => skiaPainter.OnCanvasPaint_Open2Title(sender, args);
            void canvas_Title2Open(object sender, SKPaintSurfaceEventArgs args) => skiaPainter.OnCanvasPaint_Title2Open(sender, args);

        
    }
}
