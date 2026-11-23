import { useCallback, useEffect, useMemo, useState } from "react";
import { AuthContext } from "./authContext";
import { ApiError, loginUser, logoutUser, refreshSession } from "@/services/authApi";


const EMPTY_SESSION = { user: null, accessToken: null, expiresAtUtc: null };

export function AuthProvider({ children }) {
  const [auth, setAuth] = useState({ status: "checking", ...EMPTY_SESSION });

  const applySession = useCallback(({ user, accessToken, expiresAtUtc }) => {
    setAuth({ status: "authenticated", user, accessToken, expiresAtUtc });
  }, []);

  const clearSession = useCallback(() => {
    setAuth({ status: "anonymous", ...EMPTY_SESSION });
  }, []);

  useEffect(() => {
    let active = true;
    refreshSession()
      .then((session) => active && applySession(session))
      .catch(() => active && clearSession());
    return () => {
      active = false;
    };
  }, [applySession, clearSession]);

  const login = useCallback(
    async (credentials) => {
      const session = await loginUser(credentials);
      applySession(session);
      return session;
    },
    [applySession],
  );

  const refreshAccessToken = useCallback(async () => {
    try {
      const session = await refreshSession();
      applySession(session);
      return session;
    } catch (error) {
      if (error instanceof ApiError && [401, 403].includes(error.status))
        clearSession();
      throw error;
    }
  }, [applySession, clearSession]);

  const logout = useCallback(async () => {
    await logoutUser();
    clearSession();
  }, [clearSession]);

  const contextValue = useMemo(
    () => ({
      ...auth,
      isAuthenticated:
        auth.status === "authenticated" && Boolean(auth.accessToken),
      login,
      refreshAccessToken,
      logout,
    }),
    [auth, login, refreshAccessToken, logout],
  );

  return (
    <AuthContext.Provider value={contextValue}>{children}</AuthContext.Provider>
  );
}