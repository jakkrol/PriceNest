import Navbar from "@/components/Navbar";
//import ProtectedRoute from "@/components/ProtectedRoutes";

export default function DashboardLayout({ children }: { children: React.ReactNode }) {
    return (
        <div>
            <main className="dashboard-container">
                <Navbar/>
                {children}
            </main>
        </div>
    );
}