'use client';

import { useTheme } from "next-themes";
import { useEffect, useState } from "react";

export function ThemeToggle() {
    const { theme, setTheme } = useTheme()
    const [mounted, setMounted] = useState(false)

    useEffect(() => {
        setMounted(true);
    }, []);

    // useEffect(() => {
    //     console.log("test")
    // }, [theme])

    if (!mounted) return <div className='p-9 m-9'></div>

    return (
        <button
            onClick={() => setTheme(theme === "dark" ? "light" : "dark")}
            className="p-2 rounded-lg bg-slate-100 dark:bg-slate-800 hover:bg-slate-200 dark:hover:bg-slate-700 transition-colors"
            aria-label="Zmień motyw"
        >
            {theme === "dark" ? "☀️ Jasny" : "🌙 Ciemny"}
        </button>
    );
}