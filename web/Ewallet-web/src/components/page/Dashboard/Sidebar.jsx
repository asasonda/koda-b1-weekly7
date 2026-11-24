import { useAuth } from "@/Hooks/useAuth";
import { useState } from "react";
import { useNavigate } from "react-router";

const MENU_ITEMS = [
  { id: "beranda", label: "Beranda" },
  { id: "topup", label: "Isi Saldo" },
  { id: "transfer", label: "Transfer" },
  { id: "aktivitas", label: "Aktivitas" },
  { id: "pengaturan", label: "Pengaturan" },
  { id: "keluar", label: "Keluar" },
];

export function DashboardLayout({
  activeMenu = "beranda",
  onSelectMenu,
  user = { initials: "NP", name: "Nadia" },
  children,
}) {
  const { logout } = useAuth();
  const navigate = useNavigate();

  const [loggingOut, setLoggingOut] = useState(false);
  const [error, setError] = useState("");

  const handleLogout = async () => {
    if (loggingOut) return;

    try {
      setLoggingOut(true);
      setError("");

      await logout();

      navigate("/login", { replace: true });
    } catch (err) {
      setError(err.message || "Gagal logout. Silakan coba lagi.");
    } finally {
      setLoggingOut(false);
    }
  };

  return (
    <div className="min-h-screen bg-[#f6f8f7] text-[#17231f] flex">
      <aside className="w-60 shrink-0 bg-white border-r border-[#e6ece9] flex flex-col justify-between p-5">
        <div>
          <div className="flex items-center gap-2.5 px-2 mb-8">
            <div className="w-8 h-8 rounded-lg bg-[#2b5b4e] text-white font-semibold flex items-center justify-center text-sm">
              k
            </div>
            <span className="text-lg font-semibold tracking-tight text-[#17231f]">
              koda <span className="text-[#2f6f5e] font-medium">wallet</span>
            </span>
          </div>

          <p className="px-3 mb-2 text-[11px] font-semibold tracking-wider text-[#879590] uppercase">
            Menu Utama
          </p>

          <nav className="space-y-1">
            {MENU_ITEMS.map((item) => {
              const isActive = item.id === activeMenu;
              return (
                <button
                  key={item.id}
                  type="button"
                  disabled={item.id === "keluar" && loggingOut}
                  onClick={() => {
                    if (item.id === "keluar") {
                      handleLogout();
                      return;
                    }

                    onSelectMenu?.(item.id);
                  }}
                  className={`w-full text-left px-3.5 py-2.5 rounded-lg text-sm transition-colors ${
                    item.id === "keluar"
                      ? "text-[#5b6964] hover:bg-red-50"
                      : isActive
                        ? "bg-[#edf4f1] text-[#234e42] font-medium"
                        : "text-[#5b6964] hover:bg-[#f4f7f6] hover:text-[#17231f]"
                  }`}
                >
                  {item.id === "keluar" && loggingOut
                    ? "Sedang keluar..."
                    : item.label}
                </button>
              );
            })}
          </nav>
        </div>

        <div className="rounded-xl bg-[#f1f6f4] p-3.5 border border-[#e3ece8]">
          <p className="text-[10px] font-semibold tracking-wider text-[#7c8c86] uppercase">
            Butuh Bantuan?
          </p>
          <p className="text-xs font-semibold text-[#1e2d28] mt-1">
            Pusat Bantuan
          </p>
          <p className="text-[11px] text-[#6d7c77] mt-0.5 leading-relaxed">
            Panduan dan informasi akun
          </p>
        </div>
      </aside>

      <div className="flex-1 min-w-0 flex flex-col">
        <header className="px-8 pt-6 pb-2 flex items-start justify-between gap-4">
          <div>
            <p className="text-[11px] font-semibold tracking-widest text-[#567c70] uppercase">
              Personal Wallet
            </p>
            <h1 className="text-3xl font-bold tracking-tight text-[#14201c] mt-1">
              Beranda
            </h1>
            <p className="text-xs text-[#73827d] mt-1.5">
              Selasa, 17 November 2026 <span className="mx-1.5">•</span> Selamat
              datang kembali.
            </p>
          </div>

          <div className="flex items-center gap-2.5 bg-white border border-[#e5ece9] rounded-full pl-1.5 pr-3.5 py-1.5 shadow-2xs">
            <span className="w-7 h-7 rounded-full bg-[#2b5b4e] text-white text-xs font-medium flex items-center justify-center">
              {user.initials}
            </span>
            <span className="text-xs font-medium text-[#24322d]">
              {user.name}
            </span>
          </div>
        </header>

        <main className="px-8 py-5 space-y-5 flex-1">
          {error && (
            <div
              role="alert"
              className="rounded-lg border border-red-200 bg-red-50 px-4 py-3"
            >
              <p className="text-sm text-red-600">{error}</p>
            </div>
          )}
          {children}
        </main>
      </div>
    </div>
  );
}
