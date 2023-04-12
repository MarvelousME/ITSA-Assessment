using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Api.Helpers;
using VetClinic.Api.ViewModels;
using VetClinic.DAL.Interfaces;
using VetClinic.DAL.UnitOfWork.Interfaces;

namespace VetClinic.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PetDetailController : ControllerBase
    {
        private readonly IPetDetailManager _petDetailManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IEmailSender _emailSender;

        public PetDetailController(IMapper mapper, IPetDetailManager petDetailManager, IUnitOfWork unitOfWork, ILogger<PetDetailController> logger, IEmailSender emailSender)
        {
            _petDetailManager = petDetailManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailSender = emailSender;
        }

        [HttpGet("petdetails/{pageNumber:int}/{pageSize:int}")]
        [ProducesResponseType(200, Type = typeof(List<PetDetailViewModel>))]
        public async Task<IActionResult> GetPetDetails(int pageNumber, int pageSize)
        {
            var petDetails = await _petDetailManager.GetPetDetailsAsync(pageNumber, pageSize);

            List<PetDetailViewModel> petDetailsVM = new List<PetDetailViewModel>();

            foreach (var item in petDetails.Item1)
            {
                var userVM = _mapper.Map<PetDetailViewModel>(item);

                petDetailsVM.Add(userVM);
            }

            return Ok(petDetailsVM);
        }
    }
}
