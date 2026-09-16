import {
    useEffect,
    useState,
} from "react";

import { getPortfolio } from "../api/portfolioApi";
import { getSiteSettings } from "../api/settingsApi";
import { getServices } from "../api/servicesApi";

import Header from "../components/layout/Header";
import Footer from "../components/layout/Footer";

import Hero from "../components/home/Hero";
import PortfolioSection from "../components/home/PortfolioSection";
import AboutSection from "../components/home/AboutSection";
import ServicesSection from "../components/home/ServicesSection";
import ContactSection from "../components/home/ContactSection";

import PageState from "../components/common/PageState";
import ChatWidget from "../components/common/ChatWidget";

function HomePage() {
    const [projects, setProjects] = useState([]);
    const [services, setServices] = useState([]);
    const [settings, setSettings] = useState(null);

    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        let isCancelled = false;

        Promise.all([
            getPortfolio(),
            getServices(),
            getSiteSettings(),
        ])
            .then(
                ([
                    portfolioData,
                    servicesData,
                    settingsData,
                ]) => {
                    if (isCancelled) {
                        return;
                    }

                    setProjects(portfolioData);
                    setServices(servicesData);
                    setSettings(settingsData);

                    setError("");
                }
            )
            .catch((err) => {
                if (isCancelled) {
                    return;
                }

                console.error(err);

                setError(
                    "Ana sayfa verilerine şu anda ulaşılamıyor."
                );
            })
            .finally(() => {
                if (!isCancelled) {
                    setLoading(false);
                }
            });

        return () => {
            isCancelled = true;
        };
    }, []);

    async function handleRetry() {
        try {
            setLoading(true);
            setError("");

            const [
                portfolioData,
                servicesData,
                settingsData,
            ] = await Promise.all([
                getPortfolio(),
                getServices(),
                getSiteSettings(),
            ]);

            setProjects(portfolioData);
            setServices(servicesData);
            setSettings(settingsData);
        } catch (err) {
            console.error(err);

            setError(
                "Ana sayfa verilerine şu anda ulaşılamıyor."
            );
        } finally {
            setLoading(false);
        }
    }

    if (loading) {
        return (
            <PageState
                type="loading"
            />
        );
    }

    if (error) {
        return (
            <PageState
                type="error"
                message={error}
                onRetry={handleRetry}
            />
        );
    }

    return (
        <>
            <Header
                brandName={settings?.brandName}
            />

            <main id="top">
                <Hero
                    settings={settings}
                />

                <PortfolioSection
                    projects={projects}
                    settings={settings}
                />

                <AboutSection
                    settings={settings}
                    services={services}
                />

                <ServicesSection
                    services={services}
                    settings={settings}
                />

                <ContactSection
                    settings={settings}
                />
            </main>

            <Footer
                settings={settings}
            />

            <ChatWidget />
        </>
    );
}
export default HomePage;