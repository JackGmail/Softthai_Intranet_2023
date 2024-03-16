import React, { useState, useEffect, Dispatch, useMemo } from "react";
import { Box, Stack, Button, Divider, Tooltip, Fab, MenuItem, Typography, Select, Pagination, Grid } from '@mui/material';
import {
    LicenseInfo,
    GridColDef,
    GridColumnMenuPinningItem,
    GridColumnMenuSortItem,
    GridColumnMenuItemProps,
    GridColumnMenuProps, DataGridPro,
    GridToolbarContainer,
    GridToolbarColumnsButton,
    ToolbarPropsOverrides,
    GridRowId,
    GridRowModel,
    GRID_CHECKBOX_SELECTION_COL_DEF,

} from '@mui/x-data-grid-pro';
import { FilterAlt, Search, Clear, } from "@mui/icons-material";
import { styled } from '@mui/material/styles';
import { I18n } from "utilities/utilities";
import { DataGridProp, FilterFieldInterface } from 'components/DataGrid/DataGridProps';
import { TextBox } from 'components/Input/TextBox';
import { FormProvider, useForm } from "react-hook-form";
import { SelectItem, SelectMultipleItem } from 'components/Input/Select';
import {
    DatePickerItem,
    DateRangePickerItem,
} from "components/Input/DatePicker";
import { BtnExportExcel, BtnDelete } from 'components/Button';
import moment from "moment";

LicenseInfo.setLicenseKey("049fdd25559575ef04f65a9e1ed5aabaTz02NDk0MCxFPTE3MTM2NzY2MjYyNzIsUz1wcm8sTE09cGVycGV0dWFsLEtWPTI=");


// #region No Data
const StyledGridOverlay = styled('div')(({ theme }) => ({
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    justifyContent: 'center',
    height: '100%',
    '& .ant-empty-img-1': {
        fill: theme.palette.mode === 'light' ? '#aeb8c2' : '#262626',
    },
    '& .ant-empty-img-2': {
        fill: theme.palette.mode === 'light' ? '#f5f5f7' : '#595959',
    },
    '& .ant-empty-img-3': {
        fill: theme.palette.mode === 'light' ? '#dce0e6' : '#434343',
    },
    '& .ant-empty-img-4': {
        fill: theme.palette.mode === 'light' ? '#fff' : '#1c1c1c',
    },
    '& .ant-empty-img-5': {
        fillOpacity: theme.palette.mode === 'light' ? '0.8' : '0.08',
        fill: theme.palette.mode === 'light' ? '#f5f5f5' : '#fff',
    },
}));

function CustomNoRowsOverlay() {
    return (
        <StyledGridOverlay>
            <svg
                width="120"
                height="100"
                viewBox="0 0 184 152"
                aria-hidden
                focusable="false"
            >
                <g fill="none" fillRule="evenodd">
                    <g transform="translate(24 31.67)">
                        <ellipse
                            className="ant-empty-img-5"
                            cx="67.797"
                            cy="106.89"
                            rx="67.797"
                            ry="12.668"
                        />
                        <path
                            className="ant-empty-img-1"
                            d="M122.034 69.674L98.109 40.229c-1.148-1.386-2.826-2.225-4.593-2.225h-51.44c-1.766 0-3.444.839-4.592 2.225L13.56 69.674v15.383h108.475V69.674z"
                        />
                        <path
                            className="ant-empty-img-2"
                            d="M33.83 0h67.933a4 4 0 0 1 4 4v93.344a4 4 0 0 1-4 4H33.83a4 4 0 0 1-4-4V4a4 4 0 0 1 4-4z"
                        />
                        <path
                            className="ant-empty-img-3"
                            d="M42.678 9.953h50.237a2 2 0 0 1 2 2V36.91a2 2 0 0 1-2 2H42.678a2 2 0 0 1-2-2V11.953a2 2 0 0 1 2-2zM42.94 49.767h49.713a2.262 2.262 0 1 1 0 4.524H42.94a2.262 2.262 0 0 1 0-4.524zM42.94 61.53h49.713a2.262 2.262 0 1 1 0 4.525H42.94a2.262 2.262 0 0 1 0-4.525zM121.813 105.032c-.775 3.071-3.497 5.36-6.735 5.36H20.515c-3.238 0-5.96-2.29-6.734-5.36a7.309 7.309 0 0 1-.222-1.79V69.675h26.318c2.907 0 5.25 2.448 5.25 5.42v.04c0 2.971 2.37 5.37 5.277 5.37h34.785c2.907 0 5.277-2.421 5.277-5.393V75.1c0-2.972 2.343-5.426 5.25-5.426h26.318v33.569c0 .617-.077 1.216-.221 1.789z"
                        />
                    </g>
                    <path
                        className="ant-empty-img-3"
                        d="M149.121 33.292l-6.83 2.65a1 1 0 0 1-1.317-1.23l1.937-6.207c-2.589-2.944-4.109-6.534-4.109-10.408C138.802 8.102 148.92 0 161.402 0 173.881 0 184 8.102 184 18.097c0 9.995-10.118 18.097-22.599 18.097-4.528 0-8.744-1.066-12.28-2.902z"
                    />
                    <g className="ant-empty-img-4" transform="translate(149.65 15.383)">
                        <ellipse cx="20.654" cy="3.167" rx="2.849" ry="2.815" />
                        <path d="M5.698 5.63H0L2.898.704zM9.259.704h4.985V5.63H9.259z" />
                    </g>
                </g>
            </svg>
            <Box sx={{ mt: 1 }}>No Data</Box>
        </StyledGridOverlay>
    );
}
// #endregion

// #region Menu Colums
function MenuCloseComponent(props: GridColumnMenuItemProps) {
    return (
        <Button color="primary" onClick={props.onClick}>
            {`${I18n.SetText("closemenu")}`}
        </Button>
    );
}
function CustomColumnMenu(props: GridColumnMenuProps) {
    const itemProps = {
        colDef: props.colDef,
        onClick: props.hideMenu,
    };
    return (
        <React.Fragment>
            <Stack px={0.5} py={0.5}>
                <GridColumnMenuSortItem
                    {...itemProps}

                />
                <GridColumnMenuPinningItem {...itemProps} />
            </Stack>
            <Divider />
            <Stack px={0.5} py={0.5}>

                <MenuCloseComponent {...itemProps} />
            </Stack>
        </React.Fragment>
    );
}
// #endregion
// #region Filter
declare module '@mui/x-data-grid-pro' {
    interface FilterPanelPropsOverrides {
        isRow: boolean;
        filterField: FilterFieldInterface[];
        colums: GridColDef[];
        isLoading: boolean;
        sTableID: string
    }
    interface ToolbarPropsOverrides {
        isShowColomnTool: boolean,
        isExportExcel: boolean,
        isFilter: boolean,
        isRow: boolean;
        filterField: FilterFieldInterface[];
        colums: GridColDef[];
        isLoading: boolean;
        sTableID: string;
        isShowFilter: boolean;
        setShowFilter: Dispatch<React.SetStateAction<boolean>>;
    }

}
const ControlFilter = (item: FilterFieldInterface, colums: GridColDef[], isLoading, sTableID) => {
    let sLabelFilter = item.sLabel;
    if (!item.sLabel) {
        const objColumns = colums.find((f) => f.field === item.sFieldName);
        if (objColumns) {
            sLabelFilter = objColumns.headerName;
        }
    }
    let sIDFilter = `filter_${sTableID}_${item.sFieldName}`;
    let arrOption = item.optionSelect ? item.optionSelect : [];

    switch (item.sTypeFilterMode) {
        case "input": {
            return (
                <TextBox
                    id={sIDFilter}
                    disabled={isLoading}
                    label={sLabelFilter}
                    name={sIDFilter}
                    type={"text"}
                    maxLength={200}
                    required={false}
                    style={{ margin: "0" }}
                />
            );
        }
        case "select": {
            return (
                <SelectItem required={false} label={sLabelFilter} id={sIDFilter} name={sIDFilter} options={arrOption} />
            );
        }
        case "multiselect": {
            return (
                <SelectMultipleItem required={false} label={sLabelFilter} id={sIDFilter} name={sIDFilter} options={arrOption} nlimitTags={1} />
            );
        }
        case "daterange": {
            return (
                <DateRangePickerItem
                    name={sIDFilter}
                    id={sIDFilter}
                    labelName={sLabelFilter}
                    labelStart={`${item.sLabel} ${I18n.SetText("start")}`}
                    labelEnd={`${item.sLabel} ${I18n.SetText("end")}`}
                />
            );
        }
        case "date": {
            return (
                <DatePickerItem
                    name={sIDFilter}
                    id={sIDFilter}
                    label={sLabelFilter}
                    fullWidth
                />
            );
        }
    }
}
const GridToolbarFilterCustomButton = styled(Button)(({ theme }) => ({
    fontSize: "0.8125rem",
    padding: "4px 5px"
}));

// #endregion
// #endregion

export default function DataGrid(props: DataGridProp) {
    // const apiRef = useGridApiRef();

    const [selectionModel, setSelectionModel] = React.useState<GridRowId[]>(props.arrSelect ?? []);
    let lstPageData = [];
    let lstPage = [];

    useEffect(() => {
        setSelectionModel(props.arrSelect ?? [])
    }, [props.arrSelect])

    const CustomPagination = () => {
        const nPage = props.rows.nPageIndex + (props.handleDataMode === "client" ? 1 : 0);
        const maxRowSizePerPage = nPage * props.rows.nPageSize;
        const rowTotalCount = props.rows.nDataLength;
        const minRowSizePerPage = nPage > 1 ? (nPage - 1) * props.rows.nPageSize + 1 : 1;
        const pageCount = Math.ceil(rowTotalCount / props.rows.nPageSize);
        const sumMaxRow = rowTotalCount > maxRowSizePerPage ? maxRowSizePerPage : rowTotalCount;
        lstPageData = [];
        lstPage = [];
        for (let i = 0; i < pageCount; i++) {
            lstPageData.push({ label: i + 1, value: i });
            lstPage.push(<MenuItem value={i}>{i + 1}</MenuItem>);
        }

        return rowTotalCount < 1 || props.isHiddenPagination ? (
            <Box sx={{ display: "block", width: "100%", }}>
                <Stack
                    direction="row"
                    justifyContent="space-between"
                    alignContent="center"
                    sx={{ px: 2 }}
                >
                    {selectionModel.length > 0 && props.onDelete !== undefined ? (
                        <BtnDelete id="dl02" txt="ลบ"
                            isCircleWithOutText={false}
                            onClick={() => props.onDelete(selectionModel)}
                        />
                    ) : (
                        null
                    )}
                </Stack>
            </Box>
        ) : (
            <Stack
                direction="row"
                sx={{ px: 2, minWidth: "550px" }}
                alignItems="center"
                flex={1}
                spacing={1}
            >
                <Stack flex={1} direction={"row"} spacing={1} alignItems={"center"}>
                    {selectionModel.length > 0 && props.onDelete !== undefined ? (
                        <BtnDelete id="dl01" txt="ลบ"
                            isCircleWithOutText={false}
                            onClick={() => props.onDelete(selectionModel)}
                        />
                    ) : (
                        null
                    )}
                    {selectionModel.length > 0 && props.renderActionSelectRow != null ? props.renderActionSelectRow : null}
                </Stack>
                <Stack direction="row" alignItems="center" spacing={1}>
                    <Typography sx={{ whiteSpace: "nowrap" }}>{"Pages :"}</Typography>
                    <Select
                        size="small"
                        disabled={props.isLoading}
                        autoWidth
                        sx={{
                            height: 30,
                            ".MuiOutlinedInput-notchedOutline > legend": {
                                width: 0,
                            },
                        }}
                        value={nPage}
                        onChange={(e) => {
                            if (!props.isLoading) {
                                let cloneData = { ...props.rows, nPageIndex: e.target.value };
                                // if (props.handleDataMode == "client") cloneData.nPageIndex -= 1;
                                // cloneData.nPageIndex -= 1;
                                props.onLoadData(cloneData);
                            }
                        }}
                        MenuProps={{ PaperProps: { sx: { maxHeight: 150 } } }}
                    >
                        {lstPage.map((item, index) => (
                            <MenuItem key={`JumpPage_${index}`} value={index + 1}>
                                {index + 1}
                            </MenuItem>
                        ))}
                    </Select>
                    <Typography sx={{ whiteSpace: "nowrap" }}>{`${minRowSizePerPage > sumMaxRow ? sumMaxRow : minRowSizePerPage} - ${sumMaxRow} of ${rowTotalCount}`}</Typography>
                    <Pagination
                        color="primary"
                        sx={{
                            ".MuiPagination-ul": {
                                flexWrap: "nowrap"
                            }
                        }}
                        count={pageCount}
                        siblingCount={0} //{1}
                        boundaryCount={1}
                        page={nPage}
                        variant="outlined"
                        shape="rounded"
                        showFirstButton={true}
                        showLastButton={true}
                        size={"small"}
                        onChange={(event, value) => {
                            if (!props.isLoading) {
                                let cloneData = { ...props.rows, nPageIndex: value };
                                if (props.handleDataMode === "client") {
                                    cloneData.nPageIndex -= 1;
                                } else {
                                    cloneData.arrRows = []
                                }
                                props.onLoadData(cloneData);
                            }
                        }}
                    />
                    <Select
                        label=""
                        size="small"
                        disabled={props.isLoading}
                        autoWidth
                        sx={{
                            height: 30,
                            ".MuiOutlinedInput-notchedOutline > legend": {
                                width: 0,
                            },
                        }}
                        value={props.lstPageSize.indexOf(props.rows.nPageSize)}
                        onChange={(e) => {
                            // if (props.handleDataMode === "client") {
                            let cloneData = { ...props.rows, nPageIndex: 0, nPageSize: props.lstPageSize[e.target.value] };
                            props.onLoadData(cloneData);
                            // } else {
                            //     apiRef.current.setPageSize(props.lstPageSize[e.target.value]);
                            // }
                        }}
                    >
                        {props.lstPageSize.map((item, index) => (
                            <MenuItem key={`selPageSize_${index}`} value={index}>
                                {item}
                            </MenuItem>
                        ))}
                    </Select>
                </Stack>
            </Stack>
        );
    };

    const [isShowFilter, setShowFilter] = useState<boolean>(false);

    const CustomFilterPanel = (objFilter: ToolbarPropsOverrides) => {
        const form = useForm({
            shouldUnregister: false,
            shouldFocusError: true,
            mode: "all",
        });
        const offsets = document.getElementById('btn_filter_on_col')?.getBoundingClientRect() ?? null;

        const _setValue = () => {
            props.filterField.forEach((item) => {
                let sIDFilter = `filter_${props.sTableID}_${item.sFieldName}`;
                switch (item.sTypeFilterMode) {
                    case "input": {
                        form.setValue(sIDFilter, props.rows[item.sFieldName])
                        break;
                    }
                    case "select": {
                        form.setValue(sIDFilter, props.rows[item.sFieldName])
                        break;
                    }
                    case "multiselect": {
                        form.setValue(sIDFilter, props.rows[item.sFieldName])
                        break;
                    }
                    case "daterange": {
                        if (props.rows[item.sFieldName]) {
                            form.setValue(sIDFilter,
                                [
                                    props.rows[item.sFieldName][0] ? moment(props.rows[item.sFieldName][0]) : null,
                                    props.rows[item.sFieldName][1] ? moment(props.rows[item.sFieldName][1]) : null,
                                ]
                            )
                        }
                        break;
                    }
                    case "date": {
                        form.setValue(sIDFilter, props.rows[item.sFieldName] ? moment(props.rows[item.sFieldName]) : null)
                        break;
                    }
                }
            })
        }

        useMemo(() => {
            if (!props.isLoading) {
                !props.customFilterPanel && _setValue();
            }
        }, []);

        const onSearch = (e) => {
            let cloneData = {
                nPageSize: props.rows.nPageSize,
                nPageIndex: 1,
                sSortExpression: "",
                sSortDirection: "",
                rows: [],
            };
            props.filterField && props.filterField.forEach((item) => {
                let sIDFilter = `filter_${props.sTableID}_${item.sFieldName}`;

                switch (item.sTypeFilterMode) {
                    case "input": {
                        const data = e[sIDFilter];
                        const typeData = typeof data
                        if (data && typeData.toString() !== "undefined" && data.toString().length > 0) {
                            cloneData[item.sFieldName] = data;
                        }
                        break;
                    }
                    case "select": {
                        const data = e[sIDFilter];
                        const typeData = typeof data
                        if (data && data.length > 0 && typeData.toString() !== "undefined") {
                            cloneData[item.sFieldName] = data;
                        }
                        break;
                    }
                    case "multiselect": {
                        const data = e[sIDFilter];
                        const typeData = typeof data
                        if (data && data.length > 0 && typeData.toString() !== "undefined") {
                            cloneData[item.sFieldName] = data;
                        }
                        break;
                    }
                    case "daterange": {
                        const data = e[sIDFilter];
                        const typeData = typeof data
                        if (data && data.length > 0 && (data[0] || data[1]) && typeData.toString() !== "undefined") {
                            cloneData[item.sFieldName] = [
                                data[0] ? moment(data[0]).format("YYYY-MM-DD[T]00:00:00[.000Z]") : null,
                                data[1] ? moment(data[1]).format("YYYY-MM-DD[T]00:00:00[.000Z]") : null
                            ];
                        }
                        break;
                    }
                    case "date": {
                        const data = e[sIDFilter];
                        const typeData = typeof data
                        if (data && typeData.toString() !== "undefined") {
                            cloneData[item.sFieldName] = moment(data).format("YYYY-MM-DD[T]00:00:00[.000Z]");
                        }
                        break;
                    }
                }
            });
            props.onLoadData && props.onLoadData(cloneData);
        };

        const onClear = () => {
            props.filterField && props.filterField.forEach((item) => {
                let sIDFilter = `filter_${props.sTableID}_${item.sFieldName}`;
                form.setValue(sIDFilter, undefined);
            })
            onSearch(form.getValues());
        }
        return (
            <div style={{ maxHeight: 200, overflow: "auto", width: "100%", marginBottom: objFilter.isRow ? "1em" : "" }}>
                <Stack style={{ padding: "10px" }} direction={"row"} spacing={1}
                    sx={objFilter.isRow ? {
                        border: "1px #0168cc solid",
                        mx: "1em",
                        borderRadius: "10px",
                        ":before": {
                            content: '""',
                            position: "absolute",
                            left: offsets != null ? `${offsets.left - 10}px` : "4em",
                            top: "2.5em",
                            borderLeft: "5px solid transparent",
                            borderRight: "5px solid transparent",
                            borderBottom: " 5px solid #0168cc",
                        }
                    } : null}
                >
                    {
                        props.customFilterPanel ?
                            props.customFilterPanel
                            :
                            <FormProvider {...form}>

                                <Grid container justifyContent={"end"} flexDirection={"row"} spacing={1}>
                                    {
                                        objFilter.filterField.map((item) => (
                                            <Grid item lg={item.nGridLG || undefined} md={item.nGridMD || undefined} sm={item.nGridSM || undefined} xs={item.nGridXS || undefined}>
                                                {
                                                    ControlFilter(item, objFilter.colums, objFilter.isLoading, objFilter.sTableID)
                                                }
                                            </Grid>
                                        ))
                                    }
                                </Grid>
                                <Tooltip title={"Clear"}>
                                    <Fab
                                        sx={{ width: 40, height: 40, zIndex: 1 }}
                                        color="default"
                                        aria-label="clear"
                                        onClick={onClear}
                                    >
                                        <Clear />
                                    </Fab>
                                </Tooltip>
                                <Tooltip title={"Search"}>
                                    <Fab
                                        type="submit"
                                        sx={{ width: 40, height: 40, zIndex: 1 }}
                                        color="primary"
                                        aria-label="search"
                                        onClick={form.handleSubmit(onSearch)}
                                    >
                                        <Search />
                                    </Fab>
                                </Tooltip>
                            </FormProvider>
                    }
                </Stack>
            </div>
        );
    };

    // #region Toolbar
    const CustomToolbar = (objToolbar: ToolbarPropsOverrides) => {
        return (
            <Stack direction={"column"}>
                <Stack
                    className="head-container"
                    sx={{
                        px: 0.5,
                        pb: 0.5,
                        bgcolor: "white",
                    }}
                    direction="row"
                    justifyContent="start"
                >
                    <GridToolbarContainer>
                        <Tooltip title={I18n.SetText("toolbarColumns")}>
                            <GridToolbarColumnsButton />
                        </Tooltip>
                        {objToolbar.isFilter ?
                            <Tooltip title={I18n.SetText("filter")} >
                                <GridToolbarFilterCustomButton id="btn_filter_on_col" onClick={() => {
                                    objToolbar.setShowFilter(!objToolbar.isShowFilter)
                                }}>
                                    <FilterAlt sx={{ marginRight: "0.2em" }} />
                                    {I18n.SetText("filter")}
                                </GridToolbarFilterCustomButton>
                            </Tooltip>
                            : null}
                        {objToolbar.isExportExcel ? (
                            <Tooltip title={"Export excel"}>
                                <BtnExportExcel id="cd01" isDisabled={objToolbar.isLoading} />
                            </Tooltip>
                        ) : null}
                    </GridToolbarContainer>
                </Stack>
                {objToolbar.isShowFilter ? (
                    CustomFilterPanel(objToolbar)
                ) : null}
            </Stack>
        );
    };
    // #endregion

    return (
        <Box sx={{ width: '100%' }}>
            <DataGridPro
                showCellVerticalBorder={props.isShowGridLine}
                sx={{
                    '&.MuiDataGrid-root .MuiDataGrid-cell:focus': { //ปิด กรอบฟ้า ตอน Focus ช่องนั้นๆ
                        outline: 'none',
                    },
                    '.MuiDataGrid-columnHeaderTitleContainer': {
                        border: "0 !important",
                    },
                    '.MuiDataGrid-cell': {
                        padding:'10px',
                        alignItems: `${props.alignItemCell}`
                    }
                }}
                density="compact"
                autoHeight={true}
                rows={props.isLoading ? [] : props.rows?.arrRows || []}
                columns={props.columns}
                localeText={{
                    columnMenuSortAsc: `${I18n.SetText("sortASC")}`,
                    columnMenuSortDesc: `${I18n.SetText("sortDESC")}`,
                    columnMenuUnsort: `${I18n.SetText("unsort")}`,
                    unpin: `${I18n.SetText("unpin")}`,
                    pinToLeft: `${I18n.SetText("pintoleft")}`,
                    pinToRight: `${I18n.SetText("pintorigth")}`,
                }}
                slots={{
                    noRowsOverlay: CustomNoRowsOverlay,
                    columnMenu: props.isHiddenToolHead ? undefined : CustomColumnMenu,
                    toolbar: props.isHiddenToolHead ? undefined : CustomToolbar,
                    pagination: CustomPagination,
                }}
                slotProps={{
                    filterPanel: {
                        isRow: true,
                        filterField: props.filterField,
                        colums: props.columns,
                        isLoading: false,
                        sTableID: props.sTableID,
                    },
                    toolbar:
                    {
                        showQuickFilter: true,
                        isRow: true,
                        filterField: props.filterField,
                        colums: props.columns,
                        isLoading: false,
                        sTableID: props.sTableID,
                        isShowFilter: isShowFilter,
                        setShowFilter: setShowFilter,
                        isShowColomnTool: true,
                        isExportExcel: false,
                        // isFilter: true,
                    },
                }}
                initialState={{
                    pagination: {
                        paginationModel: {
                            pageSize: props.rows.nPageSize,
                        },
                    },
                    pinnedColumns: {
                        left: props.colPin
                    }
                }}
                rowCount={props.rows.nDataLength}
                disableMultipleColumnsSorting
                hideFooter={props.isHiddenFooter ?? false}
                onSortModelChange={(model) => {
                    if (model.length > 0) {
                        let cloneData = {
                            ...props.rows,
                            sSortExpression: model[0].field,
                            sSortDirection: model[0].sort,
                            arrRows: [],
                        };
                        props.onLoadData && props.onLoadData(cloneData);
                    }
                }}
                hideFooterSelectedRowCount
                checkboxSelection={props.isShowCheckBox}
                disableRowSelectionOnClick
                filterMode={props.handleDataMode}
                sortingMode={props.handleDataMode}
                paginationMode={props.handleDataMode}
                disableVirtualization={true}
                unstable_headerFilters={false}
                disableColumnFilter
                disableDensitySelector
                loading={props.isLoading}
                getRowId={(item) => {
                    return item.sID
                }}

                pagination
                rowSelectionModel={selectionModel}
                onRowSelectionModelChange={(itm) => {
                    setSelectionModel(itm);
                    props.onSelectionChange && props.onSelectionChange(itm);
                }}
                onPaginationModelChange={(pageSize) => {
                    let cloneData = {
                        ...props.rows,
                        nPageSize: pageSize,
                        nPageIndex: 1,
                        rows: [],
                    };
                    props.onLoadData(cloneData);
                }}
                isRowSelectable={props.onRowSelectable}
                onRowClick={props.onCickRow}
                experimentalFeatures={props.experimentalFeatures}
                columnGroupingModel={props.columnGroupingModel}
                treeData={props.isTreeData}
                getTreeDataPath={props.getTreeDataPath}
                groupingColDef={props.groupingColDef}
                getRowHeight={(p) => {
                    if ((props.rows.arrRows ?? []).some(s => s["subData"])) {
                        return props.isShowCheckBox ? 62 : 48
                    }
                    return "auto"
                }}
            />
        </Box>
    );
}

export const initRows: PaginationInterface = {
    nPageIndex: 1,
    nPageSize: 10,
    arrRows: [],
    nDataLength: 0,
    sSortExpression: "",
    sSortDirection: "",
    sID: "",
};
export interface PaginationInterface {
    nPageIndex: number;
    nPageSize: number;
    arrRows: GridRowModel[];
    nDataLength: number;
    sSortExpression: string;
    sSortDirection: string;
    sID: string;
  
}

const defaultProp: DataGridProp = {
    rows: initRows,
    isLoading: false,
    isShowGridLine: true,
    isShowCheckBox: true,
    filterField: [],
    isDisableColumnMenu: true,
    lstPageSize: [10, 25, 50, 100],
    backgroundHeaderCustom: "#ff9800",
    maxRowNoScroll: 10,
    minHeightCustom: 500,
    isHiddenToolHead: false,
    isHiddenFooter: false,
    isHiddenPagination: false,
    alignItemCell: "center",
    handleDataMode: "server",
    sxCustomHeader: { color: '#3b3b3c' },
    isDisableColumnReorder: true,
    isExpan: false,
    isShowColomnTool: true,
    expandRowLength: 0,
    columns: [],
    onLoadData: () => { },
    listFixL: [],
    listFixR: [],
    sTableID: "tb1",
    isTreeData: false,
    getTreeDataPath: undefined,
    groupingColDef: undefined,
    customFilterPanel: undefined,
    onSelectionChange: () => { },
    colPin: []
}
DataGrid.defaultProps = defaultProp;


