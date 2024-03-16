import React, { useCallback, useMemo } from "react";
import { useState, useEffect } from "react";
import { IAsyncAutoCompleteProps } from "./AutocompleteProps";
import { useFormContext, Controller } from "react-hook-form";
import { Autocomplete, TextField, IconButton, Popper, Chip, FormHelperText } from "@mui/material";
import SearchIcon from "@mui/icons-material/Search";
import axios from "axios";
import qs from "qs";
import { I18n, IsNullOrUndefined, SecureStorage } from "utilities/utilities";
import { I18NextNs } from "enum/enum";
import './Autocomplete.css';

export const AutoCompleteItem = (props: IAsyncAutoCompleteProps) => {
  const {
    name,
    IsClearable = true,
    disabled = false,
    limitTag = 1,
    label,
    required = false,
    fullWidth = true,
    sUrlAPI,
    sParam,
    ParamUrl,
    sMethodAxios = "GET",
    IsShowMessageError = true,
    IsPopperCustom = true,
    IsShrink = true,
    id
  } = props;

  const { register } = useFormContext();

  const { control } = useFormContext();
  const [inputValue, setInputValue] = useState("");
  const [optionsValueAuto, setOptionsValueAuto] = useState([] as any);
  const [txtOption, settxtOption] = useState("enterToSearch") as any;

  const GetParamSearch = () => {
    if (sParam) {
      return { strSearch: inputValue };
    }
    else {
      return { strSearch: inputValue, sParam: sParam };
    }
  }

  const SetOptionsValue = (data) => {
    let lstData = data ?? [];
    setOptionsValueAuto(lstData);
    if (lstData.length == 0) {
      settxtOption(I18n.SetText("No Data"));
    }
  }

  useEffect(() => {
    if (!inputValue) {
      setOptionsValueAuto([]);
    } else {
      const source = axios.CancelToken.source();
      const paramSearch = GetParamSearch();
      const paramObj = { ...ParamUrl, ...paramSearch };
      const authToken = SecureStorage.Get(`${process.env.REACT_APP_JWT_KEY}`) as string;
      const ConfigJwt = {
        Authorization: IsNullOrUndefined(authToken) ? "" : `Bearer ${authToken}`,
        Accept: "application/json",
        "Content-Type": "application/json"
      };

      const newParam = sUrlAPI;
      const baseUrl = process.env.REACT_APP_API_URL;
      const sPathApi = `${baseUrl}api/${newParam}`;
      const url = new URL(sPathApi, window.location.href);
      const sNewPath = url.origin + url.pathname + url.search;

      settxtOption("searching"); //กำลังค้นหา
      if (sMethodAxios === "POST") {
        axios
          .post(sNewPath, paramObj, {
            headers: ConfigJwt,
            cancelToken: source.token,
            paramsSerializer: (params) => {
              return qs.stringify(params);
            }
          })
          .then((response) => {
            SetOptionsValue(response.data);
          })
          .catch((error) => {
            if (axios.isCancel(error)) return;
          });
      }
      else {
        axios
          .get(sNewPath, {
            headers: ConfigJwt,
            params: paramObj,
            cancelToken: source.token,
            paramsSerializer: (params) => {
              return qs.stringify(params);
            }
          })
          .then((response) => {
            SetOptionsValue(response.data);
          })
          .catch((error) => {
            if (axios.isCancel(error)) return;
          });
      }
      return () => source.cancel();
    }
  }, [inputValue]);

  const PopperCustom = useCallback((props) => {
    return (
      <Popper
        {...props}
        placement="bottom-start"
        disablePortal={true}
        modifiers={[
          {
            name: "flip",
            enabled: false,
            options: {
              altBoundary: true,
              rootBoundary: "document",
              padding: 8
            }
          }
        ]}
      />
    );
  }, []);

  const rules = useMemo(() => {
    let objvalidate = {};
    if (required) {
      objvalidate["required"] = {
        value: required,
        message: `${I18n.SetText("required", I18NextNs.validation)} ${label}`
      }
    }
    if (disabled) {
      objvalidate["disabled"] = disabled;
    }
    return objvalidate;
  }, [required, disabled])

  return (
    <Controller name={name} control={control} rules={rules}
      render={({
        field: { onChange, value },
        fieldState: { invalid, error }
      }) => {
        let helperText = IsShowMessageError ? error?.message : "";
        return (
          <React.Fragment>
            <Autocomplete
              {...register(name)}
              getOptionLabel={(itemOption: any) => {
                return `${itemOption.label}`;
              }}
              filterOptions={(x) => x}
              options={optionsValueAuto}
              autoComplete
              PopperComponent={IsPopperCustom ? PopperCustom : undefined}
              size="small"
              noOptionsText={txtOption}
              blurOnSelect
              includeInputInList
              filterSelectedOptions
              disabled={disabled}
              disableClearable={!IsClearable}
              limitTags={limitTag}
              value={value ?? null}
              onChange={(event: any, newValue: any) => {
                let sValue = newValue?.value;
                if (sValue) {
                  onChange(newValue);
                  setOptionsValueAuto([]);
                }
                else {
                  onChange(null);
                }
                if (props.onChange) {
                  props.onChange(newValue);
                }
              }}
              onInputChange={async (event: any, newInputValue: string) => {
                settxtOption("enterToSearch");
                setInputValue(newInputValue);
                setOptionsValueAuto([]);
                if (!newInputValue) {
                  return undefined;
                }
              }}
              sx={{
                ".MuiOutlinedInput-root": {
                  paddingRight: "10px !important",
                }
              }}
              renderInput={(params) => (
                <TextField
                  id={id}
                  size={"small"}
                  className="auto-input"
                  label={label}
                  placeholder={props.placeholder}
                  inputRef={register(name).ref}
                  error={invalid}
                  {...params}
                  fullWidth={fullWidth}
                  required={required}
                  style={{ width: "100%" }}
                  InputProps={{
                    ...params.InputProps,
                    endAdornment: (
                      <React.Fragment>
                        <IconButton
                          style={{ padding: "0px" }}
                          disabled={disabled}
                        >
                          <SearchIcon />
                        </IconButton>
                        {params.InputProps.endAdornment}
                      </React.Fragment>
                    )
                  }}
                  InputLabelProps={{
                    disabled: disabled,
                    shrink: IsShrink ? true : undefined,
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
              )}
              renderOption={(props, option: any) => {
                return (
                  <li {...props} key={option.value}>
                    {option.label}
                  </li>
                );
              }}
              renderTags={(tagValue, getTagProps) => {
                return (
                  <React.Fragment>
                    {tagValue
                      .slice(0, limitTag | 1)
                      .map((option: any, index) => (
                        <Chip
                          {...getTagProps({ index })}
                          label={option.label}
                          disabled={disabled}
                        />
                      ))}
                  </React.Fragment>
                );
              }}
            />
            {
              (IsShowMessageError && invalid) && <FormHelperText>{helperText}</FormHelperText>
            }
          </React.Fragment>
        );
      }}
    />
  );
};