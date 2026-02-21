using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesLab.Pages.AddressBook;

public class DeleteModel : PageModel
{
    private readonly IMediator _mediator;
    private readonly IRepo<AddressBookEntry> _repo;

    public DeleteModel(IRepo<AddressBookEntry> repo, IMediator mediator)
    {
        _repo = repo;
        _mediator = mediator;
    }

    [BindProperty]
    public DeleteAddressRequest DeleteAddressRequest { get; set; }

    public IActionResult OnGet(Guid id)
    {
        // Create specification that matches entry by ID
        var specification = new EntryByIdSpecification(id);

        // Use repo to find matching entries
        var entry = _repo.Find(specification).FirstOrDefault(); // Fetches existing record

        // Check for entry
        if (entry == null)
        {
            return RedirectToPage("Index"); // redirect
        }

        // Populate the request so Razor can pre-fill the form
        DeleteAddressRequest = new DeleteAddressRequest()
        {
            Id = entry.Id,
        };

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        await _mediator.Send(DeleteAddressRequest);
        return RedirectToPage("Index");
    }
}