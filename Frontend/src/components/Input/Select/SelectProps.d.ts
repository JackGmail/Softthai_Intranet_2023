interface optionsProps {
    value: string,
    label: string,
    color?: any,
    Isnew?: boolean 
}
export interface SelectProps {
    id: string;
    name: string;
    label?: any;
    placeholder?: string;
    disabled?: boolean;
    fullWidth?: boolean;
    notOptionsText?: any;
    options: optionsProps[];
    isClearable?: boolean;
    nlimitTags?: number;
    nLimits?: number;
    isSelectAll?: boolean;
    startAdornment?: any;
    style?: React.CSSProperties;
    inputPropsStyle?: React.CSSProperties;
    isPopperCustom?: boolean;
    isDisablePortal?: boolean;
    onChange?: (value: any, event: any) => void;
    onBlur?: (event: any) => void;
    onKeyPress?: (event: any) => void;
    onKeyDown?: (event: any) => void;
    onKeyUp?: (event: any) => void;
    onFocus?: (event: any) => void;
}

export interface SelectFromProps extends SelectProps {
    required: boolean;
    isShowMessageError?: boolean;
    isShowCountSelected?: boolean;
    small?: boolean;
    defaultValue?: string;
}
interface optionsTreeProps {
    isParent: boolean,
    sParentID?: string,
    value: string,
    label?: string,
    label_th?: string,
    label_en?: string,
}
export interface ITreeSelectProps {
    id: string;
    name: string;
    label?: string;
    placeholder?: string;
    disabled?: boolean;
    fullWidth?: boolean;
    notOptionsText?: any;
    options: optionsTreeProps[];
    nlimitTags?: number;
    startAdornment?: any;
    style?: React.CSSProperties;
    inputPropsStyle?: React.CSSProperties;
    isPopperCustom?: boolean;
    hint?: string;
    subLabel?: string;
    autoSelect?: boolean;
    IsShrink?: boolean;
    onClearOptions?: any;
    selectAllLabel?: string;
    sxCustomLabelChip?: any;
    onChange?: (value: any) => void;
    onBlur?: (event: any) => void;
    onKeyDown?: (event: any) => void;
    onKeyUp?: (event: any) => void;
    onFocus?: (event: any) => void;
    required: boolean;
    isShowMessageError?: boolean;
}

export interface FilmOptionType {
    label?: string;
    value: string;
    newVaue?: string;
}
