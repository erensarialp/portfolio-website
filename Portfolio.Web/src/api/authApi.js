import apiClient from "./apiClient";

export async function login(username, password) {
    const response = await apiClient.post("/auth/login", {
        username,
        password,
    });

    return response.data;
}

export function saveToken(token) {
    sessionStorage.setItem("admin_token", token);
}

export function getToken() {
    return sessionStorage.getItem("admin_token");
}

export function logout() {
    sessionStorage.removeItem("admin_token");
}

export function isAuthenticated() {
    return !!getToken();
}