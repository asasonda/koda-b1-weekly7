import AuthAside from "@/components/page/login-register/AuthAside";
import Button from "@/components/ui/Button";
import InputForm from "@/components/ui/InputForm";
import { authApi } from "@/services/authApi";
import { useState } from "react";
import { NavLink } from "react-router";

export default function LoginPage() {
  const [formData, setFormData] = useState({
    email: "",
    password: "",
  });

  const [isLoading, setLoading] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");
  const [successMessage, setSuccessMessage] = useState("");
  const [verifiedUser, setVerifiedUser] = useState(null);

  function handleChange(event) {
    const { name, value } = event.target;

    setFormData((previous) => ({
      ...previous,
      [name]: value,
    }));

    setErrorMessage("");
    setSuccessMessage("");

    setVerifiedUser(null);
  }

  async function handleSubmit(event) {
    event.preventDefault();

    if (isLoading) return;

    setErrorMessage("");
    setSuccessMessage("");
    setVerifiedUser(null);

    if (!formData.email.trim() || !formData.password.trim()) {
      setErrorMessage("Email dan password wajib diisi");
    }

    setLoading(true);

    try {
      const response = await authApi(formData);
      setVerifiedUser(response.user);
      setSuccessMessage("Email dan password cocok");

      setFormData((previous) => ({
        ...previous,
        password: "",
      }));
    } catch (error) {
      setErrorMessage(
        error instanceof Error ? error.message : "Terjadi kesalahan saat login",
      );
    } finally {
      setLoading(false);
    }
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

          {successMessage && (
            <div
              role="status"
              className="mt-5 rounded-xl border border-green-200 bg-green-50 p-4 text-sm text-green-800"
            >
              <p className="font-semibold">Verifikasi berhasil</p>
              <p className="mt-1">{successMessage}</p>
              {verifiedUser && (
                <p className="mt-2">User: {verifiedUser.user}</p>
              )}
            </div>
          )}

          <form onSubmit={handleSubmit} className="space-y-6">
            <div>
              <InputForm
                label="Email atau username"
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
