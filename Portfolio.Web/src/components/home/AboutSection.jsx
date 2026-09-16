function AboutSection({ settings, services }) {
    return (
        <section
            id="about"
            className="about-section section"
        >
            <div className="site-container">
                <div className="about-section__grid">
                    <div className="about-section__main">
                        <p className="section-label">
                            About
                        </p>

                        <h2 className="section-title about-section__title">
                            {settings?.aboutTitle}
                        </h2>

                        {settings?.aboutDescription && (
                            <p className="about-section__description">
                                {settings.aboutDescription}
                            </p>
                        )}
                    </div>

                    <aside className="about-section__side">
                        <div className="about-section__side-block">
                            <span className="about-section__side-label">
                                Based in
                            </span>

                            <strong>
                                {settings?.location || "—"}
                            </strong>
                        </div>

                        <div className="about-section__side-block">
                            <span className="about-section__side-label">
                                Focus
                            </span>

                            <div className="about-section__focus-list">
                                {services.length > 0 ? (
                                    services.slice(0, 5).map((service) => (
                                        <span key={service.id}>
                                            {service.title}
                                        </span>
                                    ))
                                ) : (
                                    <>
                                        <span>Graphic Design</span>
                                        <span>Photography</span>
                                        <span>Video</span>
                                    </>
                                )}
                            </div>
                        </div>
                    </aside>
                </div>
            </div>
        </section>
    );
}

export default AboutSection;