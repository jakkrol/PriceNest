'use client';

import { useState } from "react";
import { useAuth } from "@/context/AuthContext";
import { useRouter } from "next/navigation"; 
import { ThemeToggle } from "@/components/ThemeToggle";
import { axiosGetProductsById } from "@/api/axios";
import { useEffect } from "react";

export function LoginForm() {
  const [userLogin, setUserLogin] = useState("");
  const [password, setPassword] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");
  
const { user, loading, login } = useAuth();
  const router = useRouter();

useEffect(() => {

    if (!loading && user) {

      router.replace("/dashboard");
    }
  }, [user, loading, router]);

  const handleFormSubmit = async (e: React.FormEvent) => { 
    e.preventDefault();
    setIsLoading(true);
    setError("");

    try {
      await login(userLogin, password);
      console.log("Success");
      router.push("/dashboard");
      
    } catch (error: any) {
      console.error("Error logging in:", error);
      setError("Błędny login lub hasło");
    } finally {
      setIsLoading(false);
    }
  };

  const handleGetProducts = async () => {
    try {
      const res = await axiosGetProductsById("1");
      console.log(res.data);
    } catch (error) {
      console.error("Error fetching data:", error);
    }
  }

  return (
    <div className="flex min-h-full flex-col justify-center px-6 py-12 lg:px-8">
      <button onClick={handleGetProducts} className="mt-4 p-2 bg-green-500 text-white rounded">Get Products</button>
      <div className="sm:mx-auto sm:w-full sm:max-w-sm">
        <h2 className="mt-10 text-center text-2xl/9 font-bold tracking-tight text-color">Sign in to your account</h2>
      </div>

      {error && (
        <div className="mt-4 p-3 text-sm text-red-500 bg-red-500/10 border border-red-500/20 rounded-md">
          {error}
        </div>
      )}

      <div className="mt-10 sm:mx-auto sm:w-full sm:max-w-sm">
        <form onSubmit={handleFormSubmit} className="space-y-6">
          <div>
            <label className="block text-sm/6 font-medium text-color">Login</label>
            <div className="mt-2">
              <input type="text" value={userLogin} onChange={(e) => setUserLogin(e.target.value)} required className="block w-full rounded-md bg-white/5 px-3 py-1.5 text-base text-white outline-1 -outline-offset-1 outline-white/10 placeholder:text-gray-500 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-500 sm:text-sm/6" />
            </div>
          </div>

          <div>
            <div className="flex items-center justify-between">
              <label className="block text-sm/6 font-medium text-color">Password</label>
            </div>
            <div className="mt-2">
              <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} required className="block w-full rounded-md bg-white/5 px-3 py-1.5 text-base text-white outline-1 -outline-offset-1 outline-white/10 placeholder:text-gray-500 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-500 sm:text-sm/6" />
            </div>
          </div>

          <div>
            <button type="submit" className="flex w-full justify-center rounded-md bg-indigo-500 px-3 py-1.5 text-sm/6 font-semibold text-white hover:bg-indigo-400 focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-500">
              {isLoading ? "Signing in..." : "Sign in"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}