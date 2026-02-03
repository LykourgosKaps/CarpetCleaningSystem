import { Link } from "react-router-dom";

export default function Home() {
    return (
        <div>
            <section className="hero">
                <div className="container">
                    <h1>Experience the Deepest Clean</h1>
                    <p>
                        Professional carpet cleaning services for your home and office.
                        We restore the beauty of your carpets with advanced eco-friendly technology.
                    </p>
                    <div style={{ display: "flex", gap: "1rem", justifyContent: "center" }}>
                        <Link to="/register" className="btn btn-primary">Get Started</Link>
                        <Link to="/login" className="btn btn-outline">Client Login</Link>
                    </div>
                </div>
            </section>

            <section className="features container">
                <div style={{ textAlign: "center" }}>
                    <h2>Why Choose Us?</h2>
                    <p style={{ color: "#64748b" }}>We treat your carpets like our own.</p>
                </div>

                <div className="features-grid">
                    <div className="feature-card">
                        <div className="feature-icon">✨</div>
                        <h3>Deep Extraction</h3>
                        <p>Our powerful industrial machines remove dirt from the base of the carpet fibers.</p>
                    </div>
                    <div className="feature-card">
                        <div className="feature-icon">🌿</div>
                        <h3>Eco-Friendly</h3>
                        <p>Safe for pets and children. We use biodegradable and non-toxic cleaning solutions.</p>
                    </div>
                    <div className="feature-card">
                        <div className="feature-icon">⏱️</div>
                        <h3>Fast Drying</h3>
                        <p>Walk on your carpets sooner. Our advanced drying techniques reduce downtime.</p>
                    </div>
                </div>
            </section>
        </div>
    );
}
