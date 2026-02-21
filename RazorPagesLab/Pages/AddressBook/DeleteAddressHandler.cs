using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace RazorPagesLab.Pages.AddressBook;
public class DeleteAddressHandler
    : IRequestHandler<DeleteAddressRequest>
{
    private readonly IRepo<AddressBookEntry> _repo;

    public DeleteAddressHandler(IRepo<AddressBookEntry> repo)
    {
        _repo = repo;
    }

    public Task<Unit> Handle(DeleteAddressRequest request, CancellationToken cancellationToken)
    {
        // Create specification to find entry by Id
        var specification = new EntryByIdSpecification(request.Id);

        // Retrieve existing entry
        var entry = _repo.Find(specification).FirstOrDefault();

        // If entry does not exist, do nothing
        if (entry == null)
        {
            return Task.FromResult(Unit.Value);
        }

        // Delete/Remove entry
        _repo.Remove(entry);

        // IRequest with no return val must return Unit.value
        return Task.FromResult(Unit.Value);
    }
}