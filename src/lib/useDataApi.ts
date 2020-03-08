import {
  useState,
  useReducer,
  useEffect,
  Reducer,
  Dispatch,
  SetStateAction
} from "react";
import { getAsync, postAsync, deleteAsync } from "./http";

export type FetchState<T> = {
  data: T;
  error?: unknown;
  isLoading: boolean;
};

type Action<T> =
  | { type: "FETCH_INIT" }
  | { type: "FETCH_SUCCESS"; data: T }
  | { type: "FETCH_FAILURE"; error: unknown };

export function useGetData<T>(initialUrl: string, initialData: T) {
  return useDataApi<T>("GET", initialUrl, initialData);
}

export function usePostData<T>(
  initialUrl: string,
  initialData: T,
  postData: unknown
) {
  if (postData == null) {
    throw new Error("postData cannot be null. Use useGetData instead.");
  }

  return useDataApi<T>("POST", initialUrl, initialData, postData);
}

export function useDeleteData<T>(initialUrl: string, initialData: T) {
  return useDataApi<T>("DELETE", initialUrl, initialData);
}

function useDataApi<T>(
  type: "GET" | "POST" | "DELETE",
  initialUrl: string,
  initialData: T,
  postData?: unknown
): [FetchState<T>, Dispatch<SetStateAction<string>>] {
  const [url, setUrl] = useState(initialUrl);
  const [state, dispatch] = useReducer<Reducer<FetchState<T>, Action<T>>>(
    dataFetchReducer,
    {
      isLoading: false,
      error: undefined,
      data: initialData
    }
  );

  useEffect(() => {
    let didCancel = false;
    const fetchData = async () => {
      dispatch({ type: "FETCH_INIT" });
      try {
        let data;
        switch (type) {
          case "POST":
            data = await postAsync<T>(url, postData);
            break;
          case "DELETE":
            data = await deleteAsync<T>(url);
            break;
          default:
            data = await getAsync<T>(url);
        }

        if (!didCancel) {
          dispatch({ type: "FETCH_SUCCESS", data });
        }
      } catch (error) {
        if (!didCancel) {
          dispatch({ type: "FETCH_FAILURE", error });
        }
      }
    };

    fetchData();

    return () => {
      didCancel = true;
    };
  }, [type, url, postData]);

  return [state, setUrl];
}

function dataFetchReducer<T>(state: FetchState<T>, action: Action<T>) {
  switch (action.type) {
    case "FETCH_INIT":
      return {
        ...state,
        isLoading: true
      };
    case "FETCH_SUCCESS":
      return {
        ...state,
        isLoading: false,
        data: action.data
      };
    case "FETCH_FAILURE":
      return {
        ...state,
        isLoading: false,
        error: action.error
      };
    default:
      throw new Error();
  }
}
