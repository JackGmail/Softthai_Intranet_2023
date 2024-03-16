using Backend.Models;
using Extensions.Common.STExtension;
using ST.INFRA;
using ST.INFRA.Common;
using Backend.Interfaces.Authentication;
using Backend.Interfaces;

namespace Backend.Services
{
    public class TestToolsService : ITestToolsService
    {
        private readonly IConfiguration _config;
        private readonly IAuthentication _authen;

        public TestToolsService(IConfiguration config, IAuthentication authen)
        {
            _config = config;
            _authen = authen;
        }

        #region Async Auto Complete
        public List<cDataOptions_AutoComplete>? SearchAllData(string strSearch)
        {
            List<cDataOptions_AutoComplete>? lstOption = new List<cDataOptions_AutoComplete>();
            for (int i = 1; i < 40; i++)
            {
                var newData = new cDataOptions_AutoComplete { label = "Label > " + i, value = "Value > " + i, };
                lstOption.Add(newData);
            }

            if (!string.IsNullOrEmpty(strSearch))
            {
                lstOption = lstOption.Where(w => w.label.Contains(strSearch)).ToList();
            }
            return lstOption;
        }
        #endregion
    }
}