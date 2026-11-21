import { useState } from 'react';
import AuthAside from '@/components/page/login-register/AuthAside';
import Button from '@/components/ui/Button';
import InputForm from '@/components/ui/InputForm';
import { NavLink } from 'react-router';

export default function RegisterPage() {
  const [formData, setFormData] = useState({
    fullName: '',
    username: '',
    email: '',
    phone: '',
    password: '',
    confirmPassword: '',
  });

  const handleChange = (field) => (e) => {
    setFormData((prev) => ({
      ...prev,
      [field]: e.target.value,
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log('Register submitted:', formData);
  };

  return (
    <div className="min-h-screen flex flex-col md:flex-row font-sans">
      <AuthAside
        title1="Satu akun,"
        title2="satu dompet."
        caption="Dompet digital akan otomatis dibuat setelah pendaftaran berhasil."
      />

      <div className="w-full md:w-[59%] bg-white flex md:mt-20 mt-10">
        <div className="w-full max-w-[840px] md:px-[73px] px-8">
          <div className="mb-11">
            <h2 className="text-2xl md:text-[28px] font-bold text-[#1A1A1A] tracking-tight mb-1.5">
              Buat akun KodaWallet
            </h2>
            <p className="text-xs md:text-sm text-[#7A7A7A]">
              Isi data pribadi untuk mulai menggunakan dompet.
            </p>
          </div>

          <form onSubmit={handleSubmit} className="space-y-5">
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-x-8 gap-y-5">
              <InputForm
                id="fullName"
                label="Nama lengkap"
                type="text"
                value={formData.fullName}
                onChange={handleChange('fullName')}
                placeholder="Nadia Putri"
                required
              />

              <InputForm
                id="username"
                label="Username"
                type="text"
                value={formData.username}
                onChange={handleChange('username')}
                placeholder="nadia"
                required
              />

              <InputForm
                id="email"
                label="Email"
                type="email"
                value={formData.email}
                onChange={handleChange('email')}
                placeholder="nadia@example.com"
                required
              />

              <InputForm
                id="phone"
                label="Nomor telepon"
                type="tel"
                value={formData.phone}
                onChange={handleChange('phone')}
                placeholder="0812 3456 7890"
                required
              />

              <InputForm
                id="password"
                label="Password"
                type="password"
                value={formData.password}
                onChange={handleChange('password')}
                placeholder="Minimal 8 karakter"
                required
              />

              <InputForm
                id="confirmPassword"
                label="Konfirmasi password"
                type="password"
                value={formData.confirmPassword}
                onChange={handleChange('confirmPassword')}
                placeholder="Ulangi password"
                required
              />
            </div>

            <p className="text-xs text-[#7A7A7A] pt-1">
              Akun baru akan mendapatkan dompet aktif dengan saldo Rp0.
            </p>

            <div className="pt-1">
              <Button type="submit" children="Buat akun"/>
            </div>
          </form>

          <div className="mt-7 mb-8 text-center text-xs font-medium text-[#666666]">
            Sudah terdaftar?{' '}
            <NavLink
              to="/login"
              className="font-semibold text-[#326259] hover:underline ml-0.5"
            >
              Masuk
            </NavLink>
          </div>
        </div>
      </div>
    </div>
  );
}