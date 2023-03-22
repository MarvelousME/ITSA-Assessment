using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VetClinic.DAL.Models
{
    public class PetOwner
    {
        [Key]
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        [Display(Name="Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [MinLength(13, ErrorMessage = "ID Number must be 13 characters in length")]
        [MaxLength(13, ErrorMessage = "ID Number must be 13 characters in length")])]
        [Display(Name = "ID Number")]
        public string IDNumber { get; set; }

        [NotMapped]
        public DateTime? Created { get; set; }
    }
}
