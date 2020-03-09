import React, { FC } from "react";

import { makeStyles, withStyles } from "@material-ui/core/styles";
import LinearProgress from "@material-ui/core/LinearProgress";

const useStyles = makeStyles({
  root: {
    alignItems: "center",
    display: "flex",
    height: "100%",
    justifyContent: "center",
    width: "100%"
  },
  laodingContainer: {
    width: "100%",
    margin: "16px 28px 16px 28px"
  }
});

const ColorLinearProgress = withStyles({
  colorPrimary: {
    backgroundColor: "#eee"
  },
  barColorPrimary: {
    backgroundColor: "#ccc"
  }
})(LinearProgress);

const BaseLoading: FC = () => {
  const classes = useStyles();

  return (
    <div className={classes.root}>
      <div className={classes.laodingContainer}>
        <ColorLinearProgress />
      </div>
    </div>
  );
};

export default BaseLoading;
