import apiClient from "./apiClient";

export async function getServices() {
    const response = await apiClient.get("/services");
    return response.data;
}

export async function getAdminServices() {
    const response = await apiClient.get("/services/admin");
    return response.data;
}

export async function createService(data) {
    const response = await apiClient.post("/services", data);
    return response.data;
}

export async function updateService(id, data) {
    const response = await apiClient.put(`/services/${id}`, data);
    return response.data;
}

export async function deleteService(id) {
    await apiClient.delete(`/services/${id}`);
}