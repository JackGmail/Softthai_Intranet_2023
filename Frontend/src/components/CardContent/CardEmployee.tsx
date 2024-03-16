import React, { useEffect, useState } from 'react'
import { Cake, Email, Phone } from '@mui/icons-material'
import { Chip, Stack, } from '@mui/material'
import defaultAvatar from "assets/images/NoImage/default-avatar.png";
import { GetMenuPermission } from 'utilities/SystemFunction';
import { FnAxios, RouterDOM } from 'utilities/utilities';
import "./Cardemp.css"

export default function CardEmployee(props) {
    const Router = RouterDOM();
    const AxiosFn = FnAxios();
    const [Permission, setPermission] = useState<boolean>(false); // Permission
    const onImageError = (e) => {
        e.target.src = defaultAvatar
    }
    useEffect(() => {
        GetMenuPermission(AxiosFn, setPermission);
    }, []);
    const onEdit = (sID) => {
        if (sID) {
            Router.GoToPage(`/employeeformedit`,
                {
                    state: {
                        sEmployeeID: sID,
                        Permission: Permission,
                    }
                }
            )

        }
    };

    return (
        <React.Fragment>
            <Stack gap={2} justifyContent={"center"} alignItems={"center"} direction={"row"} flexWrap={"wrap"}>
                <div className="cards">
                    <span className="txt-nickname">
                        {props.item.sNickName}
                    </span>
                    <span className="mail">
                        <Chip label={props.item.isActive === true ? `Active` : `Not Active`} color={props.item.isActive === true ? "success" : "error"} variant="filled" />
                    </span>
                    <div className="profile-pic">
                        <img
                            src={props.item.sFileLink ? props.item.sFileLink : defaultAvatar}
                            onError={onImageError}
                        />
                    </div>
                    <div className="bottom">
                        <div className="content">
                            <span className="txt-workdate">
                                {props.item.sTotalDate}
                            </span>
                            <span className="txt-position">
                                {props.item.sPosition}
                            </span>
                            <span className="about-me">
                                <span style={{ display: "flex", alignItems: "center" }}>
                                    <b>ประเภทพนักงาน :</b>&nbsp; {props.item.sEmpType}<br />
                                </span>
                                {
                                    !props.item.IsRetire ?
                                        (
                                            <span style={{ display: "flex", alignItems: "center" }}>
                                                <b>วันที่เริ่มงาน :</b>&nbsp; {props.item.sWorkStart}<br />
                                            </span>
                                        ) : (
                                            <span style={{ display: "flex", alignItems: "center" }}>
                                                <b>วันที่ลาออก :</b>&nbsp; {props.item.sRetire}
                                            </span>
                                        )
                                }
                                <span style={{ display: "flex", alignItems: "center" }}><Email />:&nbsp;{props.item.sEmail}</span>
                                <span style={{ display: "flex", alignItems: "center" }}><Phone />:&nbsp;{props.item.sTelephone} </span>
                                <span style={{ display: "flex", alignItems: "center" }}><Cake />:&nbsp;{props.item.sBirth}</span>
                            </span>
                        </div>
                        <div className="bottom-bottom">
                            <span className="txt-name">
                                {props.item.sFullname}
                            </span>
                            <button className="button" onClick={() => { onEdit(props.item.sID) }} >Contact Me</button>
                        </div>
                    </div>
                </div>
            </Stack>
        </React.Fragment>
    )
}

