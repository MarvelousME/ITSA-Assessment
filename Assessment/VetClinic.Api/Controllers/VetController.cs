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
    [Authorize]
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
            if (ModelState.IsValid)
            {
                var result = await _vetManager.GetVetsAsync(pageNumber, pageSize);

                if (result.Item1 != null)
                {
                    List<VetViewModel> vetsVM = new List<VetViewModel>();

                    foreach (var item in result.Item1)
                    {
                        var vetVM = _mapper.Map<VetViewModel>(item);

                        vetsVM.Add(vetVM);
                    }

                    return Ok(vetsVM);
                }

                AddError(result.Errors);
            }

            return BadRequest(ModelState);
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
                if (result.vet != null)
                {
                    VetViewModel vetVM = _mapper.Map<VetViewModel>(result.vet);

                    return Ok(vetVM);
                }

                AddError(result.Errors);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("read")]
        [ProducesResponseType(201, Type = typeof(VetViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> Read(int Id)
        {
            if (ModelState.IsValid)
            {
                var exists = _vetManager.CheckIfRecordExists(Id).Result;
                if (!exists)
                        return BadRequest($"Vet does not exist");


                var result = await _vetManager.ReadVetAsync(Id);

                if (result.vet != null)
                {
                    var vetVM = _mapper.Map<VetViewModel>(result.vet);

                    return Ok(vetVM);
                }

                AddError(result.Errors);
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

                var record = await _vetManager.ReadVetAsync(vet.Id);

                if (record.vet != null)
                {
                    record.vet.Name = vet.Name;
                    record.vet.Surname = vet.Surname;
                    record.vet.UpdatedDate = DateTime.UtcNow;

                    var result = await _vetManager.UpdateVetAsync(record.vet);
                    if (result.Succeeded)
                    {
                        return Ok(vet);
                    }

                    AddError(result.Errors);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpPost("updateMedicalLicense")]
        [ProducesResponseType(201, Type = typeof(bool))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> UpdateMedicalLicense([FromBody] VetViewModel vet)
        {
            if (ModelState.IsValid)
            {
                var exists = _vetManager.CheckIfRecordExists(vet.Id).Result;
                if (!exists)
                    return BadRequest($"Vet does not exist");

                var record = await _vetManager.ReadVetAsync(vet.Id);

                if (record.vet != null)
                {
                    record.vet.MedicalLicense = vet.MedicalLicense;
                    record.vet.UpdatedDate = DateTime.UtcNow;

                    if (_vetManager.CheckIfMedicalLicenseExists(record.vet.MedicalLicense).Result)
                        return BadRequest($"Vet Medical License already exists!");

                    var result = await _vetManager.UpdateVetAsync(record.vet);
                    if (result.Succeeded)
                    {
                        return Ok(vet);
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
                var exists = _vetManager.CheckIfRecordExists(Id).Result;
                if (!exists)
                    return BadRequest($"Vet does not exist");

                var result = await _vetManager.DeleteVetAsync(Id, delete);

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
                var exists = _vetManager.CheckIfRecordExists(Id).Result;
                if (!exists)
                    return BadRequest($"Vet does not exist");

                var result = await _vetManager.IsActiveVetAsync(Id, Activate);

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
