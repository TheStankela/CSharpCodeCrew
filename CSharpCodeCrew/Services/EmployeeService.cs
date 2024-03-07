using CSharpCodeCrew.HttpClients;
using CSharpCodeCrew.Interfaces;
using CSharpCodeCrew.Models;

namespace CSharpCodeCrew.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRCVaultClient _rcVaultClient;
        public EmployeeService(IRCVaultClient rcVaultClient)
        {
            _rcVaultClient = rcVaultClient;
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            var timeEntries = await _rcVaultClient.GetTimeEntries();

            var employees = MapToDisplayModel(timeEntries).Where(x => x.Name != null).OrderByDescending(x => x.TotalTime);

            return employees;
        }

        private List<Employee> MapToDisplayModel(IEnumerable<TimeEntry> timeEntries)
        {
            var employeeDictionary = new Dictionary<string, Employee>();

            foreach (var entry in timeEntries)
            {
                if (entry.EmployeeName != null)
                {
                    if (employeeDictionary.TryGetValue(entry.EmployeeName, out var existingEmployee))
                    {
                        existingEmployee.TotalTime += CalculateTimeDifference(entry.StarTimeUtc, entry.EndTimeUtc);
                    }
                    else
                    {
                        var newEmployee = new Employee
                        {
                            Name = entry.EmployeeName,
                            TotalTime = CalculateTimeDifference(entry.StarTimeUtc, entry.EndTimeUtc)
                        };
                        employeeDictionary.Add(newEmployee.Name, newEmployee);
                    }
                }
            }

            return employeeDictionary.Values.Select(employee => new Employee
            {
                Name = employee.Name,
                TotalTime = Math.Round(employee.TotalTime)
            }).ToList();
        }

        private decimal CalculateTimeDifference(DateTime startTime, DateTime endTime)
        {
            if (startTime < endTime)
            {
                var timeDifferenceInHours = (endTime - startTime).TotalHours;
                return (decimal)timeDifferenceInHours;
            }
            return 0;
        }
    }
}
