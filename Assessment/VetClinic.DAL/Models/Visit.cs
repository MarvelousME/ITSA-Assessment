using System;
using VetClinic.DAL.Models.Interfaces;

namespace VetClinic.DAL.Models
{
    public class Visit : IAuditableEntity
    {
        public int Id { get; set; }
        public PetDetail PetDetail { get; set; }
        public Vet Vet { get; set; }
        public DateTime VisitDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
