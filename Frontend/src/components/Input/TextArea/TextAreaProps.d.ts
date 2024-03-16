interface ITextarea {
    id: string;
    name: string;
    label?: string;
    placeholder?: string;
    defaultValue?: string;
    variant?: "outlined" | "filled" | "standard";
    margin?: "none" | "dense" | "normal";
    size?: "small" | "medium";
    maxLength: number;
    rows?: number;
    maxRows?: number;
    IsShowMessageError?: boolean;
    fullWidth?: boolean;
    disabled?: boolean;
    required: boolean;
    startAdornment?: any;
    endAdornment?: any;
    autoComplete?: string;
    onChange?: (event: any) => void;
    onBlur?: (event: any) => void;
    style?: React.CSSProperties;
    IsShrink?: boolean;
}
