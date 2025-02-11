using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using AlpineSkiHouse;  // Ensure this matches the actual namespace of your DbContext
using AlpineSkiHouse.Models;  // Ensure this matches the namespace where your Activity model is located
using Microsoft.EntityFrameworkCore;  // For using ToListAsync
using System.Linq;  // Required for LINQ queries
using System.Collections.Generic;  // Required for List<>

namespace AlpineSkiHouse.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly AlpineSkiHouseDbContext _context; 

    public List<Activity> Activities { get; private set; } = new List<Activity>();

    public IndexModel(ILogger<IndexModel> logger, AlpineSkiHouseDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task OnGetAsync()
    {
        Activities = await _context.Activities.ToListAsync();
    }

    public async Task<IActionResult> OnPostLikeAsync(int id)
    {
        var activity = await _context.Activities.FindAsync(id);
        if (activity == null)
        {
            return NotFound();
        }

        activity.Likes++;
        await _context.SaveChangesAsync();
        return new JsonResult(new { likes = activity.Likes, dislikes = activity.Dislikes });
    }

    public async Task<IActionResult> OnPostDislikeAsync(int id)
    {
        var activity = await _context.Activities.FindAsync(id);
        if (activity == null)
        {
            return NotFound();
        }

        activity.Dislikes++;
        await _context.SaveChangesAsync();
        return new JsonResult(new { likes = activity.Likes, dislikes = activity.Dislikes });
    }
}
