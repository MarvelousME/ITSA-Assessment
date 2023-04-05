namespace VetClinic.Api.ViewModels
{
    public class VisitViewModel
    {
        public int Id { get; set; }
        public int PetId { get; set; }
        public int VetId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}