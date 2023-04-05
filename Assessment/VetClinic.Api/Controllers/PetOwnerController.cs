using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Api.Helpers;
using VetClinic.Api.ViewModels;
using VetClinic.DAL.Interfaces;
using VetClinic.DAL.Managers;
using VetClinic.DAL.UnitOfWork.Interfaces;

namespace VetClinic.Api.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class PetOwnerController : ControllerBase
    {
        private readonly IPetOwnerManager _petownerManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IEmailSender _emailSender;


        public PetOwnerController(IMapper mapper, IPetOwnerManager petownerManager, IUnitOfWork unitOfWork, ILogger<PetOwnerController> logger, IEmailSender emailSender)
        {
            _mapper = mapper;
            _petownerManager = petownerManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailSender = emailSender;
        }

        [HttpGet("petowners/{pageNumber:int}/{pageSize:int}")]
        //[Authorize(Authorization.Policies.ViewAllPetOwnersPolicy)]
        [ProducesResponseType(200, Type = typeof(List<PetOwnerViewModel>))]
        public async Task<IActionResult> GetPetOwners(int pageNumber = 0, int pageSize = 10)
        {
            var petownersAndPetDetails = await _petownerManager.GetPetOwnersAndPetDetailsAsync(pageNumber, pageSize);

            List<PetOwnerViewModel> petownersVM = new List<PetOwnerViewModel>();

            foreach (var item in petownersAndPetDetails)
            {
                var petownerVM = _mapper.Map<PetOwnerViewModel>(item.petOwner);
                petownerVM.PetDetailIds = item.PetDetailIds;

                petownersVM.Add(petownerVM);
            }

            return Ok(petownersVM);
        }

        private async Task<PetOwnerViewModel> GetPetOwnerViewModelHelper(int Id)
        {
            var petownerAndPetDetails = await _petownerManager.GetPetOwnerAndPetDetailsAsync(Id);
            if (petownerAndPetDetails == null)
                return null;

            var petOwnerVM = _mapper.Map<PetOwnerViewModel>(petownerAndPetDetails.Value.petOwner);
            petOwnerVM.PetDetailIds = petownerAndPetDetails.Value.PetDetailIds;

            return petOwnerVM;
        }
        private void AddError(IEnumerable<string> errors, string key = "")
        {
            foreach (var error in errors)
            {
                AddError(error, key);
            }
        }

        private void AddError(string error, string key = "")
        {
            ModelState.AddModelError(key, error);
        }

    }
}
