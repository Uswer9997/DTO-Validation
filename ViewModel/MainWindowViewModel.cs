using BaseClasses;
using Model;
using System.Collections.ObjectModel;

namespace DTO_Validation.ViewModel
{
    internal class MainWindowViewModel : BaseInpc
    {
        private readonly ModelOfApp model;

        public  ReadOnlyObservableCollection<Person> People { get; }

        public MainWindowViewModel()
        {
            model = new ModelOfApp();
            People = model.GetPeople();
        }

        public PersonViewModel CreatePersonVM() => new PersonViewModel(model);
    }
}
