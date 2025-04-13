using System;
using System.Collections.ObjectModel;

namespace DTO_Validation.ViewModels
{
  internal class MainWindowViewModel : BaseViewModel
  {

    public ObservableCollection<Model.PersonDTO> People { get; }

    public MainWindowViewModel()
    {
      People = new ObservableCollection<Model.PersonDTO>();

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
      vm.SaveChanges = new Action<Model.PersonDTO>(SetPersonChanges);
      _personWindow.Show();
    }

    private bool OnCanAddPersonCommandExecute(object arg) => true;

    private void SetPersonChanges(Model.PersonDTO pm)
    {
      People.Add(pm);
    }
  }
}
