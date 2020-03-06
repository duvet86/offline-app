import React, { FC } from "react";
import { RouteComponentProps } from "react-router";
import { Redirect, Route } from "react-router";

import { IRouteProps } from "./types";

const AuthenticatedRoute: FC<IRouteProps> = ({ component, ...props }) => {
  // const boundRender = ({ location }: RouteComponentProps) =>
  //   getTokenFromSession() != null ? (
  //     React.createElement(component)
  //   ) : (
  //     <Redirect
  //       to={{
  //         pathname: "/login",
  //         state: { from: location }
  //       }}
  //     />
  //   );

  // return <Route exact {...props} render={boundRender} />;

  return <Route exact {...props} />;
};

export default AuthenticatedRoute;
