import { useState } from "react";
import { styled } from '@mui/material/styles';
import React from 'react';
import MuiAccordion, { AccordionProps } from '@mui/material/Accordion';
import MuiAccordionSummary, {
    AccordionSummaryProps,
} from '@mui/material/AccordionSummary';
import MuiAccordionDetails from '@mui/material/AccordionDetails';
import Typography from '@mui/material/Typography';
import { Grid } from "@mui/material";
import HTMLReactParser from "html-react-parser";
import { Checkbox } from "@mui/material";

const AccordionStyled = styled((props: AccordionProps) => (
    <MuiAccordion disableGutters elevation={0} {...props} />
))(({ theme }) => ({
    border: `1px solid ${theme.palette.divider}`,
    borderRadius: '10px !important',
    '&:not(:last-child)': {
        borderBottom: 0,
    },
    '&:before': {
        display: 'none',
    },
}));

const AccordionSummary = styled((props: AccordionSummaryProps) => (
    <MuiAccordionSummary
        // expandIcon={<ExpandCircleDownOutlinedIcon sx={{ fontSize: '1.5rem', color: "#0050EF" }} />}
        {...props}
    />
))(({ theme }) => ({
    // backgroundColor:
    //     theme.palette.mode === 'dark'
    //         ? 'rgba(255, 255, 255, .05)'
    //         : 'rgba(0, 0, 0, .03)',
    color: "#1e456c",

    flexDirection: 'row-reverse',
    // flexDirection: 'column',
    '& .MuiAccordionSummary-expandIconWrapper.Mui-expanded': {
        transform: 'rotate(180deg)',
    },
    '& .MuiAccordionSummary-content': {
        marginLeft: theme.spacing(1),
    },
}));

const AccordionDetails = styled(MuiAccordionDetails)(({ theme }) => ({
    padding: theme.spacing(2),
    // borderTop: '1px solid rgba(0, 0, 0, .125)',
}));

interface Props {
    children: React.ReactNode,
    header?: string,
    SubHeader?: any,
    props?: AccordionSummaryProps,
    onClick?: Function,
    onExpanded?: boolean, //false = เปิด หรือ true = ปิด ไว้
    maxHeight?: string,
    overflow?: string,
    bgcolor?: string,
    color?: string,
    boxRadius?: string,
}

const AccordionCustom: React.FC<Props> = ({ children, header, props, onExpanded, SubHeader,
    maxHeight = "", overflow = "", bgcolor, color, boxRadius, onClick
}) => {
    const [expanded, setExpanded] = useState<string | false>(onExpanded == true ? SubHeader ? 'panel1' : false : 'panel1');
    const [action, setAction] = useState(SubHeader != null ? true : false);
    const [isCollapseInternal, setisCollapseInternal] = useState(true);
    const [checked, setChecked] = React.useState(false);
    const handleChange1 = (event) => {
        setChecked(event.target.checked);
    };

    const border = "10px";

    const handleChange = (panel: string) => (event: React.SyntheticEvent, newExpanded: boolean) => {
        setExpanded(newExpanded ? panel : false);
        if (SubHeader) {
            setTimeout(() => {
                setAction(!action);
                setExpanded('panel1');
            }, 300)
        }

    };

    const onEditClick = (e) => {
        e.stopPropagation();
        onClick(e.target.checked)
        // alert("I'm not expanding!");
    }

    return (
        <AccordionStyled
            {...props}
            expanded={expanded === 'panel1'}
            onChange={handleChange('panel1')}
        >
            <AccordionSummary
                aria-controls="panel1d-content"
                id="panel1d-header"
                sx={{
                    backgroundColor: bgcolor,
                    borderRadius: expanded == false ? border : boxRadius,
                    alignItems: SubHeader ? "start" : "center",
                    '& .MuiAccordionSummary-expandIconWrapper': {
                        margin: SubHeader ? "12px 0" : "0",
                    }
                }}
                expandIcon={<Checkbox
                    checked={checked}
                    // indeterminate={checked[0] !== checked[1]}
                    onChange={handleChange1}
                    onClick={onEditClick}
                />}
            >
                <Grid container spacing={1} direction="row" justifyContent="start" alignItems="start">
                    <Grid item xs={12} md={12} container direction="row" alignItems="start">
                        <Typography style={{ color: color }}>{HTMLReactParser(header)}</Typography>
                    </Grid>
                </Grid>
            </AccordionSummary>

            {action == true ?
                <AccordionDetails>
                    {SubHeader}
                </AccordionDetails>
                :
                <AccordionDetails sx={{ maxHeight: maxHeight, overflow: overflow }}>
                    {children}
                </AccordionDetails>
            }

        </AccordionStyled>

    )
}

export default AccordionCustom