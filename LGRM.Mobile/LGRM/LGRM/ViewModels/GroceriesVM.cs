using LGRM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace LGRM.XamF.ViewModels
{
    public class GroceriesVM : BaseVM
    {

        #region Members...
        public string FooterText => App.V.FooterText;
        bool isLoading { get; set; } //See GroceriesVM.Initialize

        Kind kind { get; set; }
        
        ObservableCollection<Grocery> displayedGroceries { get; set; }
        public ObservableCollection<Grocery> DisplayedGroceries
        {
            get => displayedGroceries;
            set
            {
                displayedGroceries = value;
                OnPropertyChanged("DisplayedGroceries");
            }
        }

        List<object> selectedItems { get; set; } //To capture View's binding input
        public List<object> SelectedItems
        {
            get => selectedItems;
            set
            {
                if (selectedItems != value)                    
                {   
                    selectedItems = value;                    
                    if (!isLoading)                    
                    {                    
                        OnPropertyChanged("SelectedGroceries"); // neccessary?           
                        //SetShowSelectedItemsButton();
                    }
                }
            }
        }

        ObservableCollection<string> categories { get; set; }
        public ObservableCollection<string> Categories
        {
            get => categories;
            set
            {
                categories = value;
                OnPropertyChanged("Categories");
            }
        }

        object selectedCategory { get; set; }
        public object SelectedCategory
        {
            get
            {
                return selectedCategory != null
                   ? selectedCategory
                   : App.V.CategoriesPickerDefault;
            }
            set
            {
                if (!isLoading && value != null && selectedCategory != value)
                {
                    selectedCategory = value;
                    DisplayedGroceries = GetGroceries(searchQuery);                    
                }
                OnPropertyChanged("SelectedCategory");

            }
        }

        bool clearSearchIsVisible { get; set; }
        public bool ClearSearchIsVisible 
        {
            get => clearSearchIsVisible;
            set
            {
                clearSearchIsVisible = 
                    string.IsNullOrWhiteSpace(searchQuery)                  
                    ? false                  
                    : true;
                OnPropertyChanged("ClearSearchIsVisible");
            }
        }

        public void OnSearchCommand(string query) => DisplayedGroceries = GetGroceries(query);
        string searchQuery { get; set; }
        public string SearchQuery
        {
            get
            {
                ClearSearchIsVisible = true;
                return searchQuery;
            }

            set
            {
                if ( (string)selectedCategory != value )
                {                    
                    searchQuery = value;
                    OnPropertyChanged("SearchQuery");                    
                    if (!isLoading)
                    {
                        OnSearchCommand(searchQuery);
                        SetShowSelectedItemsButton();
                    }
                }
            }
        }


        public ICommand ShowSelectedItemsCommand { get; set; }
        private void OnShowSelectedItemsCommand(object obj)
        {
            if (SelectedItems.Count > 0)
            {
                if (!IsShowingSelectedItems)
                {
                    DisplayedGroceries.Clear();
                    //var selected = new List<Grocery>();
                    foreach (var s in SelectedItems)
                    {
                        DisplayedGroceries.Add((Grocery)s);
                    }
                    IsShowingSelectedItems = true;
                    SetShowSelectedItemsButton();
                }
                else
                {
                    DisplayedGroceries = GetGroceries(SearchQuery);
                    IsShowingSelectedItems = false;
                    SetShowSelectedItemsButton();
                }
            }
        }


        public void SetShowSelectedItemsButton()
        {
            if (/*!isLoading &&*/ !IsShowingSelectedItems)
            {
                DisplayedShowItemsIcon = SelectedItems.Count > 0 
                    ? showSelectedIcon
                    : noIcon;
            }
            else DisplayedShowItemsIcon = showAllIcon;
            
        }
        string IconToDisplayForShowSelectedItems { get; set; }
        public string DisplayedShowItemsIcon
        {
            get => IconToDisplayForShowSelectedItems;
            set
            {
                IconToDisplayForShowSelectedItems = value;
                OnPropertyChanged("DisplayedShowItemsIcon");
            }
        }
        public bool IsShowingSelectedItems { get; set; } = false;
        string noIcon = "no_icon_18dp.png";
        string showSelectedIcon = "show_selected_18dp.png";
        string showAllIcon = "show_all_18dp.png";





        #endregion ... members

        Kind[] kinds;
        public List<ObservableCollection<string>> CategoryLists;

        #region CTOR...
        public GroceriesVM()
        {
            kinds = new Kind[] { Kind.Lean, Kind.Green, Kind.HealthyFat, Kind.Condiment, Kind.All };

            SetCategories();

            priorCatNums = new List<int>();
            currentCatNums  = new List<int>();
            SelectedItems = new List<object>();
            SelectedGroceriesChanged = new Command(OnSelectedGroceriesChanged);

            //SearchCommand = new Command<string>(OnSearchCommand);
            DisplayedShowItemsIcon = noIcon;
            ShowSelectedItemsCommand = new Command(OnShowSelectedItemsCommand);

            isLoading = false;
        }


        public override async Task Initialize(object parameter) // parameter = object[]{ this.kind, List<int>("Catalog Numbers to be Preselected") }
        {
            var paramerters = (object[])parameter;
            this.kind = (Kind)paramerters[0] switch
            {
                Kind.Lean => Kind.Lean,
                Kind.Green => Kind.Green,
                Kind.HealthyFat => Kind.HealthyFat,
                Kind.Condiment => Kind.Condiment,
                _ => Kind.All        
            };

            //UpdateCategories();
            var updateCategories = Task.Run(() => UpdateCategories());
            updateCategories.Wait();

            SelectedCategory = Categories[0];
            DisplayedGroceries = GetGroceries();

            foreach (var g in DisplayedGroceries)
            {
                g.IsSelected = false;
            }

            SelectedItems.Clear();
            priorCatNums.Clear();

            var toBePreselected = (List<int>)paramerters[1];
            if (toBePreselected.Count > 0)
            {
                var preselectGroceries = Task.Run(() => PreselectGroceries(toBePreselected));
                preselectGroceries.Wait();
            }

            SetShowSelectedItemsButton();
        }

        public void PreselectGroceries(List<int> toBePreselected)
        {
                foreach (var catNum in toBePreselected)
                {
                    foreach (var g in DisplayedGroceries)
                    {
                        if (g.CatalogNumber == catNum)
                        {
                            var indexToHighlight = DisplayedGroceries.IndexOf(g);
                            SelectedItems.Add(DisplayedGroceries[indexToHighlight]);
                            priorCatNums.Add(catNum);
                            g.IsSelected = true;

                        }
                    }
                }
            
        }



        #endregion ...CTOR


        public Command SelectedGroceriesChanged { get; set; }
        List<int> priorCatNums { get; set; }
        List<int> currentCatNums { get; set; }
        private void OnSelectedGroceriesChanged()
        {            
            int CatalogNumberChanged;
            bool toBeAdded;
            Ingredient ingredientChanged;

            currentCatNums.Clear();
            foreach (var obj in SelectedItems)
            {
                currentCatNums.Add( (obj as Grocery).CatalogNumber );
            }

            if ( priorCatNums.Count < currentCatNums.Count ) // add item ...
            {
                CatalogNumberChanged = currentCatNums.Except(priorCatNums).ToList()[0]; // ... should only be 1 item in list
                (DisplayedGroceries.FirstOrDefault(g => g.CatalogNumber == CatalogNumberChanged)).IsSelected = true;
                toBeAdded = true;                
            }
            else // remove item ...
            {
                CatalogNumberChanged = priorCatNums.Except(currentCatNums).ToList()[0];
                (DisplayedGroceries.FirstOrDefault(g => g.CatalogNumber == CatalogNumberChanged)).IsSelected = false;
                toBeAdded = false;
            }
            SetShowSelectedItemsButton();

            ingredientChanged = new Ingredient(DisplayedGroceries.FirstOrDefault(g => g.CatalogNumber == CatalogNumberChanged));
            var ResolveData = new object[] { ingredientChanged, toBeAdded };
            
            MessagingCenter.Send<GroceriesVM, object>(this, "UpdateIngredients", ResolveData);

            // Set up for next selection change ...
            priorCatNums.Clear();
            foreach (var g in SelectedItems)
            {
                priorCatNums.Add( ( g as Grocery ).CatalogNumber );
            }
        }

        void SetCategories()
        {
            CategoryLists = new List<ObservableCollection<string>>()
                { new ObservableCollection<string>(), new ObservableCollection<string>(), new ObservableCollection<string>(), new ObservableCollection<string>(), new ObservableCollection<string>()  };

            App.Groceries ??= new ObservableCollection<Grocery>(App.MySQLite.GroceryList ??= new List<Grocery>(App.MySQLite.GetAllGroceries()));
            for (int i = 0; i < 4; i++)
            {
                CategoryLists[i].Add(App.V.CategoriesPickerDefault); // "All Categories"
                var geez = App.Groceries.Where(g => g.Kind == kinds[i]);
                foreach (var c in geez.Select(g => g.Category).Distinct().ToList())
                {
                    CategoryLists[i].Add(c);
                }
            }
            CategoryLists[4].Add("Kind.All Selected!");
            Categories = new ObservableCollection<string>(CategoryLists[0]);
            SelectedCategory = Categories[0];
        }

        void UpdateCategories()
        {
            Categories = this.kind switch
            {
                Kind.Lean => CategoryLists[0],
                Kind.Green => CategoryLists[1],
                Kind.HealthyFat => CategoryLists[2],
                Kind.Condiment => CategoryLists[3],
                _ => CategoryLists[4],
            };
        }




        #region Methods...
        public ObservableCollection<Grocery> GetGroceries( string query = "")
        {
            var result = new ObservableCollection<Grocery>();

            if(Categories == null) { UpdateCategories(); }
            
            if (string.IsNullOrEmpty(query))
            {
                if (SelectedCategory.ToString() == Categories[0])
                {
                    // by Kind only ....
                    result = new ObservableCollection<Grocery>(
                        App.Groceries.Where(g => g.Kind == this.kind
                        ).ToList().OrderBy(g => g.Name1));
                    IsShowingSelectedItems = false;
                }
                else // by Category ...
                {
                    result = new ObservableCollection<Grocery>(
                        App.Groceries.Where(g => g.Kind == this.kind
                                              && g.Category == SelectedCategory.ToString()
                                              ).ToList().OrderBy(g => g.Name1));
                    IsShowingSelectedItems = false;
                }
            }
            else // by Query ...
            {
                query = query.ToLower().Trim();

                if (SelectedCategory.ToString() == Categories[0])
                {
                    // by Query ...


                    try
                    {
                        result = new ObservableCollection<Grocery>(
                            App.Groceries.Where(g => g.Kind == this.kind
                                                && (g.Name1.ToLowerInvariant().Contains(query) ||
                                                     g.Name2.ToLowerInvariant().Contains(query) ||
                                                     g.Desc1.ToLowerInvariant().Contains(query))
                                                     ).ToList().OrderBy(g => g.Name1));
                    }
                    catch (Exception x)
                    {
                        Console.WriteLine(x);
                    }
                    
                    IsShowingSelectedItems = false;
                }
                else // by Query & Category ...
                {
                    result = new ObservableCollection<Grocery>(
                        App.Groceries.Where(g => g.Kind == this.kind
                                            && g.Category == SelectedCategory.ToString()
                                            && (g.Name1.ToLowerInvariant().Contains(query) ||
                                                 g.Name2.ToLowerInvariant().Contains(query) ||
                                                 g.Desc1.ToLowerInvariant().Contains(query))
                                                 ).ToList().OrderBy(g => g.Name1));
                    IsShowingSelectedItems = false;
                }

            }
            return result.Count > 0 ? result : new ObservableCollection<Grocery>(App.Groceries.Where(g => g.Kind == this.kind).ToList().OrderBy(g => g.Name1));
        }
        #endregion ... methods


    }
}
