import React from "react";
import { Link, Route } from "react-router-dom";

import { makeStyles, createStyles, Theme } from "@material-ui/core/styles";

import AppBar from "@material-ui/core/AppBar";
import Toolbar from "@material-ui/core/Toolbar";
import Typography from "@material-ui/core/Typography";
import IconButton from "@material-ui/core/IconButton";
import MenuIcon from "@material-ui/icons/Menu";
import Grid from "@material-ui/core/Grid";
import Button from "@material-ui/core/Button";

import DataEntryDatabase from "./lib/DataEntryDatabase";

import LogoutButton from "./login/LogoutButton";
import DataEntryTabs from "./dataEntry/DataEntryTabs";
import DataEntryForm from "./dataEntry/DataEntryForm";

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    bodyContainer: {
      padding: theme.spacing(2)
    },
    menuButton: {
      marginRight: theme.spacing(2)
    },
    title: {
      flexGrow: 1
    }
  })
);

const db = new DataEntryDatabase();

export default function App() {
  const classes = useStyles();

  return (
    <>
      <AppBar position="static">
        <Toolbar variant="dense">
          <IconButton
            edge="start"
            className={classes.menuButton}
            color="inherit"
            aria-label="menu"
          >
            <MenuIcon />
          </IconButton>
          <Button fullWidth color="inherit" component={Link} to="/">
            <Typography variant="h6" className={classes.title}>
              DATA ENTRY
            </Typography>
          </Button>
          <LogoutButton />
        </Toolbar>
      </AppBar>
      <Grid container className={classes.bodyContainer}>
        <Route exact path="/">
          <DataEntryTabs db={db} />
        </Route>
        <Route exact path="/add-record/:operation">
          <DataEntryForm db={db} />
        </Route>
      </Grid>
    </>
  );
}
