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

        //[Required]
        //[EmailAddress]
        //[DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = String.Empty;

        //[Required]
        //[MinLength(13, ErrorMessage = "ID Number must be 13 characters in length")]
        //[MaxLength(13, ErrorMessage = "ID Number must be 13 characters in length")]
        [Display(Name = "ID Number")]
        public string IDNumber { get; set; } = String.Empty;
        //public string CreatedBy { get; set; }
        //public string UpdatedBy { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }

        //Associated Pet to Pet Owner
        public ICollection<PetDetailViewModel> PetDetails { get; set; } = new List<PetDetailViewModel>();
    }

    public class PetOwnerViewModelValidator : AbstractValidator<PetOwnerViewModel>
    {
        public PetOwnerViewModelValidator()
        {
            RuleFor(petowner => petowner.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(petowner => petowner.Surname).NotEmpty().WithMessage("Surname cannot be empty");
            RuleFor(petowner => petowner.Phone)
                .NotEmpty().WithMessage("Phone Number cannot be empty")
                .Length(10).WithMessage("Phone Number cannot be less or more that 10 digits");
            RuleFor(petowner => petowner.Email)
                .NotEmpty().WithMessage("Email cannot be empty")
                .EmailAddress().WithMessage("Email should be correctly formatted i.e you@example.com");
        }
    }
}
