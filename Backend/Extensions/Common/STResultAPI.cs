using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Extensions.Common.STResultAPI
{
    public class ResultAPI
    {
        public int Status { get; set; }
        public string? Message { get; set; }
        public int nPermission { get; set; }
        public string? sID { get; set; }
        public string? sRole { get; set; }
    }

    public static class ResultStatus
    {
        public const string Duplicate = "Duplicate Data";
        public const string Error = "Error";
        public const string Success = "Success";
        public const string Failed = "Failed";
    }

    public class  Pagination : ResultAPI
    {
        public int nDataLength { get; set; } //nDataCountAll
        public int nPageIndex { get; set; }
        public int nSkip { get; set; } //nSkipData
        public int nTake { get; set; } //nTakeData
        public int nStartIndex { get; set; } //nStartItemIndex
    }
}
