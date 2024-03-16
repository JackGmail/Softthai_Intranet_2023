import React, { Fragment, useEffect } from "react";
import { useSelector } from "react-redux";
import AuthenSelectors from "store/selectors/BlockUISelectoe";
import Backdrop from "@mui/material/Backdrop";
import CircularProgress from "@mui/material/CircularProgress";
export default function BlockUI(props) {
  const OpenBackDrop = useSelector(AuthenSelectors.IsOpen);
  const HandleClose = useSelector(AuthenSelectors.HandleClose);

  return (
    <Backdrop
      sx={{
        color: "#fff",
        zIndex: (theme) => 9999,
        ...(OpenBackDrop ? {} : { display: "none" }),
      }} //theme.zIndex.drawer + 1,
      open={OpenBackDrop}
      onClick={HandleClose}
    >
      <CircularProgress color="inherit" />
    </Backdrop>
  );
}
