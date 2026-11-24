import { useEffect, useState } from "react";
import { useNavigate } from "react-router";

import { useAuth } from "@/Hooks/useAuth";
import { getAdminDashboard } from "@/services/admin";
import Sidebar from "@/components/page/Dashboard/Admin/Sidebar";
import { mapAdminDashboard } from "@/lib/dashboardMapper";

export default function DashboardPage() {
  const [activeMenu, setActiveMenu] = useState("Ringkasan");
  const [dashboard, setDashboard] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const { accessToken, logout } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    let isMounted = true;

    const fetchDashboard = async () => {
      if (!accessToken) {
        navigate("/login", { replace: true });
        return;
      }

      try {
        setLoading(true);
        setError("");

        const data = await getAdminDashboard(accessToken);

        if (!isMounted) return;

        const mappedData = mapAdminDashboard(data);
        setDashboard(mappedData);
      } catch (error) {
        if (!isMounted) return;

        if (error.status === 401) {
          await logout().catch(() => {});
          navigate("/login", { replace: true });
          return;
        }

        if (error.status === 403) {
          navigate("/forbidden", { replace: true });
          return;
        }

        setError(error.message || "Gagal mengambil data dashboard.");
      } finally {
        if (isMounted) {
          setLoading(false);
        }
      }
    };

    fetchDashboard();

    return () => {
      isMounted = false;
    };
  }, [accessToken, logout, navigate]);

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-[#f8faf9]">
        <p className="text-sm text-gray-500">Memuat dashboard...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="min-h-screen flex flex-col items-center justify-center gap-3 bg-[#f8faf9]">
        <p className="text-sm text-red-600">{error}</p>
        <button
          onClick={() => window.location.reload()}
          className="text-sm font-semibold text-[#276359] hover:underline"
        >
          Coba lagi
        </button>
      </div>
    );
  }

  if (!dashboard) return null;

  const { stats, recentUsers } = dashboard;

  return (
    <div className="flex min-h-screen bg-[#f8faf9] font-sans text-gray-900">
      <Sidebar activeMenu={activeMenu} onMenuClick={setActiveMenu} />

      <main className="flex-1 px-8 py-9 lg:px-12 overflow-y-auto">
        <div className="max-w-6xl mx-auto">
          <header className="flex flex-col sm:flex-row sm:items-start justify-between gap-4 mb-9">
            <div>
              <p className="text-[11px] font-bold tracking-wider text-[#276359] uppercase mb-1.5">
                ADMIN AREA
              </p>

              <h1 className="text-[28px] font-bold text-gray-900 tracking-tight leading-tight">
                Ringkasan platform
              </h1>

              <p className="text-[14px] text-gray-500 mt-1">
                Pantau pertumbuhan pengguna dan status akun.
              </p>
            </div>

            <div className="flex items-center gap-3 bg-white border border-gray-100 rounded-full py-1.5 pl-2 pr-5 shadow-2xs self-start">
              <div className="w-8 h-8 rounded-full bg-[#276359] text-white flex items-center justify-center text-xs font-semibold">
                AD
              </div>

              <span className="text-[14px] font-medium text-gray-800">
                Admin
              </span>
            </div>
          </header>

          {/* Statistik pengguna */}
          <section className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-7">
            {stats.map((stat) => (
              <div
                key={stat.title}
                className="bg-white rounded-2xl p-6 border border-gray-100/80 shadow-2xs"
              >
                <p className="text-[11px] font-semibold tracking-wider text-gray-500 uppercase mb-4">
                  {stat.title}
                </p>

                <p className="text-[36px] font-bold text-gray-900 leading-none mb-4">
                  {stat.value}
                </p>

                <p className="text-[13px] font-medium text-[#276359]">
                  {stat.subtitle}
                </p>
              </div>
            ))}
          </section>

          <section className="bg-white rounded-2xl p-7 border border-gray-100/80 shadow-2xs">
            <div className="flex items-center justify-between mb-6">
              <h2 className="text-[18px] font-bold text-gray-900">
                Pengguna terbaru
              </h2>

              <button className="text-[13px] font-semibold text-[#276359] hover:underline flex items-center gap-1 cursor-pointer">
                Lihat semua <span>→</span>
              </button>
            </div>

            <div className="divide-y divide-gray-100 border-t border-gray-100">
              {recentUsers.length > 0 ? (
                recentUsers.map((user) => {
                  const isActive = user.status === "ACTIVE";

                  return (
                    <div
                      key={user.id}
                      className="py-4.5 grid grid-cols-1 sm:grid-cols-12 items-center gap-2 sm:gap-4"
                    >
                      <div className="sm:col-span-4 font-semibold text-[14px] text-gray-900">
                        {user.name}
                      </div>

                      <div className="sm:col-span-5 text-[14px] text-gray-500">
                        {user.email}
                      </div>

                      <div className="sm:col-span-3 sm:flex sm:justify-end">
                        <span
                          className={`inline-flex items-center px-4 py-1 rounded-full text-[12px] font-medium ${
                            isActive
                              ? "bg-[#e3f1e8] text-[#286646]"
                              : "bg-[#f8e6e5] text-[#a33b39]"
                          }`}
                        >
                          {user.status}
                        </span>
                      </div>
                    </div>
                  );
                })
              ) : (
                <p className="py-8 text-center text-sm text-gray-500">
                  Belum ada pengguna.
                </p>
              )}
            </div>
          </section>
        </div>
      </main>
    </div>
  );
}
