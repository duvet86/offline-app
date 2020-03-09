import React, { FC } from "react";

import Paper from "@material-ui/core/Paper";
import { makeStyles } from "@material-ui/core/styles";

import BaseLoading from "./BaseLoading";

interface Props {
  isLoading: boolean;
  pastDelay: boolean;
}

const useStyles = makeStyles({
  container: {
    backgroundColor: "rgba(238, 238, 238, 0.7)",
    height: "100%",
    width: "100%",
    position: "fixed",
    zIndex: 100
  },
  loading: {
    left: "48%",
    position: "absolute",
    top: "30%"
  }
});

const BackgroundLoading: FC<Props> = ({ isLoading, pastDelay, children }) => {
  const classes = useStyles();

  return (
    <>
      {isLoading && pastDelay && (
        <div className={classes.container}>
          <Paper className={classes.loading}>
            <BaseLoading />
          </Paper>
        </div>
      )}
      {children}
    </>
  );
};

export default BackgroundLoading;
