using System;
using System.Collections.ObjectModel;

namespace DTO_Validation.ViewModel
{
  internal class MainWindowViewModel : BaseViewModel
  {

    public ObservableCollection<Model.Person> People { get; }

    public MainWindowViewModel()
    {
      People = new ObservableCollection<Model.Person>();

      AddPersonCommand = new RelayCommand(OnAddPersonCommandExecute, OnCanAddPersonCommandExecute);
    }

    public RelayCommand AddPersonCommand { get; }

    private void OnAddPersonCommandExecute(object obj)
    {
      var vm = new PersonViewModel();

      var _personWindow = new AddPersonWindow()
      {
        DataContext = vm
      };
      vm.SaveChanges = new Action<PersonViewModel>(SepPersonChanges);
      _personWindow.Show();
    }

    private bool OnCanAddPersonCommandExecute(object arg) => true;

    private void SepPersonChanges(PersonViewModel pvm)
    {
      People.Add(pvm._model);
    }
  }
}
