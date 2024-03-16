import { Fragment } from "react";
import { useSelector } from "react-redux";
import AuthenSelectors from "store/selectors/AuthenSelectors";
import { DialogError, DialogSubmit, DialogSubmitWARNNING, DialogSuccess, DialogWarning } from "./SnackBarForm";

export default function Template() {
    const SuccessOpen = useSelector(AuthenSelectors.selectSuccessOpen);
    const SuccessMsg = useSelector(AuthenSelectors.selectSuccessMsg);
    const WarningOpen = useSelector(AuthenSelectors.selectWarningOpen);
    const WarningMsg = useSelector(AuthenSelectors.selectWarningMsg);
    const ErrorOpen = useSelector(AuthenSelectors.selectErrorOpen);
    const ErrorMsg = useSelector(AuthenSelectors.selectErrorMsg);
    const SubmitOpen = useSelector(AuthenSelectors.selectSubmitOpen);
    const SubmitMsg = useSelector(AuthenSelectors.selectSubmitMsg);
    const SubmitFn = useSelector(AuthenSelectors.selectSubmitFn);
    const SubmitCancelFn = useSelector(AuthenSelectors.selectSubmitCancelFn);
    const SubmitIsload = useSelector(AuthenSelectors.selectSubmitIsload);
    const Submit_WARNNING_Open = useSelector(AuthenSelectors.selectSubmit_WARNNING_Open);
    const Submit_WARNNING_Msg = useSelector(AuthenSelectors.selectSubmit_WARNNING_Msg);
    const Submit_WARNNING_Fn = useSelector(AuthenSelectors.selectSubmit_WARNNING_Fn);
    const Submit_WARNNING_FnCancel = useSelector(AuthenSelectors.selectSubmit_WARNNING_FnCancel);
    const Submit_WARNNING_Isload = useSelector(AuthenSelectors.selectSubmit_WARNNING_Isload);
    const Submit_SUCCESS_Fn = useSelector(AuthenSelectors.selectSubmit_SUCCESS_Fn);
    const IsFullScreen = useSelector(AuthenSelectors.selectIsFullScreen);
    const IsMsgNoSpace = useSelector(AuthenSelectors.selectIsMsgNoSpace);

    return (
        <Fragment>
            <DialogSuccess
                open={SuccessOpen}
                sMsg={SuccessMsg}
                handleClick={Submit_SUCCESS_Fn}
            />

            <DialogWarning
                handleClick={(c) => { }}
                open={WarningOpen}
                sMsg={WarningMsg}
            />

            <DialogError
                handleClick={(c) => { }}
                open={ErrorOpen}
                sMsg={ErrorMsg}
            />

            <DialogSubmit
                open={SubmitOpen}
                sMsg={SubmitMsg}
                handleClickSubmit={SubmitFn}
                IsLoad={SubmitIsload}
                handleCancel={SubmitCancelFn}
                fullScreenConfig={IsFullScreen}
                MsgSpaceConfig={IsMsgNoSpace}
            />

            <DialogSubmitWARNNING
                open={Submit_WARNNING_Open}
                sMsg={Submit_WARNNING_Msg}
                handleClickSubmit={Submit_WARNNING_Fn}
                handleClickClose={Submit_WARNNING_FnCancel}
                IsLoad={Submit_WARNNING_Isload}
            />
        </Fragment>
    );
}