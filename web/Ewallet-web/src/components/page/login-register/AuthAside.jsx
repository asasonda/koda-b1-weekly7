export default function AuthAside({ logo, title1, title2, caption }){
  return (
    <div className="w-full md:w-[41%] bg-[#326259] text-white px-10 py-10 md:p-16 flex flex-col justify-between min-h-[300px] md:min-h-screen">
      <div className="text-[25px] font-bold">
        {logo}
      </div>

      <div className="my-auto py-12 md:py-0">
        <h1 className="text-4xl md:text-[44px] font-bold mb-6">
          {title1}<br/>{title2}
        </h1>
        <p className="text-sm md:text-[15px] text-white">
          {caption}
        </p>
      </div>

      <div className="w-full border-t border-white/40 pt-4" />
    </div>
  );
};