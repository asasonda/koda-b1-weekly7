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

export function BerandaPage({
  balanceText = 'Rp 1.250.000',
  walletNumberMasked = 'KW •••• 1842',
  statusLabel = 'Aktif',
  activities = INITIAL_ACTIVITIES,
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
              Nomor dompet <span className="mx-1.5">·</span>{' '}
              <span className="font-medium text-white">{walletNumberMasked}</span>
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
                <span className="text-xs text-[#879590] w-20">
                  {item.type}
                </span>
                <span
                  className={`text-sm font-semibold w-24 text-right ${
                    item.isPositive ? 'text-[#182420]' : 'text-[#182420]'
                  }`}
                >
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