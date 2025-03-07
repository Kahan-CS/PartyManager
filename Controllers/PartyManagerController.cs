using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartyManager.Data;
using PartyManager.Entities;
using PartyManager.Models;
using System.Net;
using System.Net.Mail;

namespace PartyManager.Controllers
{
    /// <summary>
    /// Controller for managing party actions of list, edit, manage, and invitation actions of add & send invitations.
    /// </summary>
    public class PartyManagerController : Controller
    {
        private readonly PartyDbContext _context;

        public PartyManagerController(PartyDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This method is used to list all the parties in the database.
        /// </summary>
        /// <returns> 
        /// It returns the list of parties, including their invitations.
        /// </returns>
        // GET: /PartyManager/List
        public IActionResult List()
        {
            // Retrieve all parties. Since Party already has an unmapped property for InvitationCount,
            // we don't need to include the invitations if only the count is displayed.
            var parties = _context.Parties.Include(p => p.Invitations).ToList();
            return View(parties);
        }

        /// <summary>
        /// This method is used to display the form for adding a new party.
        /// </summary>
        /// <returns> 
        /// It returns the view for adding a new party.
        /// </returns>
        // GET: /PartyManager/Add
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


        /// <summary>
        /// This method is used to handle the form submission for adding a new party.
        /// </summary>
        /// <param name="model">The view model containing the party and invitations data.</param>
        /// <returns> 
        /// It returns a redirect to the list of parties if successful, otherwise it returns the same view with validation errors.
        /// </returns>
        // POST: /PartyManager/Add
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


        /// <summary>
        /// This method is used to display the form for editing an existing party.
        /// </summary>
        /// <param name="id">The ID of the party to edit.</param>
        /// <returns> 
        /// It returns the view for editing the party.
        /// </returns>
        // GET: /PartyManager/Edit/{id}
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


        /// <summary>
        /// This method is used to handle the form submission for editing an existing party.
        /// </summary>
        /// <param name="model">The view model containing the party and invitations data.</param>
        /// <returns> 
        /// It returns a redirect to the list of parties if successful, otherwise it returns the same view with validation errors.
        /// </returns>
        // POST: /PartyManager/Edit
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


        /// <summary>
        /// This method is used to display the manage page for a specific party.
        /// </summary>
        /// <param name="id">The ID of the party to manage.</param>
        /// <returns> 
        /// It returns the view for managing the party, including a summary, list of invitations, and a form to add a new invitation.
        /// </returns>
        // GET: /PartyManager/Manage/{id}
        // This page shows the party summary, a list of invitations, and a form to add a new invitation.
        [HttpGet]
        public IActionResult Manage(int id)
        {
            var party = _context.Parties.Find(id);
            if (party == null)
                return NotFound();

            // Load existing invitations for the party.
            var invitations = _context.Invitations.Where(i => i.PartyId == id).ToList();

            // Create a view model for manageing the party.
            var manageViewModel = new PartyManageViewModel
            {
                Party = party,
                PartyId = party.PartyId,  // Set the PartyId for binding on POST.
                Invitations = invitations,
                // Prepare a new invitation for data binding on the add invitation form.
                NewInvitation = new Invitation { PartyId = id }
            };

            return View(manageViewModel);
        }


        /// <summary>
        /// This method is used to handle adding a new invitation to a party.
        /// </summary>
        /// <param name="model">The view model containing the party and new invitation data.</param>
        /// <returns> 
        /// It returns a redirect to the manage page for the same party if successful, otherwise it returns the same view with validation errors.
        /// </returns>
        // POST: /PartyManager/AddInvitation
        // Handles adding a new invitation to the party.
        [HttpPost]
        public IActionResult AddInvitation(PartyManageViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var errors = ModelState.Values.SelectMany(v => v.Errors)
                //                      .Select(e => e.ErrorMessage).ToList();
                //System.Diagnostics.Debug.WriteLine("Validation Errors: " + string.Join(", ", errors));

                // Ensure the new invitation is associated with the correct party.
                model.NewInvitation.PartyId = model.PartyId;
                // The invitation status will default to InviteNotSent.
                _context.Invitations.Add(model.NewInvitation);
                _context.SaveChanges();
                // Redirect back to the manage page for the same party.
                return RedirectToAction("Manage", new { id = model.PartyId });
            }
            model.Party = _context.Parties.Find(model.PartyId);
            // Reload invitations to prevent null reference
            model.Invitations = _context.Invitations.Where(i => i.PartyId == model.PartyId).ToList();
            return View("Manage", model); // Return view instead of redirecting
        }


        /// <summary>
        /// This method is used to send invitation emails to all guests whose invitations have not yet been sent.
        /// </summary>
        /// <param name="id">The ID of the party for which to send invitations.</param>
        /// <returns> 
        /// It returns a redirect to the manage page for the same party.
        /// </returns>
        // POST: /PartyManager/SendInvitations/{id}
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
            string fromAddress = "YOUR-EMAIL";
            string password = "EMAIL-CUSTOM-APP-PASSWORD";

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
                           $"<p>Please <a href=\"https://localhost:5001/Invitation/Respond?partyId={party.PartyId}&invitationId={invitation.InvitationId}\">let us know</a> if you can attend.</p>" +
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

            return RedirectToAction("Manage", new { id = party.PartyId });
        }
    }
}
