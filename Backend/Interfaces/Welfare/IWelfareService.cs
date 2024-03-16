using Backend.Models;

namespace Backend.Interfaces
{
    public interface IWelfareService
    {
        cDentalResult SelectDentalType();
        cDentalTableResult GetData_Dental(cDentalTable obj);
        cDentalResult SaveData_DentalRequest(cDentalDetail objSave);
        ResultAPI SaveApprove(cReqWelfareApprove param);
    }
}