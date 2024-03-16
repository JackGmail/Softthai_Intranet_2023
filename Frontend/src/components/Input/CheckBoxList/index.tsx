import { Checkbox, FormControl, FormHelperText, FormLabel } from '@mui/material';
import FormControlLabel from '@mui/material/FormControlLabel';
import FormGroup from '@mui/material/FormGroup';
import { Controller, useFormContext } from "react-hook-form";
import { ICheckbox } from './CheckboxProps';
import { I18n, ParseHtml } from 'utilities/utilities';
import { I18NextNs } from 'enum/enum';

const CheckBoxList = ((props: ICheckbox) => {
    const {
        label,
        id,
        name,
        disabled = false,
        required = false,
        style,
        options,
        IsShowMessageError = true,
        IsSelectAllOption = false,
        defaultValue = [],
        size = "small",
        row = true
    } = props;

    const { control, register, setValue } = useFormContext();
    console.log("options",options)

    const SetValueController = (e, arrValue) => {
        let arrData = [];
        if (arrValue && Array.isArray(arrValue)) {
            if (e.target.checked) {
                arrValue.push(e.target.value)
            }
            else {
                arrValue = arrValue.filter(f => f !== e.target.value);
            }
            arrData = arrValue;
        }
        return arrData;
    }

    const CheckAll = (checked) => {
        let arrData = [];
        if (checked) {
            options.forEach((item) => {
                let disabled = item.disabled ?? false;
                if (!disabled) {
                    arrData.push(item.value);
                }
            })
        }
        return arrData;
    }

    const CheckItem = (arrValue, item) => {
        let checked = false;
        if (arrValue && Array.isArray(arrValue) && item) {
            checked = arrValue.filter(f => f === item.value).length > 0;
        }
        return checked;
    }

    return (
        <Controller name={name} control={control} rules={{ required: required }} shouldUnregister={true} defaultValue={defaultValue}
            render={({
                field: { value, ref },
                fieldState: { invalid } }) => {
                let isValid = invalid && !disabled;
                let helperText = IsShowMessageError ? `${I18n.SetText("required", I18NextNs.validation)}` : "";
                if (label) {
                    helperText = helperText + ` ${label}`;
                }
                let sKeyCheckAll = name + "_All";
                let sIDCheckAll = name + "_All";
                let cbItem = options.filter(f => !(f.disabled ?? false));
                return (
                    <FormControl required={required} error={invalid} ref={ref} >
                        {label && <FormLabel component="legend">{label}</FormLabel>}
                        <FormGroup row={row} {...register(`${name}`)}>
                            {IsSelectAllOption && <FormControlLabel key={sKeyCheckAll}
                                control={<Checkbox
                                    id={sIDCheckAll}
                                    checked={value.length === cbItem.length}
                                    disabled={disabled}
                                    onChange={(e) => {
                                        let arrAllItem = CheckAll(e.target.checked);
                                        setValue(name, arrAllItem, { shouldValidate: true });
                                    }}
                                    style={style}
                                    size={size}
                                />}
                                label={`${I18n.SetText("selectAll")}`}
                            />
                            }
                            {
                                options.map((item, index) => {
                                    let label = item.label;
                                    let sID = id + "_" + item.value;
                                    let sKey = name + index;
                                    let IsDisabled = disabled || (item.disabled ?? false);
                                    return (<FormControlLabel key={sKey}
                                        control={<Checkbox
                                            id={sID}
                                            disabled={IsDisabled}
                                            checked={CheckItem(value, item)}
                                            value={item.value}
                                            onChange={(e) => {
                                                let arrData = SetValueController(e, value);
                                                setValue(name, arrData, { shouldValidate: true, shouldDirty: true, shouldTouch: true });
                                                if(props.onChange){
                                                    props.onChange(arrData)
                                                }
                                            }}
                                            style={style}
                                            size={size}
                                        />}
                                        label={ParseHtml(label)}
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
export default CheckBoxList;