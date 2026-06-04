'use client'

import ProtectedRoute from "@/components/ProtectedRoutes";

export default function DashboardLayout({ children }: { children: React.ReactNode }) {
    return (
        <ProtectedRoute>
            <main className="dashboard-container">
                {children}
            </main>
        </ProtectedRoute>
    );
}