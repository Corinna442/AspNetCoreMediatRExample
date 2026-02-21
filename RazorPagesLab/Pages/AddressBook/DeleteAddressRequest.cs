using System;
using MediatR;

namespace RazorPagesLab.Pages.AddressBook;

public class DeleteAddressRequest
    : IRequest
{
    // The unique identifier of the address book entry to delete.
    public Guid Id { get; set; }
}