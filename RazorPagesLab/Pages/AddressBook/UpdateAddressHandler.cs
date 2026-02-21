using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace RazorPagesLab.Pages.AddressBook;

public class UpdateAddressHandler
    : IRequestHandler<UpdateAddressRequest>
{
    private readonly IRepo<AddressBookEntry> _repo;

    public UpdateAddressHandler(IRepo<AddressBookEntry> repo)
    {
        _repo = repo;
    }

    public Task<Unit> Handle(UpdateAddressRequest request, CancellationToken cancellationToken)
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

        // Update domain entry
        entry.Update(
            request.Line1,
            request.Line2,
            request.City,
            request.State,
            request.PostalCode
        );

        // Continue update
        _repo.Update(entry);

        // IRequest with no return val must return Unit.value
        return Task.FromResult(Unit.Value);
    }
}