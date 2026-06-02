'use client';

import Image from "next/image";
import { ThemeToggle } from "@/components/ThemeToggle";
import { useAuth } from "@/context/AuthContext";

import { axiosGetProductsById } from "@/api/axios";

export default function Home() {

  const { login, user } = useAuth();

  const handleClick = async () => {
    try {
      await login("admin", "admin");
    } catch (error) {
      console.error("Error fetching data:", error);
    }
  }

  const handleGetProducts = async () => {
    try {
      const res = await axiosGetProductsById("1");
      console.log(res.data);
    } catch (error) {
      console.error("Error fetching data:", error);
    }
  }

  return (
    <div className="flex flex-col flex-1 items-center justify-center  font-sans relative ">
      <main className="flex flex-1 w-full max-w-3xl flex-col items-center justify-between py-32 px-16  sm:items-start">
        <div className="absolute top-6 right-6">
          <ThemeToggle />
        </div>
        <button onClick={handleClick} className="mt-4 p-2 bg-blue-500 text-white rounded">Login</button>
        <button onClick={handleGetProducts} className="mt-4 p-2 bg-green-500 text-white rounded">Get Products</button>
      </main>
    </div>
  );
}
