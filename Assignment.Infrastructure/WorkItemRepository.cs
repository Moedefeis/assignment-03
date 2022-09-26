using Assignment.Core;

namespace Assignment.Infrastructure;

public class WorkItemRepository : IWorkItemRepository
{
    public (Response Response, int WorkItemId) Create(WorkItemCreateDTO workItem)
    {
        throw new NotImplementedException();
    }

    public Response Delete(int workItemId)
    {
        throw new NotImplementedException();
    }

    public WorkItemDetailsDTO Find(int workItemId)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<WorkItemDTO> Read()
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<WorkItemDTO> ReadByState(Core.State state)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<WorkItemDTO> ReadByTag(string tag)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<WorkItemDTO> ReadByUser(int userId)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<WorkItemDTO> ReadRemoved()
    {
        throw new NotImplementedException();
    }

    public Response Update(WorkItemUpdateDTO workItem)
    {
        throw new NotImplementedException();
    }
}
