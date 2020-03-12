import React, { FC, useState, useCallback } from "react";

import { makeStyles, createStyles, Theme } from "@material-ui/core/styles";

import Grid from "@material-ui/core/Grid";
import Paper from "@material-ui/core/Paper";
import Tabs from "@material-ui/core/Tabs";
import Tab from "@material-ui/core/Tab";
import Typography from "@material-ui/core/Typography";
import Box from "@material-ui/core/Box";

import LoadingContainer from "../loading/LoadingContainer";
import OperationSelect from "../common/OperationSelect";
import DataEntryTable from "./DataEntryTable";

import { getAsync } from "../lib/http";
import DataEntryDatabase from "../lib/DataEntryDatabase";
import { DataEntryTab } from "../common/types";

interface Props {
  db: DataEntryDatabase;
}

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    selectContainer: {
      marginBottom: theme.spacing(2)
    },
    paper: {
      height: "100%"
    }
  })
);

interface TabPanelProps {
  children?: React.ReactNode;
  index: any;
  value: any;
}

function TabPanel(props: TabPanelProps) {
  const { children, value, index, ...other } = props;

  return (
    <Typography
      component="div"
      role="tabpanel"
      hidden={value !== index}
      id={`simple-tabpanel-${index}`}
      aria-labelledby={`simple-tab-${index}`}
      {...other}
    >
      {value === index && <Box p={3}>{children}</Box>}
    </Typography>
  );
}

const DataEntryTabs: FC<Props> = ({ db }) => {
  const classes = useStyles();

  const [value, setValue] = useState(0);
  const [operation, setOperation] = useState<string>("");
  const [dataEntryTabs, setDataEntryTabs] = useState<DataEntryTab[] | null>(
    null
  );

  const handleChange = (event: React.ChangeEvent<{}>, newValue: number) => {
    setValue(newValue);
  };

  const onOperationChange = useCallback((op: string) => {
    setOperation(op);

    getAsync<DataEntryTab[]>(`/api/loadandhauldataentry/${op}/data-entry-tabs`)
      .then(res => {
        setDataEntryTabs(res);
        setValue(0);
      })
      .catch(e => {
        console.log(e);
      });
  }, []);

  return (
    <>
      <Grid item xs={12} className={classes.selectContainer}>
        <OperationSelect onOperationChange={onOperationChange} />
      </Grid>
      <Grid item xs={12}>
        <LoadingContainer isLoading={dataEntryTabs == null}>
          {dataEntryTabs && (
            <Paper className={classes.paper}>
              <Tabs
                value={value}
                indicatorColor="primary"
                textColor="primary"
                onChange={handleChange}
                aria-label="disabled tabs example"
              >
                {dataEntryTabs.map(({ DataEntryTab, Label }) => (
                  <Tab key={DataEntryTab} label={Label} />
                ))}
              </Tabs>
              {dataEntryTabs.map(({ DataEntryTab }, i) => (
                <TabPanel key={DataEntryTab} value={value} index={i}>
                  <DataEntryTable
                    db={db}
                    operation={operation}
                    tabKey={DataEntryTab}
                  />
                </TabPanel>
              ))}
            </Paper>
          )}
        </LoadingContainer>
      </Grid>
    </>
  );
};

export default DataEntryTabs;
