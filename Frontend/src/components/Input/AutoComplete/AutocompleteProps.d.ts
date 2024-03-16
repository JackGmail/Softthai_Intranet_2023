interface optionsProps { value: any, label: any, }

export interface IAsyncAutoCompleteProps {
   id: string;
   name: string;
   label?: string;
   placeholder?: string;
   fullWidth?: boolean;
   style?: React.CSSProperties;
   inputPropsStyle?: React.CSSProperties;
   disabled?: boolean;
   IsClearable?: boolean;
   limitTag?: number; //จำนวณที่จะแสดง Tag
   TextFilterCont?: number; //จำนวณที่จะเริ่มค้นหา
   IsShowMessageError?: boolean;
   startAdornment?: any;
   sUrlAPI?: string;
   sParam?: string; //{ "strSearch": sSearch, "sParam": sParam } การส่งค่าแบบตัวเดียวควรใช้ sParam
   ParamUrl?: any; //{ "strSearch": sSearch, "sDate": sDate, "nValue": nValue, "sName": sName } การส่งค่าแบบหลายๆตัวควรใช้ ParamUrl
   sMethodAxios?: string; //("POST" / "GET") default "GET"
   onChange?: any;
   onBlur?: any;
   IsPopperCustom?: boolean;
   IsShrink?: boolean;
   required?: boolean;
}