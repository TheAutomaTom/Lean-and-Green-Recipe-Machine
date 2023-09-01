using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace LGRM.XamF.ViewModels
{
    public class BaseVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //public virtual void Initialize(object parameter)   //see NavigationService.NavigatTo()... this is used to pass "Item Selected" info to the next viewmodel called.
        public virtual async Task Initialize(object parameter)   //see NavigationService.NavigatTo()... this is used to pass "Item Selected" info to the next viewmodel called.
        {

        }


        public int CountExceptions;             //Used in OnMySelectionChangedCommand()
        public void ReportException(Exception e)
        {
            CountExceptions++;
            Debug.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~!!! Exception Count: " + CountExceptions);
            Debug.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ InnerEx......" + e.InnerException);
            Debug.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Message......" + e.Message);
            Debug.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ StackTrace..." + e.StackTrace);
            Debug.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Source......." + e.Source);
            Debug.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Data........." + e.Data);
        }



    }



}
