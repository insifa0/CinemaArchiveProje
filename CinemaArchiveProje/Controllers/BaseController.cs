using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using CinemaArchiveProje.Data;

public class BaseController : Controller
{
    protected readonly ApplicationDbContext _context;

    public BaseController(ApplicationDbContext context)
    {
        _context = context;
    }

    protected bool IsAdmin()
    {
        var role = HttpContext.Session.GetString("Role");
        return role == "Admin";
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
        ViewBag.IsAdmin = IsAdmin();  // Tüm View'lara aktar
    }
}
