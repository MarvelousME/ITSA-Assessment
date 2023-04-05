using VetClinic.DAL.Models;

namespace VetClinic.Api.ViewModels
{
    public class VisitViewModel
    {
        public int Id { get; set; }
        public int PetDetailId { get; set; }
        public PetDetail PetDetail { get; set; }
        public int VetId { get; set; }
        public DAL.Models.Vet Vet { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}