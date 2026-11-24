import { useState } from "react";
import { useAuth } from "@/Hooks/useAuth";
import { useNavigate } from "react-router";

export default function Sidebar({ activeMenu = "Ringkasan", onMenuClick }) {
  const menuItems = [
    { id: "Ringkasan", label: "Ringkasan" },
    { id: "Pengguna", label: "Pengguna" },
    { id: "Profil", label: "Profil" },
    { id: "Keluar", label: "Keluar" },
  ];

  const { logout } = useAuth();
  const navigate = useNavigate();

  const [loggingOut, setLoggingOut] = useState(false);

  const handleLogout = async () => {
    if (loggingOut) return;

    try {
      setLoggingOut(true);

      await logout();
    } catch (err) {
      console.error("Logout gagal:", err);
    } finally {
      setLoggingOut(false);
      navigate("/login", { replace: true });
    }
  };

  const handleMenuClick = (menuId) => {
    if (menuId === "Keluar") {
      handleLogout();
      return;
    }

    onMenuClick?.(menuId);
  };

  return (
    <aside className="w-[260px] min-h-screen bg-white border-r border-gray-100 flex flex-col justify-between p-6 shrink-0">
      <div>
        <div className="flex items-center gap-3 px-2 mb-10">
          <div className="w-9 h-9 rounded-xl bg-[#276359] flex items-center justify-center text-white font-bold text-lg shadow-xs">
            k
          </div>

          <div className="text-[19px] tracking-tight">
            <span className="font-bold text-gray-900">koda</span>{" "}
            <span className="font-normal text-[#276359]">wallet</span>
          </div>
        </div>

        <div className="px-3 mb-3">
          <span className="text-[11px] font-semibold tracking-wider text-gray-400 uppercase">
            PENGELOLAAN
          </span>
        </div>

        <nav className="space-y-1.5">
          {menuItems.map((item) => {
            const isActive = activeMenu === item.id;
            const isLogout = item.id === "Keluar";

            return (
              <button
                key={item.id}
                type="button"
                disabled={isLogout && loggingOut}
                onClick={() => handleMenuClick(item.id)}
                className={`w-full text-left px-4 py-3 rounded-xl text-[15px] transition-colors ${
                  isLogout
                    ? "text-gray-500 font-medium hover:bg-red-50"
                    : isActive
                      ? "bg-[#eff4f2] text-[#276359] font-semibold"
                      : "text-gray-500 font-medium hover:bg-gray-50 hover:text-gray-800"
                } ${
                  isLogout && loggingOut ? "opacity-50 cursor-not-allowed" : ""
                }`}
              >
                {isLogout && loggingOut ? "Keluar..." : item.label}
              </button>
            );
          })}
        </nav>
      </div>

      <div className="bg-[#eff4f2] rounded-2xl p-4 mt-8">
        <p className="text-[10px] font-semibold tracking-wider text-gray-500 uppercase mb-1">
          BUTUH BANTUAN?
        </p>

        <p className="text-[14px] font-semibold text-gray-900 mb-0.5">
          Pusat Bantuan
        </p>

        <p className="text-[12px] text-gray-500">Panduan dan informasi akun</p>
      </div>
    </aside>
  );
}
