import React, { useEffect, useMemo, useState } from "react";
import { CustomPaginationProp } from "./CustomPagination";
import {
  Box,
  Grid,
  Pagination,
  PaginationItem,
  Skeleton,
  Stack,
  Typography,
} from "@mui/material";
import AutoCompletePagination from "./AutoCompletePagination";
import { PaginationInterface } from "components/DataGrid";
import Loadable from 'react-loadable';

const Loading = () => {
  return (<Box sx={{ width: 320 }}>
    <Skeleton variant="rectangular" width={320} height={240} />
    <Typography component="div" variant={"body1"}>
      <Skeleton />
    </Typography>
    <Typography component="div" variant={"caption"}>
      <Skeleton />
    </Typography>
  </Box>);
}




const PaginationCustom = (props: CustomPaginationProp) => {
  let nPage = props.data.nPageIndex;
  let nPageSize = props.defaultPageSize ? props.defaultPageSize : props.data.nPageSize;
  const maxRowSizePerPage = nPage * nPageSize;
  const rowTotalCount = props.data.nDataLength;
  const minRowSizePerPage = nPage > 1 ? (nPage - 1) * nPageSize + 1 : 1;
  const pageCount = Math.ceil(rowTotalCount / nPageSize);
  const sumMaxRow =
    rowTotalCount > maxRowSizePerPage ? maxRowSizePerPage : rowTotalCount;
  const { customState } = props;


  const LoadableComponent = Loadable({
    loader: () => import(`../${props.card}`),
    loading: () => Loading(),
    props: (props) => props.item,
    render(loaded, props) {
      let Component = loaded.default;
      return <Component {...props} state={customState} />;
    },
  });

  let [lstPageData, setLstPageData] = useState([]);
  let [lstPageSize, setLstPageSize] = useState([]);

  const scrollToTop = () => {
    window.scrollTo({
      top: 0,
      behavior: "smooth",
    });
  };

  useEffect(() => {
    scrollToTop();
    if (props.data.arrRows) {
      let lstData = [];
      for (
        let i = 0;
        i < Math.ceil(props.data.nDataLength / nPageSize);
        i++
      ) {
        lstData.push({ label: i + 1, value: i + 1 });
      }
      setLstPageData(lstData);
    }
  }, [props.data,nPageSize]);

  useEffect(() => {
    if (props.lstPageSize) {
      let lstPage = [];
      props.lstPageSize.forEach((page) => {
        lstPage.push({ label: page, value: page });
      });
      setLstPageSize(lstPage);
    }
  }, [props.lstPageSize]);

  const lstData = useMemo(() => {
    let lst = [] as any;
    if(props.data?.arrRows)
    {
      lst = props.data.arrRows;
    }
    return lst;
  }, [props.data.arrRows])

  const pageNext = () => {
    return <>Next</>;
  };

  const pagePrevious = () => {
    return <>Previous</>;
  };

  const pageLast = () => {
    return <>Last</>;
  };

  const pageFirst = () => {
    return <>First</>;
  };
  return (
    <>
      {lstData.map((item, index) => (
        <Grid key={item.sKey} item xs={props.xs} sm={props.sm} md={props.md} lg={props.lg}>
          <LoadableComponent item={item} />
        </Grid>
      ))}
      <Grid item xs={12}>
        <Box
          sx={{
            display: "flex",
            margin: "2rem auto",
            justifyContent: "space-between",
            alignItems: "center",
          }}
        >
          <Box>
            <Typography
              sx={{ color: "rgb(131, 146, 171)", fontSize: "0.875rem" }}
            >{`Showing ${
              minRowSizePerPage > sumMaxRow ? sumMaxRow : minRowSizePerPage
            } to ${sumMaxRow} of ${rowTotalCount} entries`}</Typography>
          </Box>
          <Box sx={{ display: "flex", alignItems: "center" }}>
            {/* //! ต้องเช็คด้วยว่าถ้าเปลี่ยน Perpage แล้วหน้าปัจจุบันที่อยู่มีหรือเปล่า ถ้าไม่มีต้อง jump ไปหน้า 1 ก่อน */}

            <Box sx={{ width: "10px" }}></Box>
            <Typography
              sx={{
                color: "rgb(131, 146, 171)",
                fontSize: "0.875rem",
                mr: "0.5rem",
              }}
            >
              Go To Page
            </Typography>
            <AutoCompletePagination
              small
              sxCustom={{
                ".MuiOutlinedInput-root": {
                  height: "30px",
                  boxShadow: "none",
                  paddingTop: "3px !important",
                  " input": {
                    padding: "0 5px !important",
                    minWidth: "25px",
                  },
                },
              }}
              objValue={{
                label: props.data.nPageIndex,
                value: props.data.nPageIndex,
              }}
              funcOnChange={(e, n) => {
                if (!props.isLoading) {
                  let cloneData = { ...props.data, nPageIndex: n.label };
                  cloneData.arrRows = [];
                  props.onLoadData(cloneData);
                }
              }}
              lstOption={lstPageData}
              disClearable
            />
            <Stack
              spacing={2}
              style={{ marginLeft: "1rem", alignItems: "center" }}
            >
              <Pagination
                count={pageCount}
                page={nPage}
                onChange={(event, value) => {
                  if (!props.isLoading) {
                    let cloneData = { ...props.data, nPageIndex: value };
                    cloneData.arrRows = [];
                    props.onLoadData(cloneData);
                  }
                }}
                color="primary"
                variant="outlined" 
                shape="rounded"
                renderItem={(item) => (
                  <PaginationItem
                    components={{
                      first: pageFirst,
                      previous: pagePrevious,
                      next: pageNext,
                      last: pageLast,
                    }}
                    {...item}
                  />
                )}
                showFirstButton
                showLastButton
              />
            </Stack>
            <Typography
              sx={{
                color: "rgb(131, 146, 171)",
                fontSize: "0.875rem",
                mr: "0.5rem",
                ml: "0.5rem"
              }}
            >
              Page Size
            </Typography>
            <AutoCompletePagination
              small
              sxCustom={{
                ".MuiOutlinedInput-root": {
                  height: "30px",
                  boxShadow: "none",
                  paddingTop: "3px !important",
                  " input": {
                    padding: "0 5px !important",
                  },
                },
              }}
              objValue={{
                label: nPageSize,
                value: nPageSize,
              }}
              funcOnChange={(e, n) => {
                let cloneData = {
                  ...props.data,
                  nPageIndex: 0,
                  nPageSize: n.label,
                };
                props.onLoadData(cloneData);
              }}
              lstOption={lstPageSize}
              disClearable
            />
          </Box>
        </Box>
      </Grid>
    </>
  );
};

export default PaginationCustom;

export const initRowsPagination: PaginationInterface = {
  nPageIndex: 1,
  nPageSize: 6,
  arrRows: [],
  nDataLength: 0,
  sSortExpression: "",
  sSortDirection: "",
  sID: "",
};

const defaultProp: CustomPaginationProp = {
  lstPageSize: [3, 6, 9, 12],
  xs: 12,
  sm: 6,
  md: 4,
  lg: 4,
  onLoadData: () => {},
};
PaginationCustom.defaultProps = defaultProp;