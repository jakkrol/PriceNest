'use client'

import { createContext, useState, useEffect, useContext } from "react"
import { axiosLogin } from "@/api/axios"
import axiosInstance from "@/api/axiosInstance"
import { useRouter } from "next/navigation"

interface AuthContextType {
    user: any | null
    login: (username: string, password: string) => Promise<void>
    logout: () => void
    loading: boolean
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: React.ReactNode }) {
    const [user, setUser] = useState<any | null>(null);
    const [loading, setLoading] = useState<boolean>(true); 
    const router = useRouter();

    //to test - should work ok
    useEffect(() => {
        const initializeAuth = async () => {
            try {
                const storedUser = localStorage.getItem("user");
                
                if (storedUser) {      
                    await axiosInstance.post("/api/auth/refresh");
                    setUser(JSON.parse(storedUser));

                    const currentPath = window.location.pathname;
                    if(currentPath == "/" || currentPath == "/register"){
                        router.push("/dashboard")
                    }
                }
            } catch (error) {
                console.log("Session not longer active")
                setUser(null);
                localStorage.removeItem("user");
            } finally {
                setLoading(false); 
            }
        };
        initializeAuth();
    }, []);

    useEffect(() => {
        const handleForceLogout = () => {
            console.log("Forced logout triggered by Axios interceptor.");
            logout();
        };

        window.addEventListener("auth:force-logout", handleForceLogout);

        return () => {
            window.removeEventListener("auth:force-logout", handleForceLogout);
        };
    }, []);

    const login = async (username: string, password: string) => {
        await axiosLogin(username, password);
        setUser(username);
        localStorage.setItem("user", JSON.stringify(username));
    }

    const logout = () => {
        setUser(null);
        localStorage.removeItem("user");
    }

    return (
        <AuthContext.Provider value={{ user, login, logout, loading }}>
            {children}
        </AuthContext.Provider>
    );
}

export const useAuth = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error("useAuth must be used within an AuthProvider");
    }
    return context;
};