import { createMuiTheme } from "@material-ui/core/styles";

import amber from "@material-ui/core/colors/amber";
import blue from "@material-ui/core/colors/blue";

const configureTheme = () =>
  createMuiTheme({
    overrides: {
      MuiStepIcon: {
        root: {
          "&$completed": {
            color: "green"
          }
        }
      }
    },
    palette: {
      primary: amber,
      secondary: { light: blue[600], main: blue[900], dark: blue[900] }
    }
  });

export default configureTheme;
