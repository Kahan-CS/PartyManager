using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartyManager.Data;
using PartyManager.Models;

namespace PartyManager.Controllers
{
    public class InvitationController : Controller
    {
        private readonly PartyDbContext _context;

        public InvitationController(PartyDbContext context)
        {
            _context = context;
        }

        // GET: /Invitation/Respond?partyId=1&invitationId=123
        [HttpGet]
        public IActionResult Respond(int partyId, int invitationId)
        {
            // Load the invitation along with its associated party.
            var invitation = _context.Invitations
                .Include(i => i.Party)
                .FirstOrDefault(i => i.InvitationId == invitationId && i.PartyId == partyId);

            if (invitation == null || invitation.Party == null)
                return NotFound();

            // Populate the view model.
            var viewModel = new InvitationResponseViewModel
            {
                InvitationId = invitation.InvitationId,
                GuestName = invitation.GuestName,
                PartyDescription = invitation.Party.Description
            };

            return View(viewModel);
        }

        // POST: /Invitation/Respond
        [HttpPost]
        public IActionResult Respond(InvitationResponseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Retrieve the invitation record from the DB.
            var invitation = _context.Invitations.Find(model.InvitationId);
            if (invitation == null)
                return NotFound();

            // Update invitation status.
            invitation.Status = model.Status;
            _context.SaveChanges();

            // Redirect to a confirmation page.
            return RedirectToAction("ResponseConfirmation");
        }

        // GET: /Invitation/ResponseConfirmation
        public IActionResult ResponseConfirmation()
        {
            return View();
        }
    }
}
