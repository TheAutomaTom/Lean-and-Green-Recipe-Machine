using SkiaSharp.Views.Forms;
using System;
using Xamarin.Forms;

using LGRM.Model;
using LGRM.XamF.ViewModels;


namespace LGRM.XamF.Pages
{
    public partial class IngredientsCollection : StackLayout
    {
        private ActivityIndicator indicator;
                
        public Color colorA1;
        public Color colorA2;
        public Color colorB1;

        public string headerLabelString;
        public string itemsSourcePropertyString;
        public string heightRequestPropertyString;
        public string buttonText;

        public Page pageToNavigateTo;
        

        //public static GroceriesPage GroceriesPage;


        public IngredientsCollection(Kind kind)
        {
            #region Colors...
            Application.Current.Resources.TryGetValue("String2HexColor", out var resourceValue);
            var fromHex = (IValueConverter)resourceValue;
            var frameBorderColor = Color.Gray;
            Application.Current.Resources.TryGetValue("DefaultTextColor", out resourceValue);
            var defaultTextColor = (Color)resourceValue;

            #endregion Colors... ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            #region Font Sizes...
            Application.Current.Resources.TryGetValue("FontL", out resourceValue);
            var fontL = (Double)resourceValue;
            Application.Current.Resources.TryGetValue("FontM", out resourceValue);
            var fontM = (Double)resourceValue;
            Application.Current.Resources.TryGetValue("FontB", out resourceValue);
            var fontB = (Double)resourceValue;
            #endregion Font Sizes... ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            #region Kind Switch...
            switch (kind)
            {
                case Kind.Lean:
                    //pageToNavigateTo = App.LeansPage;
                    Application.Current.Resources.TryGetValue("LeansA1", out resourceValue);
                    colorA1 = (Color)resourceValue;
                    Application.Current.Resources.TryGetValue("LeansA2", out resourceValue);
                    colorA2 = (Color)resourceValue;
                    Application.Current.Resources.TryGetValue("NeutralBG1", out resourceValue);
                    colorB1 = (Color)resourceValue;
                    headerLabelString = "Leans";
                    itemsSourcePropertyString = "Leans";
                    heightRequestPropertyString = "HeightL";
                    break;

                case Kind.Green:
                    //pageToNavigateTo = App.GreensPage;
                    Application.Current.Resources.TryGetValue("GreensA1", out resourceValue);
                    colorA1 = (Color)resourceValue;
                    Application.Current.Resources.TryGetValue("GreensA2", out resourceValue);
                    colorA2 = (Color)resourceValue;
                    Application.Current.Resources.TryGetValue("LeansA1", out resourceValue);
                    colorB1 = (Color)resourceValue;
                    headerLabelString = "Greens";
                    itemsSourcePropertyString = "Greens";
                    heightRequestPropertyString = "HeightG";
                    break;

                case Kind.HealthyFat:
                    //pageToNavigateTo = App.HealthyFatsPage;
                    Application.Current.Resources.TryGetValue("HealthyFatsA1", out resourceValue);
                    colorA1 = (Color)resourceValue;
                    Application.Current.Resources.TryGetValue("HealthyFatsA2", out resourceValue);
                    colorA2 = (Color)resourceValue;
                    Application.Current.Resources.TryGetValue("GreensA1", out resourceValue);
                    colorB1 = (Color)resourceValue;
                    headerLabelString = "Healthy Fats";
                    itemsSourcePropertyString = "HealthyFats";
                    heightRequestPropertyString = "HeightH";
                    break;

                case Kind.Condiment:
                    //pageToNavigateTo = App.CondimentsPage;
                    Application.Current.Resources.TryGetValue("CondimentsA1", out resourceValue);
                    colorA1 = (Color)resourceValue;
                    Application.Current.Resources.TryGetValue("CondimentsA2", out resourceValue);
                    colorA2 = (Color)resourceValue;
                    Application.Current.Resources.TryGetValue("HealthyFatsA1", out resourceValue);
                    colorB1 = (Color)resourceValue;
                    headerLabelString = "Condiments";
                    itemsSourcePropertyString = "Condiments";
                    heightRequestPropertyString = "HeightC";
                    break;

                default:
                    break;
            }
            #endregion Kind Switch... ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            Application.Current.Resources.TryGetValue("StringExists2Visibility", out resourceValue);
            var ToBeVisible = (IValueConverter)resourceValue;
            var skiaPainter = new SkiaPainter(kind);
            BackgroundColor = colorA1;

            #region //     HEADER w/ NAV      \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            var header = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 0, BackgroundColor = colorB1, HeightRequest = 40, Margin = 0 };

            var canvasL = new SKCanvasView() { HorizontalOptions = LayoutOptions.Fill, WidthRequest = 30 };
            canvasL.PaintSurface += skiaPainter.OnCanvasPaint_Open2Title;
            var headerLabel = new Label()
            {
                Text = headerLabelString,
                TextColor = Color.White,
                FontSize = fontL,
                FontAttributes = FontAttributes.Bold,
                VerticalTextAlignment = TextAlignment.Center,
                BackgroundColor = colorA1,
                HeightRequest = 40,
                LineBreakMode = LineBreakMode.MiddleTruncation,
                Padding = new Thickness(10, 0),                 
            };
            var canvasR = new SKCanvasView() { HorizontalOptions = LayoutOptions.Fill, WidthRequest = 30 };
            canvasR.PaintSurface += skiaPainter.OnCanvasPaint_Title2Sub;




            #region // Navigation...            
            var headerButtonAddGroceries = new Button() { 
                ImageSource = "baseline_add_circle_white_24x24.png", 
                WidthRequest = 40, BackgroundColor = colorA2, Margin = new Thickness(0, 8, 0, 0), Padding = 3, CornerRadius = 0         
            };
            headerButtonAddGroceries.SetBinding(Button.CommandProperty, "NavigateToGroceriesCommand");
            headerButtonAddGroceries.CommandParameter = kind;

            #endregion // ... navigation




            var canvasR2 = new SKCanvasView() { HorizontalOptions = LayoutOptions.Fill, WidthRequest = 30 };
            canvasR2.PaintSurface += skiaPainter.OnCanvasPaint_Sub2Open;
            indicator = new ActivityIndicator() { Color = Color.PaleGoldenrod, IsRunning = false, BackgroundColor = colorB1, Margin = new Thickness(0) };

            #endregion  ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            #region//     COLLECTION CTOR      \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            var cvIngredients = new CollectionView()
            {
                ItemSizingStrategy = ItemSizingStrategy.MeasureAllItems,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(8, 4), //(8, 0, 8, 8),
                ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical) { ItemSpacing = 8 },
                EmptyView = new Label()
                {
                    Text = "No items selected, yet... ",
                    FontSize = fontM,
                    FontAttributes = FontAttributes.Italic,
                    Padding = new Thickness(30, 0, 0, 0),
                    TextColor = Color.White
                }
            };
            cvIngredients.SetBinding(CollectionView.ItemsSourceProperty, itemsSourcePropertyString);
            cvIngredients.SetBinding(CollectionView.HeightRequestProperty, heightRequestPropertyString);
            #endregion//     COLLECTION CTOR     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            #region //     DATATEMPLATE (Start)   \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            cvIngredients.ItemTemplate = new DataTemplate(() =>
            {
                var OutterStack = new StackLayout() { Margin = new Thickness(0, 0, 0, 8) }; //This creates the gaps between collection items
                var OutterFrame = new Frame()
                {
                    IsClippedToBounds = true,
                    CornerRadius = 3,
                    Padding = 0,
                    BorderColor = frameBorderColor,
                    Margin = 0

                };

                ///    GRID            \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\ 
                var dtGrid = new Grid
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Margin = new Thickness(0),
                    RowSpacing = 0
                };

                dtGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60) });
                dtGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                dtGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });


                //These rows match GroceriesPage
                dtGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(28) });
                dtGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
                dtGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(22) });
                dtGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(28) });








                #endregion //     DATATEMPLATE (Start)   ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
                #region   // DATATEMPLATE: ICON            \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

                //var iconBG = new BoxView { BackgroundColor = colorA2 };
                //dtGrid.Children.Add(iconBG, 0, 0);
                //Grid.SetRowSpan(iconBG, 4);

                var iIcon = new Image { Margin = 0, VerticalOptions = LayoutOptions.FillAndExpand };
                iIcon.SetBinding(Image.SourceProperty, "IconName");
                iIcon.SetBinding(Image.BackgroundColorProperty, "IconColor1", converter: fromHex);
                dtGrid.Children.Add(iIcon, 0, 0);
                Grid.SetRowSpan(iIcon, 4); // 3);

                var info1String = new Label()
                {
                    FontSize = fontB,
                    FontAttributes = FontAttributes.Bold,
                    HorizontalTextAlignment = TextAlignment.Center,
                    TextColor = Color.White
                };
                info1String.SetBinding(Label.TextProperty, "Info1String");
                info1String.SetBinding(Label.IsVisibleProperty, "Info1String", converter: ToBeVisible);
                dtGrid.Children.Add(info1String, 0, 2);

                #endregion //     DATATEMPLATE ICON   ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
                #region    // DATATEMPLATE: NAME & DESC     \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                var slNamesEtc = new StackLayout() { Padding = new Thickness(3, 0, 0, 0), Spacing = -1 };
                Grid.SetRowSpan(slNamesEtc, 3);

                var name1Label = new Label { FontSize = fontM, FontAttributes = FontAttributes.Bold, LineBreakMode = LineBreakMode.MiddleTruncation };
                var name2Label = new Label { FontSize = fontM, LineBreakMode = LineBreakMode.MiddleTruncation };
                var etcLabel = new Label { FontSize = fontB, LineBreakMode = LineBreakMode.MiddleTruncation };

                name1Label.SetBinding(Label.TextProperty, "Name1");
                name2Label.SetBinding(Label.TextProperty, "Name2");
                etcLabel.SetBinding(Label.TextProperty, "EtcString");

                slNamesEtc.Children.Add(name1Label);
                slNamesEtc.Children.Add(name2Label);
                slNamesEtc.Children.Add(etcLabel);
                dtGrid.Children.Add(slNamesEtc, 1, 0);

                #endregion //    NAME & DESC     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
                #region    // DATATEMPLATE: USER SERVING SIZES (Top Right Corner)    \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                var userUOMFontSize = Device.GetNamedSize(NamedSize.Body, typeof(Label));

                Application.Current.Resources.TryGetValue("UomLabel", out var resourceValue);
                var UomLabelstyle = (Style)resourceValue;
                Application.Current.Resources.TryGetValue("UomEntry", out resourceValue);
                var UomEntrystyle = (Style)resourceValue;


                var userUOMStack = new StackLayout() { HorizontalOptions = LayoutOptions.Start, Padding = new Thickness(0, 8, 0, 8), Spacing = -2, };

                var userUOMP = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.Start };
                var uomText = new String[2] { "QtyPortion", "UomPortion" };
                var entryP = new Entry() { Style = UomEntrystyle };
                entryP.SetBinding(Entry.TextProperty, uomText[0]);
                var labelP = new Label() { Style = UomLabelstyle };
                labelP.SetBinding(Label.TextProperty, uomText[1]);


                var userUOMW = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.Start };
                uomText = new String[2] { "QtyWeight", "UomWeight" };
                var entryW = new Entry() { Style = UomEntrystyle };
                entryW.SetBinding(Entry.TextProperty, uomText[0]);
                entryW.SetBinding(Entry.IsVisibleProperty, uomText[1], converter: ToBeVisible);
                var labelW = new Label() { Style = UomLabelstyle };
                labelW.SetBinding(Label.TextProperty, uomText[1]);
                labelW.SetBinding(Label.IsVisibleProperty, uomText[1], converter: ToBeVisible);


                var userUOMV = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.Start };
                uomText = new String[2] { "QtyVolume", "UomVolume" };
                var entryV = new Entry() { Style = UomEntrystyle };
                entryV.SetBinding(Entry.TextProperty, uomText[0]);
                entryV.SetBinding(Entry.IsVisibleProperty, uomText[1], converter: ToBeVisible);
                var labelV = new Label() { Style = UomLabelstyle };
                labelV.SetBinding(Label.TextProperty, uomText[1]);
                labelV.SetBinding(Label.IsVisibleProperty, uomText[1], converter: ToBeVisible);


                var userUOMC = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.Start };
                uomText = new String[2] { "QtyCount", "UomCount" };
                var entryC = new Entry() { Style = UomEntrystyle };
                entryC.SetBinding(Entry.TextProperty, uomText[0]);
                entryC.SetBinding(Entry.IsVisibleProperty, uomText[1], converter: ToBeVisible);
                var labelC = new Label() { Style = UomLabelstyle };
                labelC.SetBinding(Label.TextProperty, uomText[1]);
                labelC.SetBinding(Label.IsVisibleProperty, uomText[1], converter: ToBeVisible);

                userUOMP.Children.Add(entryP);
                userUOMP.Children.Add(labelP);
                userUOMStack.Children.Add(userUOMP);

                userUOMW.Children.Add(entryW);
                userUOMW.Children.Add(labelW);
                userUOMStack.Children.Add(userUOMW);

                userUOMV.Children.Add(entryV);
                userUOMV.Children.Add(labelV);
                userUOMStack.Children.Add(userUOMV);

                userUOMC.Children.Add(entryC);
                userUOMC.Children.Add(labelC);
                userUOMStack.Children.Add(userUOMC);

                dtGrid.Children.Add(userUOMStack, 2, 0);
                Grid.SetRowSpan(userUOMStack, 4);

                #endregion //    DATATEMPLATE: USER's SIZE INPUT (at Bottom)     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
                #region// DATATEMPLATE: WRAP UP    \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                ///dtGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(18) }); //BOTTOM MARGIN
                OutterFrame.Content = dtGrid;
                OutterStack.Children.Add(OutterFrame);
                return OutterStack;

            });
            #endregion//      END DATATEMPLATE        ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

            #region //     COMPOSE ELEMENTS      \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            header.Children.Add(canvasL);
            header.Children.Add(headerLabel);
            header.Children.Add(canvasR);
            header.Children.Add(headerButtonAddGroceries);
            header.Children.Add(canvasR2);
            header.Children.Add(indicator);


            Children.Add(header);
            Children.Add(cvIngredients);
            //Children.Add(footer);

            #endregion  ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

        }





    }
}

