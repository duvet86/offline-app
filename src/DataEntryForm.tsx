import React, { useState } from "react";
import { useHistory } from "react-router-dom";

import MaterialTable, { Column } from "material-table";
import Button from "@material-ui/core/Button";

interface Row {
  loader: string;
  loaderOperator: string;
  hauler: string;
  haulerOperator: string;
  source: string;
  material: string;
  destination: string;
}

interface TableState {
  columns: Array<Column<Row>>;
  data: Row[];
}

export default function MaterialTableDemo() {
  const history = useHistory();

  const handleLogout = () => {
    document.cookie = "CMPSession= ;";
    history.push("/login");
  };

  const [state, setState] = useState<TableState>({
    columns: [
      { title: "Loader", field: "loader" },
      { title: "LoaderOperator", field: "LoaderOperator" },
      { title: "Hauler", field: "Hauler" },
      { title: "HaulerOperator", field: "HaulerOperator" },
      { title: "Source", field: "source" },
      { title: "Material", field: "material" },
      { title: "Desintation", field: "destination" }
    ],
    data: [
      {
        loader: "EX704",
        loaderOperator: "Aleta Mooe",
        hauler: "TK401",
        haulerOperator: "Ahmad Luca",
        source: "B-HG1B",
        material: "H01",
        destination: "CHC-LIMESTONE"
      }
    ]
  });

  return (
    <>
      <Button variant="contained" color="primary" onClick={handleLogout}>
        Logout
      </Button>
      <MaterialTable
        title="Editable Example"
        columns={state.columns}
        data={state.data}
        editable={{
          onRowAdd: newData =>
            new Promise(resolve => {
              setTimeout(() => {
                resolve();
                setState(prevState => {
                  const data = [...prevState.data];
                  data.push(newData);
                  return { ...prevState, data };
                });
              }, 600);
            }),
          onRowUpdate: (newData, oldData) =>
            new Promise(resolve => {
              setTimeout(() => {
                resolve();
                if (oldData) {
                  setState(prevState => {
                    const data = [...prevState.data];
                    data[data.indexOf(oldData)] = newData;
                    return { ...prevState, data };
                  });
                }
              }, 600);
            }),
          onRowDelete: oldData =>
            new Promise(resolve => {
              setTimeout(() => {
                resolve();
                setState(prevState => {
                  const data = [...prevState.data];
                  data.splice(data.indexOf(oldData), 1);
                  return { ...prevState, data };
                });
              }, 600);
            })
        }}
      />
    </>
  );
}
