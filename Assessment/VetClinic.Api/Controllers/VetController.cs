using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Api.Helpers;
using VetClinic.Api.ViewModels;
using VetClinic.DAl.Managers;
using VetClinic.DAL.Interfaces;
using VetClinic.DAL.Models;
using VetClinic.DAL.UnitOfWork.Interfaces;

namespace VetClinic.Api.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class VetController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IVetManager _vetManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IEmailSender _emailSender;

        public VetController(IMapper mapper, IVetManager vetManager, IUnitOfWork unitOfWork, ILogger<VetController> logger, IEmailSender emailSender)
        {
            _mapper = mapper;
            _vetManager = vetManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailSender = emailSender;
        }

        [HttpGet("vets/{pageNumber:int}/{pageSize:int}")]
        [ProducesResponseType(200, Type = typeof(List<VetViewModel>))]
        public async Task<IActionResult> GetVets(int pageNumber = 1, int pageSize = 10)
        {
            var vets = await _vetManager.GetVetsAsync(pageNumber, pageSize);

            List<VetViewModel> vetsVM = new List<VetViewModel>();

            foreach (var item in vets)
            {
                var vetVM = _mapper.Map<VetViewModel>(item);

                vetsVM.Add(vetVM);
            }

            return Ok(vetsVM);
        }

        [HttpPost("create")]
        [ProducesResponseType(201, Type = typeof(VetViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> Create([FromBody] VetViewModel vet)
        {
            if (ModelState.IsValid)
            {
                if (vet == null)
                    return BadRequest($"{nameof(vet)} cannot be null");


                Vet appVet = _mapper.Map<Vet>(vet);

                var result = await _vetManager.CreateVetAsync(appVet);
                if (result.Succeeded)
                {
                    VetViewModel vetVM = _mapper.Map<VetViewModel>(appVet);

                    return (IActionResult)vetVM;
                }

                AddError(result.Errors);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("read")]
        [ProducesResponseType(201, Type = typeof(VetViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> Edit(int Id)
        {
            if (ModelState.IsValid)
            {
                var exists = _vetManager.CheckIfRecordExists(Id).Result;
                if (!exists)
                        return BadRequest($"Vet does not exist");


                var vet = await _vetManager.ReadVetsAsync(Id);

                var vetVM = _mapper.Map<VetViewModel>(vet);

                return (IActionResult) vetVM;
                
            }

            return BadRequest(ModelState);
        }

        [HttpPost("update")]
        [ProducesResponseType(201, Type = typeof(VetViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> Update([FromBody] VetViewModel vet)
        {
            if (ModelState.IsValid)
            {
                var exists = _vetManager.CheckIfRecordExists(vet.Id).Result;
                if (!exists)
                    return BadRequest($"Vet does not exist");

                var vetRecord = _mapper.Map<Vet>(vet);

                var result = await _vetManager.UpdateVetAsync(vetRecord);

                if (result.Succeeded)
                {
                    return (IActionResult)vet;
                }

                AddError(result.Errors);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("delete")]
        [ProducesResponseType(201, Type = typeof(VetViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> Delete(int Id)
        {
            if (ModelState.IsValid)
            {
                var exists = _vetManager.CheckIfRecordExists(Id).Result;
                if (!exists)
                    return BadRequest($"Vet does not exist");

                var vet = await _vetManager.ReadVetsAsync(Id);
                var vetRecord = _mapper.Map<Vet>(vet);

                vetRecord.IsDeleted = true;

                var result = await _vetManager.UpdateVetAsync(vetRecord);

                if (result.Succeeded)
                {
                    return (IActionResult)vetRecord;
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
