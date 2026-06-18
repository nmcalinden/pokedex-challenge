const API_BASE_URL = import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5001';
const SESSION_TOKEN = import.meta.env.VITE_SESSION_TOKEN ?? '';

/**
 * Fetch wrapper that prepends the API base URL and attaches the
 * Authorization: Bearer <token> header for authenticated requests.
 */
export async function apiFetch(path: string, options?: RequestInit): Promise<Response> {
  const url = `${API_BASE_URL}${path}`;
  return fetch(url, {
    ...options,
    headers: {
      'Content-Type': 'application/json',
      // The Bearer token is used by the favourites API to identify the user.
      // See README.md § Authentication for details.
      'Authorization': `Bearer ${SESSION_TOKEN}`,
      ...options?.headers,
    },
  });
}
