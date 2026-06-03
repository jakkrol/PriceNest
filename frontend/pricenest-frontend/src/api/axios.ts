import axiosInstance from "./axiosInstance";

export const axiosLogin = async (username: string, password: string) => {
    try {
        return await axiosInstance.post("/api/auth/login", { login: username, password: password });
    } catch (error) {
        console.error("Login error:", error);
        throw error;
    }
}

export const axiosGetProductsById = async (id: string) => {
    try {
        return await axiosInstance.get(`/api/product/${id}`);
    } catch (error) {
        console.error("Error fetching products:", error);
        throw error;
    }
}