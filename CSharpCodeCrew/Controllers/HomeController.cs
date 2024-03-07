using CSharpCodeCrew.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CSharpCodeCrew.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        public HomeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        public async Task<IActionResult> Index()
        {
            var employees = await _employeeService.GetEmployees();
            return View(employees);
        }
    }
}
