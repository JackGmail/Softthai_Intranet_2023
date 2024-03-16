import React from "react";
import {
  Card,
  Grid,
  CardMedia,
  Typography,
  Chip,
  Divider,
  useMediaQuery,
} from "@mui/material";
import HistoryLogProp from "./HistoryLogProps";
import defaultAvatar from "assets/images/NoImage/default-avatar.png";
import theme from "theme/themes";

export default function HistoryLog(props: HistoryLogProp) {
  const IsSM = useMediaQuery(theme.breakpoints.down("sm"));

  const onImageError = (e) => {
    e.target.src = defaultAvatar;
  };

  return (
    <>
      <Card
        sx={{
          minHeight: 180,
          border: "1px solid white",
          borderRadius: "15px",
          padding: "10px 0 10px 0",
          boxShadow:
            " 0px 1px 2px 0px,rgba(60, 64, 67, 0.3) 0px 2px 6px 2px, rgba(60, 64, 67, 0.15)",
          "&:hover": {
            border: "1px solid #3E97FF",
          },
        }}
      >
        <Grid
          container
          spacing={1}
          padding={"10px"}
          justifyContent={"space-evenly"}
        >
          <Grid item xs={12} sm={6} md={2}>
            <img
              src={
                props.imgAvatar
                  ? `${process.env.REACT_APP_API_URL}${props.imgAvatar}`
                  : defaultAvatar
              }
              alt="AvatarImage"
              onError={onImageError}
              style={{
                display: "flex",
                border: "solid white",
                width: "100px",
                height: "100px",
                borderRadius: "50%",
                margin: "auto",
              }}
            />
          </Grid>
          <Grid item xs={12} sm={6} md={8}>
            <Typography
              variant="body2"
              sx={{ fontSize: IsSM ? "12px" : "16px" }}
            >
              <b>Name</b> : {props.sName}
            </Typography>
            <Typography
              variant="body2"
              sx={{ fontSize: IsSM ? "12px" : "16px" }}
            >
              <b>Position</b> : {props.sPosition}
            </Typography>
            <Typography
              variant="body2"
              sx={{ fontSize: IsSM ? "12px" : "16px" }}
            >
              <b>Comment</b> : {props.sComment}
            </Typography>
            <Typography
              variant="body2"
              sx={{ fontSize: IsSM ? "12px" : "16px", marginBottom: "10px" }}
            >
              <b>Status</b> : {props.sStatus}
            </Typography>
            <Divider />
            <Typography
              variant="body2"
              sx={{ fontSize: IsSM ? "12px" : "16px", marginTop: "10px" }}
            >
              <b>Action Date</b> : {props.sActionDate}
            </Typography>
          </Grid>
          <Grid item xs={"auto"}>
            {props.sActionMode === "Draft" ? (
              <Chip label={props.sActionMode} color="info" />
            ) : props.sActionMode === "Submit" ? (
              <Chip label={props.sActionMode} color="primary" />
            ) : props.sActionMode === "Save Result" ? (
              <Chip label={props.sActionMode} color="primary" />
            ) : props.sActionMode === "Approve" ? (
              <Chip label={props.sActionMode} color="success" />
            ) : props.sActionMode === "Reject" ? (
              <Chip label={props.sActionMode} color="warning" />
            ) : props.sActionMode === "Cancel" ? (
              <Chip label={props.sActionMode} color="error" />
            ) : props.sActionMode === "Recall" ? (
              <Chip label={props.sActionMode} color="warning" />
            ) : props.sActionMode === "Completed" ? (
              <Chip label={props.sActionMode} color="success" />
            ) : props.sActionMode === "Closed" ? (
              <Chip label={props.sActionMode} color="success" />
            ) : (
              <></>
            )}
          </Grid>
        </Grid>
      </Card>
    </>
  );
}
