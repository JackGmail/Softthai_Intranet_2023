export interface SwitchProps {
    id: string;
    name: string;
    label?: string;
    checkText?: string;
    uncheckText?: string;
    width?: number;
    onChange?: (value: any) => void;
    disabled?: boolean;
    fontColor?: string;
    offColor?: string;
    onColor?: string;
    direction?: "row" | "column";
    defaultValue?: boolean;
}