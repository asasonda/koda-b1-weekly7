
import { ApiError } from "./authApi";

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export async function getDashboard(accessToken) {
  if (!accessToken) {
    throw new ApiError(401, "Access token tidak tersedia.");
  }

  const response = await fetch(`${API_BASE_URL}/api/dashboard`, {
    method: "GET",
    credentials: "omit",
    headers: {
      Accept: "application/json",
      Authorization: `Bearer ${accessToken}`,
    },
  }).catch(() => {
    throw new ApiError(0, "Tidak dapat menghubungi server.");
  });

  const data = await response.json().catch(() => null);

  if (!response.ok) {
    throw new ApiError(
      response.status,
      data?.detail ?? data?.title ?? "Gagal mengambil dashboard."
    );
  }

  return data;
}
