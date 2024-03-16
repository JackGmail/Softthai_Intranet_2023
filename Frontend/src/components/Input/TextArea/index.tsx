import React, { useMemo } from "react";
import { FormHelperText, TextField } from "@mui/material";
import { Controller, useFormContext } from "react-hook-form";
import { I18n } from "utilities/utilities";
import { I18NextNs } from "enum/enum";

export const TextArea = (props: ITextarea) => {
    //#region Variable
    const {
        id,
        name,
        label,
        placeholder,
        defaultValue = null,
        maxLength = 500,
        variant = "outlined",
        size = "small",
        margin = "normal",
        required = false,
        IsShowMessageError = true,
        disabled = false,
        fullWidth = true,
        startAdornment,
        endAdornment,
        rows = 3,
        maxRows = 5,
        autoComplete = "new-password",
        IsShrink
    } = props;
    const { control, register } = useFormContext();
    //#endregion

    const rules = useMemo(() => {
        let objvalidate = {};
        objvalidate["required"] = {
            value: required,
            message: `${I18n.SetText("required", I18NextNs.validation)} ${label}`
        }
        if (disabled) {
            objvalidate["disabled"] = disabled;
        }
        return objvalidate;
    }, [required, disabled])

    return (
        <React.Fragment>
            <Controller key={name} name={name} control={control} rules={rules} shouldUnregister={true} defaultValue={defaultValue}
                render={({
                    field: { onChange, ref, value },
                    fieldState: { invalid, error } }) => {
                    let helperText = IsShowMessageError ? error?.message : "";
                    let InputLabelProps = {};
                    let sValue = value + "";
                    if (IsShrink || (sValue.length > 0 && value)) {
                        InputLabelProps["shrink"] = true
                    }
                    let nMaxRows = maxRows;
                    if (maxRows < rows) {
                        nMaxRows = rows;
                    }
                    return (<React.Fragment>
                        <TextField
                            {...register(name)}
                            required={required}
                            id={id}
                            name={name}
                            inputRef={ref}
                            label={label}
                            placeholder={placeholder}
                            variant={variant}
                            type={"text"}
                            error={invalid}
                            size={size}
                            fullWidth={fullWidth}
                            margin={margin}
                            multiline={true}
                            inputProps={{
                                maxLength: maxLength
                            }}
                            InputProps={{
                                startAdornment: startAdornment,
                                endAdornment: endAdornment,
                            }}
                            InputLabelProps={InputLabelProps}
                            disabled={disabled}
                            onBlur={(e) => {
                                if (props.onBlur) {
                                    props.onBlur(e);
                                }
                            }}
                            onChange={(e) => {
                                if (props.onChange) {
                                    props.onChange(e);
                                }
                                onChange(e.target.value);
                            }}
                            onKeyDown={(e) => {
                                if (e.code === "Space") {
                                    e.stopPropagation()
                                }
                            }}
                            minRows={rows}
                            maxRows={nMaxRows}
                            autoComplete={autoComplete}
                            style={props.style}
                            value={value}
                        >
                        </TextField>
                        {
                            (IsShowMessageError && invalid) && <FormHelperText>{helperText}</FormHelperText>
                        }
                    </React.Fragment>)
                }}
            />
        </React.Fragment >
    );
};