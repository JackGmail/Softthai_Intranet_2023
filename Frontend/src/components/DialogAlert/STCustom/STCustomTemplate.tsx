import { Fragment } from "react";
import { useSelector } from "react-redux";
import AuthenSelectors from "store/selectors/AuthenSelectors";
import { DialogError, DialogSubmit, DialogSubmitWARNNING, DialogSuccess, DialogWarning } from "./STCustomForm";

export default function Template() {
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
    const WarningOpen = useSelector(AuthenSelectors.selectWarningOpen);
    const WarningMsg = useSelector(AuthenSelectors.selectWarningMsg);
    const ErrorOpen = useSelector(AuthenSelectors.selectErrorOpen);
    const ErrorMsg = useSelector(AuthenSelectors.selectErrorMsg);
    const SuccessOpen = useSelector(AuthenSelectors.selectSuccessOpen);
    const SuccessMsg = useSelector(AuthenSelectors.selectSuccessMsg);
    const IsFullScreen = useSelector(AuthenSelectors.selectIsFullScreen);
    const IsMsgNoSpace = useSelector(AuthenSelectors.selectIsMsgNoSpace);

    return (
        <Fragment>
            <DialogWarning
                open={WarningOpen}
                sMsg={WarningMsg}
                handleClick={Submit_SUCCESS_Fn}
                fullScreenConfig={IsFullScreen}
                MsgSpaceConfig={IsMsgNoSpace}
            />
            <DialogError
                open={ErrorOpen}
                sMsg={ErrorMsg}
                handleClick={Submit_SUCCESS_Fn}
                fullScreenConfig={IsFullScreen}
                MsgSpaceConfig={IsMsgNoSpace}
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
            <DialogSuccess
                open={SuccessOpen}
                sMsg={SuccessMsg}
                handleClick={Submit_SUCCESS_Fn}
                fullScreenConfig={IsFullScreen}
                MsgSpaceConfig={IsMsgNoSpace}
            />
        </Fragment>
    );
}