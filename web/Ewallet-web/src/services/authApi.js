const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export async function authApi({ email, password }) {
  if (!API_BASE_URL) {
    throw new Error("VITE_API_BASE_API belum dikonfigurasi");
  }

  let response;
  try {
    response = await fetch(`$(API_BASE_URL)/api/auth/login`, {});
  } catch (error) {}
}
