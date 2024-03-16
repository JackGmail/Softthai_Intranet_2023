import React, { useState, useMemo, useCallback } from "react";
import { AdapterMoment } from "@mui/x-date-pickers/AdapterMoment";
import {
  TextField,
  FormControl,
  Stack,
  InputAdornment,
  IconButton,
  Grid,
  FormHelperText,
  Autocomplete,
  ListItem,
  Tooltip,
} from "@mui/material";
import { Controller, useFormContext } from "react-hook-form";
import {
  IDatePickerProps,
  IDateRangePickerProp,
  IMonthYearRangePickerProps,
  IQuarterPickerProps,
} from "./DatePickerProps";
import moment from "moment";
import "moment/locale/th";
import { language } from "config/AppConfig";
import {
  DateRangePicker,
  LicenseInfo,
  DatePicker,
  LocalizationProvider,
} from "@mui/x-date-pickers-pro";
import EventIcon from "@mui/icons-material/Event";
import CloseIcon from "@mui/icons-material/Close";
import { I18NextNs, Language } from "enum/enum";
import { I18n, ParseHtml, SecureStorage } from "utilities/utilities";
import secureLocalStorage from "react-secure-storage";

/** class CustomAdapter */
class CustomAdapter extends AdapterMoment {
  constructor({ locale, formats, instance }) {
    super({ locale, formats, instance });
  }

  calYear(moment) {
    switch (this.locale) {
      case "th":
        return (parseInt(moment.format("YYYY")) + 543).toString();
      // return (parseInt(moment.format('YYYY'))).toString();
      case "en":
        return parseInt(moment.format("YYYY"));
      default:
        return parseInt(moment.format("YYYY"));
    }
  }

  renderLabel(moment, format) {
    switch (format) {
      case "year":
        return this.calYear(moment);
      case "month":
        return moment.format("MMMM");
      case "fullDate":
        return moment.format("DDMMYYYY");
      case "dayOfMonth":
        return moment.format("D");
      case "monthShort":
        return moment.format("MMMM");
      case "monthAndYear":
        return `${moment.format("MMMM ")} ${this.calYear(moment)}`;
      case "normalDate":
        return `${moment.format("dd")} ${moment.format("MMM")} ${moment.format(
          "D"
        )}`;
      default:
        return moment.format(format);
    }
  }
  startOfMonth = (date) =>
    date ? date.clone().startOf("month") : moment(new Date());

  format = (date, formatKey) => {
    return date ? this.renderLabel(date, formatKey) : moment(new Date());
  };
}

//#region Array Option Quarter
const arrOptionEN = [
  {
    value: 1,
    label: "Quater 1",
  },
  {
    value: 2,
    label: "Quater 2",
  },
  {
    value: 3,
    label: "Quater 3",
  },
  {
    value: 4,
    label: "Quater 4",
  },
];

const arrOptionTH = [
  {
    value: 1,
    label: "ไตรมาส 1",
  },
  {
    value: 2,
    label: "ไตรมาส 2",
  },
  {
    value: 3,
    label: "ไตรมาส 3",
  },
  {
    value: 4,
    label: "ไตรมาส 4",
  },
];

//#endregion Array Option Quarter

const localeMap = (language) => {
  switch (language) {
    case "th":
      moment.locale("th");
      break;
    case "en":
      moment.locale("en");
      break;
    default:
      break;
  }
};

export const DatePickerItem = (props: IDatePickerProps) => {
  const {
    name,
    label,
    fullWidth = false,
    IsMonthYearPicker = false,
    IsMonthPicker = false,
    IsYearPicker = false,
    isShowMessageError = true,
    isShowIcon = true,
    onDisableDay,
    id,
    defaultValue
  } = props;
  const minDate = props.minDate ?? null;
  const maxDate = props.maxDate ?? null;
  const required = props.required ?? false;
  const disabled = props.disabled ?? false;
  const view = IsMonthYearPicker
    ? ["year", "month"]
    : IsMonthPicker
      ? ["month"]
      : IsYearPicker
        ? ["year"]
        : props.view ?? ["year", "month", "day"];
  let format = IsMonthYearPicker
    ? "MMMM/YYYY"
    : IsMonthPicker
      ? "MMMM"
      : IsYearPicker
        ? "YYYY"
        : props.format || "DD/MM/YYYY";
  const formatDefault = format;
  const [open, setOpen] = useState(false);
  const [locale, setLocale] = useState<string>(language);
  const { control } = useFormContext();

  /** funtion **/
  const ReplaceThFormat = (text) => {
    return text
      ? text
        .toLowerCase()
        .replaceAll("d", "ว")
        .replaceAll("m", "ด")
        .replaceAll("y", "ป")
      : "";
  };

  const maskMap = {
    en: formatDefault,
    th: ReplaceThFormat(formatDefault),
  };

  const [objCheckReplace] = useState(() => {
    localeMap(locale);
    let objReturn = { isReplaceYear: false, indexArr: 0, splitText: "" };
    let formatLower = formatDefault ? formatDefault.toLowerCase() : "";
    if (formatLower.includes("YYYY")) {
      let splitText = formatDefault
        .substr(formatLower.indexOf("YYYY") - 1, 1)
        .toLowerCase();
      if (splitText !== "Y") {
        objReturn.splitText = splitText;
        objReturn.indexArr = formatLower
          .split(splitText)
          .findIndex((e) => e === "YYYY");
      }
      objReturn.isReplaceYear = true;
    }
    return objReturn;
  });

  const setFormatDate = useCallback(
    (params, value) => {
      if (locale === "th" && value) {
        if (format === "MMMM/YYYY") {
          let convertDateToString: string = moment(value)
            .locale("th")
            .format(format);

          let year = convertDateToString.split("/")[1];
          let addyear =
            parseInt(year) > 2500 ? parseInt(year) : parseInt(year) + 543;
          params.inputProps.value = convertDateToString.replaceAll(
            `${year}`,
            `${addyear}`
          );
        } else if (format === "YYYY") {
          let year: string = moment(value).format(format);
          let addyear =
            parseInt(year) > 2500 ? parseInt(year) : parseInt(year) + 543;
          params.inputProps.value = addyear;
        } else {
          let convertDateToString: string = moment(value).format(format);
          let year = objCheckReplace.splitText
            ? convertDateToString.split(objCheckReplace.splitText)[
            objCheckReplace.indexArr
            ]
            : convertDateToString.substr(6, 4);
          let addyear = parseInt(year) + 543;
          let replaceAllFormat = format.replaceAll("YYYY", "");
          params.inputProps.value =
            moment(value).format(replaceAllFormat) + addyear;
        }
      }
    },
    [locale]
  );

  const rules = useMemo(() => {
    let message = `${I18n.SetText("required", I18NextNs.validation)}`
    return {
      required: {
        value: required,
        message: message + ` ${label}`,
        // message: `${I18n.setText("required")} ${label}`,
      },
    };
  }, [required, SecureStorage.Get(I18n.envI18next)]);

  useMemo(() => {
    let lang = secureLocalStorage.getItem(I18n.envI18next);
    setLocale(lang === Language.th ? "th" : "en");
  }, [SecureStorage.Get(I18n.envI18next)]);

  return (
    <Controller
      key={name}
      name={name}
      control={control}
      rules={rules}
      defaultValue={defaultValue}
      render={({
        field: { onChange, onBlur, value, ref },
        fieldState: { invalid, error },
      }) => (
        <LocalizationProvider
          adapterLocale={locale}
          dateAdapter={CustomAdapter as any}
        >
          <FormControl
            fullWidth={fullWidth}
            sx={{
              "label.MuiInputLabel-shrink": {
                top: "0px",
              },
              ".MuiInputLabel-outlined": {
                top: "0px",
              }
            }}
          >
            <DatePicker
              dayOfWeekFormatter={(day) =>
                day.charAt(0).toUpperCase() === "เ"
                  ? "ส"
                  : day.charAt(0).toUpperCase()
              }
              inputRef={ref}
              value={value ? moment(value, formatDefault) : null}
              onChange={(e) => {
                let IsValid = moment(e).isValid();
                if (IsValid) {
                  let dDate: Date = e;
                  onChange(dDate);
                  props.onChange && props.onChange(dDate);
                } else {
                  onChange(null);
                  props.onChange && props.onChange(null);
                }
              }}
              shouldDisableDate={onDisableDay}
              open={open}
              onOpen={() => {
                if (!props.disabled) {
                  setOpen(true);
                }
              }}
              onClose={() => setOpen(false)}
              minDate={minDate}
              maxDate={maxDate}
              label={props.label}
              inputFormat={formatDefault}
              disabled={disabled}
              views={view}
              showToolbar={false}
              showDaysOutsideCurrentMonth={false}
              PaperProps={
                IsMonthPicker
                  ? {
                    sx: {
                      ".MuiPickersCalendarHeader-root": {
                        display: "none !important",
                      },
                    },
                  }
                  : {}
              }
              componentsProps={{
                actionBar: {
                  actions: ["clear"],
                },
              }}
              renderInput={(params) => {
                params.inputProps.placeholder = maskMap[locale];
                !IsMonthPicker && setFormatDate(params, value);
                return (
                  <TextField
                    {...params}
                    sx={{
                      ".MuiOutlinedInput-root": {
                        padding: isShowIcon
                          ? "0px 15px 0px 0px!important"
                          : "0px !important",
                        ".MuiInputAdornment-root": {
                          overflow: isShowIcon ? "": "auto",
                          marginLeft:'0px'
                        },
                        " fieldset > legend > span": {
                          padding: "0px !important",
                        },
                      },
                      fontWeight: 600,
                    }}
                    size={"small"}
                    label={props.label}
                    id={id}
                    name={name}
                    error={invalid}
                    required={required}
                    disabled={disabled}
                    focused={false}
                    value={params.inputProps.value || ""}
                    autoComplete={"off"}
                    inputProps={{
                      readOnly: true,
                    }}
                    onClick={() => {
                      if (!props.disabled) {
                        setOpen(true);
                      }
                    }}
                    helperText={
                      isShowMessageError && error ? error.message : null
                    }
                  />
                );
              }}
            />
          </FormControl>
        </LocalizationProvider>
      )}
    />
  );
};

export const DateRangePickerItem = (props: IDateRangePickerProp) => {
  LicenseInfo.setLicenseKey(
    "049fdd25559575ef04f65a9e1ed5aabaTz02NDk0MCxFPTE3MTM2NzY2MjYyNzIsUz1wcm8sTE09cGVycGV0dWFsLEtWPTI="
  );

  const {
    name,
    minDate,
    maxDate,
    defaultCalendarMonth = null,
    labelName = "",
    labelStart = "",
    labelEnd = "",
    required = false,
    calendarsCount = 2,
    disabled = false,
    disablePast = false,
    isShowMessageError = true,
  } = props;

  let format = "DD/MM/YYYY";

  const formatDefault = format;
  const [open, setOpen] = useState(false);
  const [anchorEl, setAnchorEl] = React.useState(null);

  const [locale, setLocale] = useState<string>(language);

  const { control } = useFormContext();

  const ReplaceThFormat = (text) => {
    return text
      ? text
        .toLowerCase()
        .replaceAll("d", "ว")
        .replaceAll("m", "ด")
        .replaceAll("y", "ป")
      : "";
  };

  const maskMap = {
    en: formatDefault,
    th: ReplaceThFormat(formatDefault),
  };

  const [objCheckReplace] = useState(() => {
    localeMap(locale);
    let objReturn = { isReplaceYear: false, indexArr: 0, splitText: "" };
    var formatLower = formatDefault.toLowerCase();
    if (formatLower.includes("yyyy")) {
      var splitText = formatDefault
        .substr(formatLower.indexOf("yyyy") - 1, 1)
        .toLowerCase();
      if (splitText !== "y") {
        objReturn.splitText = splitText;
        objReturn.indexArr = formatLower
          .split(splitText)
          .findIndex((e) => e === "yyyy");
      }
      objReturn.isReplaceYear = true;
    }
    return objReturn;
  });

  const setFormatDate = useCallback(
    (startProps: any, endProps: any, value: any) => {
      if (locale === "th") {
        //Start
        if (value[0]) {
          if (format === "MMMM/YYYY") {
            let convertDateToString: string = moment(value[0]).format(format);
            let year = convertDateToString.split("/")[1];
            let addyear =
              parseInt(year) > 2500 ? parseInt(year) : parseInt(year) + 543;
            startProps.inputProps.value = convertDateToString.replaceAll(
              `${year}`,
              `${addyear}`
            );
          } else if (format === "YYYY") {
            let year: string = moment(value[0]).format(format);
            let addyear =
              parseInt(year) > 2500 ? parseInt(year) : parseInt(year) + 543;
            startProps.inputProps.value = addyear;
          } else {
            let convertDateToString: string = moment(value[0]).format(format);
            let year = objCheckReplace.splitText
              ? convertDateToString.split(objCheckReplace.splitText)[
              objCheckReplace.indexArr
              ]
              : convertDateToString.substring(6, 4);
            let addyear = parseInt(year) + 543;
            let replaceAllFormat = format.replaceAll("YYYY", "");
            startProps.inputProps.value =
              moment(value[0]).format(replaceAllFormat) + addyear;
          }
        }

        //End
        if (value[1]) {
          if (format === "MMMM/YYYY") {
            let convertDateToString: string = moment(value[1]).format(format);
            let year = convertDateToString.split("/")[1];
            let addyear =
              parseInt(year) > 2500 ? parseInt(year) : parseInt(year) + 543;
            endProps.inputProps.value = convertDateToString.replaceAll(
              `${year}`,
              `${addyear}`
            );
          } else if (format === "YYYY") {
            let year: string = moment(value[1]).format(format);
            let addyear =
              parseInt(year) > 2500 ? parseInt(year) : parseInt(year) + 543;
            endProps.inputProps.value = addyear;
          } else {
            let convertDateToString: string = moment(value[1]).format(format);
            let year = objCheckReplace.splitText
              ? convertDateToString.split(objCheckReplace.splitText)[
              objCheckReplace.indexArr
              ]
              : convertDateToString.substring(6, 4);
            let addyear = parseInt(year) + 543;
            let replaceAllFormat = format.replaceAll("YYYY", "");
            endProps.inputProps.value =
              moment(value[1]).format(replaceAllFormat) + addyear;
          }
        }
      }
    },
    [locale]
  );

  const rules = useMemo(() => {
    let message = `${I18n.SetText("required", I18NextNs.validation)}`
    return {
      validate: (value) => {
        if (required) {
          return (
            (value[0] != null && value[1] != null) ||
            message + ` ${labelName}`
          );
        }
      },
    };
  }, [required, labelName, SecureStorage.Get(I18n.envI18next)]);

  useMemo(() => {
    let lang = SecureStorage.Get(I18n.envI18next);
    setLocale(lang === Language.th ? "th" : "en");
  }, [SecureStorage.Get(I18n.envI18next)]);

  return (
    <Controller
      key={name}
      name={name}
      control={control}
      rules={rules}
      defaultValue={[null, null]}
      render={({
        field: { onChange, onBlur, value, ref },
        fieldState: { invalid, error },
      }) => (
        <LocalizationProvider
          adapterLocale={locale}
          dateAdapter={CustomAdapter as any}
        >
          <DateRangePicker
            value={value}
            onChange={(e) => {
              let arrValid = [null, null];
              let IsValidStart = (e[0] && moment(e[0]).isValid()) ?? false;
              if (IsValidStart) {
                arrValid[0] = e[0];
              }

              let IsValidEnd = (e[1] && moment(e[1]).isValid()) ?? false;
              if (IsValidEnd) {
                arrValid[1] = e[1];
              }

              onChange(arrValid);
              props.onChange && props.onChange(arrValid);
            }}
            components={{
              OpenPickerIcon: EventIcon,
            }}
            disabled={disabled}
            PopperProps={{
              placement: "bottom-end",
              anchorEl: anchorEl,
              sx: {
                zIndex: 10,
              },
            }}
            showToolbar={false}
            defaultCalendarMonth={defaultCalendarMonth}
            inputFormat={format}
            minDate={minDate || null}
            maxDate={maxDate || null}
            calendars={calendarsCount}
            componentsProps={{
              actionBar: {
                actions: ["clear"],
              },
            }}
            disablePast={disablePast}
            open={open}
            onOpen={() => {
              if (!props.disabled) {
                setOpen(true);
              }
            }}
            onClose={() => setOpen(false)}
            renderInput={(startProps, endProps) => {
              setFormatDate(startProps, endProps, value);
              startProps.inputProps.placeholder = maskMap[locale];
              endProps.inputProps.placeholder = maskMap[locale];
              return (
                <>
                  <Stack
                    style={{
                      display: "flex",
                      flexDirection: "row",
                      alignItems: "center",
                    }}
                  >
                    <TextField
                      disabled={disabled}
                      size={"small"}
                      {...startProps}
                      error={invalid}
                      fullWidth
                      required={required}
                      label={labelStart}
                      autoComplete="off"
                      focused={false}
                      value={startProps.inputProps.value || ""}
                      onClick={(e) => {
                        setOpen(true);
                        setAnchorEl(e.currentTarget);
                      }}
                      helperText={
                        isShowMessageError && error ? error.message : null
                      }
                      InputProps={{
                        endAdornment: (
                          <InputAdornment position="end">
                            <IconButton
                              onClick={() => !disabled && setOpen(true)}
                              style={{
                                marginRight: "-15px",
                              }}
                              disabled={disabled}
                            >
                              <EventIcon />
                            </IconButton>
                          </InputAdornment>
                        ),
                      }}
                      inputProps={{ readOnly: true }}
                      sx={{
                        "label.MuiInputLabel-shrink": {
                          top: "0px",
                        },
                        ".MuiInputLabel-outlined": {
                          top: "0px",
                        },
                      }}
                    />
                  </Stack>
                  <p
                    style={{
                      padding: "0px",
                      marginTop: "9px",
                      paddingLeft: "10px",
                      paddingRight: "10px",
                      display: "flex",
                    }}
                  >
                    <span>-</span>
                  </p>
                  <Stack
                    style={{
                      display: "flex",
                      flexDirection: "row",
                      alignItems: "center",
                    }}
                  >
                    <TextField
                      disabled={disabled}
                      size={"small"}
                      {...endProps}
                      error={invalid}
                      onClick={() => setOpen(true)}
                      required={required}
                      label={labelEnd}
                      autoComplete="off"
                      focused={false}
                      value={endProps.inputProps.value || ""}
                      fullWidth
                      inputProps={{ readOnly: true }}
                      InputProps={{
                        endAdornment: (
                          <InputAdornment position="end">
                            <IconButton
                              onClick={() => !disabled && setOpen(true)}
                              style={{
                                marginRight: "-15px",
                              }}
                              disabled={disabled}
                            >
                              <EventIcon />
                            </IconButton>
                          </InputAdornment>
                        ),
                      }}
                      sx={{
                        "label.MuiInputLabel-shrink": {
                          top: "0px",
                        },
                        ".MuiInputLabel-outlined": {
                          top: "0px",
                        }
                      }}
                    />
                  </Stack>
                </>
              );
            }}
          />
        </LocalizationProvider>
      )}
    />
  );
};

export const MonthYearRangePickerItem = (props: IMonthYearRangePickerProps) => {
  const {
    name,
    fullWidth = false,
    IsMonthYearPicker = false,
    IsMonthPicker = false,
    IsYearPicker = false,
    isShowMessageError = true,
    isShowIcon = true,
    onDisableDay,
    id,
    labelStart = "",
    labelEnd = "",
    labelName = "",
  } = props;
  const minDate = props.minDate ?? null;
  const maxDate = props.maxDate ?? null;
  const required = props.required ?? false;
  const disabled = props.disabled ?? false;
  const view = IsMonthYearPicker
    ? ["year", "month"]
    : IsMonthPicker
      ? ["month"]
      : IsYearPicker
        ? ["year"]
        : props.view ?? ["year", "month", "day"];
  let format = IsMonthYearPicker
    ? "MMMM/YYYY"
    : IsMonthPicker
      ? "MMMM"
      : IsYearPicker
        ? "YYYY"
        : props.format || "DD/MM/YYYY";
  const formatDefault = format;
  const [openStart, setOpenStart] = useState(false);
  const [openEnd, setOpenEnd] = useState(false);
  const [dStartDate, setStartDate] = useState(minDate);
  const [locale, setLocale] = useState<string>(language);

  const { control } = useFormContext();

  /** funtion **/
  const ReplaceThFormat = (text) => {
    return text
      ? text
        .toLowerCase()
        .replaceAll("d", "ว")
        .replaceAll("m", "ด")
        .replaceAll("y", "ป")
      : "";
  };

  const maskMap = {
    en: formatDefault,
    th: ReplaceThFormat(formatDefault),
  };

  const [objCheckReplace] = useState(() => {
    localeMap(locale);
    let objReturn = { isReplaceYear: false, indexArr: 0, splitText: "" };
    let formatLower = formatDefault ? formatDefault.toLowerCase() : "";
    if (formatLower.includes("YYYY")) {
      let splitText = formatDefault
        .substr(formatLower.indexOf("YYYY") - 1, 1)
        .toLowerCase();
      if (splitText !== "Y") {
        objReturn.splitText = splitText;
        objReturn.indexArr = formatLower
          .split(splitText)
          .findIndex((e) => e === "YYYY");
      }
      objReturn.isReplaceYear = true;
    }
    return objReturn;
  });

  const setFormatDate = useCallback(
    (params, value) => {
      if (locale === "th" && params.inputProps.value) {
        if (format === "MMMM/YYYY") {
          let convertDateToString: string = moment(value)
            .locale("th")
            .format(format);
          let year = convertDateToString.split("/")[1];
          let addyear =
            parseInt(year) > 2500 ? parseInt(year) : parseInt(year) + 543;
          params.inputProps.value = convertDateToString.replaceAll(
            `${year}`,
            `${addyear}`
          );
        } else if (format === "YYYY") {
          let year: string = moment(value).format(format);
          let addyear =
            parseInt(year) > 2500 ? parseInt(year) : parseInt(year) + 543;
          params.inputProps.value = addyear;
        } else {
          let convertDateToString: string = moment(value).format(format);
          let year = objCheckReplace.splitText
            ? convertDateToString.split(objCheckReplace.splitText)[
            objCheckReplace.indexArr
            ]
            : convertDateToString.substr(6, 4);
          let addyear = parseInt(year) + 543;
          let replaceAllFormat = format.replaceAll("YYYY", "");
          params.inputProps.value =
            moment(value).format(replaceAllFormat) + addyear;
        }
      }
    },
    [locale]
  );

  const rules = useMemo(() => {
    let message = `${I18n.SetText("required", I18NextNs.validation)}`
    return {
      validate: (value) => {
        if (value[0] != null && value[1] != null) {
          return value[1].year() >= value[0].year() || `invalid ${labelName}`;
        }
        if (required) {
          return (
            (value[0] != null && value[1] != null) ||
            message + `${labelName}`
            // `${I18n.setText("required")} ${labelName}`
          );
        }
      },
    };
  }, [required, labelName, SecureStorage.Get(I18n.envI18next)]);

  useMemo(() => {
    let lang = SecureStorage.Get(I18n.envI18next);
    setLocale(lang === Language.th ? "th" : "en");
  }, [SecureStorage.Get(I18n.envI18next)]);

  return (
    <>
      <Controller
        key={name}
        name={name}
        control={control}
        rules={rules}
        shouldUnregister={true}
        defaultValue={[null, null]}
        render={({
          field: { onChange, onBlur, value, ref },
          fieldState: { invalid, error },
        }) => (
          <LocalizationProvider
            adapterLocale={locale}
            dateAdapter={CustomAdapter as any}
          >
            <FormControl
              fullWidth={fullWidth}
              sx={{
                ".MuiFormControl-root": {
                  display: "flex !important",
                },
                "label.MuiInputLabel-shrink": {
                  top: "0px",
                },
                ".MuiInputLabel-outlined": {
                  top: "0px",
                }
              }}
            >
              <Grid container spacing={0}>
                <Grid item flex={1}>
                  <DatePicker
                    dayOfWeekFormatter={(day) =>
                      day.charAt(0).toUpperCase() === "เ"
                        ? "ส"
                        : day.charAt(0).toUpperCase()
                    }
                    inputRef={ref}
                    value={value[0] ? moment(value[0], formatDefault) : null}
                    onChange={(e) => {
                      let IsValid = moment(e).isValid();
                      if (IsValid) {
                        value[0] = e;
                        if (e > value[1]) {
                          value[1] = null;
                        }
                        setStartDate(e);
                        onChange(value);
                        props.onChange && props.onChange(value);
                        IsYearPicker && setOpenEnd(true);
                      } else {
                        onChange([null, value[1]]);
                        props.onChange && props.onChange([null, value[1]]);
                      }
                    }}
                    onMonthChange={() => {
                      if (!value[1]) {
                        setOpenEnd(true);
                      }
                    }}
                    shouldDisableDate={onDisableDay}
                    open={openStart}
                    onOpen={() => {
                      if (!props.disabled) {
                        setOpenStart(true);
                      }
                    }}
                    onClose={() => setOpenStart(false)}
                    minDate={minDate}
                    maxDate={maxDate}
                    label={labelStart}
                    inputFormat={formatDefault}
                    disabled={disabled}
                    views={view}
                    showToolbar={false}
                    showDaysOutsideCurrentMonth={false}
                    PaperProps={
                      IsMonthPicker
                        ? {
                          sx: {
                            ".MuiPickersCalendarHeader-root": {
                              display: "none !important",
                            },
                          },
                        }
                        : {}
                    }
                    componentsProps={{
                      actionBar: {
                        actions: ["clear"],
                      },
                    }}
                    renderInput={(params) => {
                      params.inputProps.placeholder = maskMap[locale];
                      !IsMonthPicker && setFormatDate(params, value[0]);
                      return (
                        <TextField
                          {...params}
                          sx={{
                            ".MuiOutlinedInput-root": {
                              padding: isShowIcon
                                ? "0px 15px 0px 0px!important"
                                : "0px !important",
                              " fieldset > legend > span": {
                                padding: "0px !important",
                              },
                            },
                            cursor: "pointer !important",
                            fontWeight: 600,
                          }}
                          inputProps={{ readOnly: true }}
                          size={"small"}
                          label={labelStart}
                          id={id + "_start"}
                          error={invalid}
                          required={required}
                          disabled={disabled}
                          focused={false}
                          value={params.inputProps.value || ""}
                          autoComplete={"off"}
                          onClick={() => {
                            if (!props.disabled) {
                              setOpenStart(true);
                            }
                          }}
                          helperText={
                            isShowMessageError && error ? error.message : null
                          }
                        />
                      );
                    }}
                  />
                </Grid>
                <Grid item>
                  <p
                    style={{
                      padding: "0px",
                      marginTop: "9px",
                      paddingLeft: "10px",
                      paddingRight: "10px",
                      display: "flex",
                    }}
                  >
                    <span>-</span>
                  </p>
                </Grid>
                <Grid item flex={1}>
                  <DatePicker
                    dayOfWeekFormatter={(day) =>
                      day.charAt(0).toUpperCase() === "เ"
                        ? "ส"
                        : day.charAt(0).toUpperCase()
                    }
                    inputRef={ref}
                    value={value[1] ? moment(value[1], formatDefault) : null}
                    onChange={(e) => {
                      let IsValid = moment(e).isValid();
                      if (IsValid) {
                        value[1] = e;
                        onChange(value);
                        props.onChange && props.onChange(value);
                      } else {
                        onChange([value[0], null]);
                        props.onChange && props.onChange([value[0], null]);
                      }
                    }}
                    shouldDisableDate={onDisableDay}
                    open={openEnd}
                    onOpen={() => {
                      if (!props.disabled) {
                        setOpenEnd(true);
                      }
                    }}
                    onClose={() => setOpenEnd(false)}
                    minDate={dStartDate}
                    maxDate={maxDate}
                    label={labelEnd}
                    inputFormat={formatDefault}
                    disabled={disabled}
                    views={view}
                    showToolbar={false}
                    showDaysOutsideCurrentMonth={false}
                    PaperProps={
                      IsMonthPicker
                        ? {
                          sx: {
                            ".MuiPickersCalendarHeader-root": {
                              display: "none !important",
                            },
                          },
                        }
                        : {}
                    }
                    componentsProps={{
                      actionBar: {
                        actions: ["clear"],
                      },
                    }}
                    renderInput={(params) => {
                      params.inputProps.placeholder = maskMap[locale];
                      !IsMonthPicker && setFormatDate(params, value[1]);
                      return (
                        <TextField
                          {...params}
                          sx={{
                            ".MuiOutlinedInput-root": {
                              padding: isShowIcon
                                ? "0px 15px 0px 0px!important"
                                : "0px !important",
                              " fieldset > legend > span": {
                                padding: "0px !important",
                              },
                            },
                            cursor: "pointer !important",
                            fontWeight: 600,
                          }}
                          inputProps={{ readOnly: true }}
                          size={"small"}
                          label={labelEnd}
                          id={id + "_end"}
                          error={invalid}
                          required={required}
                          disabled={disabled}
                          focused={false}
                          value={params.inputProps.value || ""}
                          autoComplete={"off"}
                          onClick={() => {
                            if (!props.disabled) {
                              setOpenEnd(true);
                            }
                          }}
                        />
                      );
                    }}
                  />
                </Grid>
              </Grid>
            </FormControl>
          </LocalizationProvider>
        )}
      />
    </>
  );
};

export const QuarterPickerItem = (props: IQuarterPickerProps) => {
  const {
    name,
    fullWidth = true,
    isShowMessageError = true,
    label,
    id,
    required = false,
    disabled = false,
    minQuarter,
    maxQuarter,
  } = props;

  const [locale, setLocale] = useState<string>(language);
  const [open, setOpen] = useState(false);

  const { control } = useFormContext();

  useMemo(() => {
    let lang = SecureStorage.Get(I18n.envI18next);
    setLocale(lang === Language.th ? "th" : "en");
  }, [SecureStorage.Get(I18n.envI18next)]);

  const getOpObjValue = (val) => {
    let res = null;
    if (val)
      res = (locale === "en" ? arrOptionEN : arrOptionTH).find(
        (op) => op.value === val
      );
    return res;
  };

  const rules = useMemo(() => {
    let message = `${I18n.SetText("required", I18NextNs.validation)}`
    return {
      required: {
        value: required,
        message: message + ` ${label}`,
        // message: `${I18n.setText("required")} ${label}`,
      },
    };
  }, [required, label, SecureStorage.Get(I18n.envI18next)]);

  return (
    <>
      <Controller
        name={name}
        control={control}
        rules={rules}
        render={({
          field: { onChange, onBlur, value, ref },
          fieldState: { invalid, error },
        }) => {
          return (
            <>
              <Autocomplete
                ref={ref}
                id={id}
                disabled={disabled}
                fullWidth={fullWidth}
                open={open}
                size={"small"}
                options={locale === "en" ? arrOptionEN : arrOptionTH}
                value={getOpObjValue(value)}
                noOptionsText={I18n.SetText("noInformationFound")}
                renderOption={(props, option: any) => {
                  return (
                    <ListItem
                      {...props}
                      key={option.value}
                      style={
                        option.color ? { backgroundColor: option.color } : {}
                      }
                    >
                      {option.label}
                    </ListItem>
                  );
                }}
                getOptionLabel={(itemOption: any) => {
                  return `${itemOption?.label}`;
                }}
                filterOptions={(options, params) => {
                  console.log("options", options)
                  console.log("minQuarter", minQuarter)
                  console.log("maxQuarter", maxQuarter)
                  if (minQuarter) {
                    options = options.filter((f) => f.value >= minQuarter);
                  }

                  if (maxQuarter) {
                    options = options.filter((f) => f.value <= maxQuarter);
                  }
                  return options;
                }}
                renderInput={(params) => {
                  let TooltipTitle = null;
                  if (disabled) {
                    TooltipTitle = params.inputProps.value;
                  }
                  return (
                    <Tooltip
                      title={ParseHtml(TooltipTitle)}
                      disableHoverListener={TooltipTitle ? false : true}
                      disableFocusListener
                    >
                      <TextField
                        {...params}
                        name={name}
                        id={id}
                        error={invalid}
                        required={props.required}
                        disabled={disabled}
                        label={label}
                        size={"small"}
                        fullWidth={fullWidth}
                        onClickCapture={() => !disabled && setOpen(true)}
                        InputProps={{
                          ...params.InputProps,
                          endAdornment: (
                            <>
                              <InputAdornment position="end">
                                {value && (
                                  <IconButton
                                    onClick={() => {
                                      onChange(null);
                                    }}
                                    disabled={disabled}
                                  >
                                    {!disabled && <CloseIcon />}
                                  </IconButton>
                                )}
                                <IconButton disabled={disabled}>
                                  <EventIcon />
                                </IconButton>
                              </InputAdornment>
                            </>
                          ),
                        }}
                        sx={{
                          "label.MuiInputLabel-shrink": {
                            top: "0px",
                          },
                          ".MuiInputLabel-outlined": {
                            top: "0px",
                          },
                          ".MuiOutlinedInput-root": {
                            paddingRight: "0px !important",
                          },
                        }}
                      />
                    </Tooltip>
                  );
                }}
                onChange={(event, value) => {
                  setOpen(false);
                  onChange(value != null ? value["value"] : null);
                  props.onChange && props.onChange(value);
                }}
                onBlur={(event) => {
                  setOpen(false);
                  onBlur();
                  props.onBlur && props.onBlur(event);
                }}
                onKeyDown={(event) => {
                  props.onKeyDown && props.onKeyDown(event);
                }}
                onKeyUp={(event) => {
                  props.onKeyUp && props.onKeyUp(event);
                }}
              />
              {isShowMessageError && error ? (
                <FormHelperText>
                  {error.message}
                </FormHelperText>
              ) : null}
            </>
          );
        }}
      />
    </>
  );
};

export const QuarterRangePickerItem = (props: IQuarterPickerProps) => {
  const {
    name,
    fullWidth = true,
    isShowMessageError = true,
    labelName,
    labelStart,
    labelEnd,
    id,
    required = false,
    disabled = false,
    minQuarter,
    maxQuarter,
  } = props;

  const [openStart, setOpenStart] = useState(false);
  const [openEnd, setOpenEnd] = useState(false);
  const [nStartQuarter, setStartQuarter] = useState(minQuarter);
  const [locale, setLocale] = useState<string>(language);

  const { control } = useFormContext();

  useMemo(() => {
    let lang = SecureStorage.Get(I18n.envI18next);
    setLocale(lang === Language.th ? "th" : "en");
  }, [SecureStorage.Get(I18n.envI18next)]);

  const getOpObjValue = (val) => {
    let res = null;
    if (val)
      res = (locale === "en" ? arrOptionEN : arrOptionTH).find(
        (op) => op.value === val
      );
    return res;
  };

  const rules = useMemo(() => {
    let message = `${I18n.SetText("required", I18NextNs.validation)}`
    return {
      validate: (value) => {
        if (value[0] != null && value[1] != null) {
          return value[1] >= value[0] || `invalid ${labelName}`;
        }
        if (required) {
          return (
            (value[0] != null && value[1] != null) ||
            message + `${labelName}`
            // `${I18n.setText("required")} ${labelName}`
          );
        }
      },
    };
  }, [required, labelName, SecureStorage.Get(I18n.envI18next)]);

  return (
    <>
      <Controller
        name={name}
        control={control}
        rules={rules}
        defaultValue={[null, null]}
        render={({
          field: { onChange, onBlur, value, ref },
          fieldState: { invalid, error },
        }) => (
          <>
            <FormControl
              fullWidth={fullWidth}
              sx={{
                "label.MuiInputLabel-shrink": {
                  top: "0px",
                },
                ".MuiInputLabel-outlined": {
                  top: "0px",
                }
              }}
            >
              <Grid container spacing={0}>
                <Grid item flex={1}>
                  <Autocomplete
                    ref={ref}
                    id={id}
                    disabled={disabled}
                    fullWidth={fullWidth}
                    open={openStart}
                    size={"small"}
                    options={locale === "en" ? arrOptionEN : arrOptionTH}
                    value={getOpObjValue(value[0])}
                    noOptionsText={I18n.SetText("noInformationFound")}
                    renderOption={(props, option: any) => {
                      return (
                        <ListItem
                          {...props}
                          key={option.value}
                          style={
                            option.color
                              ? { backgroundColor: option.color }
                              : {}
                          }
                        >
                          {option.label}
                        </ListItem>
                      );
                    }}
                    getOptionLabel={(itemOption: any) => {
                      return `${itemOption?.label}`;
                    }}
                    filterOptions={(options, params) => {
                      if (minQuarter) {
                        options = options.filter((f) => f.value >= minQuarter);
                      }

                      if (maxQuarter) {
                        options = options.filter((f) => f.value <= maxQuarter);
                      }
                      return options;
                    }}
                    renderInput={(params) => {
                      let TooltipTitle = null;
                      if (disabled) {
                        TooltipTitle = params.inputProps.value;
                      }
                      return (
                        <Tooltip
                          title={ParseHtml(TooltipTitle)}
                          disableHoverListener={TooltipTitle ? false : true}
                          disableFocusListener
                        >
                          <TextField
                            {...params}
                            name={name}
                            id={id}
                            error={invalid}
                            required={props.required}
                            disabled={disabled}
                            label={labelStart}
                            size={"small"}
                            fullWidth={fullWidth}
                            onClickCapture={() => !disabled && setOpenStart(true)}
                            focused={openStart}
                            InputProps={{
                              ...params.InputProps,
                              endAdornment: (
                                <>
                                  <InputAdornment position="end">
                                    {value[0] && (
                                      <IconButton
                                        onClick={() => {
                                          onChange([null, value[1]]);
                                        }}
                                      >
                                        {!disabled && <CloseIcon />}
                                      </IconButton>
                                    )}
                                    <IconButton disabled={disabled}>
                                      <EventIcon />
                                    </IconButton>
                                  </InputAdornment>
                                </>
                              ),
                            }}
                            sx={{
                              ".MuiOutlinedInput-root": {
                                paddingRight: "0px !important",
                              },
                            }}
                          />
                        </Tooltip>
                      );
                    }}
                    onChange={(event, valueChange) => {
                      setOpenStart(false);
                      if (valueChange != null) {
                        let val = valueChange["value"];
                        value[0] = val;
                        if (val > value[1]) {
                          value[1] = null;
                        }
                        setStartQuarter(val);
                        onChange(value);
                        if (props.onChange) {
                          props.onChange(value);
                        }


                        //set focus end
                        if (!value[1]) {
                          const elEnd = document.getElementById(`end_${id}`);
                          elEnd.click();
                          setOpenEnd(true);
                        }
                      } else {
                        onChange([null, value[1]]);
                        props.onChange && props.onChange([null, value[1]]);
                      }
                    }}
                    onBlur={(event) => {
                      setOpenStart(false);
                      setOpenEnd(false);
                      onBlur();
                      props.onBlur && props.onBlur(event);
                    }}
                    onKeyDown={(event) => {
                      props.onKeyDown && props.onKeyDown(event);
                    }}
                    onKeyUp={(event) => {
                      props.onKeyUp && props.onKeyUp(event);
                    }}
                  />
                </Grid>
                <Grid item>
                  <p
                    style={{
                      padding: "0px",
                      marginTop: "9px",
                      paddingLeft: "10px",
                      paddingRight: "10px",
                      display: "flex",
                    }}
                  >
                    <span>-</span>
                  </p>
                </Grid>
                <Grid item flex={1}>
                  <Autocomplete
                    ref={ref}
                    id={`end_${id}`}
                    disabled={disabled}
                    fullWidth={fullWidth}
                    open={openEnd}
                    size={"small"}
                    options={locale === "en" ? arrOptionEN : arrOptionTH}
                    value={getOpObjValue(value[1])}
                    noOptionsText={I18n.SetText("noInformationFound")}
                    renderOption={(props, option: any) => {
                      return (
                        <ListItem
                          {...props}
                          key={option.value}
                          style={
                            option.color
                              ? { backgroundColor: option.color }
                              : {}
                          }
                        >
                          {option.label}
                        </ListItem>
                      );
                    }}
                    getOptionLabel={(itemOption: any) => {
                      return `${itemOption?.label}`;
                    }}
                    filterOptions={(options, params) => {
                      if (nStartQuarter) {
                        options = options.filter(
                          (f) => f.value >= nStartQuarter
                        );
                      }

                      if (maxQuarter) {
                        options = options.filter((f) => f.value <= maxQuarter);
                      }
                      return options;
                    }}
                    renderInput={(params) => {
                      let TooltipTitle = null;
                      if (disabled) {
                        TooltipTitle = params.inputProps.value;
                      }
                      return (
                        <Tooltip
                          title={ParseHtml(TooltipTitle)}
                          disableHoverListener={TooltipTitle ? false : true}
                          disableFocusListener
                        >
                          <TextField
                            {...params}
                            name={name}
                            id={id}
                            error={invalid}
                            required={props.required}
                            disabled={disabled}
                            label={labelEnd}
                            size={"small"}
                            fullWidth={fullWidth}
                            onClickCapture={() => !disabled && setOpenEnd(true)}
                            focused={openEnd}
                            InputProps={{
                              ...params.InputProps,
                              endAdornment: (
                                <>
                                  <InputAdornment position="end">
                                    {value[1] && (
                                      <IconButton
                                        onClick={() => {
                                          onChange([value[0], null]);
                                        }}
                                      >
                                        {!disabled && <CloseIcon />}
                                      </IconButton>
                                    )}
                                    <IconButton disabled={disabled}>
                                      <EventIcon />
                                    </IconButton>
                                  </InputAdornment>
                                </>
                              ),
                            }}
                            sx={{
                              ".MuiOutlinedInput-root": {
                                paddingRight: "0px !important",
                              },
                            }}
                          />
                        </Tooltip>
                      );
                    }}
                    onChange={(event, valueChange) => {
                      setOpenEnd(false);
                      if (valueChange != null) {
                        let val = valueChange["value"];
                        value[1] = val;
                        onChange(value);
                        props.onChange && props.onChange(value);
                      } else {
                        onChange([value[0], null]);
                        props.onChange && props.onChange([value[0], null]);
                      }
                    }}
                    onBlur={(event) => {
                      setOpenEnd(false);
                      onBlur();
                      props.onBlur && props.onBlur(event);
                    }}
                    onKeyDown={(event) => {
                      props.onKeyDown && props.onKeyDown(event);
                    }}
                    onKeyUp={(event) => {
                      props.onKeyUp && props.onKeyUp(event);
                    }}
                  />
                </Grid>
              </Grid>
            </FormControl>

            {isShowMessageError && error ? (
              <FormHelperText>
                {error.message}
              </FormHelperText>
            ) : null}
          </>
        )}
      />
    </>
  );
};