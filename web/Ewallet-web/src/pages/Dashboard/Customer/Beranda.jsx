import { Sidebar } from "@/components/page/Dashboard/Sidebar";
import { useState } from "react";
import { useEffect } from "react";
import { useAuth } from "@/Hooks/useAuth";
import { useNavigate } from "react-router";
import { mapDashboardData } from "@/lib/dashboardMapper";
import { getDashboard } from "@/services/dashboard";

function BerandaContent({
  balanceText,
  walletNumberMasked,
  statusLabel,
  activities,
  onTopUpClick,
  onTransferClick,
  onSeeAllClick,
}) {
  return (
    <div className="space-y-5 max-w-6xl">
      <div className="grid grid-cols-1 lg:grid-cols-12 gap-5 items-stretch">
        <section className="lg:col-span-8 rounded-2xl bg-[#2f5f51] text-white p-7 flex flex-col justify-between min-h-[210px]">
          <div>
            <p className="text-[11px] font-semibold tracking-widest text-[#b7d1c9] uppercase">
              Saldo Tersedia
            </p>
            <p className="text-4xl sm:text-[42px] font-bold tracking-tight mt-3 leading-none">
              {balanceText}
            </p>
          </div>

          <div className="mt-8 flex items-center justify-between gap-4">
            <p className="text-xs text-[#cfe0da]">
              Nomor dompet <span className="mx-1.5">·</span>{" "}
              <span className="font-medium text-white">
                {walletNumberMasked}
              </span>
            </p>

            <span className="inline-flex items-center gap-1.5 rounded-full bg-white text-[#234e42] text-xs font-medium px-3 py-1">
              <span className="w-1.5 h-1.5 rounded-full bg-[#2f6f5e]" />
              {statusLabel}
            </span>
          </div>
        </section>

        <section className="lg:col-span-4 rounded-2xl bg-white border border-[#e6ece9] p-6 flex flex-col justify-between">
          <div>
            <h2 className="text-base font-semibold text-[#17231f]">
              Aksi cepat
            </h2>
            <p className="text-xs text-[#7a8984] mt-1">
              Transaksi yang sering digunakan
            </p>
          </div>

          <div className="space-y-2.5 mt-6">
            <button
              type="button"
              onClick={onTopUpClick}
              className="w-full rounded-xl bg-[#2f5f51] hover:bg-[#264e42] text-white text-sm font-medium py-2.5 px-4 transition-colors cursor-pointer"
            >
              + Isi Saldo
            </button>

            <button
              type="button"
              onClick={onTransferClick}
              className="w-full rounded-xl bg-[#f3f6f5] hover:bg-[#e9efed] text-[#24322d] text-sm font-medium py-2.5 px-4 transition-colors cursor-pointer"
            >
              ↗ Transfer
            </button>
          </div>
        </section>
      </div>

      <section className="rounded-2xl bg-white border border-[#e6ece9] p-6">
        <div className="flex items-start justify-between gap-4 pb-4 border-b border-[#eef2f0]">
          <div>
            <h2 className="text-base font-semibold text-[#17231f]">
              Aktivitas terakhir
            </h2>
            <p className="text-xs text-[#7a8984] mt-1">
              Pantau pergerakan uang di dompetmu
            </p>
          </div>

          <button
            type="button"
            onClick={onSeeAllClick}
            className="text-xs font-medium text-[#2f6f5e] hover:text-[#214e42] inline-flex items-center gap-1 cursor-pointer"
          >
            Lihat semua <span>→</span>
          </button>
        </div>

        <div className="divide-y divide-[#f1f4f3]">
          {activities.map((item) => (
            <div
              key={item.id}
              className="py-4 flex flex-col sm:flex-row sm:items-center justify-between gap-2"
            >
              <div>
                <p className="text-sm font-semibold text-[#182420]">
                  {item.title}
                </p>
                <p className="text-xs text-[#879590] mt-0.5">
                  {item.timestamp}
                </p>
              </div>

              <div className="flex items-center justify-between sm:justify-end gap-8">
                <span className="text-xs text-[#879590] w-20">{item.type}</span>
                <span className="text-sm font-semibold w-24 text-right text-[#182420]">
                  {item.amountText}
                </span>
                <span className="text-xs font-medium text-[#2f6f5e] w-14 text-right">
                  {item.status}
                </span>
              </div>
            </div>
          ))}
        </div>
      </section>
    </div>
  );
}

export default function DashboardCustomer() {
  const [activeMenu, setActiveMenu] = useState("beranda");
  const [logoutError, setLogoutError] = useState("");

  const [dashboard, setDashboard] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const { accessToken, logout } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    let active = true;

    async function fetchDashboard() {
      try {
        setLoading(true);
        setError("");

        const response = await getDashboard(accessToken);

        if (!active) return;

        setDashboard(mapDashboardData(response));
      } catch (err) {
        if (!active) return;

        if (err?.status === 401) {
          await logout().catch(() => {});

          if (active) {
            navigate("/login", { replace: true });
          }

          return;
        }

        if (err?.status === 403) {
          setError("Kamu tidak memiliki akses ke dashboard ini.");
          return;
        }

        setError(err?.message ?? "Gagal mengambil data dashboard.");
      } finally {
        if (active) {
          setLoading(false);
        }
      }
    }

    if (!accessToken) {
      navigate("/login", { replace: true });
      return;
    }

    fetchDashboard();

    return () => {
      active = false;
    };
  }, [accessToken, logout, navigate]);

  const user = {
    initials: dashboard?.initials ?? "U",
    name: dashboard?.fullName ?? "Pengguna",
  };

  return (
    <div className="min-h-screen bg-[#f6f8f7] text-[#17231f] flex">
      <Sidebar
        activeMenu={activeMenu}
        onSelectMenu={setActiveMenu}
        onLogoutError={setLogoutError}
      />

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
              Selamat datang kembali, {user.name}.
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
          {logoutError && (
            <div
              role="alert"
              className="rounded-lg border border-red-200 bg-red-50 px-4 py-3"
            >
              <p className="text-sm text-red-600">{logoutError}</p>
            </div>
          )}

          {loading && (
            <div className="py-16 text-center text-sm text-[#73827d]">
              Memuat data dashboard...
            </div>
          )}

          {!loading && error && (
            <div
              role="alert"
              className="rounded-lg border border-red-200 bg-red-50 px-4 py-3"
            >
              <p className="text-sm text-red-600">{error}</p>
            </div>
          )}

          {!loading && !error && dashboard && (
            <BerandaContent
              balanceText={dashboard.balanceText}
              walletNumberMasked={dashboard.walletNumberMasked}
              statusLabel={dashboard.statusLabel}
              activities={dashboard.activities}
              onTopUpClick={() => setActiveMenu("topup")}
              onTransferClick={() => setActiveMenu("transfer")}
              onSeeAllClick={() => setActiveMenu("aktivitas")}
            />
          )}
        </main>
      </div>
    </div>
  );
}
