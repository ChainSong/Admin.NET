namespace Admin.NET.MAUI2C;

public partial class SignInFormModel : BaseFormModel
{
    [ObservableProperty]
    [Required(ErrorMessage = "请输入您的账号")]
    //[Phone(ErrorMessage = "请输入您的账号")]
    [NotifyPropertyChangedFor(nameof(UserNameErrors))]
    [NotifyDataErrorInfo]
    string userName;

    [ObservableProperty]
    [Required(ErrorMessage = "请输入密码")]
    //[Password(
    //    IncludesLower = true,
    //    IncludesNumber = true,
    //    IncludesSpecial = true,
    //    IncludesUpper = true,
    //    MinimumLength = 6,
    //    ErrorMessage = "Please enter a strong password: from 8 characters, 1 upper, 1 lower, 1 digit, 1 special character"
    //)]
    [NotifyPropertyChangedFor(nameof(PasswordErrors))]
    [NotifyDataErrorInfo]
    string password;

    public IEnumerable<ValidationResult> UserNameErrors => GetErrors(nameof(UserName));
    public IEnumerable<ValidationResult> PasswordErrors => GetErrors(nameof(Password));

    protected override string[] ValidatableAndSupportPropertyNames => new[]
    {
        nameof(UserName),
        nameof(UserNameErrors),
        nameof(Password),
        nameof(PasswordErrors),
    };
}

