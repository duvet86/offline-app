import React, { FC } from "react";
import {
  Redirect,
  Route,
  RouteComponentProps,
  RouteProps
} from "react-router-dom";

import { hasSessionCookie } from "../lib/authApi";

const AnonymousRoute: FC<RouteProps> = ({ children, ...props }) => {
  const boundRender = ({ location }: RouteComponentProps) =>
    !hasSessionCookie() ? (
      children
    ) : (
      <Redirect
        to={{
          pathname: "/",
          state: { from: location }
        }}
      />
    );

  return <Route exact {...props} render={boundRender} />;
};

export default AnonymousRoute;
