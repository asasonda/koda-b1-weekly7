export default function Button ({
  children,
  type = 'button',
  onClick,
  disabled = false,
}){
  return (
    <button
      type={type}
      onClick={onClick}
      disabled={disabled}
      className="w-full bg-[#326259] hover:bg-[#274e47] active:scale-[0.99] text-white text-xs md:text-sm font-semibold py-3.5 px-5 rounded-[10px] text-left transition-all cursor-pointer shadow-xs"
    >
      {children}
    </button>
  );
};