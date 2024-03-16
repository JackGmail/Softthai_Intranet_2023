import { Grid } from "@mui/material";
import React from "react";
import ExampleCheckBox from "./ExampleCheckBox";
import ExampleTextBox from "./ExampleTextbox";
import ExampleTextArea from "./ExampleTextArea";
import ExampleRadio from "./ExampleRadio";
import ExampleMultiSelect from "./ExampleMultiSelect";
import ExampleAlert from "./ExampleAlert";
import ExampleDatePicker from "./ExampleDatePicker";
import ExampleMonthPicker from "./ExampleMonthPicker";
import ExampleYearPicker from "./ExampleYearPicker";
import ExampleQuarterPicker from "./ExampleQuarterPicker";
import ExampleDatePickerRange from "./ExampleDatePickerRange";
import ExampleAutoComplete from "./ExampleAutoComplete";
import ExampleMonthPickerRange from "./ExampleMonthPickerRange;";
import ExampleYearPickerRange from "./ExampleYearPickerRange";
import ExampleTimePicker from "./ExampleTimePicker";
import ExampleTimePickerRange from "./ExampleTimePickerRange";
import ExampleQuarterRangePicker from "./ExampleQuarterRangePicker";
import ExampleMonthYearPicker from "./ExampleMonthYearPicker";
import ExampleMonthYearRangePicker from "./ExampleMonthYearRangePicker";
import ExampleButton from "./ExampleButton";
import ExampleUploadFile from "./ExampleUploadFile";


export default function ExampleComponents() {

  return (
    <React.Fragment>
      <Grid>
        <ExampleUploadFile/>
        <ExampleButton/>
        <ExampleAutoComplete></ExampleAutoComplete>
        <ExampleTextBox></ExampleTextBox>
        <ExampleTextArea></ExampleTextArea>
        <ExampleRadio></ExampleRadio>
        <ExampleCheckBox></ExampleCheckBox>
        <ExampleAlert></ExampleAlert>
        <ExampleMultiSelect></ExampleMultiSelect>
         <ExampleDatePicker></ExampleDatePicker>
        <ExampleMonthPicker></ExampleMonthPicker>
        <ExampleYearPicker></ExampleYearPicker>
        <ExampleQuarterPicker></ExampleQuarterPicker>
        <ExampleDatePickerRange></ExampleDatePickerRange>
        <ExampleDatePickerRange></ExampleDatePickerRange>
        <ExampleMonthPicker></ExampleMonthPicker>
        <ExampleMonthPickerRange></ExampleMonthPickerRange>
        <ExampleQuarterPicker></ExampleQuarterPicker>
        <ExampleQuarterRangePicker></ExampleQuarterRangePicker>
        <ExampleYearPicker></ExampleYearPicker>
        <ExampleYearPickerRange></ExampleYearPickerRange>
        <ExampleMonthYearPicker></ExampleMonthYearPicker>
        <ExampleMonthYearRangePicker></ExampleMonthYearRangePicker>
      {/* <ExampleTimePicker></ExampleTimePicker> */}
        {/*  <ExampleTimePickerRange></ExampleTimePickerRange> */}
      </Grid>
    </React.Fragment>
  );
}
