import { ThemeProvider } from "@/components/ThemeProvider";
import { AuthProvider } from "@/context/AuthContext";
import "@/app/globals.css";

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="pl" suppressHydrationWarning>
      <head>
        {/* Crazy method for crazy problem */}
        <script
          dangerouslySetInnerHTML={{
            __html: `
              (function() {
                function forceFreshReload() {
                  setTimeout(function() {
                    window.location.reload();
                  }, 0);
                }

                try {
                  var entries = performance.getEntriesByType("navigation");
                  if (entries.length > 0 && entries[0].type === "back_forward") {
                    forceFreshReload();
                  }
                } catch (e) {}

                window.addEventListener("pageshow", function(event) {
                  if (event.persisted) {
                    forceFreshReload();
                  }
                });
              })();
            `
          }}
        />
      </head>
      <body className="min-h-full flex flex-col">
        <ThemeProvider>
          <AuthProvider>
            {children}
          </AuthProvider>
        </ThemeProvider>
      </body>
    </html>
  );
}