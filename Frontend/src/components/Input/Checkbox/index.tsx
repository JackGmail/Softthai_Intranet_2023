import {
	Checkbox,
	FormControl,
	FormHelperText,
	FormLabel,
} from "@mui/material";
import FormControlLabel from "@mui/material/FormControlLabel";
import FormGroup from "@mui/material/FormGroup";
import { Controller, useFormContext } from "react-hook-form";
import {
	ICheckbox,
} from "./CheckboxProps";
import { FnComponents, I18n } from "utilities/utilities";
import { I18NextNs, TypeComponent } from "enum/enum";

export const CheckBoxItem = (props: ICheckbox) => {
	const {
		label = "",
		name,
		disabled = false,
		required = false,
		style,
		options,
		IsShowMessageError = true,
		defaultValue = false,
		size = "small",
		row = true,
	} = props;
	const { control, register, setValue } = useFormContext();
	return (
		<Controller
			name={name}
			control={control}
			rules={{ required: required }}
			shouldUnregister={true}
			defaultValue={defaultValue}
			render={({ field: { value, ref }, fieldState: { invalid } }) => {
				let IsValid = invalid && !disabled;
				let helperText = `${I18n.SetText("required", I18NextNs.validation)}`;
				return (
					<FormControl required={required} error={invalid} ref={ref}>
						{label && <FormLabel component="legend">{label}</FormLabel>}
						<FormGroup row={row} {...register(`${name}`)} >
							{options.map((item, index) => {
								let label = item.label;
								let sValue = item.value + "";
								let sKeyItem = name + "-" + index;
								let sID = FnComponents.GetId(TypeComponent.checkBox, sValue, name);
								let sKey = FnComponents.GetKey(TypeComponent.checkBox, sKeyItem);
								let IsDisabled = disabled || (item.disabled ?? false);
								return (
									<FormControlLabel
										key={sKey}
										control={
											<Checkbox
												id={sID}
												disabled={IsDisabled}
												checked={value}
												value={value}
												onChange={(e) => {
													let IsChecked = e.target.checked;
													setValue(name, IsChecked, {
														shouldValidate: true,
														shouldDirty: true,
														shouldTouch: true
													});
													if (props.onChange) {
														props.onChange(IsChecked, e);
													}
												}}
												style={style}
												size={size}
											/>
										}
										label={label}
									/>
								);
							})}
						</FormGroup>
						{IsShowMessageError && IsValid && (
							<FormHelperText>{helperText}</FormHelperText>
						)}
					</FormControl>
				);
			}}
		/>
	);
};