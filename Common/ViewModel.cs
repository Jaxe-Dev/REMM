using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace REMM.Common
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string property) { }

        private void RaisePropertyChanged(string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
            OnPropertyChanged(property);
        }

        protected void RaisePropertiesChanged(params string[] properties)
        {
            foreach (var property in properties) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property)); }
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string property = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) { return false; }

            field = value;
            RaisePropertyChanged(property);

            return true;
        }

        protected bool SetFields<T>(ref T field, T value, string[] others, [CallerMemberName] string property = null)
        {
            var change = SetField(ref field, value, property);
            RaisePropertiesChanged(others);

            return change;
        }
    }
}
