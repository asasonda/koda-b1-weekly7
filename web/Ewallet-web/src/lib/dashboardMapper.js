export const formatRupiah = (amount) =>
  new Intl.NumberFormat("id-ID", {
    style: "currency",
    currency: "IDR",
    maximumFractionDigits: 0,
  }).format(Number(amount) || 0);

export const formatTimestamp = (dateInput) => {
  if (!dateInput) return "-";
  const date = new Date(dateInput);
  if (Number.isNaN(date.getTime())) return String(dateInput);

  const tanggal = date.toLocaleDateString("id-ID", {
    day: "2-digit",
    month: "short",
    year: "numeric",
  });
  const jam = date
    .toLocaleTimeString("id-ID", { hour: "2-digit", minute: "2-digit" })
    .replace(":", ".");
  return `${tanggal} · ${jam}`;
};

export const mapDashboardData = (raw) => {
  const rawActivities =
    raw?.activities ?? raw?.recentActivities ?? raw?.transactions ?? [];

  const activities = rawActivities.map((item, idx) => {
    const isPositive =
      typeof item.isPositive === "boolean"
        ? item.isPositive
        : String(item.type ?? "").toLowerCase().includes("masuk") ||
          String(item.direction ?? "").toLowerCase() === "in" ||
          Number(item.amount) >= 0;

    const absAmount = Math.abs(Number(item.amount ?? 0));

    return {
      id: item.id ?? idx + 1,
      title: item.title ?? item.description ?? "Transaksi",
      timestamp: item.timestamp ?? formatTimestamp(item.createdAt),
      type: item.type ?? (isPositive ? "Pemasukan" : "Pengeluaran"),
      amountText:
        item.amountText ?? `${isPositive ? "+" : "-"} ${formatRupiah(absAmount)}`,
      isPositive,
      status: item.status ?? "Berhasil",
    };
  });

  return {
    balanceText:
      raw?.balanceText ?? formatRupiah(raw?.balance ?? raw?.availableBalance ?? 0),
    walletNumberMasked:
      raw?.walletNumberMasked ?? raw?.walletNumber ?? "KW •••• ----",
    statusLabel: raw?.statusLabel ?? raw?.status ?? "Aktif",
    activities,
  };
};