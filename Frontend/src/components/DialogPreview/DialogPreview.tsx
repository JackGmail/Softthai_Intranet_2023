import React from "react";
import {
  Box,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Divider,
  IconButton,
  InputAdornment,
  styled,
} from "@mui/material";
import { DialogTitleProps, propPopUpCustom } from "./DialogPreviewClass";

// icon x
import CloseIcon from "@mui/icons-material/Close";
import { BtnBack, BtnSave } from "components/Button";

const BootstrapDialog = styled(Dialog)(({ theme }) => ({
  "& .MuiDialogContent-root": {
    padding: theme.spacing(2),
  },
  "& .MuiDialogActions-root": {
    padding: theme.spacing(1),
  },
  "& .MuiDialog-paper": {
    borderRadius: "20px",
  },
}));


function BootstrapDialogTitle(props: DialogTitleProps) {
  const { children, color, colorbg,CloseColor, onClose, ...other } = props;

  return (
    <DialogTitle sx={{ m: 0, p: 1, backgroundColor: colorbg, color: color, paddingRight: '10%', }} {...other}>
      {children}
      {onClose ? (
        <IconButton
          id='close-dialog'
          aria-label="close"
          onClick={onClose}
          sx={{
            position: 'absolute',
            right: 8,
            top: 8,
            padding: "4px",
            // color:"#ffffff"
            color: CloseColor ??  "#ffffff" ,
            ":hover": { backgroundColor: CloseColor ? "#e0e0e0" : "#908a8a"}
          }}
        >
          <CloseIcon />
        </IconButton>
      ) : null}
    </DialogTitle>
  );
}

const DialogPreview: React.FC<propPopUpCustom> = ({
  IsOpen,
  setIsOpen,
  onClose,
  sMaxWidth = "md",
  Title,
  children,
  JsxDialogAction,
  bgColor,
  Color,
  onClick,
  Close = false,
  CloseSave = false,
  hiddenTitle = true,
  IsBackdropClick,
  startAdornment,
  styles,
  required = false,
  onCustomButton,
  CloseColor
}) => {
  const handleClose = (event, reason) => {
    if (!IsBackdropClick) {
      if (reason !== "backdropClick") {
        onClose();
      }
    } else {
      onClose();
    }
  };
  return (
    <BootstrapDialog
      open={IsOpen}
      onClose={handleClose}
      aria-labelledby="alert-dialog-title"
      aria-describedby="alert-dialog-description"
      fullWidth={true}
      maxWidth={sMaxWidth}
      style={{
        top: "5%",
      }}
    >
       <BootstrapDialogTitle id="customized-dialog-title" onClose={onClose} color={Color} colorbg={bgColor} CloseColor={CloseColor}>     
        {Title}
      </BootstrapDialogTitle>
      <DialogContent sx={{pt:'16px !important'}} style={styles}>{children}</DialogContent>
      {JsxDialogAction
        &&
        <><Divider></Divider><DialogActions sx={{ justifyContent: "center", gap: 2 }}>
          {Close &&
            <BtnBack id={"BtnBack"} txt='Back' onClick={onClose} />}
          {CloseSave &&
            <BtnSave id={"BtnSave"} onClick={onClick} />}
          {onCustomButton}
        </DialogActions></>
      }
    </BootstrapDialog>
  );
};

export default DialogPreview;