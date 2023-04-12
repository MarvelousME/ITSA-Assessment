using System.ComponentModel.DataAnnotations;
using VetClinic.Core.Enums;
using VetClinic.DAL.Models;

namespace VetClinic.Api.ViewModels
{
    public class PetDetailViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = String.Empty;

        [Required]
        [Display(Name = "Animal Type")]
        public AnimalType AnimalType { get; set; }

        [Required]
        public Breed Breed { get; set; }

        [Required]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }

    }
}
