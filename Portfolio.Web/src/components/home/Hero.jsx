function Hero({ settings }) {
    return (
        <section className="hero">
            <div className="site-container hero__inner">
                <div className="hero__content">
                    {settings?.heroEyebrow && (
                        <p className="hero__eyebrow">
                            {settings.heroEyebrow}
                        </p>
                    )}

                    <h1 className="hero__title">
                        <span>
                            {settings?.heroTitleLine1}
                        </span>

                        {settings?.heroTitleLine2 && (
                            <span>
                                {settings.heroTitleLine2}
                            </span>
                        )}

                        {settings?.heroTitleLine3 && (
                            <span>
                                {settings.heroTitleLine3}
                            </span>
                        )}
                    </h1>

                    <div className="hero__bottom">
                        <p className="hero__description">
                            {settings?.heroDescription}
                        </p>

                        <div className="hero__meta">
                            {settings?.location && (
                                <span>
                                    Based in {settings.location}
                                </span>
                            )}

                            <a
                                href="#work"
                                className="hero__scroll"
                            >
                                Scroll to work
                                <span aria-hidden="true">↓</span>
                            </a>
                        </div>
                    </div>
                </div>

                <div
                    className="hero__artwork"
                    aria-hidden="true"
                >
                    <span className="hero__artwork-number">
                        01
                    </span>

                    <div className="hero__artwork-shape" />
                </div>
            </div>
        </section>
    );
}

export default Hero;