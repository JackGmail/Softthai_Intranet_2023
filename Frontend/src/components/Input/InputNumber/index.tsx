import { Controller, useFormContext } from "react-hook-form";
import { IInputNumber } from "./InputNumberProps";
import { FormHelperText, InputAdornment, TextField } from "@mui/material";
import { useMemo, useState } from "react";
import { I18n } from "utilities/utilities";
import { I18NextNs } from "enum/enum";

export const InputNumber = (props: IInputNumber) => {
  const {
    maxInteger = 10,
    maxDigits = 5,
    IsCanMinus = false,
    arrStringAllow = [],
    IsAllowScientific = false,
    isShowMessageError = true,
    min = null,
    max = null,
    id = null,
    required,
    label,
    defaultValue = null,
    disabled = false
  } = props;
  const [IsShrink, setIsShrink] = useState(false);
  const { control, register } = useFormContext();

  const _onChange = (e, onChangeControl) => {
    let sValue = e.target.value;
    let sVal = (sValue + "").replaceAll(/,/g, "");
    sVal = sVal.replaceAll(",", "");
    //เป็นตัวเลข || สามารถใส่ Scientific ได้
    if (
      isNumber(sVal) ||
      (IsAllowScientific && (sVal.includes("e") || sVal.includes("E")))
    ) {
      let nMaxInteger = maxInteger;
      let nMaxDigits = maxDigits;
      let arrNumber = sVal.split(".");

      if (IsCanMinus && sVal.includes("-")) {
        nMaxInteger += 1;
      }

      if (IsAllowScientific && (sVal.includes("e") || sVal.includes("E"))) {
        nMaxDigits += 4;
      }

      let sInteger = sVal.toString().includes(".")
        ? arrNumber[0]
        : sVal.toString();
      sInteger = sInteger.substring(0, nMaxInteger);

      //ป้องกันการกด 0 หลายๆตัว
      let nInteger = parseFloat(sInteger);
      if (nInteger === 0 || nInteger === -0) {
        let nVal = parseFloat(sVal);
        sInteger =
          ["-0", "-00", "-0.", "-00."].includes(sVal) || nVal < 0
            ? "-" + nInteger.toString()
            : nInteger.toString();
      } else if (nInteger > 0 || nInteger < 0) {
        //ป้องกันใส่ 0 หน้าต้วเลข
        if (
          !(IsAllowScientific && (sVal.includes("e") || sVal.includes("E")))
        ) {
          sInteger = nInteger.toString();
        }
      }

      let sDigit = sVal.toString().includes(".") ? arrNumber[1] : "";
      sDigit = sDigit.substring(0, nMaxDigits);

      if (arrNumber.length > 1) {
        sVal = sInteger + "." + sDigit;
      } else {
        sVal = sInteger;
      }

      if (nMaxDigits === 0) {
        sVal = sInteger.replaceAll(".", "");
      }

      let nVal = parseFloat(sVal);
      if (nVal >= 0 || nVal === 0 || (IsCanMinus && nVal < 0)) {
        e.target.value = sVal;
      } else if (
        IsAllowScientific &&
        (sVal.includes("e") || sVal.includes("E"))
      ) {
        e.target.value = sVal;
      } else {
        e.target.value = null;
      }
    } else {
      if (arrStringAllow.length > 0 || IsAllowScientific) {
        e.target.value = sVal;
      } else {
        if (IsCanMinus && sVal === "-") {
          e.target.value = "-";
        } else {
          e.target.value = sVal.substring(0, sVal.length - 1);
        }
      }
    }
    console.log("onChangeControl", e)
    onChangeControl(e);
    props.onChange && props.onChange(e.target.value, e);
  };

  const _onBlur = (e, onChangeControl) => {
    let sValue = e.target.value;
    let sVal = (sValue + "").replaceAll(/,/g, "");
    sVal = sVal.replaceAll(",", "");

    if (arrStringAllow.length > 0) {
      let IsAllow = arrStringAllow.includes(sVal);

      //Not Allow String && Not Number
      if (!isNumber(sVal) && !IsAllow) {
        e.target.value = null;
        onChangeControl(e);
      }
    } else if (IsAllowScientific) {
      //Check is not Scientific
      if (!isNumber(sVal)) {
        e.target.value = null;
        onChangeControl(e);
      }
    }
    props.onBlur && props.onBlur(e.target.value, e);
  };

  const rules = useMemo(() => {
    return {
      required: {
        value: required,
        message: `${I18n.SetText("required", I18NextNs.validation)} ${label}`,
      },
      validate: (value) => {
        if (value) {
          //Check Min,Max
          let sValue = value;
          let sVal = (sValue + "").replaceAll(/,/g, "");
          sVal = sVal.replaceAll(",", "");

          if (isNumber(sVal) && (min || max)) {
            let nVal = +sVal;
            if (min) {
              return (
                nVal >= min ||
                `${I18n.SetText("msgMaxMin")} ${min} ${I18n.SetText(
                  "to"
                )} ${max}`
              );
            }

            if (max) {
              return (
                nVal <= max ||
                `${I18n.SetText("msgMaxMin")} ${min} ${I18n.SetText(
                  "to"
                )} ${max}`
              );
            }
          }
        }
      },
    };
  }, [required, label, disabled]);

  return (
    <Controller key={props.name} name={props.name} control={control} rules={rules} shouldUnregister={true} defaultValue={defaultValue}
      render={({
        field: { onChange, onBlur, value, ref },
        fieldState: { invalid, error },
      }) => {
        let nValue = null;
        if (value) {
          nValue = CommaSeparateNumber(value, maxDigits, props.ShowDigits);
        }
        return <>
          <div style={{ display: "flex", position: "relative" }}>
            <TextField
              type={"text"}
              id={id}
              inputRef={ref}
              {...register(props.name)}
              style={{
                width: "100%",
                ...props?.style,
              }}
              {...props}
              error={error?.message != null}
              variant={props.variant || "outlined"}
              size="small"
              label={props.label}
              inputProps={{
                style: {
                  textAlign: props.textAlign || "right",
                  paddingRight:
                    props.suffix && props.suffix !== ""
                      ? props.suffix.length > 20
                        ? `${props.suffix.length * 8 + 30}px`
                        : props.suffix.length > 15
                          ? `${props.suffix.length * 7 + 40}px`
                          : props.suffix.length > 10
                            ? `${props.suffix.length * 6 + 50}px`
                            : props.suffix.length > 5
                              ? `${props.suffix.length * 5 + 50}px`
                              : props.suffix.length > 3
                                ? `${props.suffix.length * 4 + 45}px`
                                : `${props.suffix.length * 3 + 40}px`
                      : "",
                },
              }}
              InputProps={{
                startAdornment: props.startAdornment ? (
                  <InputAdornment position="start">
                    {props.startAdornment}
                  </InputAdornment>
                ) : null,
                endAdornment: props.endAdornment ? (
                  <InputAdornment position="end">
                    {props.endAdornment}
                  </InputAdornment>
                ) : null,
              }}
              InputLabelProps={
                props.startAdornment || props.endAdornment
                  ? {
                    shrink: true,
                  }
                  : {
                    shrink: !!value || IsShrink,
                  }
              }
              value={nValue}
              onChange={(e) => {
                _onChange(e, onChange);
              }}
              onBlur={(e) => {
                _onBlur(e, onChange);
                setIsShrink(false);
              }}
              placeholder={props.placeholder}
              disabled={disabled || false}
              onKeyDown={(event) => {
                props.onKeyDown && props.onKeyDown(event);
              }}
              onKeyUp={(event) => {
                props.onKeyUp && props.onKeyUp(event);
              }}
              onFocus={() => {
                setIsShrink(true);
              }}
            />
            {props.suffix !== undefined ? (
              <span
                style={{
                  fontSize: 16,
                  borderColor: "rgba(255, 255, 255, 0)",
                  backgroundColor: "rgba(255, 255, 255, 0)",
                  paddingTop: "0.77rem",
                  paddingLeft: "0.3rem",
                  lineHeight: "18px",
                  height: "100%",
                  right: "2%",
                  position: "absolute",
                  color: "#929292",
                  borderLeft: "1.2px solid #d9d9d9",
                }}
              >
                {props.suffix}
              </span>
            ) : (
              <div></div>
            )}
          </div>
          {isShowMessageError && error ? (
            <FormHelperText>{error.message}</FormHelperText>
          ) : null}
        </>
      }}
    />
  );
};

export const isNumber = (n) => {
  return typeof n != "boolean" && !isNaN(n);
};

export const convertStringToNumber = (val) => {
  if (val !== undefined && val != null) {
    let values = (val + "").replaceAll(",", "");
    if (values && values !== "" && values != null && isNumber(values)) {
      let valConve = parseFloat(values);
      if (valConve + "" !== "NaN") {
        return valConve;
      } else {
        return null;
      }
    } else {
      return null;
    }
  }
};

export const CommaSeparateNumber = (value: any, _MaxDigits?, _MaxInteger?) => {
  if (value && isNumber(value)) {
    let indValue = (value + "").split(".");
    let valueResult = indValue[0];
    if (value + "" !== "0") {
      while (/(\d+)(\d{3})/.test(valueResult)) {
        valueResult = valueResult.replace(/(\d+)(\d{3})/, "$1" + "," + "$2");
      }
      if (indValue.length > 1) {
        valueResult = valueResult + "." + indValue[1];
      }
      return valueResult;
    } else {
      return "0";
    }
  }
};
