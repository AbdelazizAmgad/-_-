using System.ComponentModel.DataAnnotations;

namespace CinemaProject.ViewModels
{
    public class ForgetPasswordVM
    {
        [Required] public string UserNameOrEmail { get; set; } = string.Empty;
    }
}