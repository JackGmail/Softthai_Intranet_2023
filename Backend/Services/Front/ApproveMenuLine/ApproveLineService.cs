
using Backend.EF.ST_Intranet;
using Backend.Models;
using Backend.Interfaces;
using Backend.Interfaces.Authentication;
namespace Backend.Services.ApproveLineService
{

    public class ApproveLineService : IApproveLineService
    {


        private readonly ST_IntranetEntity _db;
        private readonly IAuthentication _authen;

        public ApproveLineService(ST_IntranetEntity db, IAuthentication authen)
        {
            _db = db;
            _authen = authen;
        }

        public ResponseMenu GetDataMenuLine()
        {
            ResponseMenu result = new ResponseMenu();
            try
            {
                List<int> lstMenuID = new List<int>() { 3, 4, 5, 6, 7, 8 };
                result.lstMenuLine = _db.TM_Menu.Where(w => lstMenuID.Contains(w.nMenuID)).Select(s => new cMenuLine
                {
                    sMenuName = s.sMenuName,
                    sRoute = s.sRoute,
                    nMenuID = s.nMenuID
                }).ToList();
                result.Status = StatusCodes.Status200OK;
            }

            catch (Exception error)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = error.Message;
            }
            return result;
        }

    }

}

