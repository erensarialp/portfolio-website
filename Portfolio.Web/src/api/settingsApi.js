import apiClient from "./apiClient";

export async function getSiteSettings() {
    const response = await apiClient.get("/site-settings");
    return response.data;
}

export async function updateSiteSettings(data) {
    const response = await apiClient.put("/site-settings", data);
    return response.data;
}