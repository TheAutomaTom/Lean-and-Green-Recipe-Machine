using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using LGRM.Model;
using LGRM.XamF.Pages;
using LGRM.XamF.Services;

namespace LGRM.XamF
{
    public partial class App : Application
    {
        public static SQLiteDataService MySQLite { get; } = new SQLiteDataService();
        public static VersionService V { get; } = new VersionService(new MockRemoteDataService());
        public static ObservableCollection<Grocery> Groceries { get; set; }
        public static NavigationService NavigationService { get; } = new NavigationService();

        public App()
        {
            CompareVersion();
            InitializeComponent();
            NavigationServiceConfig();
            MainPage = new NavigationPage(new CookbookPage());

        }

        public void CompareVersion()
        {
            if (!V.DbIsUpdated) // Install SQLites Groceries catalog
            {
                var createTable = Task.Run(() => MySQLite.CreateTableOfGroceriesAsync());
                createTable.Wait();

                var populateTableFromJson = Task.Run(() => MySQLite.PopulateTableOfGroceriesAsync(V.ShippedCatalog));
                populateTableFromJson.Wait();

                V.UpdateVersion();

            }

        }

        public void NavigationServiceConfig()
        {
            NavigationService.Configure("CookbookPage", typeof(CookbookPage));
            NavigationService.Configure("RecipePage", typeof(RecipePage));
            NavigationService.Configure("GroceriesPageL", typeof(GroceriesPageL));
            NavigationService.Configure("GroceriesPageG", typeof(GroceriesPageG));
            NavigationService.Configure("GroceriesPageH", typeof(GroceriesPageH));
            NavigationService.Configure("GroceriesPageC", typeof(GroceriesPageC));

        }

    }
}
