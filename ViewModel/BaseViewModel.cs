using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DTO_Validation.ViewModel
{
    public abstract class BaseViewModel : ObservableObject, INotifyDataErrorInfo
    {
        private Dictionary<string, HashSet<string>> _errors;

        public bool HasErrors => _errors?.Count > 0;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (_errors == null || !_errors.TryGetValue(propertyName, out var propertyErrors))
                yield break;

            foreach (var error in propertyErrors)
                yield return error;
        }
        public bool HasValidationErrors([CallerMemberName] string propertyName = "")
        {
            if (propertyName == null)
                propertyName = string.Empty;

            if (_errors == null || !_errors.TryGetValue(propertyName, out var propertyErrors))
                return false;

            return propertyErrors.Count > 0;
        }

        protected void AddValidationError(string error, [CallerMemberName] string propertyName = "")
        {
            if (propertyName == null)
                propertyName = string.Empty;

            if (_errors == null)
                _errors = new Dictionary<string, HashSet<string>>();

            if (!_errors.TryGetValue(propertyName, out var propertyErrors))
                _errors[propertyName] = propertyErrors = new HashSet<string>();

            if (propertyErrors.Add(error))
                RaiseValidationErrorsChanged(propertyName);
        }

        protected void ClearValidationErrors([CallerMemberName] string propertyName = "")
        {
            if (_errors == null)
                return;

            if (propertyName == null)
                propertyName = string.Empty;

            if (_errors.Remove(propertyName))
                RaiseValidationErrorsChanged(propertyName);

            if (_errors.Count == 0)
                _errors = null;
        }

        protected void ClearAllValidationErrors()
        {
            _errors = null;
            RaiseValidationErrorsChanged(string.Empty);
        }

        protected virtual void RaiseErrorsChanged(DataErrorsChangedEventArgs e) => ErrorsChanged?.Invoke(this, e);

        private void RaiseValidationErrorsChanged(string propertyName)
        {
            RaisePropertyChanged(nameof(HasErrors));
            RaiseErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
