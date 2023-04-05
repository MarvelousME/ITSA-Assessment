using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Api.Helpers;
using VetClinic.Api.ViewModels;
using VetClinic.DAL.Interfaces;
using VetClinic.DAL.UnitOfWork.Interfaces;

namespace VetClinic.Api.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class PetOwnerController : ControllerBase
    {
        private readonly IAccountManager _accountManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IEmailSender _emailSender;


        public PetOwnerController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<CustomerController> logger, IEmailSender emailSender)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailSender = emailSender;
        }

        // GET: api/values
        //[HttpGet]
        //public IActionResult Get()
        //{
        //    var allPetOwners = _unitOfWork.PetOwners.GetAll();
        //    return Ok(_mapper.Map<IEnumerable<PetOwnerViewModel>>(allPetOwners));
        //}

        [HttpGet("petowners")]
        //[Authorize(Authorization.Policies.ViewAllPetOwnersPolicy)]
        [ProducesResponseType(200, Type = typeof(List<PetOwnerViewModel>))]
        public async Task<IActionResult> GetPetOwners()
        {
            return await GetPetOwners(-1, -1);
        }


        [HttpGet("petowners/{pageNumber:int}/{pageSize:int}")]
        [Authorize(Authorization.Policies.ViewAllPetOwnersPolicy)]
        [ProducesResponseType(200, Type = typeof(List<PetOwnerViewModel>))]
        public async Task<IActionResult> GetPetOwners(int pageNumber, int pageSize)
        {
            var usersAndRoles = await _accountManager.GetUsersAndRolesAsync(pageNumber, pageSize);

            List<PetOwnerViewModel> petownersVM = new List<PetOwnerViewModel>();

            foreach (var item in usersAndRoles)
            {
                var petownerVM = _mapper.Map<PetOwnerViewModel>(item.User);
                //petownerVM.Roles = item.Roles;

                petownersVM.Add(petownerVM);
            }

            return Ok(petownersVM);
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
