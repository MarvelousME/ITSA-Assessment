using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq;
using VetClinic.Api.Controllers;
using VetClinic.DAL.Interfaces;
using VetClinic.DAL.Models;
using VetClinic.DAL.Repositories.Interfaces;
using VetClinic.Tests.MockData;

namespace VetClinic.Tests
{
    public class VetTests
    {
        private Mock<IVetManager> service;
        private Mock<VetController> controller;
        private Mock<ILogger<VetController>> _logger;

        public VetTests()
        {
            service = new Mock<IVetManager>();
            controller = new Mock<VetController>();
            _logger = new Mock<ILogger<VetController>>();
        }

        [Fact]
        public void GetAllVets_VetsInVetRepo()
        {
            //Arrange
            service = new Mock<IVetManager>();
            var vets = VetMockData.GetSampleVetList();
            //var firstEmployee = vets[0];
            //controller.
            //service.Setup(x => x.GetVetsAsync(1,10))
                //.ReturnsAsync();

            //Act
            //controller.Setup()

            //Assert
        }

        [Fact]
        public void Create_Vet()
        {

        }

        [Theory]
        [InlineData(1)]
        public void Read_Vet_By_Id(int Id)
        {
           Vet vet = VetMockData.GetSampleVetList().FirstOrDefault(v => v.Id == Id);
        }

        [Theory]
        [InlineData()]
        public void Update_Vet_Details()
        {

        }

        [Theory]
        [InlineData(false)]
        public void Make_Vet_Active_or_Disabled(bool activate)
        {

        }

        [Theory]
        [InlineData(false)]
        public void Soft_Delete_Vet_(bool activate)
        {

        }

        [Fact]
        public void Get_Vet_BadRequest()
        {
            // Arrange
            //int id = 0;
            //var mockRepo = new Mock<IVetManager>();
            //mockRepo.Setup(repo => repo..Returns<int>((a) => Single(a));
            //var controller = new VetController(mockRepo.Object);

            //// Act
            //var result = controller.Get(id);

            //// Assert
            //var actionResult = Assert.IsType<ActionResult<Reservation>>(result);
            //Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }
        
    }
}