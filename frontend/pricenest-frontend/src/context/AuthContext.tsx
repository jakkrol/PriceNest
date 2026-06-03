'use client'

import { createContext, useState, useEffect, useContext } from "react"
import { axiosLogin } from "@/api/axios"

//todo: specify user type
interface AuthContextType {
    user: any | null
    login: (username: string, password: string) => Promise<void>
    logout: () => void
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: React.ReactNode }) {
    const [user, setUser] = useState<any | null>(null);

    useEffect(() => {
        const storedUser = localStorage.getItem("user");
        if (storedUser) setUser(JSON.parse(storedUser));
    }, []);

    const login = async (username: string, password: string) => {
        await axiosLogin(username, password);
        setUser(username);
        localStorage.setItem("user", JSON.stringify(username));
    }

    const logout = () => {
        setUser(null);
        localStorage.removeItem("user");
        
        // Ponieważ backend zarządza ciastkiem HttpOnly, frontend nie może go bezpośrednio usunąć przez JS.
        // Najlepszą praktyką jest uderzenie w endpoint w .NET np. /api/auth/logout, 
        // gdzie backend wyczyści ciasteczko wysyłając Response.Cookies.Delete("token").
        // Alternatywnie na czas testów możesz usunąć je tradycyjnie (o ile nie daliśmy HttpOnly):
        document.cookie = "token=; path=/; expires=Thu, 01 Jan 1970 00:00:00 GMT;";
    }

    return (
        <AuthContext.Provider value={{ user, login, logout }}>
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