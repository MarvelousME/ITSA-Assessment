using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Api.Helpers;
using VetClinic.Api.ViewModels;
using VetClinic.DAL.UnitOfWork.Interfaces;

namespace VetClinic.Api.Controllers
{
    [Route("api/[controller]")]
    public class VetController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IEmailSender _emailSender;

        public VetController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<VetController> logger, IEmailSender emailSender)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailSender = emailSender;
        }

        //[HttpGet("vets/{pageNumber:int}/{pageSize:int}")]
        ////[Authorize(Authorization.Policies.ViewAllUsersPolicy)]
        //[ProducesResponseType(200, Type = typeof(List<VetViewModel>))]
        //public async Task<IActionResult> GetVets(int pageNumber, int pageSize)
        //{
        //    var usersAndRoles = await _accountManager.GetUsersAndRolesAsync(pageNumber, pageSize);

        //    List<VetViewModel> usersVM = new List<VetViewModel>();

        //    foreach (var item in usersAndRoles)
        //    {
        //        var userVM = _mapper.Map<VetViewModel>(item.User);
        //        userVM.Roles = item.Roles;

        //        usersVM.Add(userVM);
        //    }

        //    return Ok(usersVM);
        //}
    }
}
