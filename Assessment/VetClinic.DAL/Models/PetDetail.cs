﻿using System;
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

        [Required]
        public Breed Breed { get; set; }

        public string Owner { get; set; }

        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        //One to Many relationshp, 1 Owner can have many Pets
        public PetOwner PetOwner { get; set; }

    }
}
