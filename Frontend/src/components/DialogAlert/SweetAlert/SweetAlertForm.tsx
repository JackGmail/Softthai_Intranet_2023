import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import DialogTitle from "@mui/material/DialogTitle";
import { useTheme } from "@mui/material/styles";
import useMediaQuery from "@mui/material/useMediaQuery";
import CloseIcon from "@mui/icons-material/Close";
import ConfirmBtnIcon from "@mui/icons-material/Done";
import LinearProgress from "@mui/material/LinearProgress";
import Button from "@mui/material/Button";
import WarningIcon from "@mui/icons-material/ErrorOutline";
import { DialogActionCreators } from "store/redux/DialogAlert";
import { useDispatch } from "react-redux";
import { SxProps } from "@mui/material";
import { styled } from "@mui/material/styles";
import MuiAlert, { AlertProps } from "@mui/material/Alert";
import React, { useEffect, useState } from "react";
import { CircularProgress } from "@mui/material";
import Swal from "sweetalert2";
import { I18n } from "utilities/utilities";
import { I18NextNs } from "enum/enum";
const i18nDialog = "dialog";

const customstyle = {
    DialogTitle: {
        height: "40%",
        backgroundColor: "#28a745",
        color: "#fff",
        textAlign: "center",
    },
    DialogTitleError: {
        height: "40%",
        backgroundColor: "#ed3847",
        color: "#fff",
        textAlign: "center",
    },
    DialogTitleWarning: {
        height: "40%",
        backgroundColor: "#ffce3d",
        color: "#fff",
        textAlign: "center",
    },
    DialogTitleSubmit: {
        height: "40%",
        backgroundColor: "#163172",
        color: "#333333",
        textAlign: "center",
    },
    DialogContent: {
        height: "50%",
        backgroundColor: "#fff",
        color: "#000",
        padding: "10px 15px",
    },
    DialogAction: {
        backgroundColor: "#fff",
        color: "#000",
        justifyContent: "center !important",
    },
    DialogActionConfirm: {
        backgroundColor: "#fff",
        color: "#000",
        justifyContent: "center !important",
        height: "70px",
    },
    SuccessColor: {
        color: "#fff",
        backgroundColor: "#28a745",
        borderColor: "#28a745",
        "&:hover": {
            backgroundColor: "#28a745",
        },
    },
    ButtonDialog: {
        background: "rgb(250, 250, 250)",
        color: "#000",
        margin: "8px !important",
        width: "auto !important",
        fontSize: "1rem",
        borderRadius: "2em !important",
        padding: "4px 15px !important",
        textTransform: "none",
    },
};

const DialogStyle: SxProps = {
    ".MuiPaper-root": {
        width: "300px !important",
        height: "auto",
        minHeight: "45% !important",
        borderRadius: "20px !important",
    },
};

const BorderLinearProgress = styled(LinearProgress)(({ theme }) => ({
    root: {
        height: 10,
        borderRadius: 5,
    },
    colorPrimary: {
        backgroundColor: theme.palette.grey[200],
    },
    bar: {
        borderRadius: 5,
        backgroundColor: "#28a745",
    },
}));

const Alert = React.forwardRef<HTMLDivElement, AlertProps>(function Alert(
    props,
    ref
) {
    return <MuiAlert elevation={6} ref={ref} {...props} />;
});

export const DialogSuccess = (props: { handleClick; open; sMsg?}) => {
    const { open, sMsg } = props;

    Swal.fire({
        title: I18n.SetText(`Title.Success`, I18NextNs.validation),
        text: sMsg ? sMsg : I18n.SetText(`Message.SaveComplete`, I18NextNs.validation),
        icon: "success",
        confirmButtonText: I18n.SetText(`okText`),
    });

    return (<></>);
};

export const DialogError = (props: { handleClick; open; sMsg?}) => {
    const { open, sMsg } = props;

    Swal.fire({
        title: I18n.SetText(`Title.Error`, I18NextNs.validation),
        text: sMsg ? sMsg : I18n.SetText(`Message.Error`, I18NextNs.validation),
        icon: "error",
        confirmButtonText: I18n.SetText(`okText`),
    });

    return (<></>);
};

export const DialogWarning = (props: { handleClick; open; sMsg?}) => {
    const dispatch = useDispatch() as any;
    const { handleClick, open, sMsg } = props;

    Swal.fire({
        title: I18n.SetText(`Title.Warning`, I18NextNs.validation),
        text: sMsg ? sMsg : I18n.SetText(`Message.Warning`, I18NextNs.validation),
        icon: "warning",
        confirmButtonText: I18n.SetText(`okText`),
    });

    return (<></>);
};

export const DialogSubmit = (props: {
    open;
    sMsg?;
    handleClickSubmit;
    IsLoad;
    handleCancel;
}) => {
    const dispatch = useDispatch() as any;
    const { open, sMsg, handleClickSubmit, IsLoad, handleCancel } = props;

    Swal.fire({
        title: I18n.SetText(`${i18nDialog}.DialogConfirm`),
        text: sMsg ? sMsg : I18n.SetText(`${i18nDialog}.DialogConfirm`),
        icon: "info",
        focusConfirm: false,
        focusCancel: false,
        showCancelButton: true,
        confirmButtonText: I18n.SetText(`msgConfirm`),
        cancelButtonText: I18n.SetText(`${i18nDialog}.CloseButt`)
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire({
                title: I18n.SetText(`Title.Confirm`, I18NextNs.validation),
                text: sMsg ? sMsg : I18n.SetText(`Message.ConfirmSave`, I18NextNs.validation),
                icon: "success",
                confirmButtonText: I18n.SetText(`okText`),
            });
        } else if (result.isDenied) {
            Swal.fire({
                title: I18n.SetText(`Title.Error`, I18NextNs.validation),
                text: sMsg ? sMsg : I18n.SetText(`Message.Error`, I18NextNs.validation),
                icon: "error",
                confirmButtonText: I18n.SetText(`okText`),
            });
        }
    });

    return (<></>);
};

export const DialogSubmitWARNNING = (props: {
    open;
    sMsg?;
    handleClickSubmit;
    handleClickClose;
    IsLoad;
}) => {
    const dispatch = useDispatch() as any;
    const { open, sMsg, handleClickSubmit, IsLoad, handleClickClose } = props;
    const theme = useTheme();
    const fullScreen = useMediaQuery(theme.breakpoints.down("sm"));
    const [isClick, setisClick] = useState(false as boolean);

    const handleCkick = (e) => {
        setisClick(true);
        if (handleClickSubmit) handleClickSubmit(e);
    };

    useEffect(() => {
        if (!open) {
            setTimeout(() => {
                setisClick(false);
            }, 1000);
        }
    }, [open]);

    return (
        <Dialog fullScreen={fullScreen} open={open ? open : false} sx={DialogStyle}>
            <DialogTitle sx={{ ...customstyle.DialogTitleWarning }}>
                <WarningIcon
                    style={{
                        fontSize: "10rem",
                        fontWeight: "bold",
                        color: "white",
                    }}
                />
            </DialogTitle>
            <DialogContent sx={{ ...customstyle.DialogContent }}>
                <DialogContentText
                    style={{
                        fontSize: "1.5rem",
                        fontWeight: "bold",
                        color: "#000",
                    }}
                >
                    {I18n.SetText(`Title.Confirm`, I18NextNs.validation)}
                </DialogContentText>
                <DialogContentText
                    style={{
                        color: "#000",
                        whiteSpace: "pre-wrap",
                    }}
                >
                    {sMsg ? sMsg : I18n.SetText(`Message.ConfirmSave`, I18NextNs.validation)}
                </DialogContentText>
            </DialogContent>
            <DialogActions sx={{ ...customstyle.DialogActionConfirm }}>
                <div
                    style={{
                        display: IsLoad ? "inherit" : "none",
                    }}
                >
                    <BorderLinearProgress />
                </div>

                <div
                    style={{
                        display: !IsLoad ? "flex" : "none",
                        justifyContent: "center",
                    }}
                >
                    <Button
                        variant="contained"
                        sx={{ ...customstyle.ButtonDialog }}
                        style={{
                            background: isClick ? "#B6B6B6" : "#28a745",
                            color: "white",
                            borderRadius: "20px",
                        }}
                        size="small"
                        startIcon={
                            isClick ? (
                                <CircularProgress
                                    sx={{ color: "white" }}
                                    size={"1.2rem"}
                                    thickness={5}
                                />
                            ) : (
                                <ConfirmBtnIcon
                                    style={{
                                        fontSize: "1.5rem",
                                    }}
                                />
                            )
                        }
                        onClick={handleCkick}
                    >
                        Confirm
                        {/* {I18n.SetText(`${i18nDialog}.DialogConfirm`)} */}
                    </Button>
                    <Button
                        variant="contained"
                        sx={{ ...customstyle.ButtonDialog }}
                        style={{
                            color: "#fff",
                            backgroundColor: "#B6B6B6",
                            borderRadius: "20px",
                        }}
                        size="small"
                        startIcon={
                            <CloseIcon
                                style={{
                                    fontSize: "1.5rem",
                                }}
                            />
                        }
                        onClick={(c) => {
                            if (handleClickClose) handleClickClose();
                            dispatch(DialogActionCreators.CloseDialogSubmitWarning());
                        }}
                    >
                        Close
                        {/* {I18n.SetText(`${i18nDialog}.CloseButt`)} */}
                    </Button>
                </div>
            </DialogActions>
        </Dialog>
    );
};