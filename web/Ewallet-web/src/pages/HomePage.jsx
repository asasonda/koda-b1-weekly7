import { NavLink } from "react-router";

export default function HomePage() {
  return (
    <div className="flex items-center justify-center mt-4 gap-3">
      <NavLink to="/login" className="bg-amber-300 px-4 py-2 rounded-xl">Login</NavLink>
      <NavLink to="/register" className="bg-emerald-800 px-4 py-2 rounded-xl text-white">Register</NavLink>
    </div>
  )
}
