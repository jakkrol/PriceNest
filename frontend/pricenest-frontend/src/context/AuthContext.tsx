'use client'

import { createContext, useState, useEffect, useContext } from "react"

//todo: specify user type
interface AuthContextType {
    token: string | null
    user: any | null
    login: (token: string, user: any) => void
    logout: () => void
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: React.ReactNode }) {
    const [token, setToken] = useState<string | null>(null);
    const [user, setUser] = useState<any | null>(null);


}