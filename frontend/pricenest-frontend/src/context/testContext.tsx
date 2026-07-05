// 'use client'

// import { createContext, useState, useContext } from "react"

// interface ThemeContextType {
//     theme: string
//     toggleTheme: () => void
// }

// const ThemeContext = createContext<ThemeContextType | undefined>(undefined)

// export function ThemeProvider({ children }: { children: React.ReactNode }) {
//     const [theme, setTheme] = useState<string>("light")

//     const toggleTheme = () => {
//         if (theme === "light") {
//             setTheme("dark")
//         } else if (theme === "dark") {
//             setTheme("light")
//         }
//     }

//     return (
//         <ThemeContext.Provider value={{ theme, toggleTheme }}>
//             {children}
//         </ThemeContext.Provider>
//     );
// };

// export const useTheme = () => {
//     const context = useContext(ThemeContext)
//     if (!context) {
//         throw new Error("error")
//     }
//     return context
// }
