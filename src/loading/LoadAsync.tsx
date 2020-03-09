import React, { FC, Suspense } from "react";
import LegacyLoading from "./LegacyLoading";

const LoadAsync: FC = ({ children }) => {
  return <Suspense fallback={<LegacyLoading />}>{children}</Suspense>;
};

export default LoadAsync;
