import axios from "axios";

const axiosInstance = axios.create({
    baseURL: "http://localhost:5295",
    withCredentials: true,
    headers: {
        "Content-Type": "application/json",
    },
});

axiosInstance.interceptors.response.use(
    (response) => response,

    async (error) => {
        const originalRequest = error.config;

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