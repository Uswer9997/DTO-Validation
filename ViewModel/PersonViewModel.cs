using BaseClasses;
using Model;
using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DTO_Validation.ViewModel
{
    public partial class PersonViewModel : BaseInpc
    {
        private readonly ModelOfApp model;

        // Конструктор времени разработки.
        public PersonViewModel()
            : this(new ModelOfApp())
        { }

        public PersonViewModel(ModelOfApp model)
        {
            this.model = model;
            SaveCommand = new RelayCommand(obj => Save(), obj => !HasErrors);
        }


        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {
                Set(ref _name, value ?? string.Empty);
                RaiseErrorsChanged(nameof(SaveCommand));
            }
        }

        private DateTime _dateOfBirth = new DateTime(2010, 10, 23);
        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                Set(ref _dateOfBirth, value);
                RaiseErrorsChanged(nameof(SaveCommand));
            }
        }

        public RelayCommand SaveCommand { get; }

        private void Save()
        {
            if (HasErrors)
            {
                RaiseErrorsChanged(string.Empty);
            }
            else
            {
                model.Save(Name, DateOfBirth);
            }
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

    public partial class PersonViewModel : INotifyDataErrorInfo
    {

        public bool HasErrors =>
            string.IsNullOrWhiteSpace(Name) ||
            string.Equals(Name, "Вася", StringComparison.OrdinalIgnoreCase) ||
            !ValidateAge(DateOfBirth);


        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        protected void RaiseErrorsChanged([CallerMemberName] string propertyName = null)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            RaisePropertyChanged(nameof(HasErrors));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || propertyName is nameof(SaveCommand))
            {
                foreach (var err in GetErrors(nameof(Name)))
                {
                    yield return $"{nameof(Name)}: {err}";
                }
                foreach (var err in GetErrors(nameof(DateOfBirth)))
                {
                    yield return $"{nameof(DateOfBirth)}: {err}";
                }
                yield break;
            }

            if (propertyName is nameof(Name))
            {
                if (string.IsNullOrWhiteSpace(Name))
                {
                    yield return "Name is required";
                }

                if (string.Equals(Name, "Вася", StringComparison.OrdinalIgnoreCase))
                {
                    yield return "Васям не положено!";
                }
            }

            else if (propertyName is nameof(DateOfBirth))
            {
                if (!ValidateAge(DateOfBirth))
                {
                    yield return "Must be 18 or older";
                }
            }
        }
    }
}
