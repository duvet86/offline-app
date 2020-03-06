import { RouteComponentProps } from "react-router-dom";

export interface IRouteProps {
  component: React.ComponentType<RouteComponentProps>;
  path: string;
}
