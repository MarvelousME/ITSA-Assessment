using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Api.Helpers;
using VetClinic.DAL.UnitOfWork.Interfaces;

namespace VetClinic.Api.Controllers
{
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IEmailSender _emailSender;
    }
}
