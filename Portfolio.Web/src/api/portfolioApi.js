import apiClient from "./apiClient";

// PUBLIC

export async function getPortfolio(category = null) {
    const params = category
        ? { category }
        : {};

    const response = await apiClient.get("/portfolio", {
        params,
    });

    return response.data;
}

export async function getPortfolioBySlug(slug) {
    const response = await apiClient.get(
        `/portfolio/${slug}`
    );

    return response.data;
}

// ADMIN

export async function getAdminPortfolio() {
    const response = await apiClient.get(
        "/admin/portfolio"
    );

    return response.data;
}

export async function getAdminPortfolioById(id) {
    const response = await apiClient.get(
        `/admin/portfolio/${id}`
    );

    return response.data;
}

export async function createPortfolio(data) {
    const response = await apiClient.post(
        "/admin/portfolio",
        data
    );

    return response.data;
}

export async function updatePortfolio(id, data) {
    const response = await apiClient.put(
        `/admin/portfolio/${id}`,
        data
    );

    return response.data;
}

export async function deletePortfolio(id) {
    await apiClient.delete(
        `/admin/portfolio/${id}`
    );
}