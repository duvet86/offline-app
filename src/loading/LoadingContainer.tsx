import React, { FC, useEffect, useState } from "react";

import { usePrevious } from "../lib/hooksHelper";

import BackgroundLoading from "./BackgroundLoading";
import Loading from "./Loading";

interface Props {
  isLoading: boolean;
  delay?: number;
  error?: unknown;
  background?: boolean;
}

const LoadingContainer: FC<Props> = ({
  delay,
  error,
  isLoading,
  background,
  children
}) => {
  const [pastDelay, setPastDelay] = useState(false);
  const prevIsLoading = usePrevious(isLoading);

  useEffect(() => {
    function setDelay(delayInput: number) {
      return window.setTimeout(() => {
        setPastDelay(true);
      }, delayInput);
    }

    let clearTimeout: number | undefined;
    if (!isLoading && pastDelay) {
      setPastDelay(false);
    }
    if (!prevIsLoading && isLoading && !pastDelay) {
      clearTimeout = setDelay(delay || 200);
    }

    return () => window.clearTimeout(clearTimeout);
  }, [pastDelay, isLoading, delay, prevIsLoading]);

  if (error != null) {
    if (error instanceof Error) {
      throw error;
    }
    throw new Error(JSON.stringify(error));
  }

  return background ? (
    <BackgroundLoading isLoading={isLoading} pastDelay={pastDelay}>
      {children}
    </BackgroundLoading>
  ) : (
    <Loading isLoading={isLoading} pastDelay={pastDelay}>
      {children}
    </Loading>
  );
};

export default LoadingContainer;
