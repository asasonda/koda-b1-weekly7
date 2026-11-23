import { AuthContext } from "@/Context/authContext";
import { useContext } from "react";


export function useAuth() {
  const context = useContext(AuthContext);

  if (context === null) {
    throw new Error("useAuth harus digunakan di dalam AuthProvider.");
  }

  return context;
}