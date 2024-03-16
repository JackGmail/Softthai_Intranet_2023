import Dialog from "@mui/material/Dialog";
import { useTheme } from "@mui/material/styles";
import { Button, CircularProgress, DialogActions, DialogContent, DialogContentText, DialogTitle, LinearProgress, SxProps, Theme, styled, useMediaQuery } from "@mui/material";
import React, { useEffect, useState } from "react";
import { useDispatch } from "react-redux";
import ConfirmIcon from "@mui/icons-material/HelpOutline";
import ConfirmBtnIcon from "@mui/icons-material/Done";
import MuiAlert, { AlertProps } from "@mui/material/Alert";
import CloseIcon from "@mui/icons-material/Close";
import DialogActionCreators from "store/redux/DialogAlert";
import WarningIcon from "@mui/icons-material/ErrorOutline";
import CheckIcon from "@mui/icons-material/CheckCircleOutline";
import { I18n } from "utilities/utilities";
import { I18NextNs } from "enum/enum";

const useStyles = (() => ({
    paper: {
        width: "400px !important",
        height: "auto",
        minHeight: "55% !important"
    },
    DialogPaperTemp: {
        height: "70%",
        width: "400px",
        borderRadius: "20px"
    },
    DialogTitle: {
        height: "40%",
        backgroundColor: "#28a745",
        color: "#fff",
        textAlign: "center"
    },
    DialogTitleError: {
        height: "40%",
        backgroundColor: "#ed3847",
        color: "#fff",
        textAlign: "center"
    },
    DialogTitleWarning: {
        height: "40%",
        backgroundColor: "#ffce3d",
        color: "#fff",
        textAlign: "center"
    },
    DialogTitleSubmit: {
        height: "40%",
        backgroundColor: "#163172",
        color: "#333333",
        textAlign: "center"
    },
    DialogContent: {
        height: "50%",
        backgroundColor: "#fff",
        color: "#000",
        padding: "10px 15px"
    },
    DialogAction: {
        backgroundColor: "#fff",
        color: "#000",
        justifyContent: "center !important"
    },
    DialogActionConfirm: {
        backgroundColor: "#fff",
        color: "#000",
        justifyContent: "center !important",
        height: "70px"
    },
    SuccessColor: {
        color: "#fff",
        backgroundColor: "#28a745",
        borderColor: "#28a745",
        "&:hover": {
            backgroundColor: "#28a745",
        }
    },
    ButtonDialog: {
        background: "rgb(250, 250, 250)",
        color: "#000",
        margin: "8px !important",
        width: "auto !important",
        fontSize: "1rem",
        borderRadius: "2em !important",
        padding: "4px 15px !important",
        textTransform: "none"
    }
}));

const customstyle = {
    DialogTitle: {
        height: "40%",
        backgroundColor: "#28a745",
        color: "#fff",
        textAlign: "center"
    },
    DialogTitleError: {
        height: "40%",
        backgroundColor: "#ed3847",
        color: "#fff",
        textAlign: "center"
    },
    DialogTitleWarning: {
        height: "40%",
        backgroundColor: "#ffce3d",
        color: "#fff",
        textAlign: "center"
    },
    DialogTitleSubmit: {
        height: "40%",
        backgroundColor: "#163172",
        color: "#333333",
        textAlign: "center"
    },
    DialogTitleSuccess: {
        height: "40%",
        backgroundColor: "#28a745",
        color: "#333333",
        textAlign: "center"
    },
    DialogContent: {
        height: "150% !important",
        backgroundColor: "#fff",
        color: "#000",
        padding: "10px 15px"
    },
    DialogAction: {
        backgroundColor: "#fff",
        color: "#000",
        justifyContent: "center !important"
    },
    DialogActionConfirm: {
        backgroundColor: "#fff",
        color: "#000",
        justifyContent: "center !important",
        height: "70px"
    },
    SuccessColor: {
        color: "#fff",
        backgroundColor: "#28a745",
        borderColor: "#28a745",
        "&:hover": {
            backgroundColor: "#28a745",
        }
    },
    ButtonDialog: {
        background: "rgb(250, 250, 250)",
        color: "#000",
        margin: "8px !important",
        width: "auto !important",
        fontSize: "1rem",
        borderRadius: "2em !important",
        padding: "4px 15px !important",
        textTransform: "none"
    },
};

const DialogStyle: SxProps = {
    ".MuiPaper-root": {
        width: "300px !important",
        height: "auto",
        minHeight: "50% !important",
        borderRadius: "20px !important"
    }
};

const DialogStyleNoMsgSpace: SxProps = {
    ".MuiPaper-root": {
        width: "300px !important",
        height: "auto",
        minHeight: "45% !important",
        borderRadius: "20px !important"
    }
};

const BorderLinearProgress = styled(LinearProgress)(({ theme }) => ({
    root: {
        height: 10,
        borderRadius: 5
    },
    colorPrimary: {
        backgroundColor: theme.palette.grey[200]
    },
    bar: {
        borderRadius: 5,
        backgroundColor: "#28a745"
    }
}));

const Alert = React.forwardRef<HTMLDivElement, AlertProps>(function Alert(
    props,
    ref
) {
    return <MuiAlert elevation={6} ref={ref} {...props} />;
});

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

    const handleClick = (e) => {
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
                {
                    fullScreenConfig ?
                        FullScreen ?
                            <ConfirmIcon
                                style={{
                                    fontSize: "10rem",
                                    fontWeight: "bold",
                                    color: "#fff",
                                    marginTop: "20%"
                                }}
                            /> :
                            <ConfirmIcon
                                style={{
                                    fontSize: "10rem",
                                    fontWeight: "bold",
                                    color: "#fff",
                                }}
                            /> :
                        <ConfirmIcon
                            style={{
                                fontSize: "10rem",
                                fontWeight: "bold",
                                color: "#fff",
                            }}
                        />
                }
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
                    {sMsg ? sMsg : I18n.SetText(`Title.ConfirmSave`, I18NextNs.labelComponent)}
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
                        onClick={handleClick}
                    >
                        Confirm
                        {/* {i18n.t(`${i18nDialog}.DialogConfirm`)} */}
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
                        {/* {i18n.t(`${i18nDialog}.CloseButt`)} */}
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

    const handleClick = (e) => {
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
                        onClick={handleClick}
                    >
                        Confirm
                        {/* {i18n.t(`${i18nDialog}.DialogConfirm`)} */}
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
                        {/* {i18n.t(`${i18nDialog}.CloseButt`)} */}
                    </Button>
                </div>
            </DialogActions>
        </Dialog>
    );
};

export const DialogWarning = (props: { open; sMsg?; handleClick?; fullScreenConfig?; MsgSpaceConfig?; }) => {
    const dispatch = useDispatch() as any;
    const { open, sMsg, handleClick, fullScreenConfig, MsgSpaceConfig } = props;
    const theme = useTheme();
    const FullScreen = useMediaQuery(theme.breakpoints.down("sm"));

    return (
        <Dialog fullScreen={fullScreenConfig ? FullScreen ? true : false : false} open={open} sx={MsgSpaceConfig ? DialogStyleNoMsgSpace : fullScreenConfig ? FullScreen ? useStyles : DialogStyle : DialogStyle}>
            <DialogTitle sx={{ ...customstyle.DialogTitleWarning }}>
                {
                    fullScreenConfig ?
                        FullScreen ?
                            <WarningIcon
                                style={{
                                    fontSize: "10rem",
                                    fontWeight: "bold",
                                    color: "#fff",
                                    marginTop: "20%"
                                }}
                            /> :
                            <WarningIcon
                                style={{
                                    fontSize: "10rem",
                                    fontWeight: "bold",
                                    color: "#fff",
                                }}
                            /> :
                        <WarningIcon
                            style={{
                                fontSize: "10rem",
                                fontWeight: "bold",
                                color: "#fff",
                            }}
                        />
                }
            </DialogTitle>
            <DialogContent sx={{ ...customstyle.DialogContent }}>
                <DialogContentText
                    style={{
                        fontSize: "1.5rem",
                        fontWeight: "bold",
                        color: "#000",
                    }}
                >
                    {I18n.SetText(`Title.Warning`, I18NextNs.labelComponent)}
                </DialogContentText>
                <DialogContentText
                    style={{
                        color: "#000",
                    }}
                >
                    {sMsg ? sMsg : I18n.SetText(`Message.Warning`, I18NextNs.labelComponent)}
                </DialogContentText>
            </DialogContent>
            <DialogActions sx={{ ...customstyle.DialogActionConfirm }}>
                <div style={{ textAlign: "center" }}>
                    <Button
                        variant="contained"
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
                            if (handleClick) {
                                handleClick();
                            }
                            dispatch(DialogActionCreators.CloseDialogWarning());
                        }}
                    >
                        Close
                        {/* {i18n.t(`${i18nDialog}.CloseButt`)} */}
                    </Button>
                </div>
            </DialogActions>
        </Dialog>
    );
};

export const DialogError = (props: { open; sMsg?; handleClick?; fullScreenConfig?; MsgSpaceConfig?; }) => {
    const dispatch = useDispatch() as any;
    const { open, sMsg, handleClick, fullScreenConfig, MsgSpaceConfig } = props;
    const theme = useTheme();
    const FullScreen = useMediaQuery(theme.breakpoints.down("sm"));

    return (
        <Dialog fullScreen={fullScreenConfig ? FullScreen ? true : false : false} open={open} sx={MsgSpaceConfig ? DialogStyleNoMsgSpace : fullScreenConfig ? FullScreen ? useStyles : DialogStyle : DialogStyle}>
            <DialogTitle sx={{ ...customstyle.DialogTitleError }}>
                {
                    fullScreenConfig ?
                        FullScreen ?
                            <WarningIcon
                                style={{
                                    fontSize: "10rem",
                                    fontWeight: "bold",
                                    color: "#fff",
                                    marginTop: "20%"
                                }}
                            /> :
                            <WarningIcon
                                style={{
                                    fontSize: "10rem",
                                    fontWeight: "bold",
                                    color: "#fff",
                                }}
                            /> :
                        <WarningIcon
                            style={{
                                fontSize: "10rem",
                                fontWeight: "bold",
                                color: "#fff",
                            }}
                        />
                }
            </DialogTitle>
            <DialogContent sx={{ ...customstyle.DialogContent }}>
                <DialogContentText
                    style={{
                        fontSize: "1.5rem",
                        fontWeight: "bold",
                        color: "#000",
                    }}
                >
                    {I18n.SetText(`Title.Error`, I18NextNs.labelComponent)}
                </DialogContentText>
                <DialogContentText
                    style={{
                        color: "#000",
                    }}
                >
                    {sMsg ? sMsg : I18n.SetText(`Message.Error`, I18NextNs.labelComponent)}
                </DialogContentText>
            </DialogContent>
            <DialogActions sx={{ ...customstyle.DialogActionConfirm }}>
                <div style={{ textAlign: "center" }}>
                    <Button
                        variant="contained"
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
                            if (handleClick) {
                                handleClick();
                            }
                            dispatch(DialogActionCreators.CloseDialogError());
                        }}
                    >
                        Close
                        {/* {i18n.t(`${i18nDialog}.CloseButt`)} */}
                    </Button>
                </div>
            </DialogActions>
        </Dialog>
    );
};

export const DialogSuccess = (props: { handleClick?; fullScreenConfig; MsgSpaceConfig; open; sMsg?}) => {
    const dispatch = useDispatch() as any;
    const { handleClick, fullScreenConfig, MsgSpaceConfig, open, sMsg } = props;
    const theme = useTheme();
    const FullScreen = useMediaQuery(theme.breakpoints.down("sm"));

    return (
        <Dialog fullScreen={fullScreenConfig ? FullScreen ? true : false : false} open={open} sx={MsgSpaceConfig ? DialogStyleNoMsgSpace : fullScreenConfig ? FullScreen ? useStyles : DialogStyle : DialogStyle}>
            <DialogTitle sx={{ ...customstyle.DialogTitleSuccess }}>
                {
                    fullScreenConfig ?
                        FullScreen ?
                            <CheckIcon
                                style={{
                                    fontSize: "10rem",
                                    fontWeight: "bold",
                                    color: "#fff",
                                    marginTop: "20%"
                                }}
                            /> :
                            <CheckIcon
                                style={{
                                    fontSize: "10rem",
                                    fontWeight: "bold",
                                    color: "#fff",
                                }}
                            /> :
                        <CheckIcon
                            style={{
                                fontSize: "10rem",
                                fontWeight: "bold",
                                color: "#fff",
                            }}
                        />
                }
            </DialogTitle>
            <DialogContent sx={{ ...customstyle.DialogContent }}>
                <DialogContentText
                    style={{
                        fontSize: "1.5rem",
                        fontWeight: "bold",
                        color: "#000",
                    }}
                >
                    {I18n.SetText(`Title.Success`, I18NextNs.labelComponent)}
                </DialogContentText>
                <DialogContentText
                    style={{
                        color: "#000",
                    }}
                >
                    {sMsg ? sMsg : I18n.SetText(`Message.SaveComplete`, I18NextNs.labelComponent)}
                </DialogContentText>
            </DialogContent>
            <DialogActions sx={{ ...customstyle.DialogActionConfirm }}>
                <div style={{ textAlign: "center" }}>
                    <Button
                        variant="contained"
                        style={{
                            color: "#fff",
                            backgroundColor: "#B6B6B6",
                            borderRadius: "20px",
                            textTransform: "none",
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
                            if (handleClick) {
                                handleClick();
                            }
                            dispatch(DialogActionCreators.CloseDialogSuccess());
                        }}
                    >
                        Close
                    </Button>
                </div>
            </DialogActions>
        </Dialog>
    );
};