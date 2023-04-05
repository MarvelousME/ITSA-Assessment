using System.ComponentModel.DataAnnotations;
using VetClinic.Core.Enums;

namespace VetClinic.Api.ViewModels
{
    public class PetDetailViewModel
    {
        public int Id { get; set; }

        //[Required]
        public string Name { get; set; } = String.Empty;

        //[Required]
        [Display(Name = "Animal Type")]
        public AnimalType AnimalType { get; set; } = AnimalType.Unknown;

        //[Required]
        public Breed Breed { get; set; } = Breed.Unknown;

    }
}
