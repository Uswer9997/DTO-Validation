namespace DTO_Validation.ViewModels
{
  public class ValidationRes
  {
    public bool Success { get; }
    public string ErrorMessage { get; }
    public string PropertyName { get; }

    public ValidationRes(bool result, string errorText, string propertyName)
    {
      Success = result;
      ErrorMessage = errorText;
      PropertyName = propertyName;
    }
  }
}
