import AuthAside from "@/components/page/login-register/AuthAside";
import Button from "@/components/ui/Button";
import InputForm from "@/components/ui/InputForm";
import { useState } from "react";
import { NavLink } from "react-router";

export default function LoginPage() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log('Login submitted:', { email, password });
  };

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

          <form onSubmit={handleSubmit} className="space-y-6">
            <div>
              <InputForm 
                label="Email atau username" 
                id="email" 
                type="text" 
                value={email} 
                onChange={(e) => setEmail(e.target.value)} 
                placeholder="Masukkan email atau username"
                required={true}
                />
            </div>

            <div>
              <InputForm
                label="Password"
                id="password"
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
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

            <Button type="submit" children="Masuk ke akun"/>
          </form>

          <div className="mt-8 text-center text-xs text-[#8C8C8C]">
            Belum memiliki akun?{' '}
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
};