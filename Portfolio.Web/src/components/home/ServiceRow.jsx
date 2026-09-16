function ServiceRow({
    service,
    number,
}) {
    const formattedNumber =
        String(number).padStart(2, "0");

    return (
        <article className="service-row">
            <div className="service-row__number">
                {formattedNumber}
            </div>

            <div className="service-row__content">
                <h3>
                    {service.title}
                </h3>

                {service.description && (
                    <p>
                        {service.description}
                    </p>
                )}
            </div>

            <div
                className="service-row__arrow"
                aria-hidden="true"
            >
                ↗
            </div>
        </article>
    );
}

export default ServiceRow;