import React from "react";
import { useHistory } from "react-router-dom";

import Button from "@material-ui/core/Button";

export default function SimpleTable() {
  const history = useHistory();

  const handleLogout = () => {
    document.cookie = "CMPSession= ;";
    history.push("/login");
  };

  return (
    <Button color="inherit" onClick={handleLogout}>
      Logout
    </Button>
  );
}
