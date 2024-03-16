import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import DialogTitle from "@mui/material/DialogTitle";
import { useTheme } from "@mui/material/styles";
import useMediaQuery from "@mui/material/useMediaQuery";
import CloseIcon from "@mui/icons-material/Close";
import ConfirmIcon from "@mui/icons-material/HelpOutline";
import ConfirmBtnIcon from "@mui/icons-material/Done";
import LinearProgress from "@mui/material/LinearProgress";
import Button from "@mui/material/Button";
import WarningIcon from "@mui/icons-material/ErrorOutline";
import { DialogActionCreators } from "store/redux/DialogAlert";
import { useDispatch } from "react-redux";
import { SxProps, Theme } from "@mui/material";
import { styled } from "@mui/material/styles";
import Snackbar from "@mui/material/Snackbar";
import MuiAlert, { AlertProps } from "@mui/material/Alert";
import React, { useEffect, useState } from "react";
import IconButton from "@mui/material/IconButton";
import { CircularProgress } from "@mui/material";
import { I18n } from "utilities/utilities";
import { I18NextNs } from "enum/enum";

const nCloseTimeout = 6000;

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

const useStyles = (() => ({
    paper: {
        width: "400px !important",
        height: "auto",
        minHeight: "55% !important",
        borderRadius: "20px !important"
    },
    DialogPaperTemp: {
        height: "70%",
        width: "400px",
        borderRadius: "20px",
    },
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
}));

const DialogStyleNoMsgSpace: SxProps = {
    ".MuiPaper-root": {
        width: "300px !important",
        height: "auto",
        minHeight: "45% !important",
        borderRadius: "20px !important",
    },
};

const DialogStyle: SxProps = {
    ".MuiPaper-root": {
        width: "300px !important",
        height: "auto",
        minHeight: "50% !important",
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

export const DialogSuccess = (props: { handleClick; open; sMsg?; }) => {
    const dispatch = useDispatch() as any;
    const { open, sMsg, } = props;
    const handleClose = (
        event?: React.SyntheticEvent | Event,
        reason?: string
    ) => {
        if (reason === "clickaway") {
            return;
        }
        dispatch(DialogActionCreators.CloseDialogSuccess());
    };
    const action = (
        <React.Fragment>
            <IconButton
                size="small"
                aria-label="close"
                color="inherit"
                onClick={handleClose}
            >
                <CloseIcon fontSize="small" />
            </IconButton>
        </React.Fragment>
    );
    return (
        <Snackbar
            open={open}
            autoHideDuration={3000}
            onClose={handleClose}
            anchorOrigin={{ vertical: "top", horizontal: "center" }}
        >
            <Alert
                variant="filled"
                severity="success"
                sx={{ width: "100%" }}
                onClose={handleClose}
                action={action}
            >
                <strong> {I18n.SetText(`Title.Success`, I18NextNs.labelComponent)} - </strong>
                {sMsg ? sMsg : I18n.SetText(`Message.SaveComplete`, I18NextNs.labelComponent)}
            </Alert>
        </Snackbar>
    );
};
export const DialogError = (props: { handleClick; open; sMsg?; }) => {
    const dispatch = useDispatch() as any;
    const { open, sMsg } = props;
    const handleClose = (
        event?: React.SyntheticEvent | Event,
        reason?: string
    ) => {
        if (reason === "clickaway") {
            return;
        }
        dispatch(DialogActionCreators.CloseDialogError());
    };
    const action = (
        <React.Fragment>
            <IconButton
                size="small"
                aria-label="close"
                color="inherit"
                onClick={handleClose}
            >
                <CloseIcon fontSize="small" />
            </IconButton>
        </React.Fragment>
    );
    return (
        <Snackbar
            open={open}
            autoHideDuration={nCloseTimeout}
            onClose={handleClose}
            anchorOrigin={{ vertical: "top", horizontal: "center" }}
            action={action}
        >
            <Alert
                variant="filled"
                severity="error"
                sx={{ width: "100%" }}
                onClose={handleClose}
            >
                <strong> {I18n.SetText(`Title.Error`, I18NextNs.labelComponent)} - </strong>
                {sMsg ? sMsg : I18n.SetText(`Message.Error`, I18NextNs.labelComponent)}
            </Alert>
        </Snackbar>
    );
};
export const DialogWarning = (props: { handleClick; open; sMsg?; }) => {
    const dispatch = useDispatch() as any;
    const { handleClick, open, sMsg } = props;
    const handleClose = (
        event?: React.SyntheticEvent | Event,
        reason?: string
    ) => {
        if (reason === "clickaway") {
            return;
        }
        dispatch(DialogActionCreators.CloseDialogWarning());
        handleClick();
    };
    const action = (
        <React.Fragment>
            <IconButton
                size="small"
                aria-label="close"
                color="inherit"
                onClick={handleClose}
            >
                <CloseIcon fontSize="small" />
            </IconButton>
        </React.Fragment>
    );
    return (
        <Snackbar
            open={open}
            autoHideDuration={nCloseTimeout}
            onClose={handleClose}
            anchorOrigin={{ vertical: "top", horizontal: "center" }}
            action={action}
        >
            <Alert
                variant="filled"
                severity="warning"
                sx={{ width: "100%" }}
                onClose={handleClose}
            >
                <strong> {I18n.SetText(`Title.Warning`, I18NextNs.labelComponent)} - </strong>
                {sMsg ? sMsg : I18n.SetText(`Message.Warning`, I18NextNs.labelComponent)}
            </Alert>
        </Snackbar>
    );
};
export const DialogSubmit = (props: {
    open;
    sMsg?;
    handleClickSubmit;
    IsLoad;
    handleCancel;
    fullScreenConfig?;
    MsgSpaceConfig?;
}) => {
    const dispatch = useDispatch() as any;
    const { open, sMsg, handleClickSubmit, IsLoad, handleCancel, fullScreenConfig, MsgSpaceConfig } = props;
    const theme = useTheme();
    const FullScreen = useMediaQuery(theme.breakpoints.down("md"));
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
        <Dialog fullScreen={fullScreenConfig ? FullScreen ? true : false : false} open={open} sx={MsgSpaceConfig ? DialogStyleNoMsgSpace : fullScreenConfig ? FullScreen ? useStyles : DialogStyle : DialogStyle}>
            <DialogTitle sx={{ ...customstyle.DialogTitleSubmit }}>
                <ConfirmIcon
                    style={{
                        fontSize: "10rem",
                        fontWeight: "bold",
                        color: "#fff",
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
                    {I18n.SetText(`Title.Confirm`, I18NextNs.labelComponent)}
                </DialogContentText>
                <DialogContentText
                    style={{
                        color: "#000",
                        whiteSpace: "pre-wrap",
                    }}
                >
                    {sMsg ? sMsg : I18n.SetText(`Message.ConfirmSave`, I18NextNs.labelComponent)}
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
                        disabled={isClick}
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
                            handleCancel();
                            dispatch(DialogActionCreators.CloseDialogSubmit());
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
                    {I18n.SetText(`Title.Confirm`, I18NextNs.labelComponent)}
                </DialogContentText>
                <DialogContentText
                    style={{
                        color: "#000",
                        whiteSpace: "pre-wrap",
                    }}
                >
                    {sMsg ? sMsg : I18n.SetText(`Message.ConfirmSave`, I18NextNs.labelComponent)}
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