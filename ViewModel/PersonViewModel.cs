using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DTO_Validation.Model;

namespace DTO_Validation.ViewModel
{
  public class PersonViewModel : BaseViewModel
  {
    public Person _model { get; }

    public Action<PersonViewModel> SaveChanges { get; set; }

    public PersonViewModel()
    {
      _model = new Person()
      {
        Name = "",
        DateOfBirth = new DateTime(1975, 10, 23)
      };
      SaveCommand = new RelayCommand(obj => Save(), obj => !HasErrors);
    }

    public string Name
    {
      get => _model.Name;
      set
      {
        _model.Name = value;
        RaisePropertyChanged();

        ClearValidationErrors();
        if (string.IsNullOrEmpty(value))
          AddValidationError("Name is required");

        else if (string.Equals(value, "Вася", StringComparison.OrdinalIgnoreCase))
          AddValidationError("Васям не положено!");
      }
    }

    public DateTime DateOfBirth
    {
      get => _model.DateOfBirth;
      set
      {
        _model.DateOfBirth = value;
        RaisePropertyChanged();

        ClearValidationErrors();
        if (!ValidateAge(value))
          AddValidationError("Must be 18 or older");
      }
    }

    public RelayCommand SaveCommand
    {
      get;
    }

    public void Save()
    {
      ClearAllValidationErrors();
      SaveChanges(this);
    }

    protected override void RaiseErrorsChanged(DataErrorsChangedEventArgs e)
    {
      SaveCommand.RaiseCanExecuteChanged();
      base.RaiseErrorsChanged(e);
    }

    private bool ValidateAge(DateTime birth)
    {
      var today = DateTime.Today;
      var age = today.Year - birth.Year;
      if (today.Month < birth.Month || today.Month == birth.Month && today.Day < birth.Day)
        age--;

      return age >= 18;
    }
  }
}
