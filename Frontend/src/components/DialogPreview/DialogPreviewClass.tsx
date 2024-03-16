export interface DialogProps {
    open : any  | null;
    handleClose : any | null;
    Title : string | "";
    IMGModal : string | "";
    isFontSite : boolean;
    Transition : any | null;
    lstContent : DialoglistContent[] | [];

}

export interface DialoglistContent {
    id: string;
    Name : string;
    Floor :string;
    Room : string;
    Seating : string;
}

export interface DialogTitleProps {
    id: string;
    children?: React.ReactNode;
    onClose: () => void;
    colorbg?: string;
    color ?: string;
    CloseColor?:string;
  }

  export interface propPopUpCustom {
    IsOpen?: boolean;
    setIsOpen?: any;
    onClose?: ()=>void;
    onClick?: ()=>void;
    sMaxWidth? : 'xs' | 'sm' | 'md' | 'lg' | 'xl';
    Title?: string;
    children: React.ReactNode,
    JsxDialogAction?: boolean;
    bgColor?: string;
    Color? :string;
    Close?: boolean| true;
    hiddenTitle?: boolean | true;
    CloseSave?: boolean | true;
    IsBackdropClick?: boolean | true;
    styles?: React.CSSProperties;
    startAdornment?: any;
    required?:boolean;
    onCustomButton?: React.ReactNode;
    CloseColor?:string;
}

