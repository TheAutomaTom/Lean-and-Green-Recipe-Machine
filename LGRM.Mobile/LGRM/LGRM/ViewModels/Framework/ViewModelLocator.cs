using System;
using System.Collections.Generic;
using System.Text;

namespace LGRM.XamF.ViewModels.Framework
{
    public static class ViewModelLocator
    {
        public static CookbookVM CookbookVM { get; set; } = new CookbookVM(App.NavigationService);

        public static RecipeVM RecipeVM { get; set; } = new RecipeVM(App.NavigationService);

        public static GroceriesVM GroceriesVM { get; set; } = new GroceriesVM();


    }
}
