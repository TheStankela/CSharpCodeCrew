using CSharpCodeCrew.Domain.Models;

namespace CSharpCodeCrew.Domain.Interfaces
{
    public interface IEmployeeService
    {
        public Task<IEnumerable<Employee>> GetEmployees();
        public Task<Stream> GetPieChart();
    }
}