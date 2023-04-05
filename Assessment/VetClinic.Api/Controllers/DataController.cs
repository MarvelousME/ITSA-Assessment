using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Api.Helpers;
using VetClinic.DAL.UnitOfWork.Interfaces;

namespace VetClinic.Api.Controllers
{
    /// <summary>
    /// TODO: CRUD methods for static data i.e AnimalType, Breed
    /// </summary>
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IEmailSender _emailSender;

        public DataController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<PetDetailController> logger, IEmailSender emailSender)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailSender = emailSender;
        }
    }
}
