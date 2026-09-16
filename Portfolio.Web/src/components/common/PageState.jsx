function PageState({
    type = "loading",
    message,
    onRetry,
}) {
    const isLoading = type === "loading";

    return (
        <main className="page-state">
            <div className="page-state__inner">
                <span className="page-state__mark">
                    ✦
                </span>

                <p className="page-state__label">
                    {isLoading
                        ? "Loading portfolio"
                        : "Something went wrong"}
                </p>

                <h1 className="page-state__title">
                    {isLoading
                        ? "Birazdan buradayız."
                        : "İçerik yüklenemedi."}
                </h1>

                {message && (
                    <p className="page-state__message">
                        {message}
                    </p>
                )}

                {isLoading ? (
                    <div
                        className="page-state__loader"
                        aria-label="Yükleniyor"
                    >
                        <span />
                    </div>
                ) : (
                    onRetry && (
                        <button
                            type="button"
                            className="page-state__retry"
                            onClick={onRetry}
                        >
                            Tekrar Dene
                        </button>
                    )
                )}
            </div>
        </main>
    );
}

export default PageState;