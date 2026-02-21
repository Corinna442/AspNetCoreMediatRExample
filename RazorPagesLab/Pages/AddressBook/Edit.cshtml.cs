using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesLab.Pages.AddressBook;

public class EditModel : PageModel
{
	private readonly IMediator _mediator;
	private readonly IRepo<AddressBookEntry> _repo;

	public EditModel(IRepo<AddressBookEntry> repo, IMediator mediator)
	{
		_repo = repo;
		_mediator = mediator;
	}

	[BindProperty]
	public UpdateAddressRequest UpdateAddressRequest { get; set; }

	public IActionResult OnGet(Guid id)
	{
		// Create specification that matches entry by ID
		var specification = new EntryByIdSpecification(id);

		// Use repo to find matching entries
		var entry = _repo.Find(specification).FirstOrDefault(); // Fetches existing record

		// Check for entry
		if (entry == null) {
			return RedirectToPage("Index"); // redirect
		}

		// Populate the request so Razor can pre-fill the form
		UpdateAddressRequest = new UpdateAddressRequest()
		{
			Id = entry.Id,
			Line1 = entry.Line1,
			Line2 = entry.Line2,
			City = entry.City,
			State = entry.State,
			PostalCode = entry.PostalCode
		};

		return Page();
	}

	public async Task<IActionResult> OnPost()
	{
	
		if (ModelState.IsValid)
		{
            // Send the update request through MediatR.
            // This decouples the Razor Page from update logic
            // The correct handler will recieve the request and use the repo
            // to perform the update.
            _ = await _mediator.Send(UpdateAddressRequest);

            // After a successful update, redirect back to
            // the address book list page.
            return RedirectToPage("Index");
		}

		// If any validation fails, redisplay the page and show validation messages.
		return Page();
	}
}