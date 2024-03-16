import { useRef } from "react";
import { useFormContext, Controller } from "react-hook-form";
import { SwitchProps } from "./SwitchProps";
import Switch from "react-switch";
import { FormControl, Stack, Typography } from '@mui/material';
import { I18n } from "utilities/utilities";
import './Switch.css';
import { I18NextNs } from "enum/enum";

export const SwitchForm = (props: SwitchProps) => {
    const {
        label = "",
        id,
        name,
        checkText,
        uncheckText,
        disabled,
        fontColor = "#ffffff",
        onColor = "#8bc34a",
        offColor = "#ee3a0d",
        width = 130,
        direction = "column",
        defaultValue = true,
    } = props;
    const elementRef = useRef(null);
    const { control, register } = useFormContext();

    return (
        <Controller name={name} control={control} defaultValue={defaultValue}
            render={({
                field: { onChange, value, ref } }) => {
                let sCheckText = checkText ?? `${I18n.SetText("Switch.Check", I18NextNs.labelComponent)}`;
                let sUncheckText = uncheckText ?? `${I18n.SetText("Switch.UnCheck", I18NextNs.labelComponent)}`;
                let IsChecked = value ?? true;
                let IsValue = value ?? true;
                return (
                    <FormControl ref={ref} >
                        <Stack direction={direction} justifyContent="flex-start" alignItems="center">
                            {label && <Typography component="legend" className="react-switch-form-label">{label}</Typography>}
                            <div className="react-switch-form" ref={elementRef}>
                                <Switch
                                    id={id}
                                    {...register(name)}
                                    key={name}
                                    name={name}
                                    checked={IsChecked}
                                    value={IsValue}
                                    onChange={(e) => {
                                        onChange(e);
                                        if (props.onChange) {
                                            props.onChange(e);
                                        }
                                    }}
                                    handleDiameter={26}
                                    offColor={offColor}
                                    onColor={onColor}
                                    height={28}
                                    width={width}
                                    disabled={disabled}
                                    uncheckedIcon={<div style={{ color: fontColor }} className="react-switch-label-uncheck">{sUncheckText}</div>}
                                    checkedIcon={<div style={{ color: fontColor }} className="react-switch-label-check">{sCheckText}</div>}
                                    className={"react-switch"}
                                />
                            </div>
                        </Stack>
                    </FormControl>
                )
            }}
        />
    );
};
export default SwitchForm;