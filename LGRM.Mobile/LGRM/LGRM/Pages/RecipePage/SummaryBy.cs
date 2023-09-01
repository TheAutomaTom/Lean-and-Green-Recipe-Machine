using LGRM.Model;
using System;
using Xamarin.Forms;

namespace LGRM.XamF.Pages
{ 
    public class SummaryBy : StackLayout
    {
        public String[] text;
        Color textColor; // = Color.FromHex("#003447");
        double fontSize;

        public SummaryBy(Kind kind = Kind.All, bool isRecipeHeader = true)
        {
            Application.Current.Resources.TryGetValue("LocalConverterToEvaluateState", out var resourceValue);
            var evaluateState = (IValueConverter)resourceValue;

            if (isRecipeHeader)
            {
                Application.Current.Resources.TryGetValue("DefaultTextColor", out resourceValue);
                textColor = (Color)resourceValue;                
            }
            else
            {
                textColor = Color.White;                
            }
            Application.Current.Resources.TryGetValue("FontM", out resourceValue);
            fontSize = (Double)resourceValue;

            switch (kind)
            {
                case Kind.Lean:
                    text = new String[4] { "Leans: ", "TotalLs", "RecommendedLs", "BGStateL" };
                    break;
                case Kind.Green:
                    text = new String[4] { "Greens: ", "TotalGs", "RecommendedGs", "BGStateG" };
                    break;
                case Kind.HealthyFat:
                    text = new String[4] { "Healthy Fats: ", "TotalHs", "RecommendedHs", "BGStateH" };
                    break;
                case Kind.Condiment:
                    text = new String[4] { "Condiments: ", "TotalCs", "RecommendedCs", "BGStateC" };
                    break;
                //default:
                //    text = new string[2] { "Recipe Serves:", "Recipe.Serves" };
                //    break;
            }

            this.Orientation = StackOrientation.Horizontal;
            this.HorizontalOptions = LayoutOptions.Center;

            //if (kind != Kind.All)
            {
                var label1 = new Label() { TextColor = textColor, FontSize = fontSize, Padding = 0, WidthRequest = 110, Text = text[0] };

                var label2 = new Label() { TextColor = textColor, FontSize = fontSize, Padding = 0, WidthRequest = 80, HorizontalTextAlignment = TextAlignment.Center };
                label2.SetBinding(Label.TextProperty, text[1]);

                var label3 = new Label() { TextColor = textColor, FontSize = fontSize, Padding = 0, WidthRequest = 20, Text = "of", HorizontalTextAlignment = TextAlignment.Center };

                var label4 = new Label() { TextColor = textColor, FontSize = fontSize, Padding = 0, WidthRequest = 60, HorizontalTextAlignment = TextAlignment.Center };
                label4.SetBinding(Label.TextProperty, text[2], BindingMode.TwoWay);

                //var button1 = new Button() { Text = "Balance", FontSize = fontB, Padding = 0, Margin = new Thickness(5,0,0,0), HeightRequest = 12 };
                                
                this.Children.Add(label1);
                this.Children.Add(label2);
                this.Children.Add(label3);
                this.Children.Add(label4);

                this.SetBinding(StackLayout.BackgroundColorProperty, text[3], converter: evaluateState);
            }
            //else // This is the "Serves: X " line...
            //{
            //    var labelServes = new Label() { TextColor = textColor, FontSize = fontSize, Padding = 0, WidthRequest = 110, Text = text[0] };
            //    var entryServes = new Entry() { TextColor = textColor, FontSize = fontSize, WidthRequest = 80, HorizontalTextAlignment = TextAlignment.Center };
            //    entryServes.SetBinding(Entry.TextProperty, text[1]);

            //    this.Children.Add(labelServes);
            //    this.Children.Add(entryServes);
            //}

        }       

        
    }


}
