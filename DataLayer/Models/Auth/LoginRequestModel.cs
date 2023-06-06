using System.ComponentModel.DataAnnotations;

namespace BezeqFinalProject.Common.Models.Auth;

public class LoginRequestModel {
    [Required(ErrorMessage = "Mandatory")]
    [EmailAddress(ErrorMessage = "Illegal email address")]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Mandatory")]
    [MinLength(6, ErrorMessage = "Minimum 6 characters")]
    //[RegularExpression("^(?=.*[A-Z].*[A-Z])(?=.*[!@#$&*])(?=.*[0-9].*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{8}$", ErrorMessage = "Password is not strong enough")]
    public string Pwd { get; set; }

    public bool RemeberMe { get; set; }
}
