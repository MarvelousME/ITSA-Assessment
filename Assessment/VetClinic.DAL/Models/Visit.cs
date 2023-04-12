using System;
using VetClinic.DAL.Models.Interfaces;

namespace VetClinic.DAL.Models
{
    public class Visit : IAuditableEntity
    {
        public int Id { get; set; }
        public PetDetail? PetDetail { get; set; }
        public Vet? Vet { get; set; }
        public DateTime VisitDate { get; set; }
        public string SummayOfVisit { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
