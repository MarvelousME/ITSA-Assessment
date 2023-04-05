using System;
using VetClinic.DAL.Models.Interfaces;

namespace VetClinic.DAL.Models
{
    public class Visit : IAuditableEntity
    {
        public int Id { get; set; }
        public int PetId { get; set; }
        public int VetId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
