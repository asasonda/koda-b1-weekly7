import { Navigate, Route, Routes } from "react-router";
import LoginPage from "./pages/AuthPage/LoginPage";
import RegisterPage from "./pages/AuthPage/RegisterPage";
import DashboardCustomer from "./pages/Dashboard/Customer/Beranda";
import ProtectedRoute from "./Auth/ProtectedRoute";
import DashboardPage from "./pages/Dashboard/Admin/Beranda";

export default function App() {
  return (
    <>
      <Routes>
        {/* <Route path="/" element={<HomePage />} /> */}
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route
          path="/dashboard"
          element={
            <ProtectedRoute>
              <DashboardCustomer />
            </ProtectedRoute>
          }
        />
        <Route path="*" element={<Navigate to="/dashboard" replace />} />
        <Route path="/admin/dashboard" element={<DashboardPage />} />
        <Route
          path="/forbidden"
          element={
            <div className="min-h-screen flex items-center justify-center">
              <div className="text-center">
                <h1 className="text-3xl font-bold">403 Forbidden</h1>
                <p className="mt-2 text-gray-500">
                  Kamu tidak memiliki akses ke halaman ini.
                </p>
              </div>
            </div>
          }
        />
      </Routes>
    </>
  );
}
