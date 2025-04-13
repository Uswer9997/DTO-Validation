using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DTO_Validation.ViewModels
{
  public abstract class BaseViewModel : INotifyDataErrorInfo, IValidatable
  {
    #region INotifyDataErrorInfo

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

    private void RaiseValidationErrorsChanged(string propertyName)
    {
      RaiseErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
    }

    protected virtual void RaiseErrorsChanged(DataErrorsChangedEventArgs e) => ErrorsChanged?.Invoke(this, e);
    #endregion

    #region IValidatable
    private Dictionary<string, Func<object, ValidationRes>> _verificationMethods;

    public void Register(string propertyName, Func<object, ValidationRes> validatingMetod)
    {
      if (propertyName == null)
        propertyName = string.Empty;

      if (_verificationMethods == null)
        _verificationMethods = new Dictionary<string, Func<object, ValidationRes>>();

      if (validatingMetod != null)
        _verificationMethods[propertyName] = validatingMetod;
    }

    public bool Validate<T>(T validateValue, [CallerMemberName] string propertyName = null)
    {
      bool _isValid = true;

      if (propertyName == null)
        propertyName = string.Empty;

      ClearValidationErrors(propertyName);
      Func<object, ValidationRes> validatingMetod;

      if (_verificationMethods != null && _verificationMethods.TryGetValue(propertyName, out validatingMetod))
      {
        ValidationRes result = validatingMetod(validateValue);
        if (!result.Success) // Ошибка валидации есть
        {
          AddValidationError(result.ErrorMessage, propertyName);
          _isValid = false;
        }
      }

      return _isValid;
    }
    #endregion


  }
}
