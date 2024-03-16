import * as MuiIcons from '@mui/icons-material'
import { ManageAccountsRounded } from "@mui/icons-material";

export const IconComponents = (sIcon: string) => {
  if (sIcon) {
    const Icon = MuiIcons[sIcon]
    return <Icon sx={{ mr: 0.5 }} />;
  }
  else {
    return null
  }
}
