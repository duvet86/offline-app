import { createMuiTheme } from "@material-ui/core/styles";
import blue from "@material-ui/core/colors/blue";
import grey from "@material-ui/core/colors/grey";

const configureTheme = () =>
  createMuiTheme({
    palette: {
      primary: { light: blue[600], main: blue[700], dark: blue[900] },
      secondary: { light: grey[200], main: grey[300], dark: grey[400] }
    }
  });

export default configureTheme;
