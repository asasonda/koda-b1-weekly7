
import AuthAside from "@/components/page/login-register/AuthAside";
import Button from "@/components/ui/Button";
import InputForm from "@/components/ui/InputForm";
import { useAuth } from "@/Hooks/useAuth";
import { useState } from "react";
import { Navigate, NavLink, useNavigate } from "react-router";

const hasRole = (roles, targetRole) =>
  Array.isArray(roles) &&
  roles.some(
    (role) =>
      typeof role === "string" &&
      role.toUpperCase() === targetRole
  );

const getDashboardPath = (user) => {
  if (hasRole(user?.roles, "ADMIN")) {
    return "/admin/dashboard";
  }

  if (hasRole(user?.roles, "CUSTOMER")) {
    return "/dashboard";
  }

  return "/forbidden";
};

export default function LoginPage() {
  const { login, status, isAuthenticated, user } = useAuth();
  const navigate = useNavigate();

  const [formData, setFormData] = useState({
    email: "",
    password: "",
  });

  const [isLoading, setIsLoading] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");

  const handleChange = ({ target: { name, value } }) => {
    setFormData((prev) => ({ ...prev, [name]: value }));
    setErrorMessage("");
  };

  async function handleSubmit(e) {
    e.preventDefault();

    if (isLoading) return;

    if (!formData.email.trim() || !formData.password) {
      return setErrorMessage("Email dan password wajib diisi.");
    }

    setErrorMessage("");
    setIsLoading(true);

    try {
      const session = await login(formData);

      setFormData((prev) => ({ ...prev, password: "" }));

      navigate(getDashboardPath(session.user), {
        replace: true,
      });
    } catch (err) {
      const msg =
        err?.status === 401
          ? "Email atau password salah."
          : err?.status === 429
            ? "Terlalu banyak percobaan Login. Coba kembali nanti."
            : err instanceof Error
              ? err.message
              : "Login gagal. Silakan coba kembali.";

      setErrorMessage(msg);
    } finally {
      setIsLoading(false);
    }
  }

  if (status === "checking") {
    return (
      <main className="flex min-h-screen items-center justify-center">
        Memeriksa sesi Login...
      </main>
    );
  }

  if (isAuthenticated) {
    return (
      <Navigate
        to={getDashboardPath(user)}
        replace
      />
    );
  }

  return (
    <div className="min-h-screen flex flex-col md:flex-row font-sans">
      <AuthAside
        logo="koda wallet"
        title1="Uangmu"
        title2="lebih terarah."
        caption="Kelola saldo, kirim uang, dan lihat setiap transaksi dalam satu tempat."
      />

      <div className="w-full md:w-[59%] bg-white flex items-center justify-center p-8 md:p-16">
        <div className="w-full max-w-[380px]">
          <div className="mb-10">
            <h2 className="text-2xl md:text-[28px] font-bold text-[#1A1A1A] tracking-tight mb-2">
              Selamat datang kembali
            </h2>

            <p className="text-xs md:text-sm text-[#7A7A7A]">
              Masuk untuk melanjutkan ke dompetmu.
            </p>
          </div>

          {errorMessage && (
            <div
              role="alert"
              className="mt-5 mb-3 rounded-lg border border-red-200 bg-red-50 p-4 text-sm text-red-700"
            >
              {errorMessage}
            </div>
          )}

          <form onSubmit={handleSubmit} className="space-y-6">
            <div>
              <InputForm
                label="Email"
                id="email"
                type="email"
                value={formData.email}
                onChange={handleChange}
                placeholder="Masukkan email atau username"
                required={true}
              />
            </div>

            <div>
              <InputForm
                label="Password"
                id="password"
                type="password"
                value={formData.password}
                onChange={handleChange}
                placeholder="masukan password"
                required={true}
              />
            </div>

            <div className="flex justify-end pt-1">
              <a
                href="#forgot-password"
                className="text-xs font-semibold text-[#326259] hover:underline"
              >
                Lupa kata sandi?
              </a>
            </div>

            <Button type="submit" children="Masuk ke akun" />
          </form>

          <div className="mt-8 text-center text-xs text-[#8C8C8C]">
            Belum memiliki akun?{" "}
            <NavLink
              to="/register"
              className="font-semibold text-[#326259] hover:underline ml-1"
            >
              Daftar sekarang
            </NavLink>
          </div>
        </div>
      </div>
    </div>
  );
}
