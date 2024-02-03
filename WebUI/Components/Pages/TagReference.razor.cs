using Application.Tags.Queries.GetTagList;
using Blazorise.DataGrid;
using Mediator;
using Microsoft.AspNetCore.Components;

namespace WebUI.Components.Pages;

public partial class TagReference
{
    [Inject]
    public IMediator Mediator { get; init; }
    
    private IEnumerable<TagVm> _tagsEnumerable;
    private int _totalTags;
    
    protected override async Task OnInitializedAsync()
    {
        _tagsEnumerable = await Mediator.Send(new GetTagListQuery());
        
        await base.OnInitializedAsync();
    }

    private TagVm CreateTag()
    {
        throw new NotImplementedException();
    }

    private TagVm UpdateTag(TagVm arg)
    {
        throw new NotImplementedException();
    }

    private Task DeleteTag(TagVm arg)
    {
        throw new NotImplementedException();
    }
}