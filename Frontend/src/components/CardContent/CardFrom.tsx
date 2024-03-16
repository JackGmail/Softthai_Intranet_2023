import * as React from 'react';
import Card from '@mui/material/Card';
import { CardCustom, CardCustomFrom } from './CardItemClass';
import { Divider, Grid } from '@mui/material';


const CardFrom: React.FC<CardCustomFrom> = ({
  children, bgColor = "#fafdff", widthCard,footer
}) => {
  return (
    <Card sx={{
      display: "flex",
      flexDirection: "column",
      padding: "1rem",
      borderRadius: "20px !important",
      boxShadow: "rgba(60, 64, 67, 0.3) 0px 1px 2px 0px, rgba(60, 64, 67, 0.15) 0px 2px 6px 2px !important",
      WebkitBackdropFilter: " blur(10px)",
      backdropFilter: "blur(10px)",
      background: "inherit",
      // margin: "2rem 0rem 2rem 0rem",
      backgroundColor: bgColor,
      width: widthCard
    }}
    >
      {children}
      <Grid item padding={"10px"}></Grid>
      {footer &&
        <>
          <Grid item padding={"10px"}></Grid>
          <Divider />
          <Grid item padding={"5px"}></Grid>
          {footer}
        </>
      }
    </Card>
  );
}

export default CardFrom
