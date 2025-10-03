using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Guariba.Data;
using Guariba.Models;

namespace Guariba.Pages.Comment
{
    public class IndexModel : PageModel
    {
        private readonly Guariba.Data.SocialMediaContext _context;

        public IndexModel(Guariba.Data.SocialMediaContext context)
        {
            _context = context;
        }

        public IList<Guariba.Models.Comment> Comment { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Comment = await _context.Comment
                .Include(c => c.ParentComment)
                .Include(c => c.Post)
                .Include(c => c.User).ToListAsync();
        }
    }
}
