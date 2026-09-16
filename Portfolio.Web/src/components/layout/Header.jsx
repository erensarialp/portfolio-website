import {
    useEffect,
    useState,
} from "react";

function Header({ brandName }) {
    const [menuOpen, setMenuOpen] =
        useState(false);

    useEffect(() => {
        if (!menuOpen) {
            return;
        }

        const previousOverflow =
            document.body.style.overflow;

        document.body.style.overflow =
            "hidden";

        return () => {
            document.body.style.overflow =
                previousOverflow;
        };
    }, [menuOpen]);

    function closeMenu() {
        setMenuOpen(false);
    }

    return (
        <header className="site-header">
            <div className="site-container site-header__inner">
                <a
                    href="#top"
                    className="site-header__brand"
                    onClick={closeMenu}
                >
                    {brandName || "Portfolio"}
                </a>

                <nav className="site-header__nav site-header__nav--desktop">
                    <a href="#work">
                        Work
                    </a>

                    <a href="#about">
                        About
                    </a>

                    <a href="#services">
                        Services
                    </a>

                    <a href="#contact">
                        Contact
                    </a>
                </nav>

                <button
                    type="button"
                    className="site-header__menu-button"
                    aria-label={
                        menuOpen
                            ? "Menüyü kapat"
                            : "Menüyü aç"
                    }
                    aria-expanded={menuOpen}
                    onClick={() =>
                        setMenuOpen(
                            (current) => !current
                        )
                    }
                >
                    <span />
                    <span />
                </button>
            </div>

            <div
                className={`mobile-menu ${menuOpen
                        ? "mobile-menu--open"
                        : ""
                    }`}
            >
                <nav className="mobile-menu__nav">
                    <a
                        href="#work"
                        onClick={closeMenu}
                    >
                        Work
                    </a>

                    <a
                        href="#about"
                        onClick={closeMenu}
                    >
                        About
                    </a>

                    <a
                        href="#services"
                        onClick={closeMenu}
                    >
                        Services
                    </a>

                    <a
                        href="#contact"
                        onClick={closeMenu}
                    >
                        Contact
                    </a>
                </nav>
            </div>
        </header>
    );
}

export default Header;