using System;
using System.Runtime.CompilerServices;

namespace DTO_Validation.ViewModels
{
  public interface IValidatable
  {
    void Register(string propertyName, Func<object, ValidationRes> validatingMetod);
    bool Validate<T>(T validateValue, [CallerMemberName] string propertyName = null);
  }
}
