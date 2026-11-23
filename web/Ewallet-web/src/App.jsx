import { Navigate, Route, Routes } from "react-router";
import HomePage from "./pages/HomePage";
import LoginPage from "./pages/AuthPage/LoginPage";
import RegisterPage from "./pages/AuthPage/RegisterPage";
import DashboardCustomer from "./pages/Dashboard/Customer/Beranda";
import ProtectedRoute from "./Auth/ProtectedRoute";

export default function App() {
  return (
    <>
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/dashboard" element={
          <ProtectedRoute>
            <DashboardCustomer />
          </ProtectedRoute>
          } />
          <Route path="*" element={<Navigate to="/dashboard" replace />} />
      </Routes>
    </>
  )
}
