using System.Collections.Generic;
using System.Data;

namespace Extensions.Systems
{
    /// <summary>
    /// </summary>
    public class AllClass
    {

        #region Upload File
        /// <summary>
        /// </summary>
        public class ItemFileData
        {
            /// <summary>
            /// </summary>
            public int nID { get; set; }
            /// <summary>
            /// </summary>
            public string? sSaveToFileName { get; set; }
            /// <summary>
            /// </summary>
            public string? sSysFileName { get; set; }
            /// <summary>
            /// </summary>
            public string? sFileName { get; set; }
            /// <summary>
            /// </summary>
            public string? sFolderName { get; set; }
            /// <summary>
            /// </summary>
            public string? sSaveToPath { get; set; }
            /// <summary>
            /// </summary>
            public string? sSize { get; set; }
            /// <summary>
            /// </summary>
            public string? sUrl { get; set; }
            /// <summary>
            /// </summary>
            public bool IsNewFile { get; set; }
            /// <summary>
            /// </summary>
            public bool IsNew { get; set; }
            /// <summary>
            /// </summary>
            public bool IsCompleted { get; set; }
            /// <summary>
            /// </summary>
            public bool IsProgress { get; set; }
            /// <summary>
            /// </summary>
            public string? sMsg { get; set; }
            /// <summary>
            /// </summary>
            public string? sProgress { get; set; }
            /// <summary>
            /// </summary>
            public bool IsDelete { get; set; }
            /// <summary>
            /// </summary>
            public string? sFileType { get; set; }
            /// <summary>
            /// </summary>
            public bool IsNewTab { get; set; }
            /// <summary>
            /// </summary>
            public string? sFileLink { get; set; }
            /// <summary>
            /// </summary>
            public string? sPath { get; set; }
            /// <summary>
            /// </summary>
            public int? nSizeName { get; set; } //cal Size
            /// <summary>
            /// </summary>
            public string? sCropFileLink { get; set; }
            /// <summary>
            /// </summary>
            public string? sCropPath { get; set; }
            /// <summary>
            /// </summary>
            public string? sDescription { get; set; }
        }

        // public class cFile
        // {
        //     public string? sFileID { get; set; }
        //     public int index { get; set; }
        //     public string sFolderName { get; set; }
        //     public string sFileName { get; set; }
        //     public string sSysFileName { get; set; }
        //     public string sSizeName { get; set; }
        //     public string sUrl { get; set; }
        //     public string sFileLink { get; set; }
        //     public string sRootURL { get; set; }
        //     public bool IsNew { get; set; }
        //     public bool IsCompleted { get; set; }
        //     public string sFileType { get; set; }
        //     public bool IsNewTab { get; set; }
        //     public bool IsDelete { get; set; }
        //     public string? sProgress { get; set; }
        // }

        // public class DataFile
        // {
        //     public cFile fFile { get; set; }
        // }

        /// <summary>
        /// </summary>
        public class cParamCrop
        {
            /// <summary>
            /// <summary>
            public string? sOldPath { get; set; }
            /// <summary>
            /// </summary>
            public string? sBase64 { get; set; }
        }

        #endregion
        #region remove
        /// <summary>
        /// </summary>
        public class RequestRemove
        {
            /// <summary>
            /// </summary>
			public List<string>? lstRemove { get; set; }
            /// <summary>
            /// </summary>
			public string? sID { get; set; }
        }
        #endregion

        #region import
        public class CResultReadFile : CResutlWebMethod
        {
            public DataSet ds { get; set; }
            public DataTable dt { get; set; }
            public List<string> lstError { get; set; }
        }

        public class CResutlWebMethod
        {
            public int StatusCode { get; set; }
            public int nStatusCode { get; set; }
            public string sMsg { get; set; }
            public string sContent { get; set; }
            public List<int> lstStatus { get; set; }
        }

        public class SelectOption
        {
            public string value { get; set; }
            public string label { get; set; }
            public string sColor { get; set; }
            public string sData_Unit { get; set; }
            public string sData_UnitCode { get; set; }
            public string sEmployeeName { get; set; }
            public string sId { get; set; }
            public int nID { get; set; }
        }
        #endregion

    }
}