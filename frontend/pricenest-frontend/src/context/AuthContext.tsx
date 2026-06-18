'use client'

import { createContext, useState, useEffect, useContext } from "react"
import { axiosLogin } from "@/api/axios"

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

    // useEffect(() => {
    //     const initializeAuth = async () => {
    //         try {
    //             const storedUser = localStorage.getItem("user");

    //             if (storedUser) {      
    //                 await axiosInstance.post("/api/auth/refresh");
    //                 setUser(JSON.parse(storedUser));

    //             }
    //         } catch (error) {
    //             console.log("Session no longer active")
    //             setUser(null);
    //             localStorage.removeItem("user");
    //         } finally {
    //             setLoading(false); 
    //         }
    //     };
    //     initializeAuth();
    // }, []);
    useEffect(() => {
        try {
            const storedUser = localStorage.getItem("user");
            if (storedUser) {
                console.log("Ustawiam user na" + storedUser)
                setUser(JSON.parse(storedUser));
            }
        } catch (error) {
            console.error("Błąd odczytu localStorage:", error);
            localStorage.removeItem("user");
        } finally {
            setLoading(false);
        }
    }, []);

    useEffect(() => {
        const handleForceLogout = () => {
            console.log("Forced logout triggered by Axios interceptor.");
            logout();
        };
        window.addEventListener("force-logout", handleForceLogout);
        return () => window.removeEventListener("force-logout", handleForceLogout);
    }, []);

    const login = async (username: string, password: string) => {
        await axiosLogin(username, password);
        setUser(username);
        localStorage.setItem("user", JSON.stringify(username));
    }

    const logout = () => {
        setUser(null);
        localStorage.removeItem("user");

        window.location.href = "/?logout=true";
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