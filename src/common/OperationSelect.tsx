import React, { FC, useState, useEffect, ChangeEvent } from "react";

import { createStyles, makeStyles, Theme } from "@material-ui/core/styles";

import InputLabel from "@material-ui/core/InputLabel";
import MenuItem from "@material-ui/core/MenuItem";
import FormControl from "@material-ui/core/FormControl";
import Select from "@material-ui/core/Select";

import { OperationDtc } from "../common/types";
import { getAsync } from "../lib/http";

interface Props {
  initOperation?: string;
  onOperationChange: (operation: string) => void;
}

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    formControl: {
      margin: theme.spacing(1),
      width: "100%"
    }
  })
);

const OperationSelect: FC<Props> = ({ initOperation, onOperationChange }) => {
  const classes = useStyles();

  const [operationList, setOperationList] = useState<OperationDtc[] | null>(
    null
  );
  const [operation, setOperation] = useState(initOperation || "");

  useEffect(() => {
    getAsync<OperationDtc[]>("/api/platform/operations")
      .then(operationListParam => {
        setOperationList(operationListParam);
        const newOp = operationListParam[0].Operation;
        setOperation(newOp);
        onOperationChange(newOp);

        localStorage.setItem(
          "operationList",
          JSON.stringify(operationListParam)
        );
      })
      .catch(() => {
        const operationsString = localStorage.getItem("operationList");
        if (operationsString == null) {
          throw new Error();
        }
        const operationListParam = JSON.parse(operationsString);

        setOperationList(operationListParam);
        const newOp = operationListParam[0].Operation;
        setOperation(newOp);
        onOperationChange(newOp);
      });
  }, [onOperationChange]);

  const handleChange = (event: ChangeEvent<{ value: unknown }>) => {
    const newOp = event.target.value as string;
    setOperation(newOp);
    onOperationChange(newOp);
  };

  if (operationList == null) {
    return null;
  }

  return (
    <FormControl className={classes.formControl}>
      <InputLabel id="operation-select">Select an Operation</InputLabel>
      <Select
        autoWidth
        id="operation-select"
        value={operation}
        onChange={handleChange}
      >
        {operationList.map(({ Operation, Description }) => (
          <MenuItem key={Operation} value={Operation}>
            {Description}
          </MenuItem>
        ))}
      </Select>
    </FormControl>
  );
};

export default OperationSelect;
