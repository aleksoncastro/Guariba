using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Guariba.Data;
using Guariba.Models;

namespace Guariba.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly Guariba.Data.SocialMediaContext _context;

        public EditModel(Guariba.Data.SocialMediaContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User User { get; set; } = default!;

        // Add these properties to your EditModel class
        [BindProperty]
        public List<int> SelectedInterests { get; set; } = new List<int>();

        public List<SelectListItem> AllInterests { get; set; } = new List<SelectListItem>();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // IMPORTANT: Use .Include() to load the existing interests!
            User = await _context.User
                                 .Include(u => u.UserInterests)
                                 .FirstOrDefaultAsync(m => m.Id == id);

            if (User == null)
            {
                return NotFound();
            }

            // Get all possible enum values
            var allEnumValues = Enum.GetValues(typeof(Interest)).Cast<Interest>();

            // Get the user's currently saved interests
            var userInterests = User.UserInterests.Select(ui => ui.Interest).ToHashSet();

            // Populate the AllInterests list for the view
            AllInterests = allEnumValues.Select(interest => new SelectListItem
            {
                Value = ((int)interest).ToString(),
                Text = interest.ToString(),
                Selected = userInterests.Contains(interest) // This will check the box if the user has this interest
            }).ToList();

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Load the user from the database again, WITH their current interests
            var userToUpdate = await _context.User
                                             .Include(u => u.UserInterests)
                                             .FirstOrDefaultAsync(u => u.Id == id);

            if (userToUpdate == null)
            {
                return NotFound();
            }

            // Update the user's main properties (Username, Email, etc.)
            // This method is a safe way to update properties from the submitted form
            if (await TryUpdateModelAsync<User>(
                userToUpdate,
                "User", // This is the prefix for the form fields
                u => u.Username, u => u.Email /* add other User properties here */))
            {
                UpdateUserInterests(userToUpdate); // Call a helper method to update interests
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return Page();
        }

        private void UpdateUserInterests(User userToUpdate)
        {
            // Get the user's current interests from the database
            var currentUserInterests = userToUpdate.UserInterests.Select(ui => ui.Interest).ToHashSet();
            // Get the new selections from the form
            var selectedInterests = SelectedInterests.Select(si => (Interest)si).ToHashSet();

            // 1. Remove interests that were un-checked
            foreach (var interest in currentUserInterests)
            {
                if (!selectedInterests.Contains(interest))
                {
                    var interestToRemove = userToUpdate.UserInterests
                        .SingleOrDefault(ui => ui.Interest == interest);
                    if (interestToRemove != null)
                    {
                        _context.Remove(interestToRemove);
                    }
                }
            }

            // 2. Add interests that were newly checked
            foreach (var interest in selectedInterests)
            {
                if (!currentUserInterests.Contains(interest))
                {
                    userToUpdate.UserInterests.Add(new UserInterest
                    {
                        UserId = userToUpdate.Id,
                        Interest = interest
                    });
                }
            }
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
