import React, { FC } from "react";
import {
  Redirect,
  Route,
  RouteComponentProps,
  RouteProps
} from "react-router-dom";

import { hasSessionCookie } from "../lib/authApi";

const AuthenticatedRoute: FC<RouteProps> = ({ children, ...props }) => {
  const boundRender = ({ location }: RouteComponentProps) =>
    hasSessionCookie() ? (
      children
    ) : (
      <Redirect
        to={{
          pathname: "/login",
          state: { from: location }
        }}
      />
    );

  return <Route exact {...props} render={boundRender} />;
};

export default AuthenticatedRoute;
