using CSharpCodeCrew.Models;

namespace CSharpCodeCrew.Interfaces
{
    public interface IEmployeeService
    {
        public Task<IEnumerable<Employee>> GetEmployees();
    }
}