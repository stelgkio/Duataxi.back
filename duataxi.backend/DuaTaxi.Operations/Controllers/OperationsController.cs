using System;
using System.Threading.Tasks;
using DuaTaxi.Common.Dispatchers;
using DuaTaxi.Operations.Dto;
using DuaTaxi.Services.Operations.Services;
using Microsoft.AspNetCore.Mvc;

namespace DuaTaxi.Services.Operations.Controllers
{
    [Route("[controller]")]
    public class OperationsController : BaseController
    {
        private readonly IOperationsStorage _operationsStorage;

        public OperationsController(IDispatcher dispatcher,
            IOperationsStorage operationsStorage) : base(dispatcher)
        {
            _operationsStorage = operationsStorage;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<OperationDto>> Get(Guid id)
            => Single(await _operationsStorage.GetAsync(id.ToString()));
    }
}