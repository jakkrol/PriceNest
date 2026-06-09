import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";

export function proxy(request: NextRequest) {
  const { pathname, searchParams } = request.nextUrl;

  const isLogoutAction = searchParams.get("logout") === "true";
  let response: NextResponse;


  if (pathname === "/" && isLogoutAction) {
    response = NextResponse.next();
    response.cookies.set("access_token", "", { path: "/", expires: new Date(0) });
    response.cookies.set("refresh_token", "", { path: "/", expires: new Date(0) });
    response.headers.set("Cache-Control", "no-store, no-cache, must-revalidate, proxy-revalidate");
    return response;
  }

  const accessToken = request.cookies.get("access_token")?.value;
  const refreshToken = request.cookies.get("refresh_token")?.value;

  const isProtectedRoute = pathname.startsWith("/dashboard");


  const isUserAuthenticated = !!accessToken || !!refreshToken;

  if (isProtectedRoute && !isUserAuthenticated) {
    response = NextResponse.redirect(new URL("/", request.url));
  } else if (pathname === "/" && isUserAuthenticated) {
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