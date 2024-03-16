import React, { useMemo } from "react";
import { useState, useCallback } from "react";
import { SelectFromProps, ITreeSelectProps, FilmOptionType } from "./SelectProps";
import { useFormContext, Controller } from "react-hook-form";
import {
  Autocomplete,
  TextField,
  FormHelperText,
  Popper,
  Tooltip,
  InputAdornment,
  Typography,
  Box,
  Chip,
  ListItem,
  ListItemButton,
  Collapse,
  List,
  createFilterOptions,
} from "@mui/material";
import Checkbox from "@mui/material/Checkbox";
import CheckBoxOutlineBlankIcon from "@mui/icons-material/CheckBoxOutlineBlank";
import CheckBoxIcon from "@mui/icons-material/CheckBox";
import ExpandLess from "@mui/icons-material/ExpandLess";
import ExpandMore from "@mui/icons-material/ExpandMore";
import { I18n, ParseHtml } from "utilities/utilities";
import { I18NextNs } from "enum/enum";

const filter = createFilterOptions<FilmOptionType>();
const icon = <CheckBoxOutlineBlankIcon fontSize="small" />;
const checkedIcon = <CheckBoxIcon fontSize="small" />;

export const SelectItem = (props: SelectFromProps) => {
  const {
    id,
    name,
    label,
    fullWidth = true,
    isShowMessageError = true,
    isClearable = false,
    disabled = false,
    isPopperCustom = true,
    isDisablePortal = true,
    required = false,
    defaultValue = null,
  } = props;

  const { control, register } = useFormContext();

  const PopperCustom = useCallback((props) => {
    return (
      <Popper
        {...props}
        placement="bottom-start"
        disablePortal={isDisablePortal}
        modifiers={[
          {
            name: "flip",
            enabled: false,
            options: {
              altBoundary: true,
              rootBoundary: "document",
              padding: 8,
            },
          },
        ]}
      />
    );
  }, []);

  const getOpObjValue = useCallback((val) => {
    let res = null;
    if (props.options && val) {
      res = props.options.find((op) => op.value + "" === val + "");
    }
    return res;
  }, [props.options]);
 
  const rules = useMemo(() => {
    return {
      required: {
        value: required,
        message: `${I18n.SetText("required", I18NextNs.validation)} ${label}`,
      },
    };
  }, [required, label]);

  return (
    <Controller
      name={name}
      control={control}
      rules={rules}
      defaultValue={defaultValue}
      shouldUnregister={true}
      render={({
        field: { onChange, onBlur, value, ref },
        fieldState: { invalid, error },
      }) => {
        return (
          <>
            <Autocomplete
              {...register(name)}
              ref={ref}
              id={id}
              disabled={disabled}
              PopperComponent={isPopperCustom ? PopperCustom : undefined}
              fullWidth={fullWidth}
              size={"small"}
              options={props.options || []}
              value={getOpObjValue(value)}
              noOptionsText={
                props.notOptionsText || I18n.SetText("noInformationFound")
              }
              disableClearable={isClearable}
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
              filterOptions={(options, params) => {
                const filtered = params.inputValue
                  ? options.filter((f: any) =>
                    f.label
                      .toLowerCase()
                      .includes(params.inputValue.toLowerCase())
                  )
                  : options;
                return props.nLimits != null
                  ? filtered.slice(0, props.nLimits)
                  : filtered;
              }}
              getOptionLabel={(itemOption: any) => {
                return `${itemOption?.label}`;
              }}
              // isOptionEqualToValue={(option, value) =>
              //     option.value === value.value
              // }
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
                      label={props.label ?? ""}
                      placeholder={props.placeholder}
                      size={"small"}
                      fullWidth={fullWidth}
                      style={props.style}
                      // defaultValue={defaultValue}
                      InputProps={{
                        ...params.InputProps,
                        startAdornment: props.startAdornment ? (
                          <>
                            <InputAdornment position="start">
                              {props.startAdornment}
                            </InputAdornment>
                          </>
                        ) : (
                          params.InputProps.startAdornment
                        ),
                      }}
                    />
                  </Tooltip>
                );
              }}
              onChange={(event, value) => {
                onChange(value != null ? value["value"] : null);
                props.onChange && props.onChange(value, event);
              }}
              onBlur={(event) => {
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
  );
};
export const SelectMultipleItem = (props: SelectFromProps) => {
  const {
    id,
    name,
    label,
    fullWidth = true,
    nlimitTags = 2,
    isSelectAll = true,
    isClearable = false,
    disabled = false,
    isShowMessageError = true,
    isPopperCustom = true,
    isShowCountSelected = false,
    required = false,
  } = props;

  const { control } = useFormContext();

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
              padding: 8,
            },
          },
        ]}
      />
    );
  }, []);

  const getOpObjValue = (val) => {
    let res = [];
    if (val) {
      val.forEach((element) => {
        res.push(props.options.find((op) => op.value == element));
      });
    }
    return res;
  };

  const rules = useMemo(() => {
    return {
      required: {
        value: required,
        message: `${I18n.SetText("required", I18NextNs.validation)} ${label}`,
      },
    };
  }, [required, label]);

  return (
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
              multiple
              disableCloseOnSelect
              ref={ref}
              id={props.name}
              disabled={disabled}
              fullWidth={fullWidth}
              size={"small"}
              options={props.options || []}
              value={getOpObjValue(value)}
              noOptionsText={
                props.notOptionsText || I18n.SetText("noInformationFound")
              }
              disableClearable={isClearable}
              PopperComponent={isPopperCustom ? PopperCustom : undefined}
              renderOption={(props_, option, { selected }) => {
                return (
                  <ListItem {...props_} key={option.value}>
                    <Checkbox
                      icon={icon}
                      checkedIcon={checkedIcon}
                      style={{ marginRight: 8 }}
                      checked={
                        option.value === "SelectAll"
                          ? (value || []).length ===
                          (props.options || []).length
                          : selected
                      }
                    />
                    {option.label}
                  </ListItem>
                );
              }}
              getOptionLabel={(itemOption: any) => {
                return `${itemOption?.label}`;
              }}
              filterOptions={(options, params) => {
                const filtered = params.inputValue
                  ? options.filter((f: any) =>
                    f.label
                      .toLowerCase()
                      .includes(params.inputValue.toLowerCase())
                  )
                  : options;
                if (props.nLimits != null) {
                  let filtered_ = filtered.slice(0, props.nLimits);
                  return isSelectAll
                    ? [
                      { label: "Select All", value: "SelectAll" },
                      ...filtered_,
                    ]
                    : filtered_;
                } else {
                  return isSelectAll
                    ? [{ label: "Select All", value: "SelectAll" }, ...filtered]
                    : filtered;
                }
              }}
              renderInput={(params) => (
                <TextField
                  id={id}
                  {...params}
                  name={name}
                  error={invalid}
                  required={required}
                  disabled={disabled}
                  label={label}
                  placeholder={
                    getOpObjValue(value).length === 0 ? props.placeholder : null
                  }
                  size={"small"}
                  fullWidth={fullWidth}
                  style={props.style}
                  InputProps={{
                    ...params.InputProps,
                    startAdornment: props.startAdornment ? (
                      <>
                        <InputAdornment position="start">
                          {props.startAdornment}
                        </InputAdornment>
                        {params.InputProps.startAdornment}
                      </>
                    ) : (
                      params.InputProps.startAdornment
                    ),
                    endAdornment: (
                      <>
                        {isShowCountSelected
                          ? (value || []).length +
                          " " +
                          I18n.SetText("itemSelected")
                          : params.InputProps.endAdornment}
                        {params.InputProps.endAdornment}
                      </>
                    ),
                  }}
                />
              )}
              renderTags={(tagValue, getTagProps) => {
                return (
                  <>
                    {tagValue.slice(0, nlimitTags).map((option: any, index) => (
                      <Chip
                        key={option.value}
                        size={"small"}
                        {...getTagProps({ index })}
                        label={option.label}
                        disabled={disabled || false}
                      />
                    ))}
                    {tagValue.length > nlimitTags ? (
                      <Chip
                        size={"small"}
                        label={"+" + (tagValue.length - nlimitTags)}
                        disabled={disabled || false}
                      />
                    ) : (
                      <></>
                    )}
                  </>
                );
              }}
              onChange={(event, selectedOptions, reason) => {
                if (reason === "selectOption" || reason === "removeOption") {
                  selectedOptions = selectedOptions.filter(
                    (f) => f !== undefined && f != null
                  );
                  //Select All
                  if (
                    selectedOptions.find(
                      (option) => option.value === "SelectAll"
                    )
                  ) {
                    const IsAllSelected =
                      props.options.length === (value || []).length;
                    if (!IsAllSelected) {
                      onChange(
                        props.options
                          .filter((f) => f.value !== "SelectAll")
                          .map((m) => m.value)
                      );
                      props.onChange &&
                        props.onChange(
                          props.options
                            .filter((f) => f.value !== "SelectAll")
                            .map((m) => m.value),
                          event
                        );
                    } else {
                      onChange([]);
                      props.onChange && props.onChange([], event);
                    }
                  } else {
                    let arr = (selectedOptions || [])
                      .filter((f) => f.value !== "SelectAll")
                      .map((m) => m["value"]);
                    onChange(arr);
                    props.onChange &&
                      props.onChange(
                        selectedOptions.filter((f) => f.value !== "SelectAll"),
                        event
                      );
                  }
                } else if (reason === "clear") {
                  onChange([]);
                  props.onChange && props.onChange([], event);
                }
              }}
              onBlur={(event) => {
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
  );
};
export const TreeSelectMultipleItem = (props: ITreeSelectProps) => {
  const {
    id,
    name,
    required = false,
    disabled = false,
    subLabel,
    fullWidth = true,
    nlimitTags = 2,
    isShowMessageError = true,
    isPopperCustom = true,
    options,
    label,
    onClearOptions,
    selectAllLabel = "เลือกทั้งหมด",
    IsShrink = false,
    sxCustomLabelChip,
  } = props;

  const { register, getValues, setValue, control } = useFormContext();

  const [arrParentIDExpand, setArrParentIDExpand] = useState([]);
  const [IsFirstTimeLoad, setIsFirstTimeLoad] = useState(true);
  const handleToggleSelectAll = () => {
    const allSelected = options.length === (getValues(name) || []).length;
    if (!allSelected) {
      setValue(name, [...options.map((m) => m.value)]);
      props.onChange && props.onChange(options);
    } else {
      setValue(name, []);
      props.onChange && props.onChange([]);
    }
  };

  const onlyUnique = (value, index, self) => {
    return self.indexOf(value) === index;
  };

  const handleChange = (event, selectedOptions, reason, onChange) => {
    let nValue = event.target.value + "";
    if (!nValue) {
      return;
    }

    setValue("input_" + name, "");

    if (reason === "selectOption" || reason === "removeOption") {
      //Select All
      if (selectedOptions.find((option) => option && option.value === "0")) {
        handleToggleSelectAll();
      } else {
        onChange(selectedOptions.map((m) => m.value));

        //Parent Selected
        let arrAllParent = options.filter((f) => f.isParent);
        let arrAllParentID = arrAllParent.map((f) => f.value);
        let isParentSelected = arrAllParentID.includes(nValue);
        if (reason === "selectOption") {
          //Parent
          if (isParentSelected) {
            let arrOptionSelectedParent = options.filter(
              (f) => !f.isParent && f.sParentID === nValue
            );
            if (arrOptionSelectedParent.length === 0) {
              arrOptionSelectedParent = options.filter(
                (f) => f.value === nValue
              );
            }
            let arrSelectedAllParent = [
              ...getValues(name),
              ...arrOptionSelectedParent.map((m) => m.value),
            ];
            onChange(arrSelectedAllParent.filter(onlyUnique));

            if (arrOptionSelectedParent.length > 0) {
              setExpand(nValue);
            }
          } else {
            //Children
            let objParent = options.find((f) => f.value === nValue);
            let sParentID = objParent ? objParent.sParentID : "0";
            setExpand(sParentID);
            let nCountChildrenOfParent = options.filter(
              (f) => f.sParentID === sParentID
            ).length;
            //Children Selected in Parent
            let nCountChildrenSelectedOfParent = selectedOptions.filter(
              (f) => f.sParentID === sParentID
            ).length;
            let arrSelectedAllParent = [...selectedOptions.map((m) => m.value)];
            if (nCountChildrenSelectedOfParent >= nCountChildrenOfParent) {
              arrSelectedAllParent.push(sParentID);
            }
            onChange(arrSelectedAllParent.filter(onlyUnique));
          }
        } else if (reason === "removeOption") {
          //Parent
          if (isParentSelected) {
            let arrOptionIsParentID = options
              .filter((f) => !f.isParent && f.sParentID === nValue)
              .map((m) => m.value);

            if (!options.some((s) => !s.isParent && s.sParentID === nValue)) {
              arrOptionIsParentID = [nValue];
            }

            let arrSelectedID = getValues(name).filter(
              (f) => !arrOptionIsParentID.includes(f)
            );
            onChange(arrSelectedID);
            removeExpand(nValue);
          } else {
            //Children
            let objParent = options.find((f) => f.value === nValue);
            let sParentID = objParent ? objParent.sParentID : "0";
            // setExpand(sParentID);
            onChange(
              selectedOptions.map((m) => m.value).filter((f) => f !== sParentID)
            );
          }
        }

        props.onChange && props.onChange(selectedOptions);
      }
    } else if (reason === "clear") {
      onClearOptions && onClearOptions();
      onChange([]);
      props.onChange && props.onChange([]);
      setArrParentIDExpand([]);
    }
  };

  const setExpand = (sParentID) => {
    setArrParentIDExpand([...arrParentIDExpand, sParentID]);
  };
  const removeExpand = (sParentID) => {
    let arrCloneParentID = arrParentIDExpand;
    arrCloneParentID = arrCloneParentID.filter((f) => f !== sParentID);
    setArrParentIDExpand([...arrCloneParentID]);
  };

  const valueFrom = () => {
    let sValue = getValues(name);
    if (sValue && options) {
      return sValue.map((value) =>
        options.find((option) => option.value === value)
      );
    }
    return [];
  };

  useMemo(() => {
    //Select Parent if children select all
    if (options.length > 0) {
      let arrAllParent = options.filter((f) => f.isParent);
      let arrChildrenSeleted = getValues(name) || [];

      //Loop Parent
      arrAllParent.forEach((iP) => {
        let arrChildrenOfParent = options.filter(
          (f) => f.sParentID === iP.value
        );

        let isSelectAllChildren = true;

        if (arrChildrenOfParent.length === 0) {
          isSelectAllChildren = false;
        }
        //Loop Children
        arrChildrenOfParent.forEach((iC) => {
          if (!arrChildrenSeleted.includes(iC.value)) {
            isSelectAllChildren = false;
          }
        });
        if (isSelectAllChildren) {
          arrChildrenSeleted.push(iP.value);
        }
      });
      if (
        [...arrChildrenSeleted.filter(onlyUnique)].length > 0 &&
        IsFirstTimeLoad
      ) {
        setValue(name, [...arrChildrenSeleted.filter(onlyUnique)]);
        setIsFirstTimeLoad(false);
      }
    }
  }, [options]);

  const handleExpand = (sParentID) => {
    let arrClone = arrParentIDExpand;
    let isExist = arrClone.findIndex((f) => f === sParentID) !== -1;
    if (!isExist) {
      arrClone.push(sParentID);
    } else {
      arrClone = arrClone.filter((f) => f !== sParentID);
    }
    setArrParentIDExpand([...arrClone]);
  };

  const handleAutoCompleteChange = (e) => {
    let sText = e.target.value;
    setValue("input_" + name, sText);
  };

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
              padding: 8,
            },
          },
        ]}
      />
    );
  }, []);

  const rules = useMemo(() => {
    return {
      required: {
        value: required,
        message: `${I18n.SetText("required", I18NextNs.validation)} ${label}`,
      },
    };
  }, [required, label]);

  return (
    <Controller
      name={name}
      control={control}
      rules={rules}
      render={({
        field: { onChange, onBlur, value, ref },
        fieldState: { invalid, error },
      }) => (
        <>
          <Autocomplete
            ref={ref}
            size={"small"}
            id={name}
            multiple
            disableCloseOnSelect
            // selectOnFocus
            PopperComponent={isPopperCustom ? PopperCustom : undefined}
            onChange={(event, selOption, reason) => {
              handleChange(event, selOption, reason, onChange);
            }}
            limitTags={nlimitTags}
            disableClearable={disabled || false}
            getOptionLabel={(option: any) => option.label}
            options={options}
            value={valueFrom()}
            isOptionEqualToValue={(option, value) => {
              if (value) {
                return option.value === value.value;
              }
            }}
            getOptionDisabled={() => disabled || false}
            disabled={disabled || undefined}
            noOptionsText={"No Data"}
            renderTags={(tagValue, getTagProps) => {
              let arrParent = tagValue.filter((f) => f.isParent);
              let arrParentNotChildren = [];
              arrParent.forEach((f) => {
                if (
                  !options.some((s) => s.sParentID === f.value && !f.isParent)
                ) {
                  arrParentNotChildren.push(f);
                }
              });

              // tagValue = tagValue.filter((f) => f && !f.isParent);
              tagValue = [
                ...tagValue.filter((f) => f && !f.isParent),
                ...arrParentNotChildren,
              ];
              if (tagValue !== undefined) {
                return (
                  <React.Fragment>
                    {tagValue.slice(0, nlimitTags).map((option: any, index) => (
                      <Chip
                        key={option.value}
                        sx={{ ".MuiChip-label": { ...sxCustomLabelChip } }}
                        size={"small"}
                        {...getTagProps({ index })}
                        label={option.label}
                        disabled={disabled || false}
                        onDelete={null}
                      />
                    ))}
                    {tagValue.length > nlimitTags ? (
                      <Chip
                        size={"small"}
                        label={"+" + (tagValue.length - nlimitTags)}
                        disabled={disabled || false}
                        onDelete={null}
                      />
                    ) : (
                      <React.Fragment></React.Fragment>
                    )}
                  </React.Fragment>
                );
              }
            }}
            renderOption={(props, option, { selected }) =>
              option.isParent || option.label === selectAllLabel ? (
                <ListItemButton key={option.value} style={{ padding: 0 }}>
                  <ListItem {...props} key={option.value} value={option.value}>
                    <>
                      <Checkbox
                        icon={icon}
                        checkedIcon={checkedIcon}
                        style={{ marginRight: 8 }}
                        value={option.value}
                        checked={
                          ((value || []).length > 0 &&
                            option.value === "0" &&
                            options.length === (value || []).length) ||
                            selected
                            ? true
                            : selected
                        }
                      />
                      {option.label}
                    </>
                  </ListItem>
                  {options.some(
                    (s) => s.sParentID === option.value && !s.isParent
                  ) ? (
                    arrParentIDExpand.findIndex((f) => f === option.value) !==
                      -1 || getValues("input_" + name).length > 0 ? (
                      <Box
                        sx={{ pl: 1, pr: 1, pt: 1.5, pb: 1.5 }}
                        onClick={() => handleExpand(option.value)}
                      >
                        <ExpandLess />
                      </Box>
                    ) : (
                      <Box
                        sx={{ pl: 1, pr: 1, pt: 1.5, pb: 1.5 }}
                        onClick={() => handleExpand(option.value)}
                      >
                        <ExpandMore />
                      </Box>
                    )
                  ) : (
                    <></>
                  )}
                </ListItemButton>
              ) : (
                <Collapse
                  key={option.value}
                  in={
                    arrParentIDExpand.findIndex(
                      (f) => f === option.sParentID
                    ) !== -1 || getValues("input_" + name).length > 0
                  }
                  timeout="auto"
                  unmountOnExit
                >
                  <List component="div" disablePadding>
                    <ListItem
                      {...props}
                      key={option.value}
                      value={option.value}
                    >
                      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                      <Checkbox
                        icon={icon}
                        checkedIcon={checkedIcon}
                        style={{ marginRight: 8 }}
                        value={option.value}
                        checked={
                          ((value || []).length > 0 &&
                            option.value === "0" &&
                            options.length === (value || []).length) ||
                            selected
                            ? true
                            : false
                        }
                      />
                      {option.label}
                    </ListItem>
                  </List>
                </Collapse>
              )
            }
            renderInput={(params) => {
              return (
                <TextField
                  id={id}
                  size={"small"}
                  name={"input_" + name}
                  {...register("input_" + name)}
                  {...params}
                  required={required}
                  fullWidth={fullWidth}
                  style={{ width: "100%" }}
                  error={invalid}
                  InputLabelProps={{
                    disabled: disabled || false,
                    shrink: IsShrink ? true : undefined,
                  }}
                  onChange={handleAutoCompleteChange}
                  label={
                    label ? (
                      <>
                        {label}
                        {subLabel ? (
                          <Typography
                            fontWeight={600}
                            component="span"
                          >{`${subLabel}`}</Typography>
                        ) : null}
                      </>
                    ) : null
                  }
                />
              );
            }}
          />
          {isShowMessageError && error ? (
            <FormHelperText>
              {error.message}
            </FormHelperText>
          ) : null}
        </>
      )}
    />
  );
};

export const SelectMultipleAddOptionsFormItem = (props: SelectFromProps) => {
  const {
    name,
    fullWidth = true,
    nlimitTags = 2,
    label,
    isSelectAll = true,
    isClearable = false,
    disabled = false,
    isShowMessageError = true,
    isPopperCustom = true,
    required = false
  } = props;

  const [options_, setoptions] = useState<any>(props.options || [])
  const {
    control,
    getValues,
    setValue
  } = useFormContext();

  // const [t] = useTranslation();
  const fixedOptions = [];
  const PopperCustom = useCallback((props) => {
    return <Popper
      {...props}
      placement='bottom-start'
      disablePortal={true}
      modifiers={[
        {
          name: 'flip',
          enabled: false,
          options: {
            altBoundary: true,
            rootBoundary: 'document',
            padding: 8,
          },
        },
      ]}
    />;
  }, []);

  const getOpObjValue = (val) => {
    let res = [];
    if (val) {
      val.forEach(element => {
        res.push(props.options.find(op => op.value === element));
      });

    }
    return res;
  };

  const handleToggleSelectAll = (selectedOptions) => {
    const allSelected = props.options.length === (getValues(name) || []).length;
    let chkFix = (getValues(name) || []).filter(f => f === fixedOptions[0]);
    let chkData = chkFix.length > 0 ? [] : fixedOptions[0] ?? []
    if (!allSelected) {

      setValue(name, [...chkData, ...props.options.map(m => m.value)], { shouldValidate: true });
      props.onChange && props.onChange(props.options, null);
    } else {
      setValue(name, fixedOptions);
      props.onChange && props.onChange([], null);
    }
  };

  const handleChange = (event, selectedOptions, reason) => {

    let chkFix = (getValues(name) || []).filter(f => f == fixedOptions[0]);
    let chkData = chkFix.length > 0 ? [] : fixedOptions[0] ?? []
    if (reason === "selectOption" || reason === "removeOption") {
      if (selectedOptions.find(option => option?.value === "999")) {
        handleToggleSelectAll(selectedOptions);
      } else if (selectedOptions.find(option => option.value === "9999")) {
        props.options.push({ value: selectedOptions.find(option => option?.value === "9999")["newVaue"], label: selectedOptions.find(option => option?.value === "9999")["newVaue"], Isnew: true })
        let v = selectedOptions.filter(f => f?.value !== "9999").map(m => m["value"]);
        let o = selectedOptions.filter(f => f?.value === "9999").map(m => m["newVaue"]);
        setValue(name, [...chkData, ...v, ...o], { shouldValidate: true })
        // setValue(name, [...chkData, ...selectedOptions.filter(f => f?.value != "9999").map(m => m["value"]), ...selectedOptions.filter(f => f?.value == "9999").map(m => m["newVaue"])], { shouldValidate: true })
        props.onChange && props.onChange(selectedOptions, event);
      } else {
        // onToggleOption && onToggleOption(selectedOptions);
        setValue(name, [...chkData, ...selectedOptions.map(m => m.value)], { shouldValidate: true })
        props.onChange && props.onChange(selectedOptions, event);
      }
    } else if (reason === "clear") {
      setValue(name, fixedOptions, { shouldValidate: true });
      props.onChange && props.onChange([], event);
    }
  };

  const rules = useMemo(() => {
    return {
      required: {
        value: required,
        message: `${I18n.SetText("required", I18NextNs.validation)} ${label}`,
      },
    };
  }, [required, label]);

  return (
    <Controller
      name={name}
      control={control}
      render={({
        field: { onChange, onBlur, value, ref },
        fieldState: { invalid, error } }) => {
        return (
          <>
            <Autocomplete
              multiple
              disableCloseOnSelect
              ref={ref}
              id={name}
              disabled={disabled}
              fullWidth={fullWidth}
              size={"small"}
              options={props.options || []}
              value={getOpObjValue(value)}
              noOptionsText={props.notOptionsText || I18n.SetText("noInformationFound")}
              disableClearable={isClearable}
              limitTags={nlimitTags}
              PopperComponent={isPopperCustom ? PopperCustom : undefined}
              renderOption={(props_, option, { selected }) => {
                return (
                  <li {...props_} key={option.value}>
                    <Checkbox
                      icon={icon}
                      checkedIcon={checkedIcon}
                      style={{ marginRight: 8 }}
                      checked={((getValues(name) || []).length > 0 && option.value === "999" && props.options.length === (getValues(name) || []).length) || selected ? true : selected}
                    />
                    {option.label}
                  </li>
                );
              }}
              getOptionLabel={(itemOption: any) => {
                return `${itemOption?.label}`
              }}
              filterOptions={(options, params) => {
                const filtered = filter(options, params);
                if (params.inputValue !== '') {
                  if (props.options.filter(f => f.value === params.inputValue).length === 0) {
                    filtered.push({
                      value: "9999",
                      label: `Add "${params.inputValue}"`,
                      newVaue: params.inputValue
                    });
                  }
                }
                // return isSelectAll ? [{ label: "Select All", value: "999" }, ...filtered] : filtered;

                if (props.nLimits != null) {
                  let filtered_ = filtered.slice(0, props.nLimits);
                  return isSelectAll ? [{ label: "Select All", value: "999" }, ...filtered_] : filtered_;
                } else {
                  return isSelectAll ? [{ label: "Select All", value: "999" }, ...filtered] : filtered;
                }
              }}
              renderInput={(params) =>
                <TextField
                  {...params}
                  name={name}
                  error={error?.message != null}
                  required={props.required}
                  disabled={disabled}
                  label={props.label}
                  placeholder={getOpObjValue(value).length === 0 ? props.placeholder : null}
                  size={"small"}
                  fullWidth={fullWidth}
                  style={props.style}
                  InputProps={{
                    ...params.InputProps,
                    // startAdornment: (
                    //   <>
                    //     <InputAdornment position="start">
                    //       {props.startAdornment}
                    //     </InputAdornment>
                    //     {params.InputProps.startAdornment}
                    //   </>
                    // )
                  }}
                // sx={{
                //     "label.MuiInputLabel-shrink": {
                //         // backgroundColor: "rgb(255 255 255 / 18%)",
                //         top: "0px",
                //     },
                //     ".MuiInputLabel-outlined": {
                //         top: "0px",
                //     },
                //     ".MuiInputLabel-asterisk": {
                //         color: "red"
                //     }
                // }}
                />
              }
              onChange={handleChange}
              onBlur={(event) => {
                onBlur();
                props.onBlur && props.onBlur(event);
              }}
              onKeyPress={(event) => {
                props.onKeyPress && props.onKeyPress(event);
              }}
              onKeyDown={(event) => {
                props.onKeyDown && props.onKeyDown(event);
              }}
              onKeyUp={(event) => {
                props.onKeyPress && props.onKeyPress(event);
              }}
            />
            {isShowMessageError && error ? (
              <FormHelperText sx={{ color: "red" }}>
                {error.message}
              </FormHelperText>
            ) : null}
          </>
        )
      }
      }
    />
  );
};

