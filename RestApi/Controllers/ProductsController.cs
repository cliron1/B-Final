using BezeqFinalProject.Common.Data.Contexts;
using BezeqFinalProject.Common.Data.Entities;
using BezeqFinalProject.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BezeqFinalProject.WebApi.Controllers;

[ApiController]
[Route("products")]
[TypeFilter(typeof(StopwatchFilter))]
public class ProductsController : Controller {
    private readonly MainContext _context;

    public ProductsController(MainContext context) {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
        => Ok(await _context.Customers.ToListAsync());
}
