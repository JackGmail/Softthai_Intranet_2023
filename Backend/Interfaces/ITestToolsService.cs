using Backend.Models;

namespace Backend.Interfaces
{
    public interface ITestToolsService
    {
        List<cDataOptions_AutoComplete>? SearchAllData(string strSearch);
    }
}
