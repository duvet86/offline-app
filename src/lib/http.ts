export const getAsync = async <T>(
  url: string,
  headers?: HeadersInit
): Promise<T> => {
  try {
    const response = await fetch(Master_MapPath(url), {
      method: "GET",
      ...getCommonOptions(headers)
    });

    return handleErrors(response);
  } catch (e) {
    if (e.message === "Failed to fetch") {
      return Promise.reject("Offline");
    }
    throw e;
  }
};

export const postAsync = async <T>(
  url: string,
  data: unknown,
  headers?: HeadersInit
): Promise<T> => {
  try {
    const response = await fetch(Master_MapPath(url), {
      body: JSON.stringify(data),
      method: "POST",
      ...getCommonOptions(headers)
    });

    return handleErrors(response);
  } catch (e) {
    if (e.message === "Failed to fetch") {
      return Promise.reject("Offline");
    }
    throw e;
  }
};

export const deleteAsync = async <T>(
  url: string,
  headers?: HeadersInit
): Promise<T> => {
  try {
    const response = await fetch(Master_MapPath(url), {
      method: "DELETE",
      ...getCommonOptions(headers)
    });

    return handleErrors(response);
  } catch (e) {
    if (e.message === "Failed to fetch") {
      return Promise.reject("Offline");
    }
    throw e;
  }
};

const getCommonOptions = (headers: HeadersInit | undefined): RequestInit => ({
  headers: getDefaultHeaders(headers),
  credentials: "include"
});

const getDefaultHeaders = (headers?: HeadersInit) => ({
  "Content-Type": "application/json",
  Section: process.env.REACT_APP_CURRENT_SECTION || "",
  SiteMapPath: process.env.REACT_APP_CURRENT_SECTION || "",
  ...headers
});

const handleErrors = async (response: Response) => {
  if (!response.ok) {
    const error = await response.text();
    throw new Error(
      JSON.stringify({
        status: response.status,
        error: error && JSON.parse(error)
      })
    );
  }

  const res = await response.text();

  return (res && JSON.parse(res)) || {};
};

function Master_MapPath(relativeUrl: string) {
  return process.env.REACT_APP_BASE_URL + relativeUrl;
}
