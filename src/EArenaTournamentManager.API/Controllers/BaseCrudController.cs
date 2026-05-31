using EArenaTournamentManager.Application.Common.Extensions;
using EArenaTournamentManager.Application.Common.Results;
using EArenaTournamentManager.Application.RequestDTOs.Shared;
using EArenaTournamentManager.Application.ResponseDTOs.Shared;
using EArenaTournamentManager.Application.Services.Interfaces.Base;
using EArenaTournamentManager.Domain.Common;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace EArenaTournamentManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseCrudController<E, IEService, ERequest, EGetRequest, EResponse, EGetResponse> : ControllerBase
        where E : BaseEntity, new()
        where IEService : IBaseService<E>
        where ERequest : class, new()
        where EGetRequest : BaseGetRequest, new()
        where EResponse : class, new()
        where EGetResponse : BaseGetResponse<EResponse>, new()
    {
        protected readonly IEService _entityService;

        public BaseCrudController(IEService entityService)
        {
            _entityService = entityService;
        }

        protected virtual void PopulateEntity(E entity, ERequest model) { }
        protected virtual Expression<Func<E, bool>>? GetFilter(EGetRequest model) => null;
        protected virtual void PopulateGetResponse(EGetRequest request, EGetResponse response) { }
        protected virtual EResponse ToResponse(E entity) => new EResponse();

        [HttpGet]
        public virtual async Task<IActionResult> Get([FromQuery] EGetRequest model)
        {
            model.Pager = model.Pager ?? new PagerRequest();
            model.Pager.Page = model.Pager.Page <= 0
                                    ? 1
                                    : model.Pager.Page;
            model.Pager.PageSize = model.Pager.PageSize <= 0
                                        ? 10
                                        : model.Pager.PageSize;

            model.OrderBy ??= nameof(BaseEntity.Id);
            model.OrderBy = typeof(E).GetProperty(model.OrderBy) != null
                                ? model.OrderBy
                                : nameof(BaseEntity.Id);

            Expression<Func<E, bool>> filter = GetFilter(model)!;

            EGetResponse response = new EGetResponse();

            response.Pager = new PagerResponse();
            response.Pager.Page = model.Pager.Page;
            response.Pager.PageSize = model.Pager.PageSize;

            response.OrderBy = model.OrderBy;
            response.SortAsc = model.SortAsc;

            PopulateGetResponse(model, response);

            var countResult = _entityService.Count(filter);

            if (!countResult.IsSuccess)
            {
                return BadRequest(countResult);
            }

            response.Pager.Count = countResult.Data;

            var itemsResult = await _entityService.GetAllAsync(
                filter,
                model.OrderBy,
                model.SortAsc,
                model.Pager.Page,
                model.Pager.PageSize);

            if (!itemsResult.IsSuccess)
            {
                return BadRequest(itemsResult);
            }

            List<EResponse> responseItems = itemsResult.Data!
                .Select(e => ToResponse(e))
                .Where(dto => dto is not null)
                .ToList();

            response.Items = responseItems;

            return Ok(ServiceResult<EGetResponse>.Success(response));
        }

        [HttpGet("{id}")]       
        public virtual async Task<IActionResult> Get([FromRoute] int id)
        {
            var result = await _entityService.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            if (result.Data is null)
            {
                return NotFound(ServiceResultExtensions.Failure<EResponse>(
                    null,
                    "Global",
                    $"{typeof(E).Name} not found."
                ));
            }

            return Ok(ServiceResult<EResponse>.Success(ToResponse(result.Data)));
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] ERequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ServiceResultExtensions.Failure<EResponse>(null, ModelState));
            }

            E newEntity = new E();
            PopulateEntity(newEntity, model);            

            var result = await _entityService.SaveAsync(newEntity);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(ServiceResult<EResponse>.Success(ToResponse(result.Data!)));
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Put([FromRoute] int id, [FromBody] ERequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ServiceResultExtensions.Failure<EResponse>(null, ModelState));
            }

            var entityForUpdate = await _entityService.GetByIdAsync(id);

            if (entityForUpdate.Data is null)
            {
                return NotFound(ServiceResultExtensions.Failure<EResponse>(
                   null,
                   "Global",
                   $"{typeof(E).Name} not found."
               ));
            }

            PopulateEntity(entityForUpdate.Data!, model);            

            var result = await _entityService.SaveAsync(entityForUpdate.Data);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(ServiceResult<EResponse>.Success(ToResponse(result.Data!)));
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete([FromRoute] int id)
        {
            var entityForDelete = await _entityService.GetByIdAsync(id);

            if (entityForDelete.Data is null)
            {
                return NotFound(ServiceResultExtensions.Failure<EResponse>(
                   null,
                   "Global",
                   $"{typeof(E).Name} not found."
               ));
            }

            var result = await _entityService.DeleteAsync(entityForDelete.Data);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(ServiceResult<EResponse>.Success(ToResponse(result.Data!)));
        }
    }
}
