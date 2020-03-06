export const getAsync = async <T>(
  url: string,
  headers?: HeadersInit
): Promise<T> => {
  const response = await fetch(Master_MapPath(url), {
    method: "GET",
    ...getCommonOptions(headers)
  });

  return handleErrors(response);
};

export const postAsync = async <T>(
  url: string,
  data: unknown,
  headers?: HeadersInit
): Promise<T> => {
  const response = await fetch(Master_MapPath(url), {
    body: JSON.stringify(data),
    method: "POST",
    ...getCommonOptions(headers)
  });

  return handleErrors(response);
};

export const deleteAsync = async <T>(
  url: string,
  headers?: HeadersInit
): Promise<T> => {
  const response = await fetch(Master_MapPath(url), {
    method: "DELETE",
    ...getCommonOptions(headers)
  });

  return handleErrors(response);
};

const getCommonOptions = (headers: HeadersInit | undefined): RequestInit => ({
  headers: getDefaultHeaders(headers),
  credentials: "same-origin" // Required by old browsers to include cookies.
});

const getDefaultHeaders = (headers?: HeadersInit) => ({
  "Content-Type": "application/json",
  Section: process.env.CURRENT_SECTION || "",
  ...headers
});

const handleErrors = async (response: Response) => {
  if (response.status === 401) {
    window.location.replace(Master_MapPath("/Global/Login"));
    return;
  }

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
  return (
    `http://lmarang-au-de01/dev/${process.env.BASE_URL}/${process.env.CURRENT_SECTION}/` +
    relativeUrl
  );
}
