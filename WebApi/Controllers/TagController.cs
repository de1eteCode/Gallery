using System.Net;
using Application.Tags.Commands.CreateTag;
using Application.Tags.Queries.GetTag;
using Application.Tags.Queries.GetTagList;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Models;
using GetTagVm = Application.Tags.Queries.GetTag.TagVm;
using GetTagVmList = Application.Tags.Queries.GetTagList.TagVm;
using CreateTagDto = Application.Tags.Commands.CreateTag.TagDto;

namespace WebApi.Controllers;

/// <summary>
/// Котроллер для работы с сущностью тега
/// </summary>
public class TagController : ApiMediatorController
{
    /// <summary>
    /// Запрос на получение тега
    /// </summary>
    /// <param name="id">Идентификатор тега</param>
    /// <returns>Сущность тега</returns>
    [HttpGet]
    [ProducesResponseType(typeof(GetTagVm), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseVm), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseVm), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponseVm), (int)HttpStatusCode.InternalServerError)]
    public async Task<GetTagVm> Get(Guid id)
        => await Mediator.Send(new GetTagQuery { Id = id });
    
    /// <summary>
    /// Запрос на получение списка тегов
    /// </summary>
    /// <returns>Список сущностей тега</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetTagVmList>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseVm), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseVm), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponseVm), (int)HttpStatusCode.InternalServerError)]
    public async Task<IEnumerable<GetTagVmList>> GetList()
        => await Mediator.Send(new GetTagListQuery());

    /// <summary>
    /// Команда создания тега
    /// </summary>
    /// <param name="dto">Объект передачи данных сущности тега</param>
    /// <returns>Идентификатор созданной сущности</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseVm), (int)HttpStatusCode.Conflict)]
    [ProducesResponseType(typeof(ErrorResponseVm), (int)HttpStatusCode.InternalServerError)]
    public async Task<Guid> Create([FromBody] CreateTagDto dto)
        => await Mediator.Send(new CreateTagCommand { Dto = dto });
}