import React, { FC, useState, useCallback } from "react";
import { Link } from "react-router-dom";
import { getYear, getMonth, getDate } from "date-fns";

import { makeStyles, createStyles, Theme } from "@material-ui/core/styles";

import LinearProgress from "@material-ui/core/LinearProgress";
import Table from "@material-ui/core/Table";
import TableBody from "@material-ui/core/TableBody";
import TableCell from "@material-ui/core/TableCell";
import TableContainer from "@material-ui/core/TableContainer";
import TableHead from "@material-ui/core/TableHead";
import TableRow from "@material-ui/core/TableRow";
import Paper from "@material-ui/core/Paper";
import Fab from "@material-ui/core/Fab";
import AddIcon from "@material-ui/icons/Add";
import Grid from "@material-ui/core/Grid";

import OperationSelect from "../common/OperationSelect";
import { getAsync } from "../lib/http";
import { DataEntryRecord } from "../common/types";

import DataEntryDatabase from "../lib/DataEntryDatabase";

interface Props {
  db: DataEntryDatabase;
}

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    container: {
      padding: theme.spacing(2)
    },
    selectContainer: {
      marginBottom: theme.spacing(2)
    },
    table: {
      minWidth: 650
    },
    addButton: {
      marginTop: theme.spacing(2),
      float: "right"
    }
  })
);

const DataEntryTable: FC<Props> = ({ db }) => {
  const classes = useStyles();

  const [records, setRecords] = useState<DataEntryRecord[] | null>(null);
  const [operation, setOperation] = useState<string>("");

  const onOperationChange = useCallback(
    (operation: string) => {
      setOperation(operation);

      db.table("formData")
        .where("operation")
        .equals(operation)
        .toArray()
        .then(res => {
          const asd = res.map(
            ({ loader, operator, source, material, destination, loads }) => ({
              Loader: loader,
              LoaderOperatorId: operator,
              Origin: source,
              Material: material,
              Destination: destination,
              Loads: loads
            })
          );

          setRecords(asd as DataEntryRecord[]);
        });

      // const today = new Date();
      // const year = getYear(today);
      // const month = getMonth(today) + 1;
      // const day = getDate(today) - 1;

      // const intervalString = `${year}${month > 9 ? month : "0" + month}${
      //   day > 9 ? day : "0" + day
      // }_D`;

      // getAsync<DataEntryRecord[]>(
      //   `/api/loadandhauldataentry/shift-records?operation=${operation}&intervalString=${intervalString}&tabKey=BOG`
      // ).then(recordsParam => {
      //   setRecords(recordsParam);
      //   setOperation(operation);
      // });
    },
    [db]
  );

  return (
    <>
      <Grid item xs={12} className={classes.selectContainer}>
        <OperationSelect onOperationChange={onOperationChange} />
      </Grid>
      <Grid item xs={12}>
        {records == null ? (
          <LinearProgress />
        ) : (
          <>
            <TableContainer component={Paper}>
              <Table className={classes.table} aria-label="simple table">
                <TableHead>
                  <TableRow>
                    <TableCell>#</TableCell>
                    <TableCell>Loader</TableCell>
                    <TableCell>Operator</TableCell>
                    <TableCell>Source</TableCell>
                    <TableCell>Material</TableCell>
                    <TableCell>Destination</TableCell>
                    <TableCell align="right">Loads</TableCell>
                    <TableCell align="right">Nominal Volume</TableCell>
                    <TableCell align="right">Nominal Weight</TableCell>
                    <TableCell>Last Modified</TableCell>
                    <TableCell>Modified By</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {records.length === 0 ? (
                    <TableRow>
                      <TableCell colSpan={11}>No Data</TableCell>
                    </TableRow>
                  ) : (
                    records.map(
                      (
                        {
                          Loader,
                          LoaderOperatorId,
                          Origin,
                          Material,
                          Destination,
                          Loads,
                          NominalVolume,
                          NominalWeight,
                          DateTimeModified,
                          ModifiedBy
                        },
                        i
                      ) => (
                        <TableRow key={i}>
                          <TableCell component="th" scope="row">
                            {i}
                          </TableCell>
                          <TableCell>{Loader}</TableCell>
                          <TableCell>{LoaderOperatorId}</TableCell>
                          <TableCell>{Origin}</TableCell>
                          <TableCell>{Material}</TableCell>
                          <TableCell>{Destination}</TableCell>
                          <TableCell align="right">{Loads}</TableCell>
                          <TableCell align="right">{NominalVolume}</TableCell>
                          <TableCell align="right">{NominalWeight}</TableCell>
                          <TableCell>{DateTimeModified}</TableCell>
                          <TableCell>{ModifiedBy}</TableCell>
                        </TableRow>
                      )
                    )
                  )}
                </TableBody>
              </Table>
            </TableContainer>
            <Fab
              className={classes.addButton}
              component={Link}
              to={`/add-record/${operation}`}
              color="primary"
              aria-label="add"
            >
              <AddIcon />
            </Fab>
          </>
        )}
      </Grid>
    </>
  );
};

export default DataEntryTable;
