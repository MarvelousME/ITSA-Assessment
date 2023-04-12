using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Api.Helpers;
using VetClinic.Api.ViewModels;
using VetClinic.DAL.Interfaces;
using VetClinic.DAL.Managers;
using VetClinic.DAL.Models;
using VetClinic.DAL.UnitOfWork.Interfaces;

namespace VetClinic.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PetOwnerController : ControllerBase
    {
        private readonly IPetOwnerManager _petOwnerManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IEmailSender _emailSender;


        public PetOwnerController(IMapper mapper, IPetOwnerManager petownerManager, IUnitOfWork unitOfWork, ILogger<PetOwnerController> logger, IEmailSender emailSender)
        {
            _mapper = mapper;
            _petOwnerManager = petownerManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailSender = emailSender;
        }

        [HttpGet("petowners/{pageNumber:int}/{pageSize:int}")]
        [ProducesResponseType(200, Type = typeof(List<PetOwnerViewModel>))]
        public async Task<IActionResult> GetPetOwners(int pageNumber = 1, int pageSize = 10)
        {
            var petownersAndPetDetails = await _petOwnerManager.GetPetOwnerAndPetDetailsAsync(pageNumber, pageSize);

            List<PetOwnerViewModel> petownersVM = new List<PetOwnerViewModel>();

            foreach (var item in petownersAndPetDetails)
            {
                var petownerVM = _mapper.Map<PetOwnerViewModel>(item.petOwner);
                petownerVM.PetDetailIds = item.PetDetailIds;

                petownersVM.Add(petownerVM);
            }

            return Ok(petownersVM);
        }
        [HttpPost("create")]
        [ProducesResponseType(201, Type = typeof(PetOwnerViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> Create([FromBody] PetOwnerViewModel petOwner)
        {
            if (ModelState.IsValid)
            {
                if (petOwner == null)
                    return BadRequest($"{nameof(petOwner)} cannot be null");


                PetOwner appPetOwner = _mapper.Map<PetOwner>(petOwner);

                var result = await _petOwnerManager.CreatePetOwnerAsync(appPetOwner);
                if (result.petOwner != null)
                {
                    PetOwnerViewModel petOwnerVM = _mapper.Map<PetOwnerViewModel>(result.petOwner);

                    return Ok(petOwnerVM);
                }

                AddError(result.Errors);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("read")]
        [ProducesResponseType(201, Type = typeof(PetOwnerViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> Read(int Id)
        {
            if (ModelState.IsValid)
            {
                var exists = _petOwnerManager.CheckIfRecordExists(Id).Result;
                if (!exists)
                    return BadRequest($"Pet Owner does not exist");


                var result = await _petOwnerManager.ReadPetOwnerAsync(Id);

                if (result.petOwner != null)
                {
                    var petOwnerVM = _mapper.Map<PetOwnerViewModel>(result.petOwner);

                    return Ok(petOwnerVM);
                }

                AddError(result.Errors);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("update")]
        [ProducesResponseType(201, Type = typeof(PetOwnerViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> Update([FromBody] PetOwnerViewModel petOwner)
        {
            if (ModelState.IsValid)
            {
                var exists = _petOwnerManager.CheckIfRecordExists(petOwner.Id).Result;
                if (!exists)
                    return BadRequest($"Pet Owner does not exist");

                var record = await _petOwnerManager.ReadPetOwnerAsync(petOwner.Id);

                if (record.petOwner != null)
                {
                    record.petOwner.Name = petOwner.Name;
                    record.petOwner.Surname = petOwner.Surname;
                    record.petOwner.UpdatedDate = DateTime.UtcNow;

                    var result = await _petOwnerManager.UpdatePetOwnerAsync(record.petOwner);
                    if (result.Succeeded)
                    {
                        return Ok(petOwner);
                    }

                    AddError(result.Errors);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpPost("delete/{Id:int}/{delete:bool}")]
        [ProducesResponseType(201, Type = typeof(bool))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> Delete(int Id, bool delete)
        {
            if (ModelState.IsValid)
            {
                var exists = _petOwnerManager.CheckIfRecordExists(Id).Result;
                if (!exists)
                    return BadRequest($"Pet Owner does not exist");

                var result = await _petOwnerManager.DeletePetOwnerAsync(Id, delete);

                if (result.Succeeded)
                {
                    return Ok(result.Succeeded);
                }

                AddError(result.Errors);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("isActive/{Id:int}/{Activate:bool}")]
        [ProducesResponseType(201, Type = typeof(bool))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> IsActive(int Id, bool Activate)
        {
            if (ModelState.IsValid)
            {
                var exists = _petOwnerManager.CheckIfRecordExists(Id).Result;
                if (!exists)
                    return BadRequest($"Vet does not exist");

                var result = await _petOwnerManager.IsActivePetOwnerAsync(Id, Activate);

                if (result.Succeeded)
                {
                    return Ok(result.Succeeded);
                }

                AddError(result.Errors);
            }

            return BadRequest(ModelState);
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
