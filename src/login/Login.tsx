import React, { useState, ChangeEvent } from "react";
import { useHistory } from "react-router-dom";

import { makeStyles } from "@material-ui/core/styles";
import Button from "@material-ui/core/Button";
import TextField from "@material-ui/core/TextField";

import { postAsync } from "../lib/http";

interface LoginResultDtc {
  Success: boolean;
  Status: string;
  FailMessage: string;
  SessionToken: string;
  ExpiryTime: Date;
}

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

  const history = useHistory();

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    postAsync<LoginResultDtc>("/api/auth/sessions", {
      username,
      password
    })
      .then(({ SessionToken }) => {
        document.cookie = "CMPSession=" + SessionToken + ";";
        history.push("/");
      })
      .catch(e => setError(e.message));
  };

  const handleUsername = (event: ChangeEvent<HTMLInputElement>) => {
    setUsername(event.target.value);
  };

  const handlePassword = (event: ChangeEvent<HTMLInputElement>) => {
    setPassword(event.target.value);
  };

  return (
    <div className={classes.container}>
      <div>
        <form
          onSubmit={handleSubmit}
          className={classes.form}
          noValidate
          autoComplete="off"
        >
          <div>
            <TextField
              label="Username"
              type="text"
              value={username}
              onChange={handleUsername}
            />
          </div>
          <div>
            <TextField
              label="Password"
              type="password"
              value={password}
              onChange={handlePassword}
            />
          </div>

          <div>{error}</div>

          <Button
            type="submit"
            className={classes.loginButton}
            variant="contained"
            color="primary"
          >
            Login
          </Button>
        </form>
      </div>
    </div>
  );
}
