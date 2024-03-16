export interface IDatePickerProps {
    label?: string; /**ชื่อข้อความ */
    disabled?: boolean;
    id?: any;
    name: string;
    fullWidth?: boolean; /**ขนาดเต็ม */
    variant?: "outlined" | "filled" | "standard";
    styles?: React.CSSProperties;
    format?: string; /**รูปแบบ */
    minDate?: any; /**กำหนวดวันน้อยสุด */
    maxDate?: any; /**กำหนดวันมากสุด */
    shouldDisableDate?: boolean;
    shouldDisableYear?: boolean;
    isShowIcon?: boolean;
    isShowMessageError?: boolean;
    view?: any; /** รูปแบบ ["year", "month", "day"] */
    IsShrink?: boolean;
    onChange?: (value: any) => void;
    onBlur?: (event: any) => void;
    onKeyDown?: (event: any) => void;
    onKeyUp?: (event: any) => void;
    onFocus?: (event: any) => void;
    onDisableDay?: (day: any) => boolean;
    IsMonthYearPicker?: boolean;
    IsMonthPicker?: boolean;
    IsYearPicker?: boolean;
    required?: boolean;
    defaultValue?: any
}

export interface IDateRangePickerProp {
    id?: any;
    name: string;
    required?: boolean;
    disabled?: boolean;
    view?: any; /** รูปแบบ ["year", "month", "day"] */
    format?: string; /** "DD/MM/YYYY" */
    labelName: string;
    labelStart?: string;
    labelEnd?: string;
    calendarsCount?: 1 | 2 | 3;
    minDate?: any;  /** กำหนวดวันน้อยสุด */
    maxDate?: any;  /** กำหนดวันมากสุด */
    defaultCalendarMonth?: any;
    disablePast?: boolean;
    isAllPopup?: boolean;
    IsShrink?: boolean;
    onChange?: (value: any) => void;
    isShowMessageError?: boolean;
}
export interface IMonthYearRangePickerProps {
    label?: string; /**ชื่อข้อความ */
    disabled?: boolean;
    id?: any;
    name: string;
    fullWidth?: boolean; /**ขนาดเต็ม */
    variant?: "outlined" | "filled" | "standard";
    styles?: React.CSSProperties;
    format?: string; /**รูปแบบ */
    minDate?: any; /**กำหนวดวันน้อยสุด */
    maxDate?: any; /**กำหนดวันมากสุด */
    shouldDisableDate?: boolean;
    shouldDisableYear?: boolean;
    isShowIcon?: boolean;
    isShowMessageError?: boolean;
    view?: any; /** รูปแบบ ["year", "month", "day"] */
    IsShrink?: boolean;
    onChange?: (value: any) => void;
    onBlur?: (event: any) => void;
    onKeyDown?: (event: any) => void;
    onKeyUp?: (event: any) => void;
    onFocus?: (event: any) => void;
    onDisableDay?: (day: any) => boolean;
    IsMonthYearPicker?: boolean;
    IsMonthPicker?: boolean;
    IsYearPicker?: boolean;
    IsMonthYearPicker?: boolean;
    IsMonthPicker?: boolean;
    IsYearPicker?: boolean;
    labelStart?: string;
    labelEnd?: string;
    labelName: string;
    required?: boolean;
}

export interface IQuarterPickerProps {
    label?: string; /**ชื่อข้อความ */
    id?: any;
    name: string;
    disabled?: boolean;
    fullWidth?: boolean; /**ขนาดเต็ม */
    variant?: "outlined" | "filled" | "standard";
    minQuarter?: any;
    maxQuarter?: any;
    isShowMessageError?: boolean;
    onChange?: (value: any) => void;
    onBlur?: (event: any) => void;
    onKeyDown?: (event: any) => void;
    onKeyUp?: (event: any) => void;
    labelStart?: string;
    labelEnd?: string;
    labelName?: string;
    required?: boolean;
}