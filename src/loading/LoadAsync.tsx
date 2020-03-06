import React, { FC, Suspense } from "react";

import BaseLoading from "./BaseLoading";

const LoadAsync: FC = ({ children }) => {
  return <Suspense fallback={<BaseLoading />}>{children}</Suspense>;
};

export default LoadAsync;
