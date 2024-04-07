using System.Net;
using Application.Posts.Commands.CreatePost;
using Application.Posts.Commands.DeletePost;
using Application.Posts.Commands.RestorePost;
using Application.Posts.Commands.UpdatePost;
using Application.Posts.Queries.DownloadPostFile;
using Application.Posts.Queries.GetPost;
using Application.Posts.Queries.GetPostGrid;
using Application.Posts.Queries.GetPostInformation;
using Gridify;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Models;
using CreatePostDto = Application.Posts.Commands.CreatePost.PostDto;
using UpdatePostDto = Application.Posts.Commands.UpdatePost.PostDto;
using GetPostVm = Application.Posts.Queries.GetPost.PostVm;
using GetPostInformationVm = Application.Posts.Queries.GetPostInformation.PostVm;
using GetPostGridVm = Application.Posts.Queries.GetPostGrid.PostVm;

namespace WebApi.Controllers;

/// <summary>
/// Контроллер для работы с сущностью пост
/// </summary>
public class PostController : ApiMediatorController
{
    /// <summary>
    /// Команда создания тега
    /// </summary>
    /// <param name="dto">Объект передачи данных сущности тега</param>
    /// <returns>Идентификатор созданной сущности</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseVm), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseVm), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponseVm), (int)HttpStatusCode.InternalServerError)]
    public async Task<Guid> Create([FromForm] CreatePostDto dto)
        => await Mediator.Send(new CreatePostCommand { Dto = dto });

    /// <summary>
    /// Команда обновления тега
    /// </summary>
    /// <param name="dto">Объект передачи данных сущности тега</param>
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseVm), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseVm), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponseVm), (int)HttpStatusCode.InternalServerError)]
    public async Task Update([FromForm] UpdatePostDto dto)
        => await Mediator.Send(new UpdatePostCommand { Dto = dto });
    
    /// <summary>
    /// Команда удаления тега
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    [HttpDelete]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseVm), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponseVm), (int)HttpStatusCode.InternalServerError)]
    public async Task Delete(Guid id)
        => await Mediator.Send(new DeletePostCommand { Id = id });

    /// <summary>
    /// Команда восстановления тега
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseVm), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseVm), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponseVm), (int)HttpStatusCode.InternalServerError)]
    public async Task Restore(Guid id)
        => await Mediator.Send(new RestorePostCommand { Id = id });

    /// <summary>
    /// Запрос получения поста
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <returns>Пост</returns>
    [HttpGet]
    [ProducesResponseType(typeof(GetPostVm), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseVm), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponseVm), (int)HttpStatusCode.InternalServerError)]
    public async Task<GetPostVm> Get(Guid id)
        => await Mediator.Send(new GetPostQuery { Id = id });

    /// <summary>
    /// Запрос получения информации о посте
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <returns>Пост</returns>
    [HttpGet]
    [ProducesResponseType(typeof(GetPostInformationVm), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseVm), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponseVm), (int)HttpStatusCode.InternalServerError)]
    public async Task<GetPostInformationVm> GetInformation(Guid id)
        => await Mediator.Send(new GetPostInformationQuery { Id = id });

    /// <summary>
    /// Запрос получения таблицы сущностей постов
    /// </summary>
    /// <param name="gridify">Модель</param>
    /// <returns>Пост</returns>
    [HttpGet]
    [ProducesResponseType(typeof(Paging<GetPostGridVm>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseVm), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponseVm), (int)HttpStatusCode.InternalServerError)]
    public async Task<Paging<GetPostGridVm>> GetGrid([FromQuery] PostGridifyQuery gridify)
        => await Mediator.Send(new GetPostGridQuery { Gridify = gridify });
    
    /// <summary>
    /// Команда скачивания файла поста
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <returns>Файл поста</returns>
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseVm), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponseVm), (int)HttpStatusCode.InternalServerError)]
    public async Task<FileResult> Preview(Guid id)
    {
        var fileDto = await Mediator.Send(new DownloadPostFileQuery { PostId = id });

        return File(fileDto.Data.ToArray(), fileDto.ContentType, enableRangeProcessing: true);
    }
    
    /// <summary>
    /// Команда скачивания файла поста
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <returns>Файл поста</returns>
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseVm), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponseVm), (int)HttpStatusCode.InternalServerError)]
    public async Task<FileResult> Download(Guid id)
    {
        var fileDto = await Mediator.Send(new DownloadPostFileQuery { PostId = id });

        return File(fileDto.Data.ToArray(), fileDto.ContentType, fileDto.FileName);
    }
}