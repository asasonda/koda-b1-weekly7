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

export function Sidebar({ activeMenu = "beranda", onSelectMenu, onLogoutError }) {
  const { logout } = useAuth();
  const navigate = useNavigate();

  const [loggingOut, setLoggingOut] = useState(false);

  const handleLogout = async () => {
    if (loggingOut) return;

    try {
      setLoggingOut(true);
      onLogoutError?.("");

      await logout();
      navigate("/login", { replace: true });
    } catch (err) {
      onLogoutError?.(err.message || "Gagal logout. Silakan coba lagi.");
    } finally {
      setLoggingOut(false);
    }
  };

  return (
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
                className={`w-full text-left px-3.5 py-2.5 rounded-lg text-sm transition-colors cursor-pointer ${
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
  );
}