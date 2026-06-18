'use client'

import { useState } from "react"
import Link from "next/link"
import { useAuth } from "@/context/AuthContext"
import { ThemeToggle } from "@/components/ThemeToggle"

export default function Navbar() {
    const { user, logout } = useAuth();
    const [isMenuOpen, setIsMenuOpen] = useState(false);

    const handleLogout = () => {
        logout();
        setIsMenuOpen(false);
    };

    const renderUsername = () => {
        console.log("User: " + user)
        if (!user) return "";
        if (typeof user === "object") {
            return user.username || user.login || user.name || "User";
        }
        return user;
    };

    return (
        <nav className="bg-background sticky top-0 z-50  duration-200 min-w-full">
            <div className=" mx-auto px-4 sm:px-6 lg:px-8">
                <div className="flex items-center justify-between h-16">

                    {/* LOGO */}
                    <div className="shrink-0">
                        <h1 className="text-xl font-bold tracking-wider text-nav-accent">
                            SCRAPER_APP
                        </h1>
                    </div>

                    {/* DESKTOP MENU */}
                    <div className="hidden md:flex items-center space-x-6">
                        <Link href="/dashboard" className="text-sm font-medium hover:opacity-85 transition-opacity">
                            Dashboard
                        </Link>
                        <Link href="/dashboard/watchlist" className="text-sm font-medium hover:opacity-85 transition-opacity">
                            Watchlist
                        </Link>
                        <Link href="/dashboard/profile" className="text-sm font-medium hover:opacity-85 transition-opacity">
                            Profile
                        </Link>

                        <div className="flex items-center space-x-4 border-l border-nav-border pl-6">
                            <span className="text-sm text-nav-muted">
                                Hello, <strong className="text-nav-text">{renderUsername()}</strong>
                            </span>
                            <button
                                onClick={handleLogout}
                                className="bg-red-600 hover:bg-red-500 text-white px-3 py-1.5 rounded-md text-sm font-medium transition-colors cursor-pointer"
                            >
                                Logout
                            </button>
                        </div>

                        <div className="border-l border-nav-border pl-4 flex items-center justify-center">
                            <ThemeToggle />
                        </div>
                    </div>

                    {/* MOBILE CONTROLS */}
                    <div className="md:hidden flex items-center space-x-4">
                        <ThemeToggle />

                        <button
                            onClick={() => setIsMenuOpen(!isMenuOpen)}
                            type="button"
                            className="inline-flex items-center justify-center p-2 rounded-md text-nav-muted hover:text-nav-text hover:bg-nav-border/30 focus:outline-none cursor-pointer"
                        >
                            {isMenuOpen ? (
                                <svg className="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M6 18L18 6M6 6l12 12" />
                                </svg>
                            ) : (
                                <svg className="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M4 6h16M4 12h16M4 18h16" />
                                </svg>
                            )}
                        </button>
                    </div>
                </div>
            </div>

            {/* MOBILE MENU */}
            {isMenuOpen && (
                <div className="md:hidden bg-nav-bg border-t border-nav-border">
                    <div className="px-2 pt-2 pb-3 space-y-1 sm:px-3">
                        <Link
                            href="/dashboard"
                            onClick={() => setIsMenuOpen(false)}
                            className="block px-3 py-2 rounded-md text-base font-medium hover:bg-nav-border/30"
                        >
                            Dashboard
                        </Link>
                        <Link
                            href="/dashboard/watchlist"
                            onClick={() => setIsMenuOpen(false)}
                            className="block px-3 py-2 rounded-md text-base font-medium hover:bg-nav-border/30"
                        >
                            Watchlista
                        </Link>
                        <Link
                            href="/dashboard/profile"
                            onClick={() => setIsMenuOpen(false)}
                            className="block px-3 py-2 rounded-md text-base font-medium hover:bg-nav-border/30"
                        >
                            Profil
                        </Link>
                        <div className="pt-4 pb-2 border-t border-nav-border px-3 flex flex-col space-y-3">
                            <span className="text-sm text-nav-muted">
                                Zalogowany jako: <strong className="text-nav-text">{renderUsername()}</strong>
                            </span>
                            <button
                                onClick={handleLogout}
                                className="w-full bg-red-600 hover:bg-red-500 text-white px-4 py-2 rounded-md text-base font-medium transition-colors cursor-pointer"
                            >
                                Wyloguj się
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </nav>
    );
}