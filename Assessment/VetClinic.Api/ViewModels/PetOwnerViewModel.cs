using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace VetClinic.Api.ViewModels
{
    public class PetOwnerViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = String.Empty;

        public string Surname { get; set; } = String.Empty;

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = String.Empty;

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        public string AccountNumber { get; set; }

        [Display(Name = "ID Number")]
        public string IDNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public string[] PetDetailIds { get; set; }

        //Associated Pet to Pet Owner
        public ICollection<PetDetailViewModel> PetDetails { get; set; } = new List<PetDetailViewModel>();
    }

    /// <summary>
    /// NB: Was trying out Fluent Validation, Data annotations seem better to me
    /// </summary>
    public class PetOwnerViewModelValidator : AbstractValidator<PetOwnerViewModel>
    {
        public PetOwnerViewModelValidator()
        {
            RuleFor(petowner => petowner.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(petowner => petowner.Surname).NotEmpty().WithMessage("Surname cannot be empty");
            RuleFor(petowner => petowner.AccountNumber)
               .NotEmpty().WithMessage("Account Number cannot be empty");
            RuleFor(petowner => petowner.Phone)
                .NotEmpty().WithMessage("Phone Number cannot be empty")
                .Length(10).WithMessage("Phone Number cannot be less or more that 10 digits");
            RuleFor(petowner => petowner.Email)
                .NotEmpty().WithMessage("Email cannot be empty")
                .EmailAddress().WithMessage("Email should be correctly formatted i.e you@example.com");
        }
    }
}
