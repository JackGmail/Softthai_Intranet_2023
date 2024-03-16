import { Button, Fab, Theme, Tooltip } from "@mui/material";
import {
  Search,
  ArrowBackIos,
  DeleteForever,
  Add,
  Info,
  Cancel,
  Summarize,
  CloseRounded,
  Home,
  SaveAlt,
  RefreshRounded,
  RemoveRedEye,
  Image,
  Edit,
  Link,
  Delete,
  Print,
  Clear,
  Visibility,
  Undo,
  Save,
  TaskAltRounded,
  UploadFile,
  DoneAll,
  Logout,
  List,
  Task,
} from "@mui/icons-material";
import { styled } from '@mui/material/styles';
import { BtnProp, ButtonCustomProp } from "./Button";
import { MdRecycling, MdOutlineNavigateBefore, MdOutlineNavigateNext } from "react-icons/md";
import { HiOutlineSave } from "react-icons/hi";
import { AiOutlineComment } from "react-icons/ai";
import { FaSyncAlt } from "react-icons/fa";
import { FiSend } from "react-icons/fi";
import { BiEdit, BiReset, BiCollapse, BiExpand } from "react-icons/bi";
import { BsFileEarmarkExcel } from "react-icons/bs";
import TaskAltOutlinedIcon from '@mui/icons-material/TaskAltOutlined';
import { GridActionsCellItem } from "@mui/x-data-grid-pro";
import DescriptionIcon from '@mui/icons-material/Description';
import { blue } from "@mui/material/colors";
import DehazeIcon from '@mui/icons-material/Dehaze';
import { I18n, ParseHtml } from "utilities/utilities";
import { I18NextNs } from "enum/enum";
import EventBusyIcon from '@mui/icons-material/EventBusy';
import { TbCalendarTime } from "react-icons/tb";
import { SiMicrosoftword } from "react-icons/si";
import React from "react";

export const BtnBaseButton = (props: ButtonCustomProp) => {

  const {
    id,
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    startIcon,
    txt = "",
    isCircleWithOutText = false,
    tooltipPlacement = "bottom",
    size = "medium",
    fontColor = "#fff",
    bgcolor = "#4b7902",
    bgcolorHover = "#4b7902",
    isRadius = true,
    isFullwidth = false,
    className = "",
    sx,
  } = props;

  const styleRectangle = txt ? {} : {
    width: "36.5px",
    minWidth: "20px",
    height: "39.5px",
    borderRadius: "8px !important"
  };

  return (
    <Tooltip placement={tooltipPlacement} title={ParseHtml(txt)}>
      {!isCircleWithOutText ?
        <Button
          variant="contained"
          id={id}
          size={size}
          startIcon={startIcon}
          disabled={isDisabled}
          className={className}
          fullWidth={isFullwidth}
          hidden={isHidden}
          onClick={onClick}
          sx={(theme) => ({
            ...sx,
            boxShadow: 'none',
            textTransform: 'none',
            border: '1px solid',
            lineHeight: 1.5,
            color: fontColor,
            backgroundColor: bgcolor,
            borderColor: bgcolor,
            borderRadius: isRadius && txt ? "20px" : "8px",
            ...styleRectangle,
            ":hover": {
              bgcolor: bgcolorHover,
              borderColor: bgcolorHover
            },
            ".Mui-disabled": {
              color: 'rgba(0, 0, 0, 0.26)',
              boxShadow: 'none',
              // backgroundColor: 'rgba(0, 0, 0, 0.12)',
              borderColor: 'rgba(0, 0, 0, 0.12)'
            },
            ".MuiButton-startIcon": txt ? {} : { margin: '0' },
            [theme.breakpoints.down('sm')]: !isFullwidth ? {
              width: "40px",
              minWidth: "40px",
              height: "40px",
              fontSize: "0px",
              lineHeight: "0px",
              "& .MuiButton-startIcon": { margin: 0 }

            }
              : {},
          })}

        >
          {txt}
        </Button>
        :
        <Fab
          id={id}
          className={className}
          onClick={onClick}
          sx={{
            ...sx,
            width: 40,
            height: 40,
            color: fontColor,
            backgroundColor: bgcolor,
            borderColor: bgcolor,
            ":hover": {
              bgcolor: bgcolorHover,
              borderColor: bgcolorHover
            },
            ".MuiSvgIcon-root": {
              color: '#ffffff !important'
            }
          }}
        >
          {startIcon}
        </Fab>
      }
    </Tooltip>
  );
};

export const BtnSave = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = I18n.SetText("Button.Save", I18NextNs.labelComponent),
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={<Save />}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#33a64c"
      bgcolorHover="#33a64c"
      fontColor="#ffffff"
      tooltipPlacement="bottom"

      className={props.className}
      id={props.id}
    />
  );
};

export const BtnApprove = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = I18n.SetText("Button.Approve", I18NextNs.labelComponent),
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={<TaskAltRounded />}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#33a64c"
      bgcolorHover="#33a64c"
      fontColor="#ffffff"
      tooltipPlacement="bottom"

      className={props.className}
      id={props.id}
    />
  );
};

export const BtnConfirm = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = I18n.SetText("Button.Confirm", I18NextNs.labelComponent),
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={<TaskAltOutlinedIcon />}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#1976d2"
      bgcolorHover="#1976d2"
      fontColor="#ffffff"
      tooltipPlacement="bottom"
      className={props.className}
      id={props.id}
    />
  );
};

export const BtnDelete = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = I18n.SetText("Button.Delete", I18NextNs.labelComponent),
    // txt = "ลบ",
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={<DeleteForever />}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#ed3847"
      bgcolorHover="#ed3847"
      fontColor="#ffffff"
      tooltipPlacement="bottom"

      className={props.className}
      id={props.id}
    />
  );
};

export const BtnCancelForm = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = I18n.SetText("Button.Cancel", I18NextNs.labelComponent),
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={<CloseRounded />}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#ed3847"
      bgcolorHover="#ed3847"
      fontColor="#ffffff"
      tooltipPlacement="bottom"

      className={props.className}
      id={props.id}
    />
  );
};

export const BtnPreview = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = I18n.SetText("Button.Preview", I18NextNs.labelComponent),
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={<Info />}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#e39a2d"
      bgcolorHover="#e39a2d"
      fontColor="#ffffff"
      tooltipPlacement="bottom"

      className={props.className}
      id={props.id}
    />
  );
};

export const BtnClose = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = I18n.SetText("Button.Close", I18NextNs.labelComponent),
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={<Cancel />}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#8c98ac"
      bgcolorHover="#8c98ac"
      fontColor="#ffffff"
      tooltipPlacement="bottom"
      className={props.className}
      id={props.id}
    />
  );
};

export const BtnCancel = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = I18n.SetText("Button.Cancel", I18NextNs.labelComponent),
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={<CloseRounded />}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#dc3545"
      bgcolorHover="#dc3545"
      fontColor="#ffffff"
      tooltipPlacement="bottom"

      className={props.className}
      id={props.id}
    />
  );
};


export const BtnBack = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = I18n.SetText("Button.Back", I18NextNs.labelComponent),
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={<ArrowBackIos />}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#888888"
      bgcolorHover="#888888"
      fontColor="#ffffff"
      tooltipPlacement="bottom"
      className={props.className}
      id={props.id}
    />
  );
};


export const BtnAdd = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    // txt = "เพิ่ม",
    txt = I18n.SetText("Button.Add", I18NextNs.labelComponent),
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={<Add />}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#05a6e8"
      bgcolorHover="#05a6e8"
      fontColor="#ffffff"
      tooltipPlacement="bottom"
      className={props.className}
      id={props.id}
    />
  );
};


export const BtnSearch = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = I18n.SetText("Button.Search", I18NextNs.labelComponent),
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={<Search />}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#0070ff"
      bgcolorHover="#0b5ec9"
      fontColor="#ffffff"
      tooltipPlacement="bottom"
      className={props.className}
      id={props.id}
    />
  );
};

export const BtnClear = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = I18n.SetText("Button.Clear", I18NextNs.labelComponent),
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={<RefreshRounded />}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#dcdbdb"
      bgcolorHover="#b7b7b7"
      fontColor="#000000"
      tooltipPlacement="bottom"
      className={props.className}
      id={props.id}
    />
  );
};

export const BtnHome = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = I18n.SetText("Button.Home", I18NextNs.labelComponent),
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={<Home />}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#dcdcdc"
      bgcolorHover="#dcdcdc"
      fontColor="#000000"
      tooltipPlacement="bottom"
      className={props.className}
      id={props.id}
    />
  );
};

export const BtnEdit = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = I18n.SetText("Button.Edit", I18NextNs.labelComponent),
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={<BiEdit />}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#F6C000"
      bgcolorHover="#F6C000"
      fontColor="#ffffff"
      tooltipPlacement="bottom"
      className={props.className}
      id={props.id}
    />
  );
};

export const BtnExportExcel = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = I18n.SetText("Button.Excel", I18NextNs.labelComponent),
    isCircleWithOutText = false,
    isRadius = true,
    icon = <BsFileEarmarkExcel />
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={icon}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#34956d"
      bgcolorHover="#1e855b"
      fontColor="#ffffff"
      tooltipPlacement="bottom"
      className={props.className}
      id={props.id}
    />
  );
};

export const BtnDownload = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = I18n.SetText("Button.Download", I18NextNs.labelComponent),
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={<SaveAlt />}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#1000f3"
      bgcolorHover="#1000f3"
      fontColor="#ffffff"
      tooltipPlacement="bottom"

      className={props.className}
      id={props.id}
    />
  );
};

export const BtnDraft = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = I18n.SetText("Button.Draft", I18NextNs.labelComponent),
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={<HiOutlineSave />}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#3cc1ac"
      bgcolorHover="#239b88"
      fontColor="#ffffff"
      tooltipPlacement="bottom"
      className={props.className}
      id={props.id}
    />
  );
};

export const BtnAdditionnel = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = I18n.SetText("Button.Additionnel", I18NextNs.labelComponent),
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={<List />}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#43619D"
      bgcolorHover="#496397"
      fontColor="#ffffff"
      tooltipPlacement="bottom"

      className={props.className}
      id={props.id}
    />
  );
};

export const BtnRecycle = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = I18n.SetText("Button.Recycle", I18NextNs.labelComponent),
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={<MdRecycling />}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#f59a23"
      bgcolorHover="#e6880c"
      fontColor="#ffffff"
      tooltipPlacement="bottom"
      className={props.className}
      id={props.id}
    />
  );
};

export const BtnNote = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = I18n.SetText("Button.Note", I18NextNs.labelComponent),
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={<Summarize />}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#f59a23"
      bgcolorHover="#e6880c"
      fontColor="#ffffff"
      tooltipPlacement="bottom"
      className={props.className}
      id={props.id}
    />
  );
};

export const BtnSend = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = I18n.SetText("Button.Send", I18NextNs.labelComponent),
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={<FiSend />}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#00aeef"
      bgcolorHover="#0799d0"
      fontColor="#ffffff"
      tooltipPlacement="bottom"
      className={props.className}
      id={props.id}
    />
  );
};

export const BtnRecall = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = I18n.SetText("Button.Recall", I18NextNs.labelComponent),
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={<BiReset />}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#f59b26"
      bgcolorHover="#d98c28"
      fontColor="#ffffff"
      tooltipPlacement="bottom"
      className={props.className}
      id={props.id}
    />
  );
};

export const BtnComment = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = I18n.SetText("Button.Comment", I18NextNs.labelComponent),
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={<AiOutlineComment />}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#f7f7ff"
      bgcolorHover="#dfdfea"
      fontColor="#000000"
      tooltipPlacement="bottom"
      className={props.className}
      id={props.id}
    />
  );
};

export const BtnNext = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = I18n.SetText("Button.Next", I18NextNs.labelComponent),
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={<MdOutlineNavigateNext />}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#05a6e8"
      bgcolorHover="#0c93cb"
      fontColor="#ffffff"
      tooltipPlacement="bottom"
      className={props.className}
      id={props.id}
    />
  );
};

export const BtnPrevious = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = I18n.SetText("Button.Previous", I18NextNs.labelComponent),
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={<MdOutlineNavigateBefore />}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#05a6e8"
      bgcolorHover="#0c93cb"
      fontColor="#ffffff"
      tooltipPlacement="bottom"
      className={props.className}
      id={props.id}
    />
  );
};

export const BtnCollapse = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = I18n.SetText("Button.Collapse", I18NextNs.labelComponent),
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={<BiCollapse />}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#4caf50"
      bgcolorHover="#3d9840"
      fontColor="#ffffff"
      tooltipPlacement="bottom"
      className={props.className}
      id={props.id}
    />
  );
};

export const BtnExpand = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = I18n.SetText("Button.Expand", I18NextNs.labelComponent),
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={<BiExpand />}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#668ecf"
      bgcolorHover="#5379b7"
      fontColor="#ffffff"
      tooltipPlacement="bottom"
      className={props.className}
      id={props.id}
    />
  );
};

export const BtnLoadSync = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = I18n.SetText("Button.LoadSync", I18NextNs.labelComponent),
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={<FaSyncAlt />}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#e3b43f"
      bgcolorHover="#d3a83b"
      fontColor="#ffffff"
      tooltipPlacement="bottom"
      className={props.className}
      id={props.id}
    />
  );
};


export const BtnReject = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = I18n.SetText("Button.Reject", I18NextNs.labelComponent),

    // txt = "Reject",
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={<Undo />}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#f5a323"
      bgcolorHover="#f5a323"
      fontColor="#ffffff"
      tooltipPlacement="bottom"

      className={props.className}
      id={props.id}
    />
  );
};

export const BtnDes = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = I18n.SetText("Button.Description", I18NextNs.labelComponent),
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={<DescriptionIcon />}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#5197ff" bgcolorHover='#0f80d7'
      fontColor="#ffffff"
      tooltipPlacement="bottom"
      className={props.className}
      id={props.id}
    />
  );
};

export const BtnExportExcelTemplate = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = I18n.SetText("Button.Excel", I18NextNs.labelComponent),

    // txt = "Excel",
    isCircleWithOutText = false,
    isRadius = true,
    icon = <DescriptionIcon />
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={icon}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor={blue[500]}
      bgcolorHover={blue[700]}
      fontColor="#ffffff"
      tooltipPlacement="bottom"

      className={props.className}
      id={props.id}
    />
  );
};

export const BtnLogout = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = I18n.SetText("Button.Logout", I18NextNs.labelComponent),

    // txt = "Excel",
    isCircleWithOutText = true,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      txt={txt}
      size={"medium"}
      startIcon={<Logout />}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor={"#ed3847"}
      bgcolorHover={"#ed1325"}
      fontColor="#ffffff"
      tooltipPlacement="bottom"
      className={props.className}
      id={props.id}
    />
  );
};



//#region OnTable
export const BtnAddOnTable = ({
  IsDisabled = false,
  IsHisabled = false,
  txt = I18n.SetText("Button.Add", I18NextNs.labelComponent),
  onClick = () => { },
  id
}) => (
  <Tooltip title={txt}>
    <GridActionsCellItem
      id={id}
      disabled={IsDisabled}
      hidden={IsHisabled}
      icon={<Add />}
      label={txt}
      sx={{
        bgcolor: "#1976d2",
        color: "white",
        ":hover": {
          bgcolor: "#0946a2",
        },
      }}
      onClick={onClick}
      aria-label={txt}
    />
  </Tooltip>
);

export const BtnPreviewOnTable = ({
  IsDisabled = false,
  IsHisabled = false,
  txt = I18n.SetText("Button.Preview", I18NextNs.labelComponent),
  onClick = () => { },
  id
}) => (
  <Tooltip title={txt}>
    <GridActionsCellItem
      id={id}
      disabled={IsDisabled}
      hidden={IsHisabled}
      icon={<RemoveRedEye />}
      label={txt}
      sx={(theme: Theme) => ({
        color: "#fff",
        backgroundColor: theme.palette.info.main,
        borderColor: theme.palette.info.light,
        ":hover": { bgcolor: theme.palette.info.dark },
      })}
      onClick={onClick}
      aria-label={txt}
    />
  </Tooltip>
);

export const BtnLinkOnTable = ({
  IsDisabled = false,
  IsHisabled = false,
  txt = I18n.SetText("Button.Link", I18NextNs.labelComponent),
  onClick = () => { },
  id
}) => (
  <Tooltip title={txt}>
    <GridActionsCellItem
      id={id}
      disabled={IsDisabled}
      hidden={IsHisabled}
      icon={<Link />}
      label={txt}
      sx={(theme: Theme) => ({
        color: "#fff",
        backgroundColor: "#9e3eda",
        borderColor: "#9e3eda",
        ":hover": { bgcolor: "#671d96" },
      })}
      onClick={onClick}
      aria-label={txt}
    />
  </Tooltip>
);

export const BtnViewImageOnTable = ({
  IsDisabled = false,
  IsHisabled = false,
  txt = I18n.SetText("Button.ViewImage", I18NextNs.labelComponent),
  onClick = () => { },
  id
}) => (
  <Tooltip title={txt}>
    <GridActionsCellItem
      id={id}
      disabled={IsDisabled}
      hidden={IsHisabled}
      icon={<Image />}
      label={txt}
      sx={{
        backgroundColor: "tranparent",
        color: "#4f5bef",
        ":hover": { bgcolor: "#d7d7d7" },
      }}
      onClick={onClick}
      aria-label={txt}
    />
  </Tooltip>
)

export const BtnEditOnTable = ({
  IsDisabled = false,
  IsHisabled = false,
  txt = I18n.SetText("Button.Edit", I18NextNs.labelComponent),
  onClick = () => { },
  id
}) => (
  <Tooltip title={txt}>
    <GridActionsCellItem
      id={id}
      disabled={IsDisabled}
      hidden={IsHisabled}
      icon={<Edit />}
      label={txt}
      sx={
        IsHisabled !== true ? {
          backgroundColor: "#ffc107",
          color: "white",
          ":hover": { bgcolor: "#cc9900" },
          margin: "1%",
        }
          :
          {
            display: "none",
            backgroundColor: "#ffc107",
            color: "white",
            ":hover": { bgcolor: "#cc9900" },
            margin: "1%",
          }
      }
      onClick={onClick}
      aria-label={txt}
    />
  </Tooltip>
)

export const BtnViewOnTable = ({
  IsDisabled = false,
  IsHisabled = false,
  txt = I18n.SetText("Button.View", I18NextNs.labelComponent),
  onClick = () => { },
  id
}) => (
  <Tooltip title={txt}>
    <GridActionsCellItem
      id={id}
      disabled={IsDisabled}
      hidden={IsHisabled}
      icon={<Visibility />}
      label={txt}
      sx={{
        backgroundColor: "#6FDBFF",
        color: "white",
        ":hover": { bgcolor: "#5DB6D4" },
        margin: "1%",
      }}
      onClick={onClick}
      aria-label={txt}
    />
  </Tooltip>
)

export const BtnDeleteOnTable = ({
  IsDisabled = false,
  IsHisabled = false,
  txt = I18n.SetText("Button.Delete", I18NextNs.labelComponent),
  onClick = () => { },
  id
}) => (
  <Tooltip title={txt}>
    <GridActionsCellItem
      id={id}
      disabled={IsDisabled}
      hidden={IsHisabled}
      icon={<Delete />}
      label={txt}
      sx={{
        backgroundColor: "#f30800",
        color: "white",
        ":hover": { bgcolor: "#d8352f" },
        margin: "1%",
      }}
      onClick={onClick}
      aria-label={txt}
    />
  </Tooltip>
)

export const BtnSearchOnTable = ({
  IsDisabled = false,
  IsHisabled = false,
  txt = I18n.SetText("Button.Search", I18NextNs.labelComponent),
  onClick = () => { },
  id
}) => (
  <Tooltip title={txt}>
    <GridActionsCellItem
      id={id}
      disabled={IsDisabled}
      hidden={IsHisabled}
      icon={<Search />}
      label={txt}
      sx={{
        backgroundColor: "#096ddd",
        color: "white", //black
        ":hover": { bgcolor: "#004290" },
      }}
      onClick={onClick}
      aria-label={txt}
    />
  </Tooltip>
)

export const BtnPrintOnTable = ({
  IsDisabled = false,
  IsHisabled = false,
  txt = I18n.SetText("Button.Print", I18NextNs.labelComponent),
  onClick = () => { },
  id
}) => (
  <Tooltip title={txt}>
    <GridActionsCellItem
      id={id}
      disabled={IsDisabled}
      hidden={IsHisabled}
      icon={<Print />}
      label={txt}
      sx={{
        backgroundColor: "#096ddd",
        color: "orange", //black
        ":hover": { bgcolor: "#004290", cursor: "pointer" },
      }}
      onClick={onClick}
      aria-label={txt}
    />
  </Tooltip>
)

export const BtnReserveOnTable = ({
  IsDisabled = false,
  IsHisabled = false,
  txt = I18n.SetText("Button.Reserve", I18NextNs.labelComponent),
  onClick = () => { },
  id
}) => (
  <Tooltip title={txt}>
    <GridActionsCellItem
      id={id}
      disabled={IsDisabled}
      hidden={IsHisabled}
      icon={<Edit />}
      label={txt}
      sx={{
        backgroundColor: "#1000f3",
        color: "white", //black
        ":hover": { bgcolor: "#2d22b9" },
      }}
      onClick={onClick}
      aria-label={txt}
    />
  </Tooltip>
)

export const BtnDownloadOnTable = ({
  IsDisabled = false,
  IsHisabled = false,
  txt = I18n.SetText("Button.Download", I18NextNs.labelComponent),
  onClick = () => { },
  id
}) => (
  <Tooltip title={txt}>
    <GridActionsCellItem
      id={id}
      disabled={IsDisabled}
      hidden={IsHisabled}
      icon={<SaveAlt />}
      label={txt}
      sx={{
        backgroundColor: "#4db9cf",
        color: "white", //black
        ":hover": { bgcolor: "#4db9cf" },
        margin: "1%",
      }}
      onClick={onClick}
      aria-label={txt}
    />
  </Tooltip>
)

export const BtnClearOnTable = ({
  IsDisabled = false,
  IsHisabled = false,
  txt = I18n.SetText("Button.Clear", I18NextNs.labelComponent),

  // txt = "ล้าง",
  onClick = () => { },
  id
}) => (
  <Tooltip title={txt}>
    <GridActionsCellItem
      id={id}
      disabled={IsDisabled}
      hidden={IsHisabled}
      icon={<Clear />}
      label={txt}
      sx={{
        backgroundColor: "#f32400",
        color: "white",
        ":hover": { bgcolor: "#f32400" },
        margin: "1%",
      }}
      onClick={onClick}
      aria-label={txt}
    />
  </Tooltip>
)


export const BtnSubOnTable = ({
  IsDisabled = false,
  IsHisabled = false,
  txt = I18n.SetText("Button.SubDetail", I18NextNs.labelComponent),

  // txt = "ดูข้อมูล",
  onClick = () => { },
  id
}) => (
  <Tooltip title={txt}>
    <GridActionsCellItem
      id={id}
      disabled={IsDisabled}
      hidden={IsHisabled}
      icon={<DehazeIcon />}
      label={txt}
      sx={{
        color: "#1976d2",
        ":hover": {
          bgcolor: "#0946a2",
        },
      }}
      onClick={onClick}
      aria-label={txt}
    />
  </Tooltip>
);

export const BtnViewDataOnTable = ({
  IsDisabled = false,
  IsHisabled = false,
  txt = I18n.SetText("Button.View", I18NextNs.labelComponent),
  onClick = () => { },
  id
}) => (
  <Tooltip title={txt}>
    <GridActionsCellItem
      id={id}
      disabled={IsDisabled}
      hidden={IsHisabled}
      icon={<DescriptionIcon />}
      label={txt}
      sx={
        IsHisabled !== true ? {
          backgroundColor: "#1976d2",
          color: "white",
          ":hover": { bgcolor: "#0946a2" },
          margin: "1%",
        }
          :
          {
            display: "none",
            backgroundColor: "#1976d2",
            color: "white",
            ":hover": { bgcolor: "#0946a2" },
            margin: "1%",
          }
      }
      onClick={onClick}
      aria-label={txt}
    />
  </Tooltip>
);

export const BtnUpdateDataOnTable = ({
  IsDisabled = false,
  IsHisabled = false,
  txt = I18n.SetText("Button.UpdateData", I18NextNs.labelComponent),
  onClick = () => { },
  id
}) => (
  <Tooltip title={txt}>
    <GridActionsCellItem
      id={id}
      disabled={IsDisabled}
      hidden={IsHisabled}
      icon={<UploadFile />}
      label={txt}
      sx={
        IsHisabled !== true ? {
          backgroundColor: "#A569BD",
          color: "white",
          ":hover": { bgcolor: "#7D3C98" },
          margin: "1%",
        }
          :
          {
            display: "none",
            backgroundColor: "#A569BD",
            color: "white",
            ":hover": { bgcolor: "#7D3C98" },
            margin: "1%",
          }
      }
      onClick={onClick}
      aria-label={txt}
    />
  </Tooltip>
);

export const BtnApproveOnTable = ({
  IsDisabled = false,
  IsHisabled = false,
  txt = I18n.SetText("Button.Approve", I18NextNs.labelComponent),
  onClick = () => { },
  id
}) => (
  <Tooltip title={txt}>
    <GridActionsCellItem
      id={id}
      disabled={IsDisabled}
      hidden={IsHisabled}
      icon={<DoneAll />}
      label={txt}
      //เขียว
      sx={
        IsHisabled !== true ? {
          backgroundColor: "#58D68D",
          color: "white",
          ":hover": { bgcolor: "#2ECC71" },
          margin: "1%",
        }
          :
          {
            display: "none",
            backgroundColor: "#58D68D",
            color: "white",
            ":hover": { bgcolor: "#2ECC71" },
            margin: "1%",
          }
      }
      onClick={onClick}
      aria-label={txt}
    />
  </Tooltip>
);
export const BottonBooking = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = "Booking",
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      id={props.id}
      txt={txt}
      size={"medium"}
      startIcon={<TbCalendarTime style={{ fontSize: "1.5rem" }} />}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#0474ae"
      bgcolorHover="#3daae3"
      fontColor="#ffffff"
      tooltipPlacement="bottom"
    />
  );
};

export const BottonCreateBook = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = "สร้างห้องประชุม",
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      id={props.id}
      txt={txt}
      startIcon={<Add />}
      size={"medium"}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#1976d2"
      bgcolorHover="#0946a2"
      fontColor="#ffffff"
      tooltipPlacement="bottom"
    />
  );
};

export const BottonUnavailable = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = "Unavailable",
    isCircleWithOutText = false,
    isRadius = true,
  } = props;

  return (
    <BtnBaseButton
      id={props.id}
      txt={txt}
      size={"medium"}
      isRadius={isRadius}
      startIcon={<EventBusyIcon />}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#ed3847"
      bgcolorHover="rgb(220, 53, 69)"
      fontColor="#ffffff"
      tooltipPlacement="bottom"
    />
  );
};

export const BtnClearFilter = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = "Clear Filter",
    isCircleWithOutText = false,
    isRadius = true,
  } = props;
  return (<BtnBaseButton
    id={props.id}
    txt={txt}
    size={"medium"}
    isRadius={isRadius}
    startIcon={<Clear />}
    isDisabled={isDisabled}
    isHidden={isHidden}
    onClick={onClick}
    isCircleWithOutText={isCircleWithOutText}
    bgcolor="#dcdbdb"
    bgcolorHover="#dcdbdb"
    fontColor="#ffffff"
    tooltipPlacement="bottom"
  />
  );
};

export const BtnBooking = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = "Booking",
    isCircleWithOutText = false,
    isRadius = true,
  } = props;
  return (<BtnBaseButton
    id={props.id}
    txt={txt}
    size={"medium"}
    isRadius={isRadius}
    isDisabled={isDisabled}
    isHidden={isHidden}
    onClick={onClick}
    isCircleWithOutText={isCircleWithOutText}
    bgcolor="#f1a43f"
    bgcolorHover="#e0932d"
    fontColor="#ffffff"
    tooltipPlacement="bottom"
  />
  );
};


export const BtnCreateBook = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = "Create Book",
    isCircleWithOutText = false,
    isRadius = true,
  } = props;
  return (<BtnBaseButton
    id={props.id}
    txt={txt}
    size={"medium"}
    isRadius={isRadius}
    isDisabled={isDisabled}
    isHidden={isHidden}
    onClick={onClick}
    isCircleWithOutText={isCircleWithOutText}
    bgcolor="#f1a43f"
    bgcolorHover="#e0932d"
    fontColor="#ffffff"
    tooltipPlacement="bottom"
  />
  );
};

export const BtnCustomIcon = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = "Custom",
    isCircleWithOutText = false,
    isRadius = true,
  } = props;
  return (<BtnBaseButton
    id={props.id}
    txt={txt}
    size={"medium"}
    isRadius={isRadius}
    isDisabled={isDisabled}
    isHidden={isHidden}
    onClick={onClick}
    isCircleWithOutText={isCircleWithOutText}
    bgcolor="#f1a43f"
    bgcolorHover="#e0932d"
    fontColor="#ffffff"
    tooltipPlacement="bottom"
  />
  );
};

export const BtnExportWord = (props: BtnProp) => {
  const {
    isDisabled = false,
    isHidden = false,
    onClick = () => { },
    txt = "Word",
    isCircleWithOutText = false,
    isRadius = true,
    icon = <SiMicrosoftword />
  } = props;

  return (
    <BtnBaseButton
      id={props.id}
      txt={txt}
      size={"medium"}
      startIcon={icon}
      isRadius={isRadius}
      isDisabled={isDisabled}
      isHidden={isHidden}
      onClick={onClick}
      isCircleWithOutText={isCircleWithOutText}
      bgcolor="#1000f3"
      bgcolorHover="#1000f3"
      fontColor="#ffffff"
      tooltipPlacement="bottom"
    />
  );
};

export const BtnUploadOnTable = ({
  id,
  IsDisabled = false,
  IsHisabled = false,
  txt = "Upload",
  onClick = () => { },
}) => (
  <GridActionsCellItem
    id={id}
    disabled={IsDisabled}
    hidden={IsHisabled}
    icon={<Tooltip title={txt}><UploadFile /></Tooltip>}
    label={txt}
    sx={{
      backgroundColor: "#4db9cf",
      color: "white", //black
      ":hover": { bgcolor: "#4db9cf" },
      margin: "1%",
    }}
    onClick={onClick}
    aria-label={txt}
  />
)

export const BtnTask = ({
  IsDisabled = false,
  IsHisabled = false,
  txt = I18n.SetText("Button.Task", I18NextNs.labelComponent),
  onClick = () => { },
  id
}) => (
  <Tooltip title={txt}>
    <GridActionsCellItem
      id={id}
      disabled={IsDisabled}
      hidden={IsHisabled}
      icon={<Task />}
      label={txt}
      sx={
        IsHisabled !== true ? {
          backgroundColor: "#33a64c",
          color: "white",
          ":hover": { bgcolor: "#33a64c" },
          margin: "1%",
        }
          :
          {
            display: "none",
            backgroundColor: "#33a64c",
            color: "white",
            ":hover": { bgcolor: "#33a64c" },
            margin: "1%",
          }
      }
      onClick={onClick}
      aria-label={txt}
    />
  </Tooltip>
);

//#endregion
