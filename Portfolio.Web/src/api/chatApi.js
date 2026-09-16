import apiClient from "./apiClient";

export async function sendChatMessage({
    sessionId,
    message,
}) {
    const response = await apiClient.post(
        "/chat/message",
        {
            sessionId,
            message,
        }
    );

    return response.data;
}