using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace VetClinic.Api.ViewModels
{
    public class VetViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Medical License")]
        public string MedicalLicense { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}