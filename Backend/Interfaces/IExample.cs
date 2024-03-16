using Extensions.Common.STResultAPI;
using Backend.Models.ExampleModel;

namespace Backend.Interfaces
{
    public interface IExampleService
    {
        Task<ResultAPI> SaveData(cExampleData objData);
        Task<cExampleData> GetData();
    }
}