export interface IRadio {
    id: string;
    name: string;
    label?: string;
    options: IRadioItem[];
    required: boolean;
    disabled?: boolean;
    IsShowMessageError?: boolean;
    onChange?: (value: any) => void,
    style?: React.CSSProperties;
    defaultValue? : string;
    size?: "small" | "medium";
    row?: boolean;
}

export interface IRadioItem {
    label: string;
    value: string;
    disabled?: boolean;
}
