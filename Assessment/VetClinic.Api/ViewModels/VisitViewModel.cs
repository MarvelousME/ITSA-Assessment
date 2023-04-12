using VetClinic.DAL.Models;

namespace VetClinic.Api.ViewModels
{
    public class VisitViewModel
    {
        public int Id { get; set; }
        public PetDetail PetDetail { get; set; }
        public Vet Vet { get; set; }
        public DateTime CreatedDate { get; set; }
        public string SummayOfVisit { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}