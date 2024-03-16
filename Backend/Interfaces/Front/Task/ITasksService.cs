using Backend.Models;

namespace Backend.Interfaces
{
    public interface ITasksService
    {
        ResultAPI getDentalRemainMoney();
        ResultAPI LoadDataLeaveSummaryTotalDate();
    }
}