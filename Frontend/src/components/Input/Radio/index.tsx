import { Radio, FormControl, FormHelperText, FormLabel } from '@mui/material';
import FormControlLabel from '@mui/material/FormControlLabel';
import FormGroup from '@mui/material/FormGroup';
import { Controller, useFormContext } from "react-hook-form";

import { I18n } from 'utilities/utilities';
import { IRadio } from './RadioProps';
import { I18NextNs } from 'enum/enum';

const RadioList = ((props: IRadio) => {
    const {
        label,
        id,
        name,
        disabled = false,
        required = false,
        style,
        options,
        IsShowMessageError = true,
        defaultValue = "",
        size = "small",
        row = true
    } = props;

    const { control, register, setValue } = useFormContext();

    return (
        <Controller name={name} control={control} rules={{ required: required }} shouldUnregister={true} defaultValue={defaultValue}
            render={({
                field: { value, ref },
                fieldState: { invalid } }) => {
                let isValid = invalid && !disabled;
                let helperText = IsShowMessageError ? `${I18n.SetText("required",I18NextNs.validation)}` : "";
                if (label) {
                    helperText = helperText + ` ${label}`;
                }
                return (
                    <FormControl required={required} error={invalid} ref={ref} >
                        {label && <FormLabel component="legend">{label}</FormLabel>}
                        <FormGroup row={row} {...register(`${name}`)}>
                            {
                                options.map((item, index) => {
                                    let label = item.label;
                                    let sID = id + "_" + item.value;
                                    let sKey = name + index;
                                    let IsDisabled = disabled || (item.disabled ?? false);
                                    return (<FormControlLabel key={sKey}
                                        control={<Radio
                                            id={sID}
                                            disabled={IsDisabled}
                                            checked={value == item.value}
                                            value={item.value}
                                            onChange={(e) => {
                                               
                                                setValue(name, e.target.value, { shouldValidate: true, shouldDirty: true, shouldTouch: true });
                                                if(props.onChange){
                                                    props.onChange(e.target.value)
                                                }
                                            }}
                                            size={size}
                                            style={style}
                                        />}
                                        label={label}
                                    />)
                                })
                            }
                        </FormGroup>
                        {
                            (IsShowMessageError && isValid) && <FormHelperText>{helperText}</FormHelperText>
                        }
                    </FormControl >
                )
            }}
        />
    );
});
export default RadioList;