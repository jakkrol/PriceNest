import axiosInstance from "./axiosInstance";

export const axiosLogin = async (username: string, password: string) => {
    try {
        const res = await axiosInstance.post("/api/auth/login", { login: username, password: password });
        localStorage.setItem("token", res.data.token);
        return res;
    } catch (error) {
        console.error("Login error:", error);
        throw error;
    }
}

export const axiosGetProductsById = async (id: string) => {
    try {
        const token = localStorage.getItem("token");
        const res = await axiosInstance.get(`/api/product/${id}`, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
        return res;
    } catch (error) {
        console.error("Error fetching products:", error);
        throw error;
    }
}