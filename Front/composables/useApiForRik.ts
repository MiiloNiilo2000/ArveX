import type { NitroFetchOptions, NitroFetchRequest } from "nitropack";

export const useApiForRik = () => {
  const runtimeConfig = useRuntimeConfig();

  const customFetchForRik = async <T>(
    url: string,
    options?: NitroFetchOptions<NitroFetchRequest>
  ) => {

    return await $fetch<T>(url, {
      baseURL: runtimeConfig.public.apiUrl,
      ...options,
    });

  };

  return { customFetchForRik };
};
