export interface ICheckbox {
    id: string;
    name: string;
    label?: string;
    options?: ICheckboxItem[];
    required: boolean;
    disabled?: boolean;
    IsShowMessageError?: boolean;
    IsSelectAllOption?: boolean;
    onChange?: (value: any) => void;
    style?: React.CSSProperties;
    defaultValue?: (string)[];
    size?: "small" | "medium";
    row?: boolean;
}

export interface ICheckboxItem {
    label: string;
    value: string;
    disabled?: boolean;
}