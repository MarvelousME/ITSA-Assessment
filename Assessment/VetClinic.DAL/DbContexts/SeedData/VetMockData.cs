﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.DAL.Models;

namespace VetClinic.DAL.SeedData
{
    public class VetMockData
    {
        public static IEnumerable<Vet> GetSampleVetList()
        {
            List<Vet> vets = new List<Vet>();

            Vet vet = new Vet()
            {
                Name = "Marvin",
                Surname = "Saunders",
                MedicalLicense = "ML00001",
                IsActive = true,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                CreatedBy = "Tester",
                UpdatedBy = "Tester"
            };
            vets.Add(vet);

            vet = new Vet()
            {
                Name = "Niquole",
                Surname = "Pieters",
                MedicalLicense = "ML00002",
                IsActive = true,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                CreatedBy = "Tester2",
                UpdatedBy = "Tester2"
            };
            vets.Add(vet);

            vet = new Vet()
            {
                Name = "Bradley",
                Surname = "Wade",
                MedicalLicense = "ML00003",
                IsActive = true,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                CreatedBy = "Tester",
                UpdatedBy = "Tester"
            };
            vets.Add(vet);

            return vets;
        }
    }
}
