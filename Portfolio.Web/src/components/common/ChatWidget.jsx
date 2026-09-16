import {
    useEffect,
    useRef,
    useState,
} from "react";

import {
    sendChatMessage,
} from "../../api/chatApi";

const quickOptions = [
    "Graphic Design",
    "Social Media",
    "Photography",
    "Video / Reels",
    "Branding",
    "Other",
];

const initialMessages = [
    {
        id: "welcome",
        sender: "bot",
        text:
            "Yeni bir proje mi düşünüyorsun? " +
            "Bir hizmet seçerek başlayabiliriz.",
    },
];

function ChatWidget() {
    const [isOpen, setIsOpen] =
        useState(false);

    const [sessionId, setSessionId] =
        useState(null);

    const [message, setMessage] =
        useState("");

    const [messages, setMessages] =
        useState(initialMessages);

    const [sending, setSending] =
        useState(false);

    const [error, setError] =
        useState("");

    const [isComplete, setIsComplete] =
        useState(false);

    const [whatsAppUrl, setWhatsAppUrl] =
        useState(null);

    const messagesEndRef = useRef(null);
    const inputRef = useRef(null);

    // Mesajlara stabil ID vermek için kullanıyoruz.
    const messageIdRef = useRef(1);

    function createMessageId(sender) {
        const id = `${sender}-${messageIdRef.current}`;

        messageIdRef.current += 1;

        return id;
    }

    useEffect(() => {
        messagesEndRef.current?.scrollIntoView({
            behavior: "smooth",
        });
    }, [messages]);

    useEffect(() => {
        if (
            isOpen &&
            !sending &&
            !isComplete &&
            sessionId
        ) {
            inputRef.current?.focus();
        }
    }, [
        isOpen,
        sending,
        isComplete,
        sessionId,
        messages,
    ]);

    function toggleChat() {
        setIsOpen((current) => !current);
    }

    async function sendMessageToBackend(
        text
    ) {
        const trimmedMessage = text.trim();

        if (
            !trimmedMessage ||
            sending ||
            isComplete
        ) {
            return;
        }

        const userMessage = {
            id: createMessageId("user"),
            sender: "user",
            text: trimmedMessage,
        };

        setMessages((current) => [
            ...current,
            userMessage,
        ]);

        setSending(true);
        setError("");

        try {
            const response =
                await sendChatMessage({
                    sessionId,
                    message: trimmedMessage,
                });

            setSessionId(response.sessionId);

            const botMessage = {
                id: createMessageId("bot"),
                sender: "bot",
                text: response.message,
            };

            setMessages((current) => [
                ...current,
                botMessage,
            ]);

            setIsComplete(
                response.isComplete
            );

            setWhatsAppUrl(
                response.whatsAppUrl ?? null
            );
        } catch (err) {
            console.error(err);

            setError(
                "Şu anda mesaj gönderilemiyor. Lütfen tekrar dene."
            );
        } finally {
            setSending(false);
        }
    }

    async function handleQuickOption(
        option
    ) {
        await sendMessageToBackend(option);
    }

    async function handleSubmit(event) {
        event.preventDefault();

        const currentMessage = message;

        setMessage("");

        await sendMessageToBackend(
            currentMessage
        );
    }

    function handleRestart() {
        setSessionId(null);
        setMessages(initialMessages);
        setMessage("");
        setError("");
        setIsComplete(false);
        setWhatsAppUrl(null);

        messageIdRef.current = 1;
    }

    return (
        <>
            <button
                type="button"
                className={`chat-launcher ${isOpen
                        ? "chat-launcher--hidden"
                        : ""
                    }`}
                onClick={toggleChat}
                aria-label="Sohbeti aç"
            >
                <span className="chat-launcher__icon">
                    ✦
                </span>

                <span>
                    Chat
                </span>
            </button>

            <aside
                className={`chat-widget ${isOpen
                        ? "chat-widget--open"
                        : ""
                    }`}
                aria-hidden={!isOpen}
            >
                <div className="chat-widget__header">
                    <div>
                        <span className="chat-widget__eyebrow">
                            Creative Assistant
                        </span>

                        <h2>
                            Projen hakkında konuşalım.
                        </h2>
                    </div>

                    <button
                        type="button"
                        className="chat-widget__close"
                        onClick={toggleChat}
                        aria-label="Sohbeti kapat"
                    >
                        ×
                    </button>
                </div>

                <div className="chat-widget__body">
                    <div className="chat-widget__messages">
                        {messages.map((item) => (
                            <div
                                key={item.id}
                                className={`chat-message ${item.sender === "user"
                                        ? "chat-message--user"
                                        : "chat-message--bot"
                                    }`}
                            >
                                <p>
                                    {item.text}
                                </p>
                            </div>
                        ))}

                        {sending && (
                            <div className="chat-message chat-message--bot">
                                <div className="chat-typing">
                                    <span />
                                    <span />
                                    <span />
                                </div>
                            </div>
                        )}

                        <div ref={messagesEndRef} />
                    </div>

                    {!sessionId &&
                        !isComplete && (
                            <div className="chat-widget__quick-options">
                                {quickOptions.map(
                                    (option) => (
                                        <button
                                            key={option}
                                            type="button"
                                            disabled={sending}
                                            onClick={() =>
                                                handleQuickOption(
                                                    option
                                                )
                                            }
                                        >
                                            {option}
                                        </button>
                                    )
                                )}
                            </div>
                        )}

                    {error && (
                        <div className="chat-widget__error">
                            {error}
                        </div>
                    )}

                    {isComplete && (
                        <div className="chat-complete">
                            <span className="chat-complete__mark">
                                ✦
                            </span>

                            <h3>
                                Brief hazır.
                            </h3>

                            <p>
                                Proje bilgilerin alındı.
                                Buradan doğrudan iletişime
                                geçebilirsin.
                            </p>

                            {whatsAppUrl && (
                                <a
                                    href={whatsAppUrl}
                                    target="_blank"
                                    rel="noreferrer"
                                    className="chat-complete__whatsapp"
                                >
                                    WhatsApp'tan devam et
                                    <span>↗</span>
                                </a>
                            )}

                            <button
                                type="button"
                                className="chat-complete__restart"
                                onClick={handleRestart}
                            >
                                Yeni konuşma başlat
                            </button>
                        </div>
                    )}
                </div>

                {!isComplete && (
                    <form
                        className="chat-widget__form"
                        onSubmit={handleSubmit}
                    >
                        <input
                            ref={inputRef}
                            type="text"
                            value={message}
                            disabled={sending}
                            onChange={(event) =>
                                setMessage(
                                    event.target.value
                                )
                            }
                            placeholder={
                                sending
                                    ? "Yanıt bekleniyor..."
                                    : "Mesajınızı yazın..."
                            }
                            aria-label="Mesaj"
                        />

                        <button
                            type="submit"
                            disabled={
                                sending ||
                                !message.trim()
                            }
                            aria-label="Mesaj gönder"
                        >
                            →
                        </button>
                    </form>
                )}
            </aside>
        </>
    );
}

export default ChatWidget;