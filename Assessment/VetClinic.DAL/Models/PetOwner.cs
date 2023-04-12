using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VetClinic.DAL.Models.Interfaces;

namespace VetClinic.DAL.Models
{
    public class PetOwner : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [MaxLength(13, ErrorMessage = "ID Number must be 13 characters in length")]
        [Display(Name = "ID Number")]
        public string IDNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

        //Associated Pet to Pet Owner
        public ICollection<PetDetail>? PetDetails { get; set; }
    }
}
