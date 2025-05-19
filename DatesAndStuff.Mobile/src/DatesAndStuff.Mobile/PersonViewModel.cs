using DatesAndStuff;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace DatesAndStuff.Mobile;

public class PersonViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
{
    public PersonViewModel()
    {
        ValidateProperty(nameof(SalaryIncreasePercentage), nameof(SalaryIncreasePercentage));
    }

    public Person Person { get; set; } = new DatesAndStuff.Person("Test Kálmán",
                    new EmploymentInformation(5000, new Employer("RO12312312", "Valami utca sok", "Gipsz Jakab", null)),
                    new UselessPaymentService(),
                    new LocalTaxData("UAT Gazdag varos"),
                    new FoodPreferenceParams() { CanEatChocolate = true, CanEatGluten = false });

    public class SalaryIncreaseData
    {
        [Required(ErrorMessage = "Percentage should be specified")]
        [Range(-10d, Double.MaxValue, ErrorMessage = "The specified percentag should be between -10 and infinity.")]
        public double? SalaryIncreasePercentage { get; set; }
    }

    private SalaryIncreaseData salaryIncreaseData = new SalaryIncreaseData();
    public string SalaryIncreasePercentage
    {
        get => salaryIncreaseData.SalaryIncreasePercentage?.ToString();
        set
        {
            if (double.TryParse(value, out var parsed))
                salaryIncreaseData.SalaryIncreasePercentage = parsed;
            else
                salaryIncreaseData.SalaryIncreasePercentage = null;

            ValidateProperty(nameof(SalaryIncreasePercentage));
            OnPropertyChanged();
        }
    }

    public string SalaryIncreasePercentageValidationMessage =>
    _errors.TryGetValue(nameof(SalaryIncreasePercentage), out var errors)
        ? string.Join(Environment.NewLine, errors)
        : string.Empty;

    public ICommand SubmitCommand => new Command(OnSubmit);

    private void OnSubmit()
    {
        if (!HasValidationErrors)
        {
            Person.IncreaseSalary(salaryIncreaseData.SalaryIncreasePercentage.Value);
            OnPropertyChanged(nameof(Person));
        }
    }

    #region Validation
    private readonly Dictionary<string, List<string>> _errors = new();

    public bool HasValidationErrors => _errors.Any();

    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    public IEnumerable GetErrors(string? propertyName)
    {
        if (propertyName != null && _errors.TryGetValue(propertyName, out var errors))
            return errors;

        return null;
    }

    public bool HasErrors => HasValidationErrors;

    private void ValidateProperty(object value, [CallerMemberName] string propertyName = "")
    {
        _errors.Remove(propertyName);

        var context = new ValidationContext(salaryIncreaseData) { MemberName = propertyName };
        var propertyInfo = typeof(SalaryIncreaseData).GetProperty(propertyName);
        var valueToValidate = propertyInfo?.GetValue(salaryIncreaseData);

        var results = new List<ValidationResult>();
        if (!Validator.TryValidateProperty(valueToValidate, context, results))
        {
            _errors[propertyName] = results.Select(r => r.ErrorMessage).ToList();
        }

        // the validation message might have changed and the state of errors
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        OnPropertyChanged(propertyName + "ValidationMessage");
        OnPropertyChanged(nameof(HasValidationErrors));
    }
    #endregion

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}