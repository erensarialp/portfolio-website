function Footer({ settings }) {
    const currentYear = new Date().getFullYear();

    return (
        <footer className="site-footer">
            <div className="site-container site-footer__inner">
                <div className="site-footer__brand">
                    {settings?.brandName || "Portfolio"}
                </div>

                <div className="site-footer__meta">
                    {settings?.location && (
                        <span>
                            {settings.location}
                        </span>
                    )}

                    <span>
                        © {currentYear}
                    </span>
                </div>
            </div>
        </footer>
    );
}

export default Footer;