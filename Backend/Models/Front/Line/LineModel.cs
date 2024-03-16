using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ST_API.Models
{
    ///<Summary>
    /// /// ข้อมูล User
    ///</Summary>
    public class cUser
    {
        ///<Summary>
        /// ชื่อที่แสดง
        ///</Summary>
        [JsonPropertyName("displayName")]
        public string? sDisplayName { get; set; }

        ///<Summary>
        /// User ID
        ///</Summary>
        [JsonPropertyName("userId")]
        public string? sUserID { get; set; }

        ///<Summary>
        ///  ภาษา
        ///</Summary>
        [JsonPropertyName("language")]
        public string? sLanguage { get; set; }

        ///<Summary>
        /// รูป
        ///</Summary>
        [JsonPropertyName("pictureUrl")]
        public string? sPictureUrl { get; set; }
    }

    /// ข้อมูล Group
    public class cGroup
    {
        /// ข้อมูล Group ID
        [JsonPropertyName("groupId")]
        public string? sGroupId { get; set; }

        /// ข้อมูล Group Name
        [JsonPropertyName("groupName")]
        public string? sGroupName { get; set; }

        /// ข้อมูล Picture
        [JsonPropertyName("pictureUrl")]
        public string? sPictureUrl { get; set; }
    }

    /// ข้อมูล Event List
    public class Events
    {
        /// ข้อมูล Event List
        [JsonPropertyName("events")]
        public List<EventsList>? lstEvent { get; set; }
    }

    ///<Summary>
    /// ข้อมูล Event List
    ///</Summary>
    public class EventsList
    {
        [JsonPropertyName("replyToken")]
        public string sReplyToken { get; set; }

        [JsonPropertyName("type")]

        public string sType { get; set; }

        [JsonPropertyName("message")]
        public Message objMessage { get; set; }

        [JsonPropertyName("source")]
        public Source objSource { get; set; }

        [JsonPropertyName("joined")]
        public Joined objJoined { get; set; }
        [JsonPropertyName("postback")]
        public Postback Postback { get; set; }
        [JsonPropertyName("timestamp")]

        public double nTimestamp { get; set; }



    }

    ///<Summary>
    /// ข้อมูล Join
    ///</Summary>
    public class Joined
    {
        ///<Summary>
        /// ข้อมูล Member
        ///</Summary>
        [JsonPropertyName("members")]
        public List<Members>? lstMember { get; set; }
    }

    ///<Summary>
    /// ข้อมูล Member
    ///</Summary>
    public class Members
    {
        ///<Summary>
        /// ข้อมูล Type
        ///</Summary>
        [JsonPropertyName("type")]
        public string? sType { get; set; }

        ///<Summary>
        /// ข้อมูล User ID
        ///</Summary>
        [JsonPropertyName("userId")]
        public string? sUserID { get; set; }
    }

    ///<Summary>
    /// ข้อมูล Message
    ///</Summary>
    public class Message
    {
        ///<Summary>
        /// ข้อมูล Text
        ///</Summary>
        [JsonPropertyName("text")]
        public string? sText { get; set; }
    }
    ///<Summary>
    /// ข้อมูล Postback
    ///</Summary>
    public class Postback
    {
        ///<Summary>
        /// ข้อมูล Text
        ///</Summary>
        [JsonPropertyName("data")]
        public string? sData { get; set; }
    }
    ///<Summary>
    /// ข้อมูล Source
    ///</Summary>
    public class Source
    {
        ///<Summary>
        /// ข้อมูล User ID
        ///</Summary>
        [JsonPropertyName("userId")]
        public string? sUserID { get; set; }

        ///<Summary>
        /// ข้อมูล Room ID
        ///</Summary>
        [JsonPropertyName("roomId")]
        public string? sRoomId { get; set; }

        ///<Summary>
        /// Group ID
        ///</Summary>
        [JsonPropertyName("groupId")]
        public string? sGroupId { get; set; }
    }
    public class cResult
    {
        [JsonPropertyName("message")]
        public string sMssage { get; set; }
    }
    public class cSend
    {
        [JsonPropertyName("to")]
        public string sTo { get; set; }
        [JsonPropertyName("messages")]
        public List<cMessages> lstMessages { get; set; }
    }
    public class cMessages
    {
        [JsonPropertyName("type")]
        public string sType { get; set; }
        [JsonPropertyName("altText")]
        public string sAltText { get; set; }
        [JsonPropertyName("contents")]
        public cContents objContents { get; set; }

    }
    public class cContents
    {
        [JsonPropertyName("type")]
        public string sType { get; set; }
        [JsonPropertyName("size")]
        public string sSize { get; set; }
        [JsonPropertyName("header")]
        public cHeader sHeader { get; set; }
    }
    public class cHeader
    {
        [JsonPropertyName("type")]
        public string sType { get; set; }
        [JsonPropertyName("layout")]
        public string slayout { get; set; }
    }
    public class cParamSendLine
    {
        public int nRequesterID { get; set; }
        public int nTemplateID { get; set; }
        public string? sTo { get; set; }
        public string? sPathImage { get; set; }
        public List<int>? lstEmpTo { get; set; }
        public string sNameAccount { get; set; } = "";
        public string sTime { get; set; } = "";
        public string sDate { get; set; } = "";
        public string sStartDate { get; set; } = "";
        public string sStartTime { get; set; } = "";
        public string sEndDate { get; set; } = "";
        public string sEndTime { get; set; } = "";
        public string sLocation { get; set; } = "";
        public string sDelay { get; set; } = "";
        public string sProject { get; set; } = "";
        public string sDetail { get; set; } = "";
        public string sDetailRequest { get; set; } = "";
        public string sType { get; set; } = "";
        public string sRemark { get; set; } = "";
        public string sStatus { get; set; } = "";
        public string sPathApprove { get; set; } = "";
        public string sPathReject { get; set; } = "";
        public string sPathDetail { get; set; } = "";
        public string sPathCancel { get; set; } = "";
        public string sHours { get; set; } = "";
        public string sHoursActual { get; set; } = "";
        public string sParam11 { get; set; } = "";
        public string sParam12 { get; set; } = "";
        public string sParam13 { get; set; } = "";
        public string sParam14 { get; set; } = "";
        public string sParam15 { get; set; } = "";
        public string sGUID { get; set; } = "";
        public string sMoney { get; set; } = "";
         public string sDay { get; set; } = "";
        public string sMonth { get; set; } = "";
        public string sHospital { get; set; } = "";
        public string sRoom {get;set;} = "";
        public string sTitle { get; set; } = "";
        public string sDateRequest { get; set; } = "";
        public string sTimeRequest { get; set; } = "";
        public string sDetailTemplate { get; set; } = "";

    }
}
