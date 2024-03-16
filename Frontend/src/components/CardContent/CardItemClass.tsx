export interface CardCustom {
    sMaxWidth? : string;
    Title?: string;
    children: React.ReactNode,
    JsxDialogAction?: React.ReactNode;
    bgColor?: string;
    imageG? :string;
    height? : string;
    Actionbuttom?: React.ReactNode,
    onClick?: () => void;
}

export interface CardCustomFrom {
    footer?:React.ReactNode,
    children: React.ReactNode,
    bgColor?: string;
    widthCard?: string;
    IsNoMarginLeftRight?:boolean;
}

