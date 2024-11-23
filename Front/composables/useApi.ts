import type { NitroFetchOptions, NitroFetchRequest } from "nitropack";

export const useApi = () => {
  const runtimeConfig = useRuntimeConfig();

  const customFetch = async <T>(
    url: string,
    options?: NitroFetchOptions<NitroFetchRequest>
  ) => {
    const headers = {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`,
      ...(options?.headers || {})
    };

    try {
      return await $fetch<T>(url, {
        baseURL: runtimeConfig.public.apiUrl,
        headers,
        ...(options || {}),
        body: typeof options?.body === 'object' ? JSON.stringify(options.body) : options?.body
      });
    } catch (error: any) {
      console.error('Fetch error:', error?.data || error);
      throw error;
    }
  };

  return { customFetch };
};

