import Breadcrumbs from '@mui/material/Breadcrumbs';
import Typography from '@mui/material/Typography';
import Link from '@mui/material/Link';
import { IBreadcrumb, IBreadcrumbItem } from './BreadcrumbsBar'
import "./Breadcrumbs.css"
import { FnAxios } from 'utilities/utilities';
import { ApiNavigation } from 'enum/api';
import { useMemo, useState } from 'react';
import { I18n, RouterDOM, SecureStorage } from 'utilities/utilities';
import { IconComponents } from 'components/Icon';
import { Stack } from '@mui/material';

const BreadcrumbsBar = (props: IBreadcrumb) => {
    const Router = RouterDOM();
    const AxiosFn = FnAxios();
    const { Item } = props;
    const [arrData, setBreadcrumbs] = useState<IBreadcrumbItem[]>([]);

    useMemo(() => {
        if (Item) {
            setBreadcrumbs([...Item]);
        }
        else {
            let sLanguages = I18n.GetLanguage();
            let sRoute = Router.GetCurrentRoute();
            AxiosFn.Get(ApiNavigation.GetBreadcrumbs, { sRoute: sRoute, sLanguages: sLanguages }, (d) => {
                let result: IBreadcrumbItem[] = [];
                result = d.objResult.lstBreadcrumbs;
                setBreadcrumbs([...result]);
            })
        }
    }, [window.location.pathname, SecureStorage.Get(I18n.envI18next)]);

    return (
        <div className='div-breadcrumbs' >
            <Stack justifyContent={"left"} alignItems={"center"} direction={"row"}>
                <Breadcrumbs maxItems={3} aria-label="breadcrumb">
                    {
                        arrData.map((item, index) => {
                            let sKey = "Breadcrumbs_" + item.nMenuID;
                            let sMenuName = item.sMenuName;
                            let sRoute = item.sRoute ?? "#";
                            let sIcon = item.sIcon;
                            let IsLastItem = index === (arrData.length - 1);
                            return (sRoute !== "#" && !IsLastItem ?
                                <Link key={sKey} underline="hover" onClick={() => { Router.GoToPage(sRoute) }} >
                                    {IconComponents(sIcon)} {sMenuName}
                                </Link>
                                :
                                <Typography key={sKey}>{IconComponents(sIcon)} {sMenuName}</Typography>
                            )
                        })
                    }
                </Breadcrumbs>
            </Stack>
        </div>
    )
}
export default BreadcrumbsBar;