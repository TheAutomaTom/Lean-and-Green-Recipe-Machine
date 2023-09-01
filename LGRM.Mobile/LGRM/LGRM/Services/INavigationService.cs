using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LGRM.XamF.Services
{
    public interface INavigationService
    {
        Page MainPage { get; }

        void Configure(string key, Type pageType);
        void GoBack();
        Task NavigateTo(string pageKey, object parameter = null);
    }
}