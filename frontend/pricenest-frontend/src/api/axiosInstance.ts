import axios from "axios";

const getApiUrl = () => {
    if (typeof window === 'undefined') return process.env.NEXT_PUBLIC_API_URL || "http://localhost:5295";

    if (window.location.hostname.includes('jakkrol.pl')) {
        return "https://api-ceny.jakkrol.pl";
    }

    return process.env.NEXT_PUBLIC_API_URL || "http://localhost:5295";
};

const axiosInstance = axios.create({
    baseURL: getApiUrl(),
    withCredentials: true,
    headers: {
        "Content-Type": "application/json",
    },
});

axiosInstance.interceptors.response.use(
    (response) => response,

    async (error) => {
        const originalRequest = error.config;

        if (originalRequest.url?.includes("/login") && error.response.status == 401) {
            return Promise.reject(error);
        }

        if (error.response.status == 401 && !originalRequest._retry) {
            originalRequest._retry = true;
            console.log("Trying to refresh token")
            try {
                await axiosInstance.post("/api/auth/refresh");
                return axiosInstance(originalRequest);
            } catch (err) {
                const logoutEvent = new CustomEvent("force-logout")
                window.dispatchEvent(logoutEvent)
                return Promise.reject(err)
            }
        }
        return Promise.reject(error)
    }
)

export default axiosInstance;        