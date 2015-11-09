using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IDemotivator.Models
{
    public class ProfileViewModel
    {
        public AspNetUser User { get; set; }
        public ICollection<Demotivator> Demotivator { get; set; }
        public Single Rate { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = Resources.Resource.PassErrorMessage, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = Resources.Resource.PassNotConfirm)]
        public string ConfirmPassword { get; set; }
    }
}
