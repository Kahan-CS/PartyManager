using Microsoft.AspNetCore.Mvc;
using PartyManager.Data;
using PartyManager.Entities;
using PartyManager.Models;
using System.Net;
using System.Net.Mail;

namespace PartyManager.Controllers
{
    public class PartyManagerController : Controller
    {
        private readonly PartyDbContext _context;

        public PartyManagerController(PartyDbContext context)
        {
            _context = context;
        }

        // GET: /Party/List
        public IActionResult List()
        {
            // Retrieve all parties. Since Party already has an unmapped property for InvitationCount,
            // we don't need to include the invitations if only the count is displayed.
            var parties = _context.Parties.ToList();
            return View(parties);
        }

        // GET: /Party/Add
        [HttpGet]
        public IActionResult Add()
        {
            // Create a new view model with an empty Party and an empty invitations list.
            var partyViewModel = new PartyViewModel
            {
                Party = new Party(),
                Invitations = new List<Invitation>()
            };
            return View(partyViewModel);
        }

        // POST: /Party/Add
        [HttpPost]
        public IActionResult Add(PartyViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Add the new party to the database.
                _context.Parties.Add(model.Party);
                _context.SaveChanges();
                return RedirectToAction("List");
            }
            return View(model);
        }

        // GET: /Party/Edit/{id}
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var party = _context.Parties.Find(id);
            if (party == null)
                return NotFound();

            // For editing, you may choose to include invitations if needed.
            var partyViewModel = new PartyViewModel
            {
                Party = party,
                Invitations = _context.Invitations.Where(i => i.PartyId == id).ToList()
            };

            return View(partyViewModel);
        }

        // POST: /Party/Edit
        [HttpPost]
        public IActionResult Edit(PartyViewModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Parties.Update(model.Party);
                _context.SaveChanges();
                return RedirectToAction("List");
            }
            return View(model);
        }

        // GET: /Party/Modify/{id}
        // This page shows the party summary, a list of invitations, and a form to add a new invitation.
        [HttpGet]
        public IActionResult Modify(int id)
        {
            var party = _context.Parties.Find(id);
            if (party == null)
                return NotFound();

            // Load existing invitations for the party.
            var invitations = _context.Invitations.Where(i => i.PartyId == id).ToList();

            // Create a view model for modifying the party.
            var modifyViewModel = new PartyModifyViewModel
            {
                Party = party,
                Invitations = invitations,
                // Prepare a new invitation for data binding on the add invitation form.
                NewInvitation = new Invitation { PartyId = id }
            };

            return View(modifyViewModel);
        }

        // POST: /Party/AddInvitation
        // Handles adding a new invitation to the party.
        [HttpPost]
        public IActionResult AddInvitation(PartyModifyViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Ensure the new invitation is associated with the correct party.
                model.NewInvitation.PartyId = model.Party.PartyId;
                // The invitation status will default to InviteNotSent.
                _context.Invitations.Add(model.NewInvitation);
                _context.SaveChanges();
            }
            // Redirect back to the modify page for the same party.
            return RedirectToAction("Modify", new { id = model.Party.PartyId });
        }

        // POST: /Party/SendInvitations/{id}
        // Sends invitation emails to all guests whose invitations have not yet been sent.
        [HttpPost]
        public IActionResult SendInvitations(int id)
        {
            var party = _context.Parties.Find(id);
            if (party == null)
                return NotFound();

            // Retrieve all invitations for the party with status InviteNotSent.
            var unsentInvitations = _context.Invitations
                .Where(i => i.PartyId == id && i.Status == InvitationStatus.InviteNotSent)
                .ToList();

            // Email settings: these should be stored securely in configuration.
            string fromAddress = "TEST-GMAIL-ADDRESS";
            string password = "YOUR-GENERATED-PASSWORD";

            foreach (var invitation in unsentInvitations)
            {
                // Configure the SMTP client.
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromAddress, password),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    EnableSsl = true,
                };

                // Build the email message with variables replaced.
                var mailMessage = new MailMessage()
                {
                    From = new MailAddress(fromAddress),
                    Subject = $"You have been invited to {party.Description} Party!",
                    Body = $"<h1>Hello {invitation.GuestName}</h1>" +
                           $"<p>You have been invited to the {party.Description} party at {party.Location} on {party.EventDate:MMMM dd, yyyy}!</p>" +
                           $"<p>Please <a href=\"http://localhost:7269/Invitation/Respond?partyId={party.PartyId}&invitationId={invitation.InvitationId}\">let us know</a> if you can attend.</p>" +
                           $"<p>Sincerely,<br>Party Manager App</p>",
                    IsBodyHtml = true
                };

                mailMessage.To.Add(invitation.GuestEmail);

                try
                {
                    smtpClient.Send(mailMessage);
                    // Update the invitation status to InviteSent upon successful sending.
                    invitation.Status = InvitationStatus.InviteSent;
                }
                catch (Exception ex)
                {
                    // Log the exception or handle errors as needed.
                    // For example, you might use a logging framework here.
                }
            }

            // Save the status updates.
            _context.SaveChanges();

            return RedirectToAction("Modify", new { id = party.PartyId });
        }
    }
}
