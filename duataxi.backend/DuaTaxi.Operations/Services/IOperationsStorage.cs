using System;
using System.Threading.Tasks;
using DuaTaxi.Operations.Dto;

namespace DuaTaxi.Services.Operations.Services
{
    public interface IOperationsStorage
    {
        Task<OperationDto> GetAsync(string id);

        Task SetAsync(string id, Guid userId, string name,  OperationState state, 
            string resource, string code = null, string reason = null);
    }
}