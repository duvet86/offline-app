import React from "react";

function LegacyLoading() {
  return (
    <div className="master-loading-container">
      <div className="master-loading-message-container">
        <i
          className="fa fa-spin fa-circle-o-notch fa-3x"
          aria-hidden="true"
        ></i>
        <div className="master-loading-message">Loading...</div>
      </div>
    </div>
  );
}

export default LegacyLoading;
