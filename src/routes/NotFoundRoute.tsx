import React from "react";
import { Route } from "react-router-dom";

const NotFound = () => <div>Not Found.</div>;
const NotFoundRoute = () => <Route component={NotFound} />;

export default NotFoundRoute;
