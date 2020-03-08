import React, { FC, useState, ChangeEvent } from "react";
import { useHistory, useParams } from "react-router-dom";

import { makeStyles, createStyles, Theme } from "@material-ui/core/styles";

import Card from "@material-ui/core/Card";
import CardActions from "@material-ui/core/CardActions";
import CardContent from "@material-ui/core/CardContent";
import Button from "@material-ui/core/Button";
import Typography from "@material-ui/core/Typography";
import InputLabel from "@material-ui/core/InputLabel";
import MenuItem from "@material-ui/core/MenuItem";
import FormHelperText from "@material-ui/core/FormHelperText";
import FormControl from "@material-ui/core/FormControl";
import Select from "@material-ui/core/Select";
import TextField from "@material-ui/core/TextField";
import Grid from "@material-ui/core/Grid";
import LinearProgress from "@material-ui/core/LinearProgress";

import ArrowBackIosIcon from "@material-ui/icons/ArrowBackIos";
import SaveIcon from "@material-ui/icons/Save";

import DataEntryDatabase from "../lib/DataEntryDatabase";

interface Props {
  db: DataEntryDatabase;
}

interface FormData {
  loader: string;
  operator: string;
  source: string;
  material: string;
  destination: string;
  loads: number;
}

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    formControl: {
      margin: theme.spacing(1),
      width: "100%"
    },
    cardActions: {
      justifyContent: "flex-end"
    },
    titleContainer: {
      display: "flex",
      justifyContent: "space-between",
      alignItems: "center",
      marginBottom: theme.spacing(2)
    }
  })
);

const DataEntryForm: FC<Props> = ({ db }) => {
  const classes = useStyles();

  const history = useHistory();
  const { operation } = useParams();
  const [isLoading, setIsLoading] = useState(false);
  const [formData, setFormData] = useState<FormData>({
    loader: "",
    operator: "",
    source: "",
    material: "",
    destination: "",
    loads: 0
  });

  const handleBackClick = () => {
    history.goBack();
  };

  const handleChange = (type: string) => (
    event: ChangeEvent<{ value: unknown }>
  ) => {
    setFormData(prevData => ({
      ...prevData,
      [type]: event.target.value as string
    }));
  };

  const handkeSaveForm = () => {
    setIsLoading(true);
    db.transaction("rw", db.formData, async () => {
      const formDataWithOperation = {
        ...formData,
        operation
      };
      await db.table("formData").add(formDataWithOperation);
      setIsLoading(false);
      history.push("/");
    }).catch(e => {
      // log any errors
      console.log(e.stack || e);
    });
  };

  return (
    <Grid item xs={12}>
      <div className={classes.titleContainer}>
        <Typography variant="h4">Add Record</Typography>
        <Button
          color="secondary"
          variant="contained"
          startIcon={<ArrowBackIosIcon />}
          onClick={handleBackClick}
        >
          Back
        </Button>
      </div>
      <Card>
        <CardContent>
          {isLoading ? (
            <Grid item xs={12}>
              <LinearProgress />
            </Grid>
          ) : (
            <Grid container spacing={1}>
              <Grid item xs={12}>
                <FormControl
                  className={classes.formControl}
                  variant="filled"
                  size="small"
                  error={formData.loader === ""}
                >
                  <InputLabel id="loader-label">Loader *</InputLabel>
                  <Select
                    labelId="loader-label"
                    value={formData.loader}
                    onChange={handleChange("loader")}
                    required
                  >
                    <MenuItem value={10}>R173</MenuItem>
                    <MenuItem value={20}>R174</MenuItem>
                    <MenuItem value={30}>R176</MenuItem>
                  </Select>
                  <FormHelperText>Required</FormHelperText>
                </FormControl>
              </Grid>
              <Grid item xs={12}>
                <FormControl
                  className={classes.formControl}
                  variant="filled"
                  size="small"
                  error={formData.operator === ""}
                >
                  <InputLabel id="operator-label">Operator *</InputLabel>
                  <Select
                    labelId="operator-label"
                    value={formData.operator}
                    onChange={handleChange("operator")}
                    required
                  >
                    <MenuItem value={10}>004095</MenuItem>
                    <MenuItem value={20}>005000</MenuItem>
                    <MenuItem value={30}>062346</MenuItem>
                  </Select>
                  <FormHelperText>Required</FormHelperText>
                </FormControl>
              </Grid>
              <Grid item xs={12}>
                <FormControl
                  className={classes.formControl}
                  variant="filled"
                  size="small"
                  error={formData.source === ""}
                >
                  <InputLabel id="source-label">Source *</InputLabel>
                  <Select
                    labelId="source-label"
                    value={formData.source}
                    onChange={handleChange("source")}
                    required
                  >
                    <MenuItem value={10}>030-2399</MenuItem>
                    <MenuItem value={20}>030-2399-01</MenuItem>
                    <MenuItem value={30}>040-2301</MenuItem>
                  </Select>
                  <FormHelperText>Required</FormHelperText>
                </FormControl>
              </Grid>
              <Grid item xs={12}>
                <FormControl
                  className={classes.formControl}
                  variant="filled"
                  size="small"
                  error={formData.material === ""}
                >
                  <InputLabel id="material-label">Material *</InputLabel>
                  <Select
                    labelId="material-label"
                    value={formData.material}
                    onChange={handleChange("material")}
                    required
                  >
                    <MenuItem value={10}>ASHK</MenuItem>
                    <MenuItem value={20}>ASHO</MenuItem>
                    <MenuItem value={30}>CSHD</MenuItem>
                  </Select>
                  <FormHelperText>Required</FormHelperText>
                </FormControl>
              </Grid>
              <Grid item xs={12}>
                <FormControl
                  className={classes.formControl}
                  variant="filled"
                  size="small"
                  error={formData.destination === ""}
                >
                  <InputLabel id="destination-label">Destination *</InputLabel>
                  <Select
                    labelId="destination-label"
                    value={formData.destination}
                    onChange={handleChange("destination")}
                    required
                  >
                    <MenuItem value={10}>030-2399</MenuItem>
                    <MenuItem value={20}>030-2399-01</MenuItem>
                    <MenuItem value={30}>040-2301</MenuItem>
                  </Select>
                  <FormHelperText>Required</FormHelperText>
                </FormControl>
              </Grid>
              <Grid item xs={12}>
                <TextField
                  className={classes.formControl}
                  type="number"
                  value={formData.loads}
                  onChange={handleChange("loads")}
                  label="Loads"
                  variant="filled"
                  size="small"
                />
              </Grid>
            </Grid>
          )}
        </CardContent>
        <CardActions className={classes.cardActions}>
          <Button
            color="primary"
            variant="contained"
            startIcon={<SaveIcon />}
            onClick={handkeSaveForm}
          >
            Save
          </Button>
        </CardActions>
      </Card>
    </Grid>
  );
};

export default DataEntryForm;
