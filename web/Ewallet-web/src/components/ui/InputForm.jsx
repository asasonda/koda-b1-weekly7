export default function InputForm({
  id,
  label,
  type = 'text',
  value,
  onChange,
  placeholder,
  required = false,
  className = '',
}){

  return (
    <div>
      {label && (
        <label
          className="block text-xs font-bold text-[#666666] mb-2">
          {label}
        </label>
      )}
      <input
        id={id}
        name={id}
        type={type}
        value={value}
        onChange={onChange}
        placeholder={placeholder}
        required={required}
        className={`w-full px-3.5 py-2.5 text-sm text-[#1A1A1A] bg-[#F5F8FC] border border-transparent focus:border-[#326259] focus:bg-gray-50/50 rounded-[9px] outline-none transition-colors ${className}`}
      />
    </div>
  );
};