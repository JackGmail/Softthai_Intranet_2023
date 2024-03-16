import React from 'react'
import {
    Autocomplete, TextField, 
} from "@mui/material";

export default function AutoCompleteInTable({
    lstOption,
    funcOnChange,
    objValue,
    small = false,
    disClearable,
    label = null,
    disabled = false,
    sxCustom = {},
    fullWidth = false,
}) {
    return (
        <Autocomplete
            renderOption={(props, option) => {
                return (
                    <li {...props} key={option.value}>
                        {option.label}
                    </li>
                );
            }}
            disableClearable={disClearable}
            disabled={disabled}
            size={small ? "small" : "medium"}
            value={{ label: objValue.label, value: objValue.value }}
            onChange={(event: any, newValue: any) => {
                funcOnChange(event, newValue)
            }}
            options={lstOption}
            fullWidth={fullWidth}
            getOptionLabel={(item) => {
                return `${item.label}`;
            }}
            renderInput={(params) => (
                <TextField 
                    style={{ width : '50%'}}
                    label={null}
                    sx={{
                        ".MuiOutlinedInput-root": {
                            padding: "0px 5px !important",
                            " fieldset > legend > span": {
                                padding: "0px !important"
                            }
                        }, ...sxCustom
                    }}
                    {...params}
                />
            )}
        />
    )
}
