using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Api.Helpers;
using VetClinic.Api.ViewModels;
using VetClinic.DAL.Interfaces;
using VetClinic.DAL.Managers;
using VetClinic.DAL.Models;
using VetClinic.DAL.UnitOfWork.Interfaces;


namespace VisitClinic.Api.Controllers
{
    [Route("api/[controller]")]
    public class VisitController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IVisitManager _visitManager;
        private readonly IPetDetailManager _petDetailManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IEmailSender _emailSender;

        public VisitController(IMapper mapper, IVisitManager visitManager,IPetDetailManager petDetailManager ,IUnitOfWork unitOfWork, ILogger<VisitController> logger, IEmailSender emailSender)
        {
            _mapper = mapper;
            _visitManager = visitManager;
            _petDetailManager = petDetailManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailSender = emailSender;
        }

        //[HttpGet("visits/{pageNumber:int}/{pageSize:int}")]
        //[ProducesResponseType(200, Type = typeof(List<VisitViewModel>))]
        //public async Task<IActionResult> GetVisits(int pageNumber = 1, int pageSize = 10)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = await _visitManager.GetVisitsAsync(pageNumber, pageSize);

        //        if (result.Item1 != null)
        //        {
        //            List<VisitViewModel> VisitsVM = new List<VisitViewModel>();

        //            foreach (var item in result.Item1)
        //            {
        //                var VisitVM = _mapper.Map<VisitViewModel>(item);

        //                VisitsVM.Add(VisitVM);
        //            }

        //            return Ok(VisitsVM);
        //        }

        //        AddError(result.Errors);
        //    }

        //    return BadRequest(ModelState);
        //}

        [HttpPost("create")]
        public async Task<IActionResult> CreateVisitAsync([FromBody]Visit visit)
        {
            if (ModelState.IsValid)
            {
                if (visit.Vet.Id == 0 || visit.PetDetail.Id == 0)
                    return BadRequest($"Please make sure Vet with Id '{visit.Vet.Id}' and Pet with Id '{visit.PetDetail.Id}' exist");

                var result = await _visitManager.CreateVisitAsync(visit);
                if (result.visit != null)
                {
                    var msg = $"The Vet has a visit booked for '{visit.PetDetail.Name}' on date '{visit.VisitDate}' with the owner '{visit.PetDetail.Owner}' ";

                    return Ok(msg);
                }

                AddError(result.Errors);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("read")]
        [ProducesResponseType(201, Type = typeof(VisitViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> Read(int Id)
        {
            if (ModelState.IsValid)
            {
                var exists = _visitManager.CheckIfRecordExists(Id).Result;
                if (!exists)
                    return BadRequest($"Visit does not exist");


                var result = await _visitManager.ReadVisitAsync(Id);

                if (result.visit != null)
                {
                    var VisitVM = _mapper.Map<VisitViewModel>(result.visit);

                    return Ok(VisitVM);
                }

                AddError(result.Errors);
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
                var exists = _visitManager.CheckIfRecordExists(Id).Result;
                if (!exists)
                    return BadRequest($"Visit does not exist");

                var result = await _visitManager.DeleteVisitAsync(Id, delete);

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
                var exists = _visitManager.CheckIfRecordExists(Id).Result;
                if (!exists)
                    return BadRequest($"Visit does not exist");

                var result = await _visitManager.IsActiveVisitAsync(Id, Activate);

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
