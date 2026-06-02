import axios from "axios";

const axiosInstance = axios.create({
    baseURL: "http://localhost:5295",
    headers: {
        "Content-Type": "application/json",
    },
})

export default axiosInstance;        