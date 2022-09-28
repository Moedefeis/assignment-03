using Assignment.Core;

namespace Assignment.Infrastructure;

public class TagRepository : ITagRepository
{
    private readonly KanbanContext _context;

    public TagRepository(KanbanContext context)
    {
        this._context = context;
    }

    public (Response Response, int TagId) Create(TagCreateDTO tagDTO)
    {
        var entity = _context.Tags.FirstOrDefault(tag => tag.Name == tagDTO.Name);
        Response response;

        if(entity == null)
        {
            entity = new Tag();
            entity.Name = tagDTO.Name;

            _context.Tags.Add(entity);
            _context.SaveChanges();

            response = Response.Created;
        }
        else
        {
            response = Response.Conflict;
        }


        return (response, entity.Id);

    }

    public Response Delete(int tagId, bool force = false)
    {
        throw new NotImplementedException();
    }

    public TagDTO Find(int tagId)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<TagDTO> Read()
    {
        throw new NotImplementedException();
    }

    public Response Update(TagUpdateDTO tag)
    {
        throw new NotImplementedException();
    }
}
