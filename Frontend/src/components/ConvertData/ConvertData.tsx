import { List as LINQ } from "linqts";

export const IsNumberic = (sVal) => {
    sVal = (sVal + "").replace(/,/g, "");
    return !isNaN(sVal) && sVal !== "";
};

export const addCommas = (nStr) => {
    nStr += "";
    let x = nStr.split(".");
    let x1 = x[0];
    let x2 = x.length > 1 ? "." + x[1] : "";
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, "$1" + "," + "$2");
    }
    return x1 + x2;
};

export const CommaSeparateNumber = (value: any, _MaxDigits?, _MaxInteger?) => {
    let MaxDigits = _MaxDigits || null;
    let MaxInteger = _MaxInteger || 25;
    if (value != undefined && value != null && (value + "") != "" && value != "N/A") {
        let val = parseFloat((value + "").replaceAll(",", ""))
        var sign = 1;
        if (val < 0) {
            sign = -1;
            val = -val;
        }
        let num = val.toString().includes('.') ? val.toString().split('.')[0] : val.toString();
        num = num.substring(0, MaxInteger);
        if (val.toString().includes('.')) {

            if (MaxDigits) {
                num = (+val).toLocaleString(undefined, { maximumFractionDigits: MaxDigits }).replaceAll(",", ""); //ตัดเศษแบบปัดเศษ
            } else {
                num = num + '.' + (val.toString().split('.')[1]);
            }
        }
        //ตัดเศษแบบปัดเศษ กรณีส่ง Digits และมีค่า
        if (MaxDigits && num != undefined && num != null && num != "") {
            if (MaxDigits) {
                num = (+num).toLocaleString(undefined, { maximumFractionDigits: MaxDigits }).replaceAll(",", "");
            } else {
                num = num;
            }
        }
        if (num != undefined && num != null && num != "NaN" && num != "") {
            let indValue = (num + "").split(".");
            let valueResult = indValue[0];
            if ((num + "") != "0") {
                while (/(\d+)(\d{3})/.test(valueResult)) {
                    valueResult = valueResult.replace(/(\d+)(\d{3})/, '$1' + ',' + '$2');
                }
                if (indValue.length > 1) {
                    valueResult = valueResult + "." + indValue[1]
                }
                return sign < 0 ? '-' + valueResult : valueResult;
            } else {
                return sign < 0 ? '-' + "0" : "0";
            }
        } else {
            return "";
        }
    } else {
        if (value == "N/A") {
            return value;
        } else {
            return "";
        }
    }
}

export const SetFormatNumber = (nNumber, nDecimal, sEmpty) => {
    if (IsNumberic(nNumber)) {
        if (IsNumberic(nDecimal)) return addCommas(nNumber.toFixed(nDecimal));
        else return addCommas(nNumber);
    } else {
        return !nNumber ? (sEmpty === undefined ? "" : sEmpty) : nNumber;
    }
};

export const SetValueSum = (arr) => {
    let v = arr.filter(f => f != undefined && f != null);
    if (v.length > 0) {
        let val = new LINQ<any>(v).Sum(s => s);
        return CommaSeparateNumber(val);
    } else {
        return "";
    }
}

export const ConvertStringToNumber = (val) => {
    if (val != undefined && val != null) {
        let values = (val + "").replaceAll(",", "")
        if (values && values != "" && values != null) {
            let valConve = parseFloat(values);
            return valConve;
        } else {
            return null;
        }
    }
}

export const FormatNumber = (num: any, digit: number) => {
    if (num != null && num !== "N/A" && num !== "") {
        let indValue = (num + "").split(".");
        let valueResult = indValue[0];
        if ((num + "") != "0") {
            valueResult = valueResult.replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');
            let valueDigit = "0000";
            if (indValue.length > 1) {
                let valueDigit = indValue[1];
                if (valueDigit.length < 4) {
                    // let nDigit = digit - valueDigit.length;
                    for (let index = valueDigit.length; index < digit; index++) {
                        valueDigit = valueDigit + "0";
                    }
                }
                valueResult = valueResult + "." + valueDigit.substring(0, digit);
            }
            else {
                valueResult = valueResult + "." + valueDigit;
            }
            return valueResult;
        }
        return parseFloat(num).toFixed(digit);
    }
    else {
        return num
    }
}