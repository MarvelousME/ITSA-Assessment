using System;
using System.ComponentModel.DataAnnotations;
using VetClinic.Core.Enums;
using VetClinic.DAL.Models.Interfaces;

namespace VetClinic.DAL.Models
{
    public class PetDetail : IAuditableEntity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Animal Type")]
        public AnimalType AnimalType { get; set; }

        //public int AnimalTypeId { get; set; }

        [Required]
        public Breed Breed { get; set; }

        public int BreedId { get; set; }

        //[Required]
        public string Owner { get; set; }

        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        //One to Many relationshp, 1 Owner can have many Pets
        //public int PetOwnerId { get; set; }
        public PetOwner PetOwner { get; set; }

    }
}
