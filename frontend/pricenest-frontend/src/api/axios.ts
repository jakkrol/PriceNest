import axios from "axios";
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

/////////////////////////////////////////////////////////////////////////////
export const axiosScrapeProduct = async (item: string) => {
    try {
        console.log("Wysyłam zapytanie do scrapera z itemem:", item);

        return await axiosInstance.post(`/api/product/scrape?item=${encodeURIComponent(item)}`, {});
    } catch (error) {
        console.error("Error scraping product:", error);
        throw error;
    }
}


// TO TEST ENDPOINT
export const axiosAddToWatchlist = async (prodyctName: string, currentUrl: string, currentPrice: number, targetPrice: number) => {
    try {
        console.log("Wysyłam zapytanie do dodania do watchlisty z danymi:", { prodyctName, currentUrl, currentPrice, targetPrice });
        return await axiosInstance.post("/api/watchlist", { prodyctName, currentUrl, currentPrice, targetPrice });
    } catch (error) {
        console.error("Error adding product to watchlist:", error);
        throw error;
    }
}

