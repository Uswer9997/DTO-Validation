using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DTO_Validation.Model;

namespace DTO_Validation.ViewModels
{
  internal class PersonViewModel : BaseViewModel
  {
    public PersonDTO Person { get; }

    public Action<PersonDTO> SaveChanges { get; set; }

    public PersonViewModel()
    {
      Person = new PersonDTO()
      {
        Name = "",
        DateOfBirth = new DateTime(1975, 10, 23)
      };
      SaveCommand = new RelayCommand(obj => Save(), obj => true);

      Register("Name", ValidateName);
      Register("DateOfBirth", ValidateDateOfBirth);
    }


    public string Name
    {
      get => Person.Name;
      set
      {
        Person.Name = value;
        Validate(value);
      }
    }

    public DateTime DateOfBirth
    {
      get => Person.DateOfBirth;
      set
      {
        Person.DateOfBirth = value;
        Validate(value);
      }
    }

    public RelayCommand SaveCommand
    {
      get;
    }

    public void Save()
    {
      if (IsValid(Person))
      {
        SaveChanges(Person);
      }
    }

    protected override void RaiseErrorsChanged(DataErrorsChangedEventArgs e)
    {
      SaveCommand.RaiseCanExecuteChanged();
      base.RaiseErrorsChanged(e);
    }
       
    private ValidationRes ValidateDateOfBirth(object arg)
    {
      if (!ValidateAge((DateTime)arg))
      {
        return new ValidationRes(false, "Must be 18 or older", "DateOfBirth");
      }

      return new ValidationRes(true, "", "DateOfBirth");
    }

    private bool ValidateAge(DateTime birth)
    {
      var today = DateTime.Today;
      var age = today.Year - birth.Year;
      if (today.Month < birth.Month || today.Month == birth.Month && today.Day < birth.Day)
        age--;

      return age >= 18;
    }

    private ValidationRes ValidateName(object value)
    {
      if (string.IsNullOrEmpty((string)value))
        return new ValidationRes(false, "Name is required", "Name");

      if (string.Equals((string)value, "Вася", StringComparison.OrdinalIgnoreCase))
        return new ValidationRes(false, "Васям не положено!", "Name");

      return new ValidationRes(true, "", "Name");
    }

    private bool IsValid(PersonDTO validatingPerson)
    {
      if (!Validate(validatingPerson.Name, "Name") | !Validate(validatingPerson.DateOfBirth, "DateOfBirth"))
        return false;

      return true;
    }

  }
}
