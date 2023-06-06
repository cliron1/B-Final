using System.ComponentModel.DataAnnotations;

namespace BezeqFinalProject.Common.Models.Auth;

public class SignupRequestModel : LoginRequestModel {
    [Required(ErrorMessage = "Mandatory")]
    public string Name { get; set; }
}
