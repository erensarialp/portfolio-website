function ContactSection({ settings }) {
    return (
        <section
            id="contact"
            className="contact-section section"
        >
            <div className="site-container">
                <div className="contact-section__inner">
                    <div className="contact-section__headline">
                        <p className="section-label">
                            Contact
                        </p>

                        <h2 className="contact-section__title">
                            {settings?.contactTitle || "Let's create something memorable."}
                        </h2>
                    </div>

                    <div className="contact-section__bottom">
                        <div className="contact-section__description">
                            {settings?.contactDescription && (
                                <p>
                                    {settings.contactDescription}
                                </p>
                            )}
                        </div>

                        <div className="contact-section__links">
                            {settings?.email && (
                                <a href={`mailto:${settings.email}`}>
                                    <span>Email</span>
                                    <strong>{settings.email}</strong>
                                </a>
                            )}

                            {settings?.instagramUrl && (
                                <a
                                    href={settings.instagramUrl}
                                    target="_blank"
                                    rel="noreferrer"
                                >
                                    <span>Instagram</span>
                                    <strong>Visit profile ↗</strong>
                                </a>
                            )}

                            {settings?.whatsAppUrl && (
                                <a
                                    href={settings.whatsAppUrl}
                                    target="_blank"
                                    rel="noreferrer"
                                >
                                    <span>WhatsApp</span>
                                    <strong>Start a conversation ↗</strong>
                                </a>
                            )}

                            {settings?.vimeoUrl && (
                                <a
                                    href={settings.vimeoUrl}
                                    target="_blank"
                                    rel="noreferrer"
                                >
                                    <span>Vimeo</span>
                                    <strong>Watch work ↗</strong>
                                </a>
                            )}
                        </div>
                    </div>
                </div>
            </div>
        </section>
    );
}

export default ContactSection;