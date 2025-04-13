using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DTO_Validation
{
  public abstract class ObservableObject : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        protected void RaisePropertyChanging([CallerMemberName] string propertyName = null) => RaisePropertyChanging(new PropertyChangingEventArgs(propertyName));
        protected virtual void RaisePropertyChanging(PropertyChangingEventArgs e) => PropertyChanging?.Invoke(this, e);

        protected void RaiseAllPropertiesChanged() => RaisePropertyChanged(string.Empty);
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null) => RaisePropertyChanged(new PropertyChangedEventArgs(propertyName));
        protected virtual void RaisePropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

    }
}
