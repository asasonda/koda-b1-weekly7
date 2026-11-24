import { BerandaPage } from "@/components/page/Dashboard/BerandaPage";
import { DashboardLayout } from "@/components/page/Dashboard/Sidebar";
import { useState } from "react";
// import { DashboardLayout } from "./components/layout/DashboardLayout";
// import { BerandaPage } from "./pages/BerandaPage";

export default function DashboardCustomer() {
  const [activeMenu, setActiveMenu] = useState("beranda");
  const INITIAL_ACTIVITIES = [
  {
    id: 1,
    title: 'Top up melalui QRIS',
    timestamp: '17 Nov 2026 · 09.34',
    type: 'Pemasukan',
    amountText: '+ Rp250.000',
    isPositive: true,
    status: 'Berhasil',
  },
  {
    id: 2,
    title: 'Transfer ke Bima Pratama',
    timestamp: '16 Nov 2026 · 14.20',
    type: 'Pengeluaran',
    amountText: '- Rp75.000',
    isPositive: false,
    status: 'Berhasil',
  },
  {
    id: 3,
    title: 'Transfer dari Rina',
    timestamp: '15 Nov 2026 · 12.08',
    type: 'Pemasukan',
    amountText: '+ Rp120.000',
    isPositive: true,
    status: 'Berhasil',
  },
];

  return (
    <DashboardLayout
      activeMenu={activeMenu}
      onSelectMenu={setActiveMenu}
      user={{ initials: "NP", name: "Nadia" }}
    >
      <BerandaPage
        onTopUpClick={() => setActiveMenu("topup")}
        onTransferClick={() => setActiveMenu("transfer")}
        onSeeAllClick={() => setActiveMenu("aktivitas")}
      />
    </DashboardLayout>
  );
}
