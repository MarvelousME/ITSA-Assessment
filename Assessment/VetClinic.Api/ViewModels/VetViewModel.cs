﻿namespace VetClinic.Api.ViewModels
{
    public class VetViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string MedicalLicense { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}