using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace CarMate.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class RegisterExternalLoginModel
    {
        [Required(ErrorMessageResourceName = "LoginRequired", ErrorMessageResourceType = typeof(Resources.Account))]
        [Display(Name = "Login", ResourceType = typeof(Resources.Account))]
        public string UserName { get; set; }

        //[Required(ErrorMessage = "Адрес электронной почты - обязательное поле")]
        //[Display(Name = "Email", ResourceType = typeof(Resources.Account))]
        //[DataType(DataType.EmailAddress)]
        //public string Email { get; set; }

        [Required(ErrorMessageResourceName = "UnitFuelConsumptionRequired", ErrorMessageResourceType = typeof(Resources.Account))]
        [Display(Name = "UnitFuelConsumption", ResourceType = typeof(Resources.Account))]
        public int UnitFuelConsumptionId { set; get; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required(ErrorMessageResourceName = "OldPasswordRequired", ErrorMessageResourceType = typeof(Resources.Account))]
        [DataType(DataType.Password)]
        [Display(Name = "OldPassword", ResourceType = typeof(Resources.Account))]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceName = "NewPasswordRequired", ErrorMessageResourceType = typeof(Resources.Account))]
        [StringLength(100, ErrorMessageResourceName = "PasswordStringLength", ErrorMessageResourceType = typeof(Resources.Account), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "NewPassword", ResourceType = typeof(Resources.Account))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resources.Account))]
        [Compare("NewPassword", ErrorMessageResourceName="ConfirmPasswordCompare", ErrorMessageResourceType = typeof(Resources.Account))]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required(ErrorMessageResourceName = "LoginRequired", ErrorMessageResourceType = typeof(Resources.Account))]
        [Display(Name = "Login", ResourceType = typeof(Resources.Account))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Resources.Account))]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resources.Account))]
        public string Password { get; set; }

        [Display(Name = "RememberMe", ResourceType = typeof(Resources.Account))]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required(ErrorMessageResourceName = "LoginRequired", ErrorMessageResourceType = typeof(Resources.Account))]
        [Display(Name = "Login", ResourceType = typeof(Resources.Account))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Resources.Account))]
        [StringLength(100, ErrorMessageResourceName = "PasswordStringLength", ErrorMessageResourceType = typeof(Resources.Account), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resources.Account))]
        public string Password { get; set; }

        [Required(ErrorMessageResourceName = "ConfirmPasswordRequired", ErrorMessageResourceType = typeof(Resources.Account))]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resources.Account))]
        [Compare("NewPassword", ErrorMessageResourceName = "ConfirmPasswordCompare", ErrorMessageResourceType = typeof(Resources.Account))]
        public string ConfirmPassword { get; set; }






        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Resources.Account))]
        [Display(Name = "Email", ResourceType = typeof(Resources.Account))]
        //[DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessageResourceName = "EmailType", ErrorMessageResourceType = typeof(Resources.Account), ErrorMessage = null)]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "UnitFuelConsumptionRequired", ErrorMessageResourceType = typeof(Resources.Account))]
        [Display(Name = "UnitFuelConsumption", ResourceType = typeof(Resources.Account))]
        public int UnitFuelConsumptionId { set; get; }

        //[Display(Name = "Имя", ResourceType = typeof(Resources.Account))]
        //public string FirstName { get; set; }

        //[Display(Name = "Фамилия", ResourceType = typeof(Resources.Account))]
        //public string LastName { get; set; }

        //[Display(Name = "День рождения", ResourceType = typeof(Resources.Account))]
        //[DataType(DataType.Date)]
        //public DateTime? Year { get; set; }

        //[Display(Name = "Страна", ResourceType = typeof(Resources.Account))]
        //public string Country { get; set; }

        //[Display(Name = "Регион", ResourceType = typeof(Resources.Account))]
        //public string Region { get; set; }

        //[Display(Name = "Город", ResourceType = typeof(Resources.Account))]
        //public string City { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
