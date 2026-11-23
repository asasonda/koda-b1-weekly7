import { BerandaPage } from "@/components/page/Dashboard/BerandaPage";
import { DashboardLayout } from "@/components/page/Dashboard/Sidebar";
import { useState } from "react";
// import { DashboardLayout } from "./components/layout/DashboardLayout";
// import { BerandaPage } from "./pages/BerandaPage";

export default function DashboardCustomer() {
  const [activeMenu, setActiveMenu] = useState("beranda");

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
