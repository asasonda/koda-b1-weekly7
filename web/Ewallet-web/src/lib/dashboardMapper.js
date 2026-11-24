
export const formatRupiah = (amount) =>
  new Intl.NumberFormat("id-ID", {
    style: "currency",
    currency: "IDR",
    maximumFractionDigits: 0,
  }).format(Number(amount) || 0);

export const formatTimestamp = (dateInput) => {
  if (!dateInput) return "-";

  const normalizedDate = /(?:Z|[+-]\d{2}:\d{2})$/i.test(dateInput)
    ? dateInput
    : `${dateInput}Z`;

  const date = new Date(normalizedDate);

  if (Number.isNaN(date.getTime())) return "-";

  const tanggal = date.toLocaleDateString("id-ID", {
    timeZone: "Asia/Jakarta",
    day: "2-digit",
    month: "short",
    year: "numeric",
  });

  const jam = date.toLocaleTimeString("id-ID", {
    timeZone: "Asia/Jakarta",
    hour: "2-digit",
    minute: "2-digit",
    hourCycle: "h23",
  }).replace(":", ".");

  return `${tanggal} · ${jam}`;
};

export const maskWalletNumber = (walletNumber) => {
  if (!walletNumber) return "-";

  const lastFour = walletNumber.slice(-4);

  return `W${lastFour}`;
};

export const mapDashboardData = (raw) => {
  const activities = (raw?.recentMutations ?? []).map(
    (item, index) => {
      const isPositive =
        item.direction?.toUpperCase() === "IN";

      return {
        id: `${item.walletId}-${item.createdAtUtc}-${index}`,
        title: isPositive ? "Dana masuk" : "Dana keluar",
        timestamp: formatTimestamp(item.createdAtUtc),
        type: isPositive ? "Pemasukan" : "Pengeluaran",
        amountText: `${isPositive ? "+" : "-"} ${formatRupiah(
          Math.abs(item.amount)
        )}`,
        isPositive,
        status: "Tercatat",
      };
    }
  );

  return {
    fullName: raw?.fullName ?? raw?.username ?? "Pengguna",
    initials: (raw?.fullName ?? raw?.username ?? "U")
      .trim()
      .split(/\s+/)
      .slice(0, 2)
      .map((word) => word[0])
      .join("")
      .toUpperCase(),

    balanceText: formatRupiah(raw?.wallet?.balance),

    walletNumberMasked: maskWalletNumber(
      raw?.wallet?.walletNumber
    ),

    statusLabel: raw?.wallet?.status ?? "-",

    activities,
  };
};


export const mapAdminDashboard = (data) => ({
  stats: [
    {
      title: "TOTAL PENGGUNA",
      value: Number(data.totalUsers).toLocaleString("id-ID"),
      subtitle: `Seluruh pengguna`,
    },
    {
      title: "AKUN AKTIF",
      value: Number(data.activeUsers).toLocaleString("id-ID"),
      subtitle: `${data.totalUsers > 0
        ? ((data.activeUsers / data.totalUsers) * 100).toFixed(1)
        : 0}% dari seluruh akun`,
    },
    {
      title: "AKUN DITANGGUHKAN",
      value: Number(data.suspendedUsers).toLocaleString("id-ID"),
      subtitle: "Perlu pemantauan",
    },
  ],

  recentUsers: (data.recentUsers ?? []).map((user) => ({
    id: user.id,
    name: user.fullName,
    email: user.email,
    status: user.status,
  })),
});
