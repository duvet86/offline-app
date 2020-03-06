import "./index.css";
import "typeface-roboto";

import React from "react";
import { render } from "react-dom";
import { BrowserRouter, Switch } from "react-router-dom";

import CssBaseline from "@material-ui/core/CssBaseline";
import { MuiThemeProvider } from "@material-ui/core/styles";

import configureTheme from "./configureTheme";

import LoadAsync from "./LoadAsync";
import Login from "./Login";

import * as serviceWorker from "./serviceWorker";

const theme = configureTheme();

render(
  <BrowserRouter>
    <MuiThemeProvider theme={theme}>
      <CssBaseline />
      <LoadAsync>
        <Switch>
          <Login />
          {/* <AnonymousRoute path="/login" component={LoginContainerAsync} />
          <AuthenticatedRoute path="/" component={AppContainerAsync} /> */}
        </Switch>
      </LoadAsync>
    </MuiThemeProvider>
  </BrowserRouter>,
  document.getElementById("root")
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
