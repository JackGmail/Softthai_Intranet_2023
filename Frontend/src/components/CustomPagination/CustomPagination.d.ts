export interface  CustomPaginationProp{
    data? : any;
    card? : any;
    xs? : number;
    sm? : number;
    md? : number;
    lg? : number;
    defaultPageSize? : number;
    lstPageSize? : number[];
    onLoadData: (dataPagination: any) => void;
    isLoading?: boolean;
    customState? : any;
}