import axios from "axios";

const apiClient = axios.create({
    baseURL: import.meta.env.VITE_API_BASE_URL,
    headers: {
        "Content-Type": "application/json",
    },
});

apiClient.interceptors.request.use(
    (config) => {
        const token = sessionStorage.getItem("admin_token");

        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }

        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

apiClient.interceptors.response.use(
    (response) => {
        return response;
    },
    (error) => {
        if (error.response?.status === 401) {
            sessionStorage.removeItem("admin_token");

            if (
                window.location.pathname.startsWith("/admin") &&
                window.location.pathname !== "/admin/login"
            ) {
                window.location.href = "/admin/login";
            }
        }

        return Promise.reject(error);
    }
);

export default apiClient;