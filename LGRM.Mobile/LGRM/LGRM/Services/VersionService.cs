using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Xamarin.Essentials;

namespace LGRM.XamF.Services
{
    public class VersionService
    {
        //      MEMBERS         \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\


        public DateTime InstalledDbVersion { get; set; }
        public DateTime LatestDbVersion { get; } // from RemoteDataService
        public bool DbIsUpdated => (InstalledDbVersion >= LatestDbVersion);

        public readonly string ShippedCatalog;
        public readonly string FooterText;
        public readonly string CategoriesPickerDefault;


        //      CTOR            \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public VersionService(IRemoteDataService remote)
        {
            #region Read appsettings.json...
            var assembly = Assembly.GetExecutingAssembly();
            var resName = assembly.GetManifestResourceNames()
                ?.FirstOrDefault(r => r.EndsWith("appsettings.json", StringComparison.OrdinalIgnoreCase));
            var file = assembly.GetManifestResourceStream(resName);
            var sr = new StreamReader(file);
            var json = sr.ReadToEnd();
            var j = JObject.Parse(json);
            #endregion read appsettings.json

            // InstalledDbVersion...
            if (Preferences.ContainsKey("installedDbVersion"))
            {
                InstalledDbVersion = Preferences.Get("installedDbVersion", DateTime.Now);
            }
            else // ContainsKey = false... this is a first run.
            {
                InstalledDbVersion = new DateTime(1900);
            }

            LatestDbVersion = remote.GetDbUpdated();
            // DbIsUpdated = InstalledDbVersion >= LatestDbVersion;

            ShippedCatalog = j.Value<string>("shippedCatalog");
            FooterText = j.Value<string>("footerText") + " " + DateTime.Now.ToString("yyMMdd");
            CategoriesPickerDefault = j.Value<string>("categoriesPickerDefault");
        }

        //      METHODS         \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public void UpdateVersion()
        {
            InstalledDbVersion = DateTime.Now;
            Preferences.Set("installedDbVersion", InstalledDbVersion);
        }





























    }




}

