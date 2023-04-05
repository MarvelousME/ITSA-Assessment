using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Api.Helpers;
using VetClinic.Api.ViewModels;
using VetClinic.DAL.UnitOfWork.Interfaces;

namespace VetClinic.Api.Controllers
{
    [Route("api/[controller]")]
    public class VisitController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IEmailSender _emailSender;

        public VisitController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<VisitController> logger, IEmailSender emailSender)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailSender = emailSender;
        }

        //[HttpGet("visits/{pageNumber:int}/{pageSize:int}")]
        ////[Authorize(Authorization.Policies.ViewAllUsersPolicy)]
        //[ProducesResponseType(200, Type = typeof(List<VisitViewModel>))]
        //public async Task<IActionResult> GetVisits(int pageNumber, int pageSize)
        //{
        //    var usersAndRoles = await _accountManager.GetUsersAndRolesAsync(pageNumber, pageSize);

        //    List<VisitViewModel> usersVM = new List<VisitViewModel>();

        //    foreach (var item in usersAndRoles)
        //    {
        //        var userVM = _mapper.Map<VisitViewModel>(item.User);
        //        userVM.Roles = item.Roles;

        //        usersVM.Add(userVM);
        //    }

        //    return Ok(usersVM);
        //}
    }
}
