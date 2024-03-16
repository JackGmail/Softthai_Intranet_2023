import React, { useMemo } from "react";
import { FormHelperText, TextField } from "@mui/material";
import { Controller, FieldError, useFormContext } from "react-hook-form";
import { I18n } from "utilities/utilities";
import { I18NextNs } from "enum/enum";

export const TextBox = (props: ITextbox) => {
    //#region Variable
    const {
        id,
        name,
        label,
        placeholder,
        defaultValue = null,
        pattern,
        maxLength = 200,
        type = "text",
        variant = "outlined",
        size = "small",
        margin = "dense",
        required = false,
        IsShowMessageError = true,
        disabled = false,
        fullWidth = true,
        startAdornment,
        endAdornment,
        autoComplete = "new-password",
        IsShrink
    } = props;
    const { control, register } = useFormContext();
    //#endregion

    const rules = useMemo(() => {
        let objvalidate = {};
        if (!disabled) {
            if (required) {
                objvalidate["required"] = {
                    value: required
                }
            }
            switch (type) {
                case "text":
                    switch (pattern) {
                        case "th":
                            objvalidate["pattern"] = {
                                value: /^[\u0E00-\u0E7F\s]+$/
                            }
                            break;
                        case "th-number":
                            objvalidate["pattern"] = {
                                value: /^[\u0E00-\u0E7F0-9\s]+$/
                            }
                            break;
                        case "en":
                            objvalidate["pattern"] = {
                                value: /^[A-Za-z\s]+$/g
                            }
                            break;
                        case "en-number":
                            objvalidate["pattern"] = {
                                value: /^[A-Za-z0-9\s]+$/g
                            }
                            break;
                    }
                    break;
                case "email":
                    objvalidate["pattern"] = {
                        value: /^[a-z0-9._%+-.]+@[a-z0-9.-]+\.[a-z]{2,4}$/
                    }
                    break;
                case "password":
                    if (pattern === "password") {
                        objvalidate["pattern"] = {
                            value: /((?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])(?=.{8,}))/g
                        }
                    }
                    break;
            }
        }
        return objvalidate;
    }, [required, disabled])

    const GetMessagevalidate = (error: FieldError) => {
        let sMessage = "";
        if (error) {
            switch (error.type) {
                case "required":
                    sMessage = `${I18n.SetText("required", I18NextNs.validation)}`;

                    if (label) {
                        sMessage = sMessage + ` ${label} `;
                    }
                    break;
                case "pattern":
                    switch (type) {
                        case "text":
                            switch (pattern) {
                                case "th":
                                    sMessage = `${I18n.SetText("patternThai", I18NextNs.validation)}`;
                                    break;
                                case "th-number":
                                    sMessage = `${I18n.SetText("patternThaiNumber", I18NextNs.validation)}`;
                                    break;
                                case "en":
                                    sMessage = `${I18n.SetText("patternEng", I18NextNs.validation)}`;
                                    break;
                                case "en-number":
                                    sMessage = `${I18n.SetText("patternEngNumber", I18NextNs.validation)}`;
                                    break;
                            }
                            break;
                        case "email":
                            sMessage = `${I18n.SetText("patternEmail", I18NextNs.validation)}`;
                            break;
                        case "password":
                            sMessage = `${I18n.SetText("patternPassword", I18NextNs.validation)}`;
                            break;
                    }
                    break;
            }
        }
        return sMessage;
    }

    return (
        <Controller key={"key" + name} name={name} control={control} rules={rules} shouldUnregister={true} defaultValue={defaultValue}
            render={({
                field: { onChange, ref, value },
                fieldState: { invalid, error } }) => {
                let helperText = IsShowMessageError ? GetMessagevalidate(error) : "";
                let isValid = invalid && !disabled;
                let InputLabelProps = {};
                let sValue = value + "";
                if (IsShrink || (sValue.length > 0 && value)) {
                    InputLabelProps["shrink"] = true
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
                        type={type}
                        error={isValid}
                        size={size}
                        fullWidth={fullWidth}
                        margin={margin}
                        inputProps={{ maxLength: maxLength }}
                        InputProps={{
                            startAdornment: startAdornment,
                            endAdornment: endAdornment
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
                            onChange(e);
                        }}
                        autoComplete={autoComplete}
                        style={props.style}
                    >
                    </TextField>
                    {
                        (IsShowMessageError && isValid) && <FormHelperText>{helperText}</FormHelperText>
                    }
                </React.Fragment>)
            }}
        />

    );
};