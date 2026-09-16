import ServiceRow from "./ServiceRow";

function ServicesSection({
    services,
    settings,
}) {
    return (
        <section
            id="services"
            className="services-section section"
        >
            <div className="site-container">
                <div className="services-section__header">
                    <div>
                        <p className="section-label">
                            Services
                        </p>

                        <h2 className="section-title">
                            {settings?.servicesTitle ||
                                "What I Do"}
                        </h2>
                    </div>

                    <p className="services-section__note">
                        A selection of creative services
                        across design, image and motion.
                    </p>
                </div>

                <div className="services-list">
                    {services.map(
                        (service, index) => (
                            <ServiceRow
                                key={service.id}
                                service={service}
                                number={index + 1}
                            />
                        )
                    )}
                </div>
            </div>
        </section>
    );
}

export default ServicesSection;