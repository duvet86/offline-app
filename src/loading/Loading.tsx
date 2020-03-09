import React, { FC } from "react";

import BaseLoading from "./BaseLoading";

interface Props {
  isLoading: boolean;
  pastDelay: boolean;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  children: any;
}

const Loading: FC<Props> = ({ isLoading, pastDelay, children }) => {
  if (isLoading) {
    if (pastDelay) {
      // When the loader has taken longer than the delay show a spinner.
      return <BaseLoading />;
    } else {
      return null;
    }
  }

  // When the loader has finished.
  return children;
};

export default Loading;
