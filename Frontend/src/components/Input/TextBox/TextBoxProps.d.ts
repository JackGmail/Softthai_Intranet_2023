interface ITextbox {
    id: string;
    name: string;
    label?: string;
    placeholder?: string;
    defaultValue?: string;
    type: "text" | "email" | "password";
    variant?: "outlined" | "filled" | "standard";
    margin?: "none" | "dense" | "normal";
    pattern?: "th" | "th-number" | "en" | "en-number" | "password";
    size?: "small" | "medium";
    maxLength: number;
    IsShowMessageError?: boolean;
    fullWidth?: boolean;
    disabled?: boolean;
    required: boolean;
    startAdornment?: React.ReactNode;
    endAdornment?: React.ReactNode;
    autoComplete?: string;
    onChange?: (event: any) => void;
    onBlur?: (event: any) => void;
    style?: React.CSSProperties;
    IsShrink?: boolean;
}
