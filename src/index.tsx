import "./index.css";
import "typeface-roboto";

import React from "react";
import { render } from "react-dom";
import { BrowserRouter, Switch } from "react-router-dom";

import CssBaseline from "@material-ui/core/CssBaseline";
import { MuiThemeProvider } from "@material-ui/core/styles";

import configureTheme from "./lib/configureTheme";

import LoadAsync from "./loading/LoadAsync";
import Login from "./login/Login";
import App from "./App";

import AnonymousRoute from "./routes/AnonymousRoute";
import AuthenticatedRoute from "./routes/AuthenticatedRoute";

import * as serviceWorker from "./serviceWorker";

const theme = configureTheme();

render(
  <BrowserRouter>
    <MuiThemeProvider theme={theme}>
      <CssBaseline />
      <LoadAsync>
        <Switch>
          <AnonymousRoute exact path="/login">
            <Login />
          </AnonymousRoute>
          <AuthenticatedRoute path="/">
            <App />
          </AuthenticatedRoute>
        </Switch>
      </LoadAsync>
    </MuiThemeProvider>
  </BrowserRouter>,
  document.getElementById("root")
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.register();
