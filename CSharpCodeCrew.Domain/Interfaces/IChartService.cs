namespace CSharpCodeCrew.Domain.Interfaces
{
    public interface IChartService
    {
        Task<byte[]> GenerateChart(string jsonData);
    }
}