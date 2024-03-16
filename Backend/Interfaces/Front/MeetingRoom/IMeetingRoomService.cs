using Extensions.Common.STResultAPI;
using Backend.Models;
using ResultAPI = Backend.Models.ResultAPI;

namespace Backend.Interfaces
{
    /// <summary>
    /// </summary>
    public interface IMeetingRoomService
    {
        /// <summary>
        /// GetListCalendar 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <response code="200">ผ่าน</response>
        clsResultMeeting GetListCalendar(clsFilterMeeting param);
        /// <summary>
        /// GetOption 
        /// </summary>
        /// <returns></returns>
        /// <response code="200">ผ่าน</response>
        clsResultMeeting GetOption(clsFilterCheckCo param);
        /// <summary>
        /// SaveForm 
        /// </summary>
        /// /// <param name="obj"></param>
        /// <returns></returns>
        /// <response code="200">ผ่าน</response>
        ResultAPI SaveForm(clsSaveMeeting obj);
        /// <summary>
        /// GetData 
        /// </summary>
        /// /// <param name="nID"></param>
        /// <returns></returns>
        /// <response code="200">ผ่าน</response>
        clsSaveMeeting GetData(string nID, string? Mode);
        /// <summary>
        /// GetListRoom 
        /// </summary>
        /// <returns></returns>
        /// <response code="200">ผ่าน</response>
        clsResultMeeting GetListRoom();
        /// <summary>
        /// GetPerson 
        /// </summary>
        /// /// <param name="nProjectID"></param>
        /// <returns></returns>
        /// <response code="200">ผ่าน</response>
        clsResultMeeting GetPerson(int nProjectID);
        /// <summary>
        /// GetFileZip 
        /// </summary>
        /// /// <param name="param"></param>
        /// <returns></returns>
        /// <response code="200">ผ่าน</response>
        cFileZip GetFileZip(cFileID param);
        /// <summary>
        /// GetAllInprogress 
        /// </summary>
        /// /// /// <param name="obj"></param>
        /// <returns></returns>
        /// <response code="200">ผ่าน</response>
        clsFilter GetAllInprogress(objGetData obj);
        /// <summary>
        /// SaveDataRoom 
        /// </summary>
        /// /// /// <param name="param"></param>
        /// <returns></returns>
        /// <response code="200">ผ่าน</response>
        ResultAPI SaveDataRoom(cSaveRoom param);
        /// <summary>
        /// GetDataTableFileALL 
        /// </summary>
        /// /// /// <param name="param"></param>
        /// <returns></returns>
        /// <response code="200">ผ่าน</response>
        clsFileAllTable GetDataTableFileALL(cFilterTable param);
         /// <summary>
        /// DeleteRoom 
        /// </summary>
        /// /// <param name="nID"></param>
        /// <returns></returns>
        /// <response code="200">ผ่าน</response>
        ResultAPI DeleteRoom(int nID);
         /// <summary>
        /// GetDataRoom 
        /// </summary>
        /// /// <param name="nID"></param>
        /// <returns></returns>
        /// <response code="200">ผ่าน</response>
        cSaveRoom GetDataRoom(int nID);
          /// <summary>
        /// Cancel 
        /// </summary>
        /// /// <param name="nID"></param>
        /// <returns></returns>
        /// <response code="200">ผ่าน</response>
         ResultAPI Cancel(cReqDataOT param);

    }
}

