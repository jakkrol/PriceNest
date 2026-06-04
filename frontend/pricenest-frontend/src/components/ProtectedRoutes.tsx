'use client'

import { useAuth } from "@/context/AuthContext"
import { redirect } from "next/navigation"

export default function ProtectedRoute({ children }: { children: React.ReactNode }) {
    const { user, loading } = useAuth(); 
    console.log(user)
    if (loading) {
        return null; 
    }
    
    if (!user) {
        redirect("/"); 
    }

    return <>{children}</>;
}