import React from "react";

import { makeStyles } from '@material-ui/core/styles';
import Button from "@material-ui/core/Button";
import TextField from '@material-ui/core/TextField';

const useStyles = makeStyles({
  container: {
    display: "flex",
    justifyContent: "center"
  },
  form: {
    margin: 20
  },
  loginButton: {
    marginTop: 20,
    marginLeft: 75
  }

});

export default function DisableElevation() {
  const classes = useStyles();

  return (
    <div className={classes.container}> 

      <div>
        <form className={classes.form} noValidate autoComplete="off">
          <TextField id="standard-basic" label="Username" />

        </form>
        <form className={classes.form} noValidate autoComplete="off">
          <TextField id="standard-basic" label="Password" />

        </form>

        <Button className={classes.loginButton} variant="contained" color="primary" disableElevation>
          Login
    </Button>
      </div>
    </div>
  );
}