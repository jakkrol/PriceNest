import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";

export function middleware(request: NextRequest) {
  const { pathname, searchParams } = request.nextUrl;
  

  const isLogoutAction = searchParams.get("logout") === "true";

  let response: NextResponse;

  if (pathname === "/" && isLogoutAction) {
    response = NextResponse.next();

    response.cookies.set("access_token", "", { path: "/", expires: new Date(0) });
    

    response.headers.set("Cache-Control", "no-store, no-cache, must-revalidate, proxy-revalidate");
    return response;
  }


  const token = request.cookies.get("access_token")?.value;
  const isProtectedRoute = pathname.startsWith("/dashboard");

  if (isProtectedRoute && !token) {
    response = NextResponse.redirect(new URL("/", request.url));
  } else if (pathname === "/" && token) {
    response = NextResponse.redirect(new URL("/dashboard", request.url));
  } else {
    response = NextResponse.next();
  }


  response.headers.set("Cache-Control", "no-store, no-cache, must-revalidate, proxy-revalidate");
  response.headers.set("Pragma", "no-cache");
  response.headers.set("Expires", "0");

  return response;
}

export const config = {
  matcher: ["/", "/dashboard/:path*"],
};