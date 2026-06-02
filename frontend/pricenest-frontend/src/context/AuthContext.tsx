'use client'

import { createContext, useState, useEffect, useContext } from "react"
import { axiosLogin } from "@/api/axios"

//todo: specify user type
interface AuthContextType {
    token: string | null
    user: any | null
    login: (username: string, password: string) => Promise<void>
    logout: () => void
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: React.ReactNode }) {
    const [token, setToken] = useState<string | null>(null);
    const [user, setUser] = useState<any | null>(null);

    useEffect(() => {
        const storedToken = localStorage.getItem("token");
        const storedUser = localStorage.getItem("user");
        if (storedToken) setToken(storedToken);
        if (storedUser) setUser(JSON.parse(storedUser));
    }, []);

    const login = async (username: string, password: string) => {
        const res = await axiosLogin(username, password);
        console.log(res.data.token);
        setToken(res.data.token);
        setUser(username);
        localStorage.setItem("token", res.data.token);
        localStorage.setItem("user", JSON.stringify(username));
    }

    const logout = () => {
        setToken(null);
        setUser(null);
        localStorage.removeItem("token");
        localStorage.removeItem("user");
    }

    return (
        <AuthContext.Provider value={{ token, user, login, logout }}>
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