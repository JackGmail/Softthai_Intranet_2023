import { array } from '@amcharts/amcharts4/core';
import { SxProps } from '@mui/system';
import { GridColumnGroupingModel } from '@mui/x-data-grid-pro';
export interface FilterFieldInterface {
    sFieldName: string;
    sLabel: string;
    nGridLG?: number;
    nGridMD?: number;
    nGridSM?: number;
    nGridXS?: number;
    sInputMode?: "text" | "numeric" | "none" | "search" | "email" | "decimal" | "tel" | "url";
    sTypeFilterMode: "input" | "select" | "multiselect" | "daterange" | "date";
    sType?: string;
    optionSelect?: OptionSelect[];
}

export interface OptionSelect {
    value: any;
    label: any;
    // keyId: any;
}


export interface DataGridProp {
    columns: GridColumns;
    onLoadData: (dataPagination: any) => void;
    rows?: PaginationInterface;
    isHiddenToolHead?: boolean;
    modeFilter?: number;
    isLoading?: boolean;
    groupRowByField?: string;
    isShowCheckBox?: boolean;
    isDisableColumnMenu?: boolean;
    filterField?: FilterFieldInterface[];
    onDelete?: (value: any) => void;
    lstPageSize?: Array;
    backgroundHeaderCustom?: string;
    sxCustomHeader?: SxProps<Theme>;
    sxCustomTable?: SxProps<Theme>;
    maxRowNoScroll?: number;
    minHeightCustom?: number;
    onCickRow?: (event: any) => void;
    onFilterCustom?: (data: any, event: any) => void;
    onExportExcel?: (event: any) => void;
    isExportExcel?: boolean;
    handleDataMode?: "server" | "client";
    contentExpand?: any;
    onExpand?: (event: any) => void;
    expandRowLength?: number;
    id?: string;
    getDetailPanelContent?: any;
    getDetailPanelHeight?: any;
    isRowReordering?: boolean;
    onRowOrderChange?: (event: any) => void;
    isHiddenFooter?: boolean;
    isHiddenPagination?: boolean;
    arrSelect?: Array;
    setArrSelect?: any;
    isDisableColumnReorder?: boolean;
    isExpan?: boolean;
    onExpandRowLength?: any;
    rowReordering?: any;
    sxCustomTable?: any;
    onRowSelectable?: (params: GridRowParams<any>) => boolean;
    nExpandRowLength?: number;
    customFilterPanel?: any;
    onClearFilter?: (event: any) => void;
    customNoRowsOverlay?: any;
    isShowGridLine?: boolean;
    isShowColomnTool?: boolean;
    renderActionSelectRow?: any;
    experimentalFeatures?: Partial<GridExperimentalProFeatures>;
    columnGroupingModel?: GridColumnGroupingModel;
    listFixL: PropTypes.object;
    listFixR: PropTypes.object;
    RefAPI?: any;
    sTableID: string;
    isTreeData?: boolean;
    getTreeDataPath?: any;
    groupingColDef?: any;
    onSelectionChange?: (event: any) => void;
    alignItemCell: "center" | "start" | "end",
    colPin: array
}
